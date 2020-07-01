﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class GameAssetRequestWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(GameAssetRequest), typeof(System.Object));
		L.RegFunction("OnLoadComplete", OnLoadComplete);
		L.RegFunction("IsLoadComplete", IsLoadComplete);
		L.RegFunction("Unload", Unload);
		L.RegFunction("GetAsset", GetAsset);
		L.RegFunction("GetAssetBundleInfo", GetAssetBundleInfo);
		L.RegFunction("New", _CreateGameAssetRequest);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameAssetRequest(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				System.Collections.Generic.List<LoadAssetRequest> arg0 = (System.Collections.Generic.List<LoadAssetRequest>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.List<LoadAssetRequest>));
				GameAssetRequest obj = new GameAssetRequest(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 2)
			{
				System.Collections.Generic.List<LoadAssetRequest> arg0 = (System.Collections.Generic.List<LoadAssetRequest>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.List<LoadAssetRequest>));
				System.Action<UnityEngine.Object[]> arg1 = (System.Action<UnityEngine.Object[]>)ToLua.CheckDelegate<System.Action<UnityEngine.Object[]>>(L, 2);
				GameAssetRequest obj = new GameAssetRequest(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 3)
			{
				System.Collections.Generic.List<LoadAssetRequest> arg0 = (System.Collections.Generic.List<LoadAssetRequest>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.List<LoadAssetRequest>));
				System.Action<UnityEngine.Object[]> arg1 = (System.Action<UnityEngine.Object[]>)ToLua.CheckDelegate<System.Action<UnityEngine.Object[]>>(L, 2);
				LuaFunction arg2 = ToLua.CheckLuaFunction(L, 3);
				GameAssetRequest obj = new GameAssetRequest(arg0, arg1, arg2);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: GameAssetRequest.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnLoadComplete(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
			obj.OnLoadComplete();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsLoadComplete(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
			bool o = obj.IsLoadComplete();
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Unload(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
			obj.Unload();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAsset(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
				UnityEngine.Object o = obj.GetAsset();
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2)
			{
				GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				UnityEngine.Object o = obj.GetAsset(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: GameAssetRequest.GetAsset");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAssetBundleInfo(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
				AssetBundleInfo o = obj.GetAssetBundleInfo();
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2)
			{
				GameAssetRequest obj = (GameAssetRequest)ToLua.CheckObject<GameAssetRequest>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				AssetBundleInfo o = obj.GetAssetBundleInfo(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: GameAssetRequest.GetAssetBundleInfo");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

