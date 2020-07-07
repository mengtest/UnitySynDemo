using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;



public class AppBuild {

    /**
    * @name: xsddxr909
    * @test: 
    * @msg: 一键移除 所有依赖清空打包的资源. 
    * @param {type} 
    * @return: 
    */
    public static void removeDependencies()
    {
         string projectPath= Application.dataPath.Replace("/Assets","");
      //   DebugLog.Log(projectPath);
         string outPath = projectPath + "/" + AssetBundleConst.AssetBundleFolder ;
    //     DebugLog.Log(outPath);
        if (!Directory.Exists(outPath))
        {
           FileUtil.CreateDir(outPath);
        }
        string signedOutputPath = outPath+"signed";
         if (!Directory.Exists(signedOutputPath))
        {
           FileUtil.CreateDir(signedOutputPath);
        }
         FileUtil.ClearDirectory(outPath);
          string localFolderPath = Path.Combine(Application.streamingAssetsPath, AssetBundleConst.AssetBundleFolderSigned);
         FileUtil.ClearDirectory(localFolderPath);
          FileUtil.ClearDirectory(signedOutputPath);
        // DirectoryInfo dir = new DirectoryInfo(outPath);
        // DirectoryInfo[] dirArr = dir.GetDirectories("*", SearchOption.AllDirectories);
        // FileInfo[] fiArr = dir.GetFiles("*.manifest");
        // for (int y = 0; y < fiArr.Length; y++)
        // {
        //     FileInfo fi = fiArr[y];
        //     File.Delete(fi.FullName);
        // }
        // for (int p = 0; p < dirArr.Length; p++)
        // {
        //     fiArr = dirArr[p].GetFiles("*.manifest", SearchOption.AllDirectories);
        //     for (int y = 0; y < fiArr.Length; y++)
        //     {
        //         FileInfo fi = fiArr[y];
        //         File.Delete(fi.FullName);
        //     }
        // }

        //  string[] fileArr = new string[6] { "texture", "effect", "avatar","monster","character","building"};
        // for (int i = 0; i < fileArr.Length; i++)
        // {
        //     string oPath = outPath + "/"+fileArr[i];
        //     if (Directory.Exists(oPath))
        //     {
        //         DirectoryInfo dii = new DirectoryInfo(oPath);
        //         dii.Delete(true);
        //     }
        //     Directory.CreateDirectory(oPath);
        // }

        ///////移除AssetBundleNames 的方法
        int length = AssetDatabase.GetAllAssetBundleNames().Length;
        string[] oldAssetBundleNames = new string[length];
        for (int y = 0; y < length; y++)
        {
            oldAssetBundleNames[y] = AssetDatabase.GetAllAssetBundleNames()[y];
        }

        for (int t = 0; t < oldAssetBundleNames.Length; t++)
        {
            AssetDatabase.RemoveAssetBundleName(oldAssetBundleNames[t], true);
        }

        EditorUtility.DisplayDialog("完成", "移除完成", "OK");
    }

  private static List<AssetBundleBuild> abAllList = new List<AssetBundleBuild>();
  private static List<string> _imageAllList = new List<string>();
   /**
    * @name: xsddxr909
    * @test: 
    * @msg: 一键依赖打包.
    * @param {type} 
    * @return: 
    */
   public static void DependenciesBuild(bool copyStreamAsset,bool copyToWeb){
         DateTime startTime = DateTime.Now;
         abAllList.Clear();
         _imageAllList.Clear();
         allEffect();
         allChar();
         allUI();
         BuildTarget platform;
         switch (AssetBundleConst.platformID)
         {
             case 0:
                 platform = BuildTarget.Android;
                 break;
             case 1:
                 platform = BuildTarget.iOS;
                 break;
             default:
                 platform = BuildTarget.StandaloneWindows64;
                 break;
         }
         string projectPath= Application.dataPath.Replace("/Assets","");
         string outPath = projectPath + "/" + AssetBundleConst.AssetBundleFolder ;
         BuildAssetBundleOptions op = 0;
         op = op | BuildAssetBundleOptions.DeterministicAssetBundle;
         op = op | BuildAssetBundleOptions.IgnoreTypeTreeChanges;
         if (BuildPipeline.BuildAssetBundles(outPath, abAllList.ToArray(), op, platform) == null)
         {
             EditorUtility.DisplayDialog("警告", "出现打包异常问题", "OK");
         }
         else
         {
             EditorUtility.DisplayDialog("完成", "资源打包完成", "OK");
         }
    }
    public  static void onlyLua(bool copyStreamAsset,bool copyToWeb){
         string projectPath= Application.dataPath.Replace("/Assets","");
         string outPath = projectPath + "/" + AssetBundleConst.AssetBundleFolder ;
         BuildLuaBundle();
         AssetDatabase.Refresh();
         GenerateFileIndex(outPath);
         CopyFiles(outPath,copyStreamAsset,copyToWeb);
        EditorUtility.DisplayDialog("完成", "拷贝资源", "OK");
    }
     public  static void copyFiles(bool copyStreamAsset,bool copyToWeb){
         string projectPath= Application.dataPath.Replace("/Assets","");
         string outPath = projectPath + "/" + AssetBundleConst.AssetBundleFolder ;
         GenerateFileIndex(outPath);
         CopyFiles(outPath,copyStreamAsset,copyToWeb);
        EditorUtility.DisplayDialog("完成", "拷贝资源", "OK");
    }
    static void BuildLuaBundle()
    {

         BuildTarget platform;
         switch (AssetBundleConst.platformID)
         {
             case 0:
                 platform = BuildTarget.Android;
                 break;
             case 1:
                 platform = BuildTarget.iOS;
                 break;
             default:
                 platform = BuildTarget.StandaloneWindows64;
                 break;
         }
        string projectPath= Application.dataPath.Replace("/Assets","");
        string outPath = projectPath + "/" + AssetBundleConst.AssetBundleFolder ;
            // 打包前先清空
      //  FileUtil.DeleteDirectory(outPath);
      //  FileUtil.CreateDirectorySafely(outPath);
        try
        {
            LuaBundleAdd();
            AssetDatabase.Refresh();
            var option = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DisableWriteTypeTree | BuildAssetBundleOptions.DeterministicAssetBundle;
            BuildPipeline.BuildAssetBundles(outPath, abAllList.ToArray(), option, platform);
            abAllList.Clear();
        }
        finally
        {
            LuaTempDel();
        }

         /*-------------------proto协议二进制文件打包----------------------*/
         string tempProtoPath = Application.dataPath + "/ScriptsLua/PB";
         string tempCmdpkgPath = tempProtoPath + "/cmdpkg.bytes";
         string tempCmdpkgTargetPath = outPath + "/cmdpkg.bytes";
         string tempMbPath = tempProtoPath + "/md.bytes";
         string tempMbTargetPath = outPath + "/md.bytes";
         File.Copy(tempCmdpkgPath.Replace("/", "\\"), tempCmdpkgTargetPath.Replace("/", "\\"), true);
         File.Copy(tempMbPath.Replace("/", "\\"), tempMbTargetPath.Replace("/", "\\"), true);
         /*-------------------proto协议二进制文件打包----------------------*/

        //  // 生成md5文件
        //  StringBuilder md5 = new StringBuilder();
        //  string path = outPath;
        //  foreach (var item in Directory.GetFiles(path, "*lua*", SearchOption.AllDirectories).OrderBy(x => !x.Contains("platform")))
        //  {
        //      if (item.EndsWith(".manifest") || item.EndsWith(".meta")) continue;
        //      FileInfo info = new FileInfo(item);
        //      md5.AppendLine(item.Replace(path, "").Replace("\\", "/") + ":" + FileUtil.md5file(item) + ":" + info.Length + ":lua:");
        //  }

        //  /*-------------------proto协议二进制文件打包----------------------*/
        //  foreach (var item in Directory.GetFiles(path, "*bytes*", SearchOption.AllDirectories).OrderBy(x => !x.Contains("platform")))
        //  {
        //      if (item.EndsWith(".manifest") || item.EndsWith(".meta")) continue;
        //      FileInfo info = new FileInfo(item);
        //      md5.AppendLine(item.Replace(path, "").Replace("\\", "/") + ":" + FileUtil.md5file(item) + ":" + info.Length + ":lua:");
        //  }
        //  /*-------------------proto协议二进制文件打包----------------------*/

        // File.WriteAllText(outPath + "/lua_md5", md5.ToString());

        Debug.Log("Lua包导出成功");
    }
    
     static void LuaBundleAdd()
    {
        LuaBundleAdd("32");
        LuaBundleAdd("64");
    }
    static  void LuaBundleAdd(string plat)
    {
            var luajitpath = Application.dataPath.Replace("\\", "/").Replace("/Assets", "/Luajit" + plat + "/Build.bat");
            if (!File.Exists(luajitpath))
            {
                Debug.LogError(luajitpath + "该文件不存在");
                EditorApplication.Exit(0);
                return;
            }
          //  DebugLog.Log(Application.dataPath + "/_platform" + plat);
            FileUtil.CreateDirectorySafely(Application.dataPath + "/_platform" + plat);
            var savepaths = new List<string>();
            var luapaths = EditorCommon.LuaPaths;
            List<string> uList = null;
            foreach (var luapath in luapaths)
            {
                 uList = new List<string>();
                //"Assets/ToLua/Lua", "lua"
                var select = luapath.Key;
                var abName = luapath.Value;
                List<string>  LuaPaths = new List<string>();
                string[] assetPaths = AssetDatabase.GetAllAssetPaths();
                 foreach (string path in assetPaths)
                {
                    if (path.StartsWith(luapath.Key) && path.EndsWith(".lua"))
                    {
                      LuaPaths.Add(path);
                    }
                }
                for (var i = 0; i < LuaPaths.Count; i++)
                {
                    var item = LuaPaths[i];
                    var nitem = item.Replace(luapath.Key + "/", "").Replace(".lua", "");
                    var newname = "Assets/_platform" + plat + "/" + nitem + ".bytes";
                    uList.Add(newname);
                    string bpath= EditorCommon.realPathToAbsPath(newname);
              //      DebugLog.Log(bpath);
                    var savePath = bpath.Replace(".bytes", ".lua");
                //    DebugLog.Log(savePath);
                    //"E:/F_work/NgGunfire/Project/gunFire/Assets/_platform32/slot.lua"
                    savepaths.Add(savePath);
                    if (File.Exists(savePath))
                    {
                        Debug.LogError(Path.GetFileName(item) + "该文件有冲突");
                        EditorApplication.Exit(0);
                        return;
                    }
                    string  sourceP=EditorCommon.realPathToAbsPath(item);
                    string  saveDp=FileUtil.GetPathDir(savePath);
            //          DebugLog.Log("saveDp",saveDp);
                    if (!Directory.Exists(saveDp))
                    {
                       FileUtil.CreateDir(saveDp);
                    }
                   // if()
                    File.Copy(sourceP, savePath,true);
                }
                AssetBundleBuild ab = new AssetBundleBuild();
                ab.assetBundleName =  abName + plat;
                ab.assetNames = uList.ToArray();
                abAllList.Add(ab);
            }  
            // Assets/_platform32 Assets/_platform64 
            EditorCommon.StartCmd(luajitpath, EditorCommon.realPathToAbsPath("Assets/_platform" + plat));
            foreach (var item in savepaths)
            {
                //E:/F_work/NgGunfire/Project/gunFire/Assets/_platform32/socket/http.lua
                if (File.Exists(item + ".bytes"))
                {
                    File.Move(item + ".bytes", item.Replace(".lua", "") + ".bytes");
                }
                else
                {
                    File.Move(item, item.Replace(".lua", "") + ".bytes");
                }
            }
        }
        static void LuaTempDel()
        {
            FileUtil.DeleteDirectory(Application.dataPath + "/_platform32");
            FileUtil.DeleteDirectory(Application.dataPath + "/_platform64");
        }
      static bool IsLuaPath(string path)
    {
        return (path.StartsWith(AssetBundleConst.LuaPath) 
            || path.StartsWith(AssetBundleConst.ToLuaPath))
            && path != AssetBundleConst.LuaEditorDebugFile
            && path.EndsWith(".lua");
    }

   static void GenerateFileIndex(string outPath)
    {
        string indexPath = Path.Combine(outPath, AssetBundleConst.AssetBundleFiles);
        if (File.Exists(indexPath))
        {
            File.Delete(indexPath);
        }
        StringBuilder builder = new StringBuilder();
        DirectoryInfo directoryInfo = new DirectoryInfo(outPath);
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
            if (fileInfo.Name.EndsWith(".manifest") || fileInfo.Name.Contains(".DS_Store"))
            {
                continue;
            }
            string md5 = FileUtil.MD5File(fileInfo.FullName);
            string fileName = fileInfo.Name;
            AssetBundleFileInfo bundleFileInfo = new AssetBundleFileInfo(fileName, md5, (ulong)fileInfo.Length);
            builder.AppendLine(bundleFileInfo.ToString());
            
        }
        File.WriteAllText(indexPath, builder.ToString());
    }
    static void CopyFiles(string outPath,bool copyStreamAsset,bool copyToWeb)
    {
        string localFolderPath = Path.Combine(Application.streamingAssetsPath, AssetBundleConst.AssetBundleFolderSigned);
        string signedOutputPath = outPath+"signed";
         string projectPath= Application.dataPath.Replace("/Assets","");
         string webPath = projectPath + "/web/" + AssetBundleConst.AssetBundleFolderSigned;
        if(copyStreamAsset){
            if (!Directory.Exists(localFolderPath))
            {
            FileUtil.CreateDir(localFolderPath);
            }else{
                FileUtil.ClearDirectory(localFolderPath);
            }
        }
        if(copyToWeb){
            if (!Directory.Exists(webPath))
            {
               FileUtil.CreateDir(webPath);
            }else{
                FileUtil.ClearDirectory(webPath);
            }
        }
        FileUtil.ClearDirectory(signedOutputPath);
        AssetDatabase.Refresh();

        string timestamp = GameUtils.ConvertUTCDateTimeToSecond(DateTime.Now).ToString();
        AssetBundleFileInfo.BuildTimestamp = timestamp;
        List<AssetBundleFileInfo> fileInfoList = new List<AssetBundleFileInfo>();
        AssetBundleFileInfo configInfo = GetConfigInfo(outPath, AssetBundleConst.AssetBundleFiles);
        fileInfoList.Add(configInfo);
        fileInfoList.AddRange(GetFileInfos(outPath, AssetBundleConst.AssetBundleFiles));

        foreach (AssetBundleFileInfo fileInfo in fileInfoList)
        {
            string sourcePath = Path.Combine(outPath, fileInfo.fileName);
            string localPath = Path.Combine(localFolderPath, fileInfo.signedFileName);
            string sigendPath = Path.Combine(signedOutputPath, fileInfo.signedFileName);
             string webfilePath = Path.Combine(webPath, fileInfo.signedFileName);
            if(copyStreamAsset){
                 File.Copy(sourcePath, localPath, true);
            }
            if(copyToWeb){
               File.Copy(sourcePath, webfilePath, true);
            }
            File.Copy(sourcePath, sigendPath, true);
        }
        AssetBundleBuildInfo buildInfo = new AssetBundleBuildInfo(timestamp, configInfo.md5);
        if(copyStreamAsset){
              string buildInfoPath = Path.Combine(localFolderPath, AssetBundleConst.AssetBundleBuildInfo);
              File.WriteAllText(buildInfoPath, buildInfo.ToString());
        }
         if(copyToWeb){
            string  buildInfoPath = Path.Combine(webPath, AssetBundleConst.AssetBundleBuildInfo);
             File.WriteAllText(buildInfoPath, buildInfo.ToString());
        }
    }

    static AssetBundleFileInfo GetConfigInfo(string outputPath, string fileName)
    {
        string filePath = Path.Combine(outputPath, fileName);
        AssetBundleFileInfo configFileInfo = new AssetBundleFileInfo(fileName, FileUtil.MD5File(filePath), 1024);
        return configFileInfo;
    }

    static List<AssetBundleFileInfo> GetFileInfos(string outputPath, string fileName)
    {
        string filePath = Path.Combine(outputPath, fileName);
        string configText = File.ReadAllText(filePath);
        AssetBundleFileManifest manifest = new AssetBundleFileManifest(configText);
        List<AssetBundleFileInfo> fileInfoList = new List<AssetBundleFileInfo>(manifest.fileInfoDict.Values);
        return fileInfoList;
    }
    public static void allUI(){
        string basePath = "Assets/Res/View/";
        string ffName = "";
        string prefabStr = "";
        string assetsPath = "";

        string sourcePath = basePath ;
        DirectoryInfo[] di = new DirectoryInfo(sourcePath).GetDirectories();
        for (int k = 0; k < di.Length; k++)
        {
            ffName = di[k].Name;
            FileInfo[] iArr = di[k].GetFiles("*.prefab", SearchOption.TopDirectoryOnly);
            if (iArr.Length == 0) continue;
            List<string> uList = new List<string>();
            for (int h = 0; h < iArr.Length; h++)
            {
                prefabStr = iArr[h].Name.Replace(".prefab", "");
                assetsPath = sourcePath + "/" + ffName + "/" + iArr[h].Name;
                uList.Add(assetsPath);
                // string[] depArrs = AssetDatabase.GetDependencies(assetsPath);
                // for (int t = 0; t < depArrs.Length; t++)
                // {
                //     string dStr = depArrs[t];
                //     if (dStr.IndexOf(".jpg") != -1 || dStr.IndexOf(".jpeg") != -1 || dStr.IndexOf(".png") != -1 || dStr.IndexOf(".psd") != -1 || dStr.IndexOf(".tga") != -1 || dStr.IndexOf(".bmp") != -1)
                //     {
                //         string[] uArr = dStr.Split('/');
                //         string cName = uArr[uArr.Length - 1];
                //         cName = cName.Split('.')[0];
                //         if (_imageAllList.IndexOf(cName) != -1) continue;
                //         _imageAllList.Add(cName);
                //         AssetBundleBuild abc = new AssetBundleBuild();
                //         abc.assetBundleName = "tx_" + cName ;
                //         abc.assetNames = new string[1] { dStr };
                //         abAllList.Add(abc);
                //     }
                // }
            }
            
            AssetBundleBuild ab = new AssetBundleBuild();
            string outP = "view_";
            ab.assetBundleName = outP + ffName;
           ab.assetBundleName=  ab.assetBundleName.ToLower();
            ab.assetNames = uList.ToArray();
            abAllList.Add(ab);
        }
        
    }
    public static void allChar()
    {
        // "Avatar" 
     //   string[] fileArr = new string[1] { "Avatar"};
        string[] fileArr = new string[4] { "Monster", "Avatar","Building","Character" };
        string basePath = "Assets/Res/";
        string assetsPath = "";
        string ffName = "";
        string prefabStr = "";
        List<string> uList = null;
           
        for (int i = 0; i < fileArr.Length; i++)
        {
            string sourcePath =basePath + fileArr[i];
            DirectoryInfo[] di = new DirectoryInfo(sourcePath).GetDirectories();
            for (int k = 0; k < di.Length; k++)
            {
                ffName = di[k].Name;
                FileInfo[] iArr = di[k].GetFiles("*.prefab", SearchOption.TopDirectoryOnly);
                if (iArr.Length == 0) continue;
                if(fileArr[i]=="Avatar"){
                   //带时装
                 //   prefabStr = iArr[0].Name.Replace(".prefab", "");

                    string pp = sourcePath +"/"+ffName + "/Model/";
                    FileInfo[] fileList = new DirectoryInfo(pp).GetFiles();
                    for (int c = 0; c < fileList.Length; c++)
                    {
                        FileInfo fi = fileList[c];
                        if (fi.Extension != ".prefab") continue;
                        string re = pp + fi.Name;
                        string reName = fi.Name.Replace(".prefab", "");
                        if(reName==ffName){
                            uList = new List<string>();
                            uList.Add(re);
                            //带动画文件 rm为带动画骨骼文件 //可以加上特效
                            AssetBundleBuild asb = new AssetBundleBuild();
                            asb.assetBundleName = fileArr[i] + "_" + ffName ;
                            asb.assetNames = uList.ToArray();
                            abAllList.Add(asb);
                       }else{
                            uList = new List<string>();
                            // string[] depArr = AssetDatabase.GetDependencies(re);
                            //     //获取依赖.
                            // for (int t = 0; t < depArr.Length; t++)
                            // {
                            //     string dStr = depArr[t];
                            //     if (dStr.IndexOf(".jpg") != -1 || dStr.IndexOf(".jpeg") != -1 || dStr.IndexOf(".png") != -1 || dStr.IndexOf(".psd") != -1 || dStr.IndexOf(".tga") != -1 || dStr.IndexOf(".bmp") != -1)
                            //     {
                            //         string[] uArr = dStr.Split('/');
                            //         string cName = uArr[uArr.Length - 1];
                            //         cName = cName.Split('.')[0];
                            //         if (_imageAllList.IndexOf(cName) != -1) continue;
                            //         _imageAllList.Add(cName);
                            //         AssetBundleBuild abc = new AssetBundleBuild();
                            //         abc.assetBundleName = "tx_" + cName ;
                            //         abc.assetNames = new string[1] { dStr };
                            //         abAllList.Add(abc);
                            //     }
                                
                            // }

                            uList.Add(re);
                            uList.Add("Assets/Res/TXTBonesInfo/" + reName+".txt");
                            AssetBundleBuild ab = new AssetBundleBuild();
                            string outP = fileArr[i] + "_";
                            ab.assetBundleName = outP + reName ;
                            ab.assetNames = uList.ToArray();
                            abAllList.Add(ab);
                       }



                    }
                }else{
                    //没有时装
                    uList = new List<string>();
                    for (int o = 0; o < iArr.Length; o++)
                    {
                        prefabStr = iArr[o].Name.Replace(".prefab", "");
                        if(prefabStr==ffName){
                            continue;
                        }
                        assetsPath = sourcePath +"/" + ffName + "/"  + iArr[o].Name;
                    //     string[] depArr = AssetDatabase.GetDependencies(assetsPath);
                    //     for (int t = 0; t < depArr.Length; t++)
                    //     {
                    //         string dStr = depArr[t];
                    // //        DebugLog.Log(dStr);
                    //         if (dStr.IndexOf(".jpg") != -1 || dStr.IndexOf(".jpeg") != -1 || dStr.IndexOf(".png") != -1 || dStr.IndexOf(".psd") != -1 || dStr.IndexOf(".tga") != -1 || dStr.IndexOf(".bmp") != -1)
                    //         {
                    //             string[] uArr = dStr.Split('/');
                    //             string cName = uArr[uArr.Length - 1];
                    //             cName = cName.Split('.')[0];
                    //             if (_imageAllList.IndexOf(cName) != -1) continue;
                    //             _imageAllList.Add(cName);
                    //             AssetBundleBuild abc = new AssetBundleBuild();
                    //             abc.assetBundleName = "tx_" + cName;
                    //             abc.assetNames = new string[1] { dStr };
                    //             abAllList.Add(abc);
                    //         }
                    //     }
                       uList.Add(assetsPath);
                    }
                    AssetBundleBuild ab = new AssetBundleBuild();
                    ab.assetBundleName = fileArr[i] + "_" + ffName ;
                    ab.assetNames = uList.ToArray();
                    abAllList.Add(ab);
                }

            }

        }
    }
   public static void allEffect()
    {
       // string[] fileArr = new string[7] { "Character", "hit", "Monster", "Qita", "UIEffect","Boss","Common" };
        string[] fileArr = new string[1] { "hit" };
        string basePath = "Assets/Res/Effect/";
        string ffName = "";
        string prefabStr = "";
        string assetsPath = "";
        
        for (int i = 0; i < fileArr.Length; i++)
        {
            string sourcePath = basePath + fileArr[i];
            DirectoryInfo[] di = new DirectoryInfo(sourcePath).GetDirectories();
            for (int k = 0; k < di.Length; k++)
            {
                ffName = di[k].Name;
                FileInfo[] iArr = di[k].GetFiles("*.prefab", SearchOption.TopDirectoryOnly);
                if (iArr.Length == 0) continue;
                List<string> uList = new List<string>();
                for (int h = 0; h < iArr.Length; h++)
                {
                    prefabStr = iArr[h].Name.Replace(".prefab", "");
                    assetsPath = sourcePath + "/" + ffName + "/" + iArr[h].Name;
                    uList.Add(assetsPath);
                    // string[] depArrs = AssetDatabase.GetDependencies(assetsPath);
                    // for (int t = 0; t < depArrs.Length; t++)
                    // {
                    //     string dStr = depArrs[t];
                    //     if (dStr.IndexOf(".jpg") != -1 || dStr.IndexOf(".jpeg") != -1 || dStr.IndexOf(".png") != -1 || dStr.IndexOf(".psd") != -1 || dStr.IndexOf(".tga") != -1 || dStr.IndexOf(".bmp") != -1)
                    //     {
                    //         string[] uArr = dStr.Split('/');
                    //         string cName = uArr[uArr.Length - 1];
                    //         cName = cName.Split('.')[0];
                    //         if (_imageAllList.IndexOf(cName) != -1) continue;
                    //         _imageAllList.Add(cName);
                    //         AssetBundleBuild abc = new AssetBundleBuild();
                    //         abc.assetBundleName = "tx_" + cName ;
                    //         abc.assetNames = new string[1] { dStr };
                    //         abAllList.Add(abc);
                    //     }
                    // }
                }
               
                AssetBundleBuild ab = new AssetBundleBuild();
                string outP = "effect_";
                ab.assetBundleName = outP + ffName;
                ab.assetNames = uList.ToArray();
                abAllList.Add(ab);
            }
        }
    }
}

