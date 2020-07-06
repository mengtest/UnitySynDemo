//游戏调用加载 其他等主方法口  提供lua调用. 

using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

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
}