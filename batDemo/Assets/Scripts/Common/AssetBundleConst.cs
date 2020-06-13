using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetBundleConst
{

 #if UNITY_IPHONE || UNITY_IOS
    public static int platformID = 1;
    public static string platform = "iOS";
#elif UNITY_ANDROID
    public static int platformID = 0;
    public static string platform = "Android";
#elif UNITY_STANDALONE_WIN
    public static int platformID = 2;
    public static string platform = "Win";
#elif UNITY_WEBGL
    public static int platformID = 3;
    public static string platform = "WebGL";
#endif

    //AssetBundle生成文件夹
    public static string AssetBundleFolder{
        get{ return "res_"+platform.ToLower() ; }
        set{  }
    }

    public static string AssetBundleFolderSigned{
        get{ return "res_"+platform.ToLower()+"signed" ; }
        set{  }
    }
    //AssetBundle上传目录
    public const string AssetBundleUploadFolder = "upload";
    //AssetBundle下载临时目录
    public const string AssetBundleTempFolder = "assetbundlestemp";
    //AssetBundle配置文件
    public const string AssetBundleFiles = "assetbundlefiles.txt";
    //记录当前打包的时间戳，作为配置文件的签名
    public const string AssetBundleBuildInfo = "assetbundlebuildinfo.txt";
    //Manifest文件名
    public const string BundleManifestName = "assetbundles";
    //Lua Manifest文件名
    public const string LuaBundleManifestName = "lua_assetbundles";
    //生成Lua Bundle时需要为lua添加bytes后缀
    public const string LuaFileExtension = ".bytes";
    //Lua Bundle前缀
    public const string LuaBundlePrefix = "lua_";
    //ToLua Bundle名
    public const string ToLuaBundleName = "tolua";
    //生成Lua Bundle的临时文件夹
    public const string LuaTempFolder = "LuaTemp";
    //Lua脚本目录
    public const string LuaPath = "Assets/Scripts/Lua/";
    //Lua编辑器调试脚本，打包时忽略
    public const string LuaEditorDebugFile = "Assets/Scripts/Lua/EditorDebug.lua";
    //ToLua脚本目录
    public const string ToLuaPath = "Assets/ToLua/Lua/";
    //游戏资源目录
    public const string EditorResPath="Assets/Res/";
   
}
