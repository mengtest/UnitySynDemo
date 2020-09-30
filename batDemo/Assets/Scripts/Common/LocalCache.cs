using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LuaInterface;
using System.Diagnostics;
using System.Collections;
//using SimpleJSON;

struct Pack
{
	public LuaTable table;
}
[AutoRegistLua]
public class LocalCache
{
	/// <summary>
	/// 通过键存储对应的布尔值
	/// </summary>
	public static void SetBool(string key, bool value)
	{
        key = dealKey(key);
        if (value)
			SetInt(key, 1);
		else
			SetInt(key, 0);
	}

	/// <summary>
	/// 通过对应的键获取布尔值
	/// </summary>
	public static bool GetBool(string key,bool defaultValue=false)
	{
        key = dealKey(key);
        int def=defaultValue==true?1:0;
        int boolValue = GetInt(key,def);
		if (boolValue == 1)
			return true;
		return false;
	}

	/// <summary>
	/// 通过键存储对应的整形数据
	/// </summary>
	public static void SetInt(string key, int value)
	{
        key = dealKey(key);
        if (key != null)
		{
			PlayerPrefs.SetInt(key, value);
		}
	}

	/// <summary>
	/// 通过键获取对应的整形数据
	/// </summary>
	public static int GetInt(string key,int defaultValue=0)
	{
        key = dealKey(key);
        if (PlayerPrefs.HasKey(key))
			return PlayerPrefs.GetInt(key);
		return defaultValue; 
	}

	/// <summary>
	/// 通过键存储对应的浮点数据
	/// </summary>
	public static void SetFloat(string key, float value)
	{
        key = dealKey(key);
        if (key != null)
		{
			PlayerPrefs.SetFloat(key, value);
		}
	}

	/// <summary>
	/// 通过键获取对应的浮点数据
	/// </summary>
	/// <param name="key"></param>
	/// <returns>若返回值为-9999意为找不到键值对 </returns>
	public static float GetFloat(string key,float defaultValue=0)
	{
        key = dealKey(key);
        if (PlayerPrefs.HasKey(key))
			return PlayerPrefs.GetFloat(key);
		return defaultValue;
	}

	/// <summary>
	/// 通过键存储字符串数据
	/// </summary>
	public static void SetString(string key, string value)
	{
        key = dealKey(key);
        if (key != null)
		{
			PlayerPrefs.SetString(key, value);
		}
	}

	/// <summary>
	/// 通过键获取对应的字符串数据
	/// </summary>
	public static string GetString(string key,string defaultValue="")
	{
        key = dealKey(key);
        if (PlayerPrefs.HasKey(key))
			return PlayerPrefs.GetString(key);
		return defaultValue;
	}


	/// <summary>
	/// 通过键存储对应的基类型数据
	/// </summary>
	public static void SetToJson(string key, LuaTable table)
	{
        key = dealKey(key);
        if (key != null && table != null)
		{
			Pack pack = new Pack();
			pack.table = table;
			string json = JsonUtility.ToJson(pack, true);
			PlayerPrefs.SetString(key, json);
			DebugLog.Log("设置json成功");
		}
	}

	/// <summary>
	/// 通过键值获取基类型
	/// </summary>
	public static object GetFromJson(string key)
	{
        key = dealKey(key);
        if (PlayerPrefs.HasKey(key))
		{
			string json = PlayerPrefs.GetString(key);
			Pack pack = JsonUtility.FromJson<Pack>(json);
			LuaTable table = pack.table;
			return table;
		}
		return null;
	}
	/// <summary>
	/// 清除所有缓存数据
	/// </summary>
	public static void Clear()
	{
		PlayerPrefs.DeleteAll();
	}

	/// <summary>
	/// 清除指定的键值对数据
	/// </summary>
	public static void RemovekeyValue(string key)
	{
        key = dealKey(key);
        if (PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.DeleteKey(key);	
		}
	}

    static string dealKey(string key)
    {
#if UNITY_EDITOR
        return Application.dataPath + key;
#else
        return key;
#endif
    }


    //测试：在c#进行序列化，反序列化json快	 还是在lua层
    //时间：
    //lua层：   lua序列化/反序列化时间+lua传字符串给c#时间
    //c#层：    lua传表给c# + c#序列化/反序列化时间
    /*
	测试1： 


	 */
}

