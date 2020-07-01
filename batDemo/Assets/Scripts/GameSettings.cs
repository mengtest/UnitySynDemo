using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    static GameSettings mInstance;
    public static GameSettings Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = Resources.Load<GameSettings>("GameSettings");
                if(mInstance == null)
                {
                    mInstance = ScriptableObject.CreateInstance<GameSettings>();
                    DebugLog.LogWarning("No GameSettings.");
                }
                mInstance.init();
            }
            return mInstance;
        }
    }

    public readonly static string[] TextEffectType = new string[]
    {
        "None",         //无文字效果
        "ButtonGreen",  //绿色按钮上文字的投影效果
        "ButtonYellow", //黄色按钮上文字的投影效果
        "ButtonRed",    //红色按钮上文字的投影效果
        "ButtonBlue",   //蓝色按钮上文字的投影效果
        "ShadowWhite",  //白色投影
        "ShadowBrown",  //棕色投影
        "OutlineWhite", //白色描边
        "OutlineBrown",  //棕色描边
        "CustomOutline", //自定义描边
        "CustomShadow", //自定义投影
    };



    //是否不加载assetBundle. 编辑可以不加载. 不加载AB 就是load prefab
    public  bool useAssetBundle = true;

    public bool localLua =false;

    public string currentLanguage=SupportedLanguages.Chinese;
    
    // 是否测试模式
    public bool isDebugModel = false;

    //是否 加载远程资源.
    public bool isLoadRemoteAsset = true;

     //游戏更新配置 服务器列表Url mainfest md5 Remotofest md5  ver  
    public string _gameConfigUrl = "";

    public  string gameConfigUrl
    {
        get { return _gameConfigUrl ; }
        set {   
            _gameConfigUrl=value ;  
#if UNITY_EDITOR
          UnityEditor.EditorPrefs.SetString("gameConfigUrl", value);
#endif
        }
    }

    public bool isDebug()
    {
        return isDebugModel;
    }

    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 初始化 游戏设置
     * @param {type} 
     * @return: 
     */
    private void init(){
#if UNITY_EDITOR
       this.useAssetBundle = UnityEditor.EditorPrefs.GetBool("useAssetBundle", false);
       this.isDebugModel = UnityEditor.EditorPrefs.GetBool("isDebugModel", false);
       this.currentLanguage = UnityEditor.EditorPrefs.GetString("currentLanguage", SupportedLanguages.Chinese);
       this.isLoadRemoteAsset = UnityEditor.EditorPrefs.GetBool("isLoadRemoteAsset", false);
        this._gameConfigUrl = UnityEditor.EditorPrefs.GetString("gameConfigUrl", "http://192.168.0.3:8080/gameconfig.xml");
        this.localLua = UnityEditor.EditorPrefs.GetBool("isReadLocalLua", true);
#else
       this.currentLanguage = SupportedLanguages.GetCurrentLanguage();
       //非编辑器加载AssetBundle
       this.useAssetBundle = true;
       this.isLoadRemoteAsset=true;
       this.localLua =false;
#endif
    }
}
