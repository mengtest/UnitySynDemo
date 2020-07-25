﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_UI_Dropdown_OptionDataWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.UI.Dropdown.OptionData), typeof(System.Object));
		L.RegFunction("New", _CreateUnityEngine_UI_Dropdown_OptionData);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("text", get_text, set_text);
		L.RegVar("image", get_image, set_image);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_UI_Dropdown_OptionData(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UnityEngine.UI.Dropdown.OptionData obj = new UnityEngine.UI.Dropdown.OptionData();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				UnityEngine.UI.Dropdown.OptionData obj = new UnityEngine.UI.Dropdown.OptionData(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes<UnityEngine.Sprite>(L, 1))
			{
				UnityEngine.Sprite arg0 = (UnityEngine.Sprite)ToLua.ToObject(L, 1);
				UnityEngine.UI.Dropdown.OptionData obj = new UnityEngine.UI.Dropdown.OptionData(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.Sprite arg1 = (UnityEngine.Sprite)ToLua.CheckObject(L, 2, typeof(UnityEngine.Sprite));
				UnityEngine.UI.Dropdown.OptionData obj = new UnityEngine.UI.Dropdown.OptionData(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UnityEngine.UI.Dropdown.OptionData.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_text(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.UI.Dropdown.OptionData obj = (UnityEngine.UI.Dropdown.OptionData)o;
			string ret = obj.text;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index text on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_image(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.UI.Dropdown.OptionData obj = (UnityEngine.UI.Dropdown.OptionData)o;
			UnityEngine.Sprite ret = obj.image;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index image on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_text(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.UI.Dropdown.OptionData obj = (UnityEngine.UI.Dropdown.OptionData)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.text = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index text on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_image(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UnityEngine.UI.Dropdown.OptionData obj = (UnityEngine.UI.Dropdown.OptionData)o;
			UnityEngine.Sprite arg0 = (UnityEngine.Sprite)ToLua.CheckObject(L, 2, typeof(UnityEngine.Sprite));
			obj.image = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index image on a nil value");
		}
	}
}

