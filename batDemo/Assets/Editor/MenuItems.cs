using System.IO;
using UnityEditor;
using UnityEngine;

public  class MenuItems
{

    const string useAssetBundles = "AssetBundle/UseAssetBundle 加载U3d";
    [MenuItem(useAssetBundles,true,100)]  
    public static bool set_AssetBundles()  
    {    
         //设置模块
         Menu.SetChecked(useAssetBundles, GameSettings.Instance.useAssetBundle);
        return true;  
    }  
    [MenuItem(useAssetBundles)]  
    public static void do_useAssetBundles()  
    { 
        GameSettings.Instance.useAssetBundle = !GameSettings.Instance.useAssetBundle;
        UnityEditor.EditorPrefs.SetBool("useAssetBundle", GameSettings.Instance.useAssetBundle);
    }  


    const string isLoadRemoteAsset = "AssetBundle/isLoadRemoteAsset 远程资源";
    [MenuItem(isLoadRemoteAsset,true,100)]  
    public static bool set_isLoadRemoteAsset()  
    {    
         //设置模块
         Menu.SetChecked(isLoadRemoteAsset, GameSettings.Instance.isLoadRemoteAsset);
        return true;  
    }  
    [MenuItem(isLoadRemoteAsset)]  
    public static void do_isLoadRemoteAsset()  
    { 
        GameSettings.Instance.isLoadRemoteAsset = !GameSettings.Instance.isLoadRemoteAsset;
        UnityEditor.EditorPrefs.SetBool("isLoadRemoteAsset", GameSettings.Instance.isLoadRemoteAsset);
    }  

    const string isLocalLua = "AssetBundle/isLocalLua 本地lua加载";
    [MenuItem(isLocalLua,true,100)]  
    public static bool set_isLocalLua()  
    {    
         //设置模块
         Menu.SetChecked(isLocalLua, GameSettings.Instance.localLua);
        return true;  
    }  
    [MenuItem(isLocalLua)]  
    public static void do_isLocalLua()  
    { 
        GameSettings.Instance.localLua = !GameSettings.Instance.localLua;
        UnityEditor.EditorPrefs.SetBool("isReadLocalLua", GameSettings.Instance.localLua);
    }  



    [MenuItem("Languages/"+SupportedLanguages.Chinese,true,200)]  
    public static bool set_Languages()  
    {    
         //设置模块
        Menu.SetChecked("Languages/"+SupportedLanguages.Chinese, GameSettings.Instance.currentLanguage == SupportedLanguages.Chinese);
        Menu.SetChecked("Languages/"+SupportedLanguages.English, GameSettings.Instance.currentLanguage == SupportedLanguages.English);
        return true;  
    }

    [MenuItem("Languages/"+SupportedLanguages.Chinese)]  
    public static void SwitchToChinese()  
    { 
        GameSettings.Instance.currentLanguage = SupportedLanguages.Chinese;
        UnityEditor.EditorPrefs.SetString("currentLanguage",  GameSettings.Instance.currentLanguage);
    }  
    [MenuItem("Languages/"+SupportedLanguages.English)]  
    public static void SwitchToEnglish()  
    { 
        GameSettings.Instance.currentLanguage = SupportedLanguages.English;
        UnityEditor.EditorPrefs.SetString("currentLanguage",  GameSettings.Instance.currentLanguage);
    }  
//********************************PlayeMode************

    [MenuItem("PlayeMode/"+GameEnum.PlayMode.SingleMode,true,200)]  
    public static bool set_PlayeMode()  
    {    
         //设置模块
        Menu.SetChecked("PlayeMode/"+GameEnum.PlayMode.SingleMode, GameSettings.Instance.playMode == GameEnum.PlayMode.SingleMode);
        Menu.SetChecked("PlayeMode/"+GameEnum.PlayMode.MultiplayerMode, GameSettings.Instance.playMode == GameEnum.PlayMode.MultiplayerMode);
        Menu.SetChecked("PlayeMode/"+GameEnum.PlayMode.ReplayMode, GameSettings.Instance.playMode ==GameEnum.PlayMode.ReplayMode);
        return true;  
    }

    [MenuItem("PlayeMode/"+GameEnum.PlayMode.SingleMode)]  
    public static void SwitchToSingleMode()  
    { 
        GameSettings.Instance.playMode = GameEnum.PlayMode.SingleMode;
        UnityEditor.EditorPrefs.SetString("playMode",  GameSettings.Instance.playMode);
    }  
    [MenuItem("PlayeMode/"+GameEnum.PlayMode.MultiplayerMode)]  
    public static void SwitchToMultiplayerMode()  
    { 
        GameSettings.Instance.playMode = GameEnum.PlayMode.MultiplayerMode;
        UnityEditor.EditorPrefs.SetString("playMode",  GameSettings.Instance.playMode);
    }  
     [MenuItem("PlayeMode/"+GameEnum.PlayMode.ReplayMode)]  
    public static void SwitchToReplayMode()  
    { 
        GameSettings.Instance.playMode = GameEnum.PlayMode.ReplayMode;
        UnityEditor.EditorPrefs.SetString("playMode",  GameSettings.Instance.playMode);
    }  

//***********************************************character*****************************************************************************//

    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 各个部件做分离打包 带动作的rm 文件  (时装部件)模型和贴图和骨骼数据的 rs文件 
             单独掉落的装备 武器 就是这样打包 角色全分离.                    用于主角
     * @param {type} 
     * @return: 
     */
     [MenuItem("Avatar/时装avatar分离-重命名", false, 100)]
    static void PackageAvatarPartReName()
    {
        string allPath = "";
        string DirName = "";
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            DirName = o.name;
            allPath = AssetDatabase.GetAssetPath(o);
            break;
        }
        CharacterBuild.CreateMaterials(allPath,DirName);
    }
    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 各个部件做分离打包 带动作的rm 文件  (时装部件)模型和贴图和骨骼数据的 rs文件 
             单独掉落的装备 武器 就是这样打包 角色全分离.                    用于主角
     * @param {type} 
     * @return: 
     */
     [MenuItem("Avatar/时装avatar分离-每个部件独立分离", false, 101)]
    static void PackageAvatarPart()
    {
        string allPath = "";
        string DirName = "";
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            DirName = o.name;
            allPath = AssetDatabase.GetAssetPath(o);
            break;
        }
        CharacterBuild.PackageCharReName(allPath,DirName,packCharType.partType);
    }
    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 各个部件做分离打包 带动作的rm 文件  (时装部件)模型和贴图和骨骼数据的 rs文件 
             body_01 wepon_01 arm_01  成为 一个set 一套时装  多套时装 打成一个包. 多用于Boss 怪物 多个不同形态,
     * @param {type} 
     * @return: 
     */
    [MenuItem("Avatar/单个文件-重命名", false, 200)]
    static void PackageAvatarOneSetRename()
    {
         string allPath = "";
        string DirName = "";
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            DirName = o.name;
            allPath = AssetDatabase.GetAssetPath(o);
            break;
        }
         CharacterBuild.CreateMaterials(allPath,DirName);
        DebugLog.Log(allPath);
        DebugLog.Log(DirName);
    }
    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 各个部件做分离打包 带动作的rm 文件  (时装部件)模型和贴图和骨骼数据的 rs文件 
             body_01 wepon_01 arm_01  成为 一个set 一套时装  多套时装 打成一个包. 多用于Boss 怪物 多个不同形态,
     * @param {type} 
     * @return: 
     */
    [MenuItem("Avatar/单个文件-多套时装一个包", false, 201)]
    static void PackageAvatarOneSet()
    {
         string allPath = "";
        string DirName = "";
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            DirName = o.name;
            allPath = AssetDatabase.GetAssetPath(o);
            break;
        }
        CharacterBuild.PackageCharReName(allPath,DirName,packCharType.allSetType);
        DebugLog.Log(allPath);
        DebugLog.Log(DirName);
    }

    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 各个部件做分离打包 带动作的rm 文件  (时装部件)模型和贴图和骨骼数据的 rs文件 
             body_01 wepon_01 arm_01  成为 一个set 一套时装  多套时装 打成一个包. 多用于Boss 怪物 多个不同形态,
     * @param {type} 
     * @return: 
     */
    [MenuItem("批量/临时打包monster character building 多套时装一个包", false, 200)]
    static void L_PackageAvatarMulSetSelected()
    {
        string[] fileArr = new string[1] {"Building" };
       // string[] fileArr = new string[4] { "Building", "Character", "CityBuilding", "Monster" };
        string basePath = "Assets/Res/";
         for (int i = 0; i < fileArr.Length; i++)
        {
            string sourcePath = basePath + fileArr[i];
            DirectoryInfo[] di = new DirectoryInfo(sourcePath).GetDirectories();
            for (int k = 0; k < di.Length; k++)
            {
                string allPath =sourcePath+"/"+di[k].Name;
                string DirName = di[k].Name;
                DebugLog.Log(DirName);
                DebugLog.Log(allPath);
       //         CharacterBuild.CreateMaterials(allPath,DirName);
       //         CharacterBuild.PackageCharReName(allPath,DirName,packCharType.allSetType);
            }
        }
     //    CharacterBuild.CreateMaterials();
   //     CharacterBuild.PackageCharReName(packCharType.allSetType);
    }

//***********************************************app Build*****************************************************************************//
  /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 
     * @return: 
     */
    [MenuItem("打包/一键清除依赖和打包资源", false, 100)]
    static void ClearAssetBunldTag()
    {
        AppBuild.removeDependencies();
    }
     /**
     * @name: xsddxr909
     * @test: 
     * @msg:
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/一键 资源 打包", false, 101)]
    static void DependenciesBuildAsset()
    {
          AppBuild.DependenciesBuild(false,false);
    }
      /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/一键 lua+资源 打包", false, 101)]
    static void DependenciesBuild()
    {
        AppBuild.DependenciesBuild(false,false);
        AppBuild.onlyLua(false,false);
    }
       /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/一键依赖打包(全包)-copyStreamAssets", false, 102)]
    static void DependenciesBuildCopy()
    {
          AppBuild.DependenciesBuild(true,false);
          AppBuild.onlyLua(true,false);
    }
    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/一键依赖打包(外网包)-copyWeb", false, 103)]
    static void DependenciesBuildCopyWeb()
    {
         AppBuild.DependenciesBuild(false,true);
         AppBuild.onlyLua(false,true);
    }
        /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/Lua(外网包)-copyWeb", false, 202)]
    static void luaBuildCopyWeb()
    {
        AppBuild.onlyLua(false,true);
    }
        /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/Lua(打包)-copyStreamAssets", false, 203)]
    static void luaBuildcopyStreamAssets()
    {
        AppBuild.onlyLua(false,true);
    }
     /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/Lua(打包)", false, 201)]
    static void luaBuild()
    {
          AppBuild.onlyLua(false,true);
    }
    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/拷贝到StreamAssets", false, 401)]
    static void CopyStreamAssets()
    {
          AppBuild.copyFiles(true,false);
    }
        /**
     * @name: xsddxr909
     * @test: 
     * @msg: 一键清除所有依赖
     * @param {type} 1
     * @return: 
     */
    [MenuItem("打包/拷贝到Web", false, 402)]
    static void CopyWeb()
    {
          AppBuild.copyFiles(false,true);
    }
}
