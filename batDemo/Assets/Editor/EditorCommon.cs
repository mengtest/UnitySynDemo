using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Reflection;
using MiniEditor;

public static class EditorCommon
{
    public static void UploadFile(string localFilePath, string ftpPath, string ftpFileName)
    {
        if (!File.Exists(localFilePath))
        {
            DebugLog.LogError("文件不存在", localFilePath);
        }
        string ftpUrl = "ftp://134.175.81.119:21/";
        FtpClient client = new FtpClient(ftpUrl, "zhifeng", "Aa87894389");
        FileInfo info = new FileInfo(localFilePath);
        if (client.fileUpload(info, ftpPath, ftpFileName))
        {
            DebugLog.Log("上传成功", localFilePath);
        }
        else
        {
            DebugLog.Log("上传失败", localFilePath);
        }
    }

    public static void UploadFolder(string localPath, string remotePath)
    {
        string ftpUrl = "ftp://134.175.81.119:21/";
        FtpClient client = new FtpClient(ftpUrl, "zhifeng", "Aa87894389");
        client.foldersUpload(localPath, ftpUrl + remotePath);
    }

    public static bool OperationConfirm()
    {
        return EditorUtility.DisplayDialog("提示", "操作确认，防止误点", "确定", "取消");
    }

    public static void StartCmd(string cmdpath, string args)
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = cmdpath;
        p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
        p.StartInfo.RedirectStandardInput = false;//接受来自调用程序的输入信息
        p.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
        p.StartInfo.RedirectStandardError = false;//重定向标准错误输出
        p.StartInfo.Arguments = args;
        p.StartInfo.CreateNoWindow = true;//不显示程序窗口
        p.Start();//启动程序
        p.WaitForExit();
    }

    public static Dictionary<string, string> LuaPaths
    {
        get
        {
            var dict = new Dictionary<string, string>() { { AssetBundleConst.ToLuaPath, AssetBundleConst.ToLuaBundle }, { AssetBundleConst.LuaPath, AssetBundleConst.LuaBundle} };
            //var dict = new Dictionary<string, string>() { { "Assets/ScriptsLua", "gamelua" } };
            return dict;
        }
    }

    private static string _AbOutPath = null;
    public static string AbOutPath
    {
        get
        {
            if (_AbOutPath == null)
            {
                _AbOutPath = Application.dataPath.Replace("Assets", "Bundle");
                _AbOutPath = _AbOutPath.Replace("\\", "/");
            }
            return _AbOutPath;
        }
    }

    private static string _GameResPath = null;
    public static string GameResPath
    {
        get
        {
            if (_GameResPath == null)
            {
                _GameResPath = Application.dataPath + "/Resources";
                _GameResPath = _GameResPath.Replace("\\", "/");
            }
            return _GameResPath;
        }
    }

    public static void showTips(string tips)
    {
        EditorUtility.DisplayDialog("提示", tips, "确定");
    }

    public static void DeleteDirectory(string path)
    {
        FileUtil.DeleteDirectory(path);
    }

    public static void CreatPath(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
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
                Debug.LogError("No Directory: " + path);
                return;
            }
        }
        System.Diagnostics.Process.Start("explorer.exe", path);
    }

    // 获取文件夹名称
    public static string GetDirName(string path)
    {
        string res = "";

        string root = Directory.GetDirectoryRoot(path);

        int lastIndex = path.LastIndexOf(root);

        res = path.Substring(lastIndex + 1);

        return res;
    }

    public static string ChangeToSystemPath(string path)
    {
        path = path.Replace("/", "\\");
        return path;
    }

    public static string ChangeToIOPath(string path)
    {
        path = path.Replace("\\", "/");
        return path;
    }

    public static string realPathToAbsPath(string reapath)
    {
        return Application.dataPath.Replace("Assets", "").Replace("\\", "/") + reapath.Replace("\\", "/");
    }

    public static string AbsPathToRealPath(string abspath)
    {
        return abspath.Replace(Application.dataPath.Replace("Assets", ""), "").Replace("\\", "/");
    }

    public static bool IsImage(string path)
    {
        return path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".gif") || path.EndsWith(".ico");
    }

    public static bool IsTexture2D(string path)
    {
        return path.IndexOf("texture2D") >= 0;
    }

    /// <summary>
    /// 根据文件名后缀判断是否是texture文件
    /// </summary>
    public static bool IsTexture(string path)
    {
        return PathEndWithExt(path, EditorConst.TextureExts);
    }

    /// <summary>
    /// 判断某路径是否带有后缀为参数列表中后缀
    /// </summary>
    public static bool PathEndWithExt(string path, string[] ext)
    {
        for (int i = 0; i < ext.Length; ++i)
        {
            if (path.EndsWith(ext[i], System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }



    







    #region system函数，无论获取，还是传入都是system数据

    /// <summary>
    /// 获取路径下的最后一个文件夹
    /// </summary>
    public static string SysGetLastFolderName(string canPath)
    {
        string[] tempList = canPath.Split('/');
        string tempStr = tempList[tempList.Length - 1];
        if (tempStr.IndexOf(".") >= 0)
            tempStr = tempList[tempList.Length - 2];
        return tempStr;
    }

    #endregion

    #region io函数，无论获取，还是传入都是io数据
    /// <summary>
    /// 转换路径格式为Asset的IO路径
    /// </summary>
    public static string FormatAssetPath(string path)
    {
        int index = path.IndexOf("Assets");
        if (index != -1)
        {
            path = path.Substring(index);
        }
        return ChangeToIOPath(path);
    }

    /// <summary>
    /// 获取文件夹下除了.meta文件以外的所有文件
    /// </summary>
    public static List<string> IOGetAllFilesInFolderPath(string path)
    {
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        List<string> temp = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta"))
            {
                continue;
            }
            temp.Add(files[i].Name);
        }
        return temp;
    }

    /// <summary>
    /// 获取文件夹下除了.meta文件以外的所有文件全路径
    /// </summary>
    public static List<string> IOGetAllFilesPathInFolder(string path)
    {
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        List<string> temp = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".meta"))
            {
                continue;
            }
            temp.Add(files[i].FullName);
        }
        return temp;
    }

    /// <summary>
    /// 获取路径下所有文件夹路径
    /// </summary>
    public static List<string> IOGetAllFoldersPathInPath(string canPath)
    {
        string[] tempFolders = Directory.GetDirectories(canPath);
        List<string> temp = new List<string>();
        for (int i = 0; i < tempFolders.Length; i++)
        {
            temp.Add(tempFolders[i]);
        }
        return temp;
    }

    /// <summary>
    /// 获取路径下的最后一个文件夹
    /// </summary>
    public static string IOGetLastFolderName(string canPath)
    {
        string[] tempList = canPath.Split('\\');
        string tempStr = tempList[tempList.Length - 1];
        if (tempStr.IndexOf(".") >= 0)
            tempStr = tempList[tempList.Length - 2];
        return tempStr;
    }
    #endregion



    #region 有关创建，编辑器模式下，使用的资源
    /// <summary>
    /// 创建过的texture字典
    /// </summary>
    private static Dictionary<Color, Texture2D> m_ColorTextureDic = new Dictionary<Color, Texture2D>();
    /// <summary>
    /// 创建一个新的texture
    /// </summary>
    public static Texture2D GetColorTexture(Color c)
    {
        Texture2D tempTexture = null;
        m_ColorTextureDic.TryGetValue(c, out tempTexture);
        if (tempTexture == null) //Texture2D对象在游戏结束时为null
        {
            tempTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tempTexture.SetPixel(0, 0, c);
            tempTexture.Apply();

            m_ColorTextureDic[c] = tempTexture;
        }
        return tempTexture;
    }


    #endregion


    /// <summary>
    /// 将object列表转换成T对象列表
    /// </summary>
    public static List<object> ToObjectList<T>(List<T> data)
    {
        if (data == null) return null;
        List<object> ret = new List<object>();
        for (int i = 0; i < data.Count; ++i)
        {
            ret.Add(data[i]);
        }
        return ret;
    }

    /// <summary>
    /// 获取当前平台
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentBuildPlatform()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            return EditorConst.PlatformAndroid;
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            return EditorConst.PlatformIos;
        }
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
        {
            return EditorConst.PlatformWindows;
        }
        else
        {
            return EditorConst.PlatformAndroid;
        }
    }

    /// <summary>
    /// 根据图片类型，获取一个像素点的字节数
    /// </summary>
    public static int GetBitsPerPixel(TextureImporterFormat format)
    {
        switch (format)
        {
            case TextureImporterFormat.Alpha8: //	 Alpha-only texture format.
                return 8;
            case TextureImporterFormat.RGB24: // A color texture format.
                return 24;
            case TextureImporterFormat.RGBA32: //Color with an alpha channel texture format.
                return 32;
            case TextureImporterFormat.ARGB32: //Color with an alpha channel texture format.
                return 32;
            case TextureImporterFormat.DXT1: // Compressed color texture format.
                return 4;
            case TextureImporterFormat.DXT5: // Compressed color with alpha channel texture format.
                return 8;
            case TextureImporterFormat.PVRTC_RGB2: //	 PowerVR (iOS) 2 bits/pixel compressed color texture format.
                return 2;
            case TextureImporterFormat.PVRTC_RGBA2: //	 PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format
                return 2;
            case TextureImporterFormat.PVRTC_RGB4: //	 PowerVR (iOS) 4 bits/pixel compressed color texture format.
                return 4;
            case TextureImporterFormat.PVRTC_RGBA4: //	 PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format
                return 4;
            case TextureImporterFormat.ETC_RGB4: //	 ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
                return 4;
            case TextureImporterFormat.ETC2_RGB4:
                return 4;
            case TextureImporterFormat.ETC2_RGBA8:
                return 8;
#pragma warning disable 0618
            case TextureImporterFormat.AutomaticCompressed:
                return 4;
            case TextureImporterFormat.AutomaticTruecolor:
                return 32;
            default:
                return 32;
#pragma warning restore 0618
        }
    }

    /// <summary>
    /// 根据传入的textur以及texture的材质，计算出贴图内存大小
    /// </summary>
    public static int CalculateTextureSizeBytes(Texture tTexture, TextureImporterFormat format)
    {
        var tWidth = tTexture.width;
        var tHeight = tTexture.height;
        if (tTexture is Texture2D)
        {
            var tTex2D = tTexture as Texture2D;
            var bitsPerPixel = GetBitsPerPixel(format);
            var mipMapCount = tTex2D.mipmapCount;
            var mipLevel = 1;
            var tSize = 0;
            while (mipLevel <= mipMapCount)
            {
                tSize += tWidth * tHeight * bitsPerPixel / 8;
                tWidth = tWidth / 2;
                tHeight = tHeight / 2;
                mipLevel++;
            }
            return tSize;
        }

        if (tTexture is Cubemap)
        {
            var bitsPerPixel = GetBitsPerPixel(format);
            return tWidth * tHeight * 6 * bitsPerPixel / 8;
        }
        return 0;
    }

    /// <summary>
    /// 根据传入的textur路径
    /// </summary>
    public static int CalculateTextureSizeBytes(string path)
    {
        TextureImporter tImport = AssetImporter.GetAtPath(path) as TextureImporter;
        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(path);
        if (tImport == null || texture == null) return 0;

        TextureImporterPlatformSettings setting = tImport.GetPlatformTextureSettings(GetCurrentBuildPlatform());

        int retSize = 0;
        if (!setting.overridden)
        {
#pragma warning disable 0618
            retSize = CalculateTextureSizeBytes(texture, tImport.textureFormat);
#pragma warning restore 0618
        }
        else
        {
            retSize = CalculateTextureSizeBytes(texture, setting.format);
        }

        Resources.UnloadAsset(texture);

        return retSize;
    }
    
    /// <summary>
    /// 根据字段属性，将属性内容转换成数值
    /// </summary>
    public static object MemberValue(object obj, MemberInfo memInfo)
    {
        if (obj == null)
            return "";
        if (memInfo == null)
            return "";

        var pi = memInfo as PropertyInfo;
        if (pi != null)
        {
            return pi.GetValue(obj, null);
        }

        var fi = memInfo as FieldInfo;
        if (fi != null)
        {
            return fi.GetValue(obj);
        }

        return "";
    }

    /// <summary>
    /// 根据字段属性，将属性内容转换成字符串
    /// </summary>
    public static string MemberToString(object obj, MemberInfo memInfo, string fmt)
    {
        object val = MemberValue(obj, memInfo);
        if (val == null)
            return "";

        if (fmt == EditorConst.BytesFormatter)
            return EditorUtility.FormatBytes((int)val);
        if (val is float)
            return ((float)val).ToString(fmt);
        if (val is double)
            return ((double)val).ToString(fmt);
        return val.ToString();
    }
}