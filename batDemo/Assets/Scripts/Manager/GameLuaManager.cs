//游戏调用加载 其他等主方法口  提供lua调用. 

using System;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AutoRegistLua]
public class GameLuaManager
{
    //单个加载
    public static  GameAssetRequest LoadAsset(string path, Type assetType, LuaFunction callback)
    {
           return GameAssetManager.Instance.LoadAssetLua(path,assetType,callback);
    }
    //多个加载
    public static  GameAssetRequest LoadAsset(string[] path, Type assetType, LuaFunction callback)
    {
           return GameAssetManager.Instance.LoadAssetLua(path,assetType,callback);
    }
    	//设置log 权限.
	public static void SetLogSwitcher(bool isOpenLog, bool isOpenError, bool isOpenWarning)
	{
	 	 DebugLog.setLogSwitcher(isOpenLog, isOpenError, isOpenWarning);
	}
    //编辑器 重新赋值shader;
    public static void RefreshShader(ref GameObject obj){
        if (GameSettings.Instance.useAssetBundle)
        {
         //   DebugLog.Log("use render",obj);
            RenderHelper.RefreshShader(ref obj);
        }
    }
    public static Vector3 ScreenPointToWorldPointInRectangle(RectTransform rectT,PointerEventData data){
        Vector3 mousePos;
         if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectT, data.position, data.pressEventCamera, out mousePos))
        {
           return mousePos;
        }
        return mousePos;
    }

     public static Character CreatCharacter(string path="",GameObject obj=null){
        return CharManager.Instance.CreatCharacter(path,obj);
     }

#if UNITY_EDITOR
    public static string GetProtoBytesPath()
    {
       //persistentDataPath = Application.dataPath + "/ScriptsLua/PB";
        string urle = Application.dataPath + "/ScriptsLua/PB/md.bytes";
        return urle;
    }
#else
    public static string GetProtoBytesPath()
    {
        string url= PatchManager.Instance.GetSignedFileLocalURL("md.bytes",false);
         return url;
    }
#endif


    #region 提供lua ScrollView使用
    /// <summary>
    /// 添加拖拽监听
    /// </summary>
    public static void AddScrollViewOnValueChangeListener(ScrollRect canRect, LuaFunction canFunc)
    {
        canRect.onValueChanged.AddListener((canVector2) =>
        {
            canFunc.Call(canVector2);
        });
    }
    #endregion
}