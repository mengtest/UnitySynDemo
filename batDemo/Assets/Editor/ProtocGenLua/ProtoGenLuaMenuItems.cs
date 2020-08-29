using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class ProtoGenLuaMenuItems
{
    static List<string> files = new List<string>();

    [MenuItem("PB导出工具/打开PB文件位置")]
    public static void OpenDirForPb()
    {
        OpenDirectory(Application.dataPath.Replace("Assets", "Tools/GameProto"), false);
        //string tempProtoPath = Application.dataPath + "/ScriptsLua/PB";
        //string tempCmdpkgPath = tempProtoPath + "/Cmdpkg.bytes";
        //string tempCmdpkgTargetPath = Application.dataPath + "/Cmdpkg.bytes";
        //string tempMbPath = tempProtoPath + "/md.bytes";
        //string tempMbTargetPath = Application.dataPath + "/md.bytes";
        ////if (File.Exists(tempCmdpkgTargetPath))
        ////{
        //    File.Copy(tempCmdpkgPath.Replace("/", "\\"), tempCmdpkgTargetPath.Replace("/", "\\"), true);
        //    File.Copy(tempMbPath.Replace("/", "\\"), tempMbTargetPath.Replace("/", "\\"), true);
        //}
        //else
        //{
        //    File.Copy(tempCmdpkgPath.Replace("/", "\\"), tempCmdpkgTargetPath.Replace("/", "\\"));
        //    File.Copy(tempMbPath.Replace("/", "\\"), tempMbTargetPath.Replace("/", "\\"));
        //}
    }
    public static void OpenDirectory(string path, bool create = false)
    {
        if (string.IsNullOrEmpty(path)) return;

        path = ChangeToSystemPath(path);
        if (!Directory.Exists(path))
        {
            if (create)
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                DebugLog.LogError("No Directory: " + path);
                return;
            }
        }
        System.Diagnostics.Process.Start("explorer.exe", path);
    }
    public static string ChangeToSystemPath(string path)
    {
        path = path.Replace("/", "\\");
        return path;
    }
    [MenuItem("PB导出工具/Build Protobuf-lua-gen File Main")]
    public static void BuildProtobufFileMain()
    {
        _BuildProtoFile("/main");
    }
    [MenuItem("PB导出工具/Build Protobuf-lua-gen File 缪树江")]
    public static void BuildProtobufFileMSJ()
    {
        _BuildProtoFile("/msj");
    }
    [MenuItem("PB导出工具/Build Protobuf-lua-gen File 桑至洪")]
    public static void BuildProtobufFileSZH()
    {
        _BuildProtoFile("/szh");
    }
    [MenuItem("PB导出工具/Build Protobuf-lua-gen File 彭琛")]
    public static void BuildProtobufFilePC()
    {
        _BuildProtoFile("/pc");
    }

    private static void _BuildProtoFile(string canPath)
    {
      //  _CheckMd5Boo = EditorUtility.DisplayDialog("导出Porot提示", "是否选择使用增量更新", "YES", "NO");

        EditorUtility.DisplayProgressBar("Proto导出", "开始导出", 0.1f);

        string AppDataPath = Application.dataPath;
        string dir = AppDataPath + "/ScriptsLua/PB";
        //string pbPath = Application.dataPath.Replace("Assets", "") + "Tools/GameProto";
        string pluginPath = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua/plugin";//proto的工具位置
        string protoExePath = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua/plugin";//proto的工具位置
        //string serverPath = Application.dataPath.Replace("Assets", "").Replace("batDemo/", "") + "server/SparkGameGO/src/sparkgame.com/miniworld/proto/ProtoSource/";//一开始使用的proto所在位置
      //  string serverPath2 = Application.dataPath.Replace("Assets", "").Replace("batDemo/", "") + "server/SparkGameGO/src/sparkgame.com/miniworld/vendor/sparkgame.com/gframe/protoSource";//后来增加的位置信息
        string cmdpkgPath = Application.dataPath.Replace("Assets", "") + "Tools/GameProto/";
        string serverPath3 = Application.dataPath.Replace("Assets", "Tools") + "/GameProto"+ canPath;

        files.Clear();

        EditorUtility.DisplayProgressBar("Proto导出", "检测文件内容", 0.3f);

        //List<ProtoFileInfo> tempFileInfoList = _GetProtoFiles(serverPath,serverPath2);
        List<ProtoFileInfo> tempFileInfoList = _GetProtoFilesForFloder(serverPath3, cmdpkgPath);

        string tempStrPath = Application.dataPath + "/ScriptsLua/pb/md.bytes";
        if (System.IO.File.Exists(tempStrPath))
        {
            File.Delete(tempStrPath);
        }

        string tempCmdPath = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua/create_protoc-pb-"+canPath.Replace("/","") + ".cmd";
        string tempProtoSoucePath = "../../GameProto" + canPath;
       // string tempOutPath = "../../GameProto";
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = tempCmdPath;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = true;
        info.WorkingDirectory = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua";
        info.ErrorDialog = true;
        System.Diagnostics.Process task = null;
        try
        {
            task = Process.Start(info);
            if (task != null)
            {
                task.WaitForExit(1000);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("e");
        }
        finally
        {
            if (task != null && task.HasExited)
            {

            }
        }
        //if (_CheckMd5Boo)
        //{
        //    files = _CheckFileMd5(tempFileInfoList);
        //}
        //else
        //{
        //    files = _NoCheckMd5(tempFileInfoList);
        //}

        //EditorUtility.DisplayProgressBar("Proto导出", "导出各个proto对应的lua文件", 0.5f);

        //string protoc = string.Empty;
        //string protoc_gen_dir = string.Empty;
        //string args = string.Empty;
        //string output = dir;
        //bool isWin = true;

        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    protoc = protoExePath + "/protoc.exe";
        //    protoc_gen_dir = "\"" + pluginPath + "/protoc-gen-lua.bat\"";
        //}
        //else if (Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    protoc = "/usr/local/bin/protoc";
        //    protoc_gen_dir = "/usr/local/bin/protoc-gen-lua";
        //    isWin = false;
        //}
        //args = " --proto_path=" + serverPath3 + " --lua_out=" + output + " --plugin=protoc-gen-lua=" + protoc_gen_dir;

        //foreach (string f in files)
        //{
        //    string name = Path.GetFileName(f);
        //    string ext = Path.GetExtension(f);
        //    string fullName = Path.GetFullPath(f);
        //    UnityEngine.Debug.Log("fullName : " + fullName);
        //    if (!ext.Equals(".proto")) continue;

        //    ProcessStartInfo info = new ProcessStartInfo();
        //    info.FileName = protoc;
        //    if (f.IndexOf(serverPath3) < 0)
        //    {
        //        info.Arguments = args = " --proto_path=" + serverPath2 + " --lua_out=" + output + " --plugin=protoc-gen-lua=" + protoc_gen_dir + " " + name;
        //    }
        //    else
        //    {
        //        info.Arguments = args + " " + name;
        //    }
        //    info.WindowStyle = ProcessWindowStyle.Hidden;
        //    info.UseShellExecute = isWin;
        //    info.WorkingDirectory = dir;
        //    info.ErrorDialog = true;
        //    UnityEngine.Debug.Log(info.FileName + " " + info.Arguments);

        //    Process pro = Process.Start(info);
        //    pro.WaitForExit();
        //}

        EditorUtility.DisplayProgressBar("Proto导出", "开始获取proto文件内容，重写pb_list", 0.7f);

        Dictionary<string, List<ProtoFileData>> tempFileDataList = _GetProtoFileContext(tempFileInfoList);

        _Writepb_list(tempFileInfoList, tempFileDataList);

        EditorUtility.DisplayProgressBar("Proto导出", "重写pb_list完毕", 0.99f);

        EditorUtility.ClearProgressBar();

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("完成", "导出proto的lua文件完成", "OK");
    }

    /// <summary>
    /// pb_list.lua路径
    /// </summary>
    private static string _Md5FilePath = "Assets/ScriptsLua/PB/pb_list.lua";
    /// <summary>
    /// 是否使用MD5增量更新
    /// </summary>
    private static bool _CheckMd5Boo = true;

    /// <summary>
    /// 检测文件的MD5值进行更新
    /// </summary>
    private static List<string> _CheckFileMd5(List<ProtoFileInfo> canFileInfos)
    {
        StreamReader sr = new StreamReader(_Md5FilePath);
        string all = sr.ReadToEnd();
        string[] s = all.Split('\n');
        bool tempMD5Boo = false;

        Dictionary<string, string> md5Dic = new Dictionary<string, string>();
        foreach(string tempStr in s)
        {
            if(tempStr.Equals("----MD5----\r"))
            {
                tempMD5Boo = true;
                continue;
            }
            if (!tempMD5Boo) continue;
            if (tempStr.Equals(string.Empty)) continue;
            string[] tempStrList = tempStr.Split('|');
            
            md5Dic.Add(tempStrList[0].Replace("--",""), tempStrList[1]);
        }
        List<string> tempReturnList = new List<string>();
        foreach(ProtoFileInfo tempInfo in canFileInfos)
        {
            string tempMd5;
            string tempStr = tempInfo.filePath.Substring(tempInfo.filePath.IndexOf("ProtoSource") + 12);
            if (md5Dic.TryGetValue(tempStr, out tempMd5)&&tempMd5 != null)
            {
                string cmStr = tempInfo.md5Str + "\r";
                if(!cmStr.Equals(tempMd5))
                {
                    tempReturnList.Add(tempInfo.filePath);
                }
            }
            else
            {
                tempReturnList.Add(tempInfo.filePath);
            }
        }
        sr.Close();
        sr.Dispose();
        return tempReturnList;
    }

    /// <summary>
    /// 不检测文件MD5
    /// </summary>
    private static List<string> _NoCheckMd5(List<ProtoFileInfo> canFileInfos)
    {
        List<string> tempReturnList = new List<string>();
        foreach (ProtoFileInfo tempInfo in canFileInfos)
        {
            tempReturnList.Add(tempInfo.filePath);
        }
        return tempReturnList;
    }

    /// <summary>
    /// 获取所有的服务器端Proto文件信息
    /// </summary>
    /// <param name="canDataPath">服务端Proto文件夹路径</param>
    private static List<ProtoFileInfo> _GetProtoFiles(string canDataPath,string canDataPath2)
    {
        List<ProtoFileInfo> tempReturnList = new List<ProtoFileInfo>();
        string copyTextPath = canDataPath + "copy.txt";
        string copyStr = File.ReadAllText(copyTextPath);

        string[] copyStrArr = copyStr.Split(Environment.NewLine.ToCharArray());

        foreach (string str in copyStrArr)
        {
            string fileName = str + ".proto";
            string fullName = canDataPath + fileName;
            if (File.Exists(fullName))
            {
                ProtoFileInfo curFileInfo = new ProtoFileInfo();
                curFileInfo.fileName = fileName.Substring(0, fileName.IndexOf("."));
                curFileInfo.md5Str = FileUtil.md5file(fullName);
                curFileInfo.filePath = fullName.Replace('\\', '/');
                tempReturnList.Add(curFileInfo);
            }
        }

		string filename = "cmdpkg.proto";
		string fullname = canDataPath2 + "\\" + filename;
		if (File.Exists(fullname))
		{
			ProtoFileInfo curFileInfo = new ProtoFileInfo();
			curFileInfo.fileName = filename.Substring(0, filename.IndexOf("."));
			curFileInfo.md5Str = FileUtil.md5file(filename);
			curFileInfo.filePath = fullname.Replace('\\', '/');
			tempReturnList.Add(curFileInfo);
		}
		return tempReturnList;
    }
    ///生成cmdpkg.proto
    private static List<ProtoFileInfo> _GetProtoFilesForFloder(string canServerPath,string canServerPath2)
    {
        List<string> fileList = new List<string>();
        List<ProtoFileInfo> tempReturnList = new List<ProtoFileInfo>();

        fileList.AddRange(Directory.GetFiles(canServerPath, "*", SearchOption.AllDirectories));
        fileList.Sort();
        for (int i = 0; i < fileList.Count; i++)
        {
            string fullName = fileList[i];
            string fileName = fullName.Substring(fullName.LastIndexOf("\\") + 1);
            if (!(fileName.EndsWith(".proto")))
            {
                continue;
            }
            if (File.Exists(fullName))
            { 
                ProtoFileInfo curFileInfo = new ProtoFileInfo();
                curFileInfo.fileName = fileName.Substring(0, fileName.IndexOf("."));
                curFileInfo.md5Str = FileUtil.md5file(fullName);
                curFileInfo.filePath = fullName.Replace('\\', '/');
                tempReturnList.Add(curFileInfo);
            }
        }

        string filename = "cmdpkg.proto";
        string fullname = canServerPath2 + "\\" + filename;
        if (File.Exists(fullname))
        {
            ProtoFileInfo curFileInfo = new ProtoFileInfo();
            curFileInfo.fileName = filename.Substring(0, filename.IndexOf("."));
            curFileInfo.md5Str = FileUtil.md5file(fullname);
            curFileInfo.filePath = fullname.Replace('\\', '/');
            tempReturnList.Add(curFileInfo);
        }
        return tempReturnList;
    }

    /// <summary>
    /// 通过文件信息将里面的内容转换成需要使用的格式
    /// </summary>
    private static Dictionary<string, List<ProtoFileData>> _GetProtoFileContext(List<ProtoFileInfo> canFileInfos)
    {
        List<ProtoFileData> tempReturnProtoMessageFileData = new List<ProtoFileData>();
        List<ProtoFileData> tempReturnProtoEnumFileData = new List<ProtoFileData>();
        foreach (ProtoFileInfo tempFileInfo in canFileInfos)
        {
            StreamReader sr = new StreamReader(tempFileInfo.filePath);
            string all = sr.ReadToEnd();
            string[] tempLines = all.Split('\n');

            //下一个是否是大类型注释
            bool tempNextNoteBoo = false;
            //下一个是否是大类型类型
            bool tempNextMessageBoo = false;

            //下一个是否是小类型类型
            bool tempNextType = false;
            //下一个是否是小类型名字
            bool tempNextTypeName = false;
            //下一个是否是小类型注释
            bool tempNextTypeNote = false;
            //上一次是否获取小类型注释
            bool tempLastGetNextTypeNote = false;

            //是否获取一个小类型结束
            bool tempNextGetTypeEndBoo = false;
            //是否获取完一个大类型结束
            bool tempNextGetMessageEndBoo = false;

            //是否添加进枚举类型注解里面
            bool tempIsEnumTypeBoo = false;

            ProtoFileData curProtoData = new ProtoFileData();
            curProtoData.fileName = tempFileInfo.fileName;
            string tempMsgNote = "";
            string tempTypeNote = "";
            string tempType = "";
            foreach (string tempCurLine in tempLines)//遍历所有行
            {
                if (tempNextGetMessageEndBoo)//判断是不是大类型结束，结束就创建一个新的类型信息类
                {
                    curProtoData = new ProtoFileData();
                    curProtoData.fileName = tempFileInfo.fileName;
                    tempNextGetMessageEndBoo = false;
                }
                char[] tempChar = { ' ', '\t' };
                string tempReplaceStr = tempCurLine.Replace('=', ' ');
                string[] tempStr = tempReplaceStr.Split(tempChar, options: StringSplitOptions.RemoveEmptyEntries);//根据空格切分行内内容
                ///切割出来的最后一个字段
                int tempLineEnd = tempStr.Length - 1;
                for (int i = 0; i < tempStr.Length; i++)//遍历切分的行内内容
                {
                    string tempValue = tempStr[i].Trim();
                    string tempValue2 = tempStr[i];
                    if (tempValue == string.Empty) continue;//空继续

                    if (tempValue == "repeated")
                        continue;

                    #region 大型注释
                    if (tempValue == "//" && !tempNextType)//是否等于“//”，并且不是小类型获取时候，进入大类型注释获取
                    {
                        if (i != tempLineEnd)//判断并且不是最后一行，开启大型注释
                        {
                            tempNextNoteBoo = true;
                        }
                        continue;
                    }

                    if(tempValue.IndexOf("//") >= 0 && !tempNextType)//判断是否带有“//”，并且不是小类型获取时候，进入大类型注释获取
                    {
                        tempMsgNote = tempMsgNote + tempValue.Replace("//", "");
                        if (i == tempLineEnd)//是否到了最后一个字段，然后关闭大型注释
                        {
                            curProtoData.messageNote = tempMsgNote;
                            tempMsgNote = string.Empty;
                            tempNextNoteBoo = false;
                            continue;
                        }
                    }

                    if (tempNextNoteBoo)
                    {
                        tempMsgNote = tempMsgNote + tempValue.Replace("//", "");
                        if (i == tempLineEnd)//是否到了最后一个字段，然后关闭大型注释
                        {
                            curProtoData.messageNote = tempMsgNote;
                            tempMsgNote = string.Empty;
                            tempNextNoteBoo = false;
                            continue;
                        }
                    }
                    #endregion


                    if (tempValue == "message")//判断是否是大型类型
                    {
                        tempNextMessageBoo = true;
                        tempIsEnumTypeBoo = false;
                        continue;
                    }

                    if(tempValue == "enum")//判断是否是大型枚举类型
                    {
                        tempNextMessageBoo = true;
                        tempIsEnumTypeBoo = true;
                        continue;
                    }

                    if (tempNextMessageBoo)//存入大型类型，并且开启小型类型
                    {
                        tempNextMessageBoo = false;
                        curProtoData.messageName = tempValue.Replace("{", "");
                        tempNextType = true;
                        tempNextGetTypeEndBoo = true;
                        continue;
                    }

                    if (tempValue == "{")
                    {
                        continue;
                    }

                    if (tempValue == "}")//判断是否已经循环了一个大型类型的所有
                    {
                        if (tempNextType)
                        {
                            tempNextType = false;
                            tempNextGetMessageEndBoo = true;
                            if (tempIsEnumTypeBoo)//根据类型还是枚举来存入数据
                            {
                                tempReturnProtoEnumFileData.Add(curProtoData);
                            }
                            else
                            {
                                tempReturnProtoMessageFileData.Add(curProtoData);
                            }
                        }
                    }

                    if (tempNextType && tempNextGetTypeEndBoo)//判断是否开启小型类型，以及是否获取完一个小型类型
                    {
                        if(tempValue == "//")//判断是否等于“//”，是否开启小型注释，
                        {
                            tempNextTypeNote = true;
                            tempLastGetNextTypeNote = true;
                            continue;
                        }

                        if (tempValue.IndexOf("//") >= 0)//判断是否带有“//”，是否开启小型注释，
                        {
                            tempNextTypeNote = true;
                            tempLastGetNextTypeNote = true;
                            tempTypeNote = tempTypeNote + tempValue.Replace("//", "");
                            //if (tempValue2.IndexOf("\r") >= 0)
                            //{
                            //    tempNextTypeNote = false;
                            //}
                            if (i==tempLineEnd)//判断是否是最后一个字段，关闭小型注释
                            {
                                tempNextTypeNote = false;
                            }
                            continue;
                        }

                        if (tempNextTypeNote)//如果开启小型注释，继续添加注释
                        {
                            tempTypeNote = tempTypeNote + tempValue;
                            //if (tempValue2.IndexOf("\r") >= 0)
                            //{
                            //    tempNextTypeNote = false;
                            //}
                            if (i == tempLineEnd)//判断是否是最后一个字段，关闭小型注释
                            {
                                tempNextTypeNote = false;
                            }
                            continue;
                        }

                        if (tempValue.IndexOf("<") >= 0)
                        {
                            tempType = tempType + tempValue;
                            if(tempValue.IndexOf(">")>=0)
                            {
                                tempNextGetTypeEndBoo = false;
                                curProtoData.messageType.Add(tempType.Replace("=", ""));
                                tempType = "";
                                tempNextTypeName = true;
                            }
                            continue;
                        }

                        tempType = tempType + tempValue;
                        tempNextGetTypeEndBoo = false;
                        curProtoData.messageType.Add(tempType.Replace("=", ""));
                        tempType = "";
                        tempNextTypeName = true;

                        if (tempLastGetNextTypeNote && tempIsEnumTypeBoo)
                        {
                            if (!curProtoData.messageTypeNoteDic.ContainsKey(tempValue))
                            {
                                curProtoData.messageTypeNoteDic.Add(tempValue, tempTypeNote);
                            }
                            tempTypeNote = string.Empty;
                            tempLastGetNextTypeNote = false;
                        }

                        continue;
                    }

                    if (tempNextTypeName)
                    {
                        if (tempLastGetNextTypeNote && !tempIsEnumTypeBoo)
                        {
                            if (!curProtoData.messageTypeNoteDic.ContainsKey(tempValue))
                            {
                                curProtoData.messageTypeNoteDic.Add(tempValue, tempTypeNote);
                            }
                            tempTypeNote = string.Empty;
                            tempLastGetNextTypeNote = false;
                        }
                        if(tempIsEnumTypeBoo)
                        {
                            if (tempValue == "=")
                                continue;
                            if(tempValue.IndexOf("=") >= 0 || tempValue.IndexOf(";")>=0)
                            {
                                tempNextTypeName = false;
                                if(tempValue.IndexOf(";") >= 0)
                                {
                                    curProtoData.messageTypeName.Add(tempValue.Replace("=", "").Replace(";",""));
                                    tempNextGetTypeEndBoo = true;
                                }
                                else
                                {
                                    curProtoData.messageTypeName.Add(tempValue.Replace("=", ""));
                                }
                                continue;
                            }
                        }
                        else
                        {
                            tempNextTypeName = false;
                            curProtoData.messageTypeName.Add(tempValue.Replace("=", ""));
                            continue;
                        }
                    }

                    if (tempValue.IndexOf(";") >0 )
                    {
                        tempNextGetTypeEndBoo = true;
                    }
                }
            }
            sr.Close();
            sr.Dispose();
        }
        Dictionary<string, List<ProtoFileData>> tempReturnDic = new Dictionary<string, List<ProtoFileData>>();
        tempReturnDic.Add("msg", tempReturnProtoMessageFileData);
        tempReturnDic.Add("enum", tempReturnProtoEnumFileData);
        return tempReturnDic;
    }

    /// <summary>
    /// 通过文件信息，重新编写pb_list文件
    /// </summary>
    private static bool _Writepb_list(List<ProtoFileInfo> canFileInfos, Dictionary<string, List<ProtoFileData>> canFileDatas)
    {
        string pb_list_path = Application.dataPath + "/ScriptsLua/PB/pb_list.lua";
        if (System.IO.File.Exists(pb_list_path))
        {
            File.Delete(pb_list_path);
        }
        StreamWriter sw;
        var u8WithoutBom = new System.Text.UTF8Encoding(false);
        sw = new StreamWriter(pb_list_path, false, u8WithoutBom);

        sw.WriteLine("--Auto Create , Don't Edit--");
        sw.WriteLine("--If you have any questions, please contact ma huibao--");
        sw.Write("\n");
      //  sw.WriteLine("local tempPath1 = Util.GetProtoBytesPath()..\"cmdpkg.bytes\"");
       // sw.WriteLine("local tempPath2 = Util.GetProtoBytesPath()..\"md.bytes\"");
         sw.WriteLine("local tempPath2 = GameLuaManager.GetProtoBytesPath()");
     //   sw.WriteLine("assert(pb.loadfile(tempPath1))");
        sw.WriteLine("assert(pb.loadfile(tempPath2))");
        //foreach(ProtoFileInfo tempInfo in canFileInfos)
        //{
        //    sw.WriteLine("require " + "\"PB." + tempInfo.fileName + "_pb\"");
        //}

        List<ProtoFileData> tempFileData;
        canFileDatas.TryGetValue("enum", out tempFileData);
        ProtoFileData tempCMDFileData = null;
        if(tempFileData != null)
        {
            foreach (ProtoFileData tempData in tempFileData)
            {
                sw.WriteLine("---" + tempData.messageNote);
                sw.WriteLine("---@class " + tempData.messageName);
                sw.WriteLine(tempData.messageName + " = {");
                for (int i = 0; i < tempData.messageType.Count; i++)
                {
                    string tempNote;
                    if (tempData.messageTypeNoteDic.TryGetValue(tempData.messageType[i], out tempNote) && tempNote != null)
                    {

                    }
                    else
                    {
                        tempNote = "";
                    }
                    sw.WriteLine("    ---@type number " + tempNote);                                        
                    sw.WriteLine("    " + tempData.messageType[i] + " = " + tempData.messageTypeName[i] + ",");
                }
                sw.WriteLine("}");
                if (tempData.messageName== "GAME_CMD")
                {
                    tempCMDFileData = tempData;
                }
            }
        }

        tempFileData = null;
        canFileDatas.TryGetValue("msg", out tempFileData);

        Dictionary<string, string> tempMsgNameDic = new Dictionary<string, string>();
        foreach (ProtoFileData tempData in tempFileData)
        {
            tempMsgNameDic.Add(tempData.messageName.ToUpper(), tempData.messageName);
        }

        if (tempCMDFileData!=null)
        {
            sw.WriteLine("MsgIDMap = {");
            for (int i = 0; i < tempCMDFileData.messageType.Count; i++)
            {
                string tempNote;
                if (tempCMDFileData.messageTypeNoteDic.TryGetValue(tempCMDFileData.messageType[i], out tempNote) && tempNote != null)
                {

                }
                else
                {
                    tempNote = "";
                }
                string tempMsgUPName = tempCMDFileData.messageType[i].Substring(9).ToUpper();
                string tempWriteName;
                if (tempMsgNameDic.TryGetValue(tempMsgUPName, out tempWriteName) && tempWriteName != null)
                {
                    sw.WriteLine("    ---@type string " + tempNote);
                    sw.WriteLine("    [" + tempCMDFileData.messageTypeName[i] + "] = \"md." + tempWriteName + "\",");
                    sw.WriteLine("    ---@type string " + tempNote + " 服务端返回");
                    sw.WriteLine("    [-" + tempCMDFileData.messageTypeName[i] + "] = \"md." + tempWriteName + "Rsp\",");
                }
            }
            sw.WriteLine("}");
        }

        sw.Write("\n");
        sw.WriteLine("if true then return  end");
        sw.Write("\n");

        foreach (ProtoFileInfo tempInfo in canFileInfos)
        {
            sw.WriteLine("---@class " + tempInfo.fileName + "_pb");
            sw.WriteLine(tempInfo.fileName + "_pb = {}");
        }
        if (tempFileData == null) return false;
        foreach(ProtoFileData tempData in tempFileData)
        {
            string tempFileNameStr = tempData.fileName + "_pb";
            string tempMessageReturnData = tempFileNameStr+"_" + tempData.messageName + "Data";
            sw.WriteLine("---@class " + tempMessageReturnData);
            sw.WriteLine(tempMessageReturnData + " = {");
            for(int i =0;i<tempData.messageType.Count;i++)
            {
                string tempNote;
                if(tempData.messageTypeNoteDic.TryGetValue(tempData.messageTypeName[i], out tempNote) && tempNote !=null)
                {

                }
                else
                {
                    tempNote = "";
                }
                sw.WriteLine("    ---@type " + tempData.messageType[i] + " "+ tempNote);
                sw.WriteLine("    "+tempData.messageTypeName[i] + " = nil,");
            }
            sw.WriteLine("}");

            sw.WriteLine("---" + tempData.messageNote);
            sw.WriteLine("---@return " + tempMessageReturnData);
            sw.WriteLine("function " + tempFileNameStr + "." + tempData.messageName + "()");
            sw.WriteLine("end");
        }
        sw.WriteLine("----MD5----");
        foreach (ProtoFileInfo tempInfo in canFileInfos)
        {
            string tempStr = tempInfo.filePath.Substring(tempInfo.filePath.IndexOf("ProtoSource") + 12);
            sw.WriteLine("--" + tempStr + "|"+tempInfo.md5Str);
        }
        sw.Flush();
        sw.Close();
        sw.Dispose();
        return true;
    }

    /// <summary>
    /// proto文件信息
    /// </summary>
    public class ProtoFileInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string filePath;
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string md5Str;
    }

    /// <summary>
    /// proto文件内信息
    /// </summary>
    public class ProtoFileData
    {
        /// <summary>
        /// 所在文件名
        /// </summary>
        public string fileName;
        /// <summary>
        /// 协议名
        /// </summary>
        public string messageName;
        /// <summary>
        /// 协议注释
        /// </summary>
        public string messageNote;
        /// <summary>
        /// 协议内参数类型列表
        /// </summary>
        public List<string> messageType = new List<string>();
        /// <summary>
        /// 协议内参数名列表
        /// </summary>
        public List<string> messageTypeName = new List<string>();

        public Dictionary<string, string> messageTypeNoteDic = new Dictionary<string, string>();

    }
}
