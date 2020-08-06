﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ObjBaseWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ObjBase), typeof(PoolObj));
		L.RegFunction("init", init);
		L.RegFunction("initView", initView);
		L.RegFunction("getDic", getDic);
		L.RegFunction("GetEvent", GetEvent);
		L.RegFunction("GetMovePart", GetMovePart);
		L.RegFunction("onGet", onGet);
		L.RegFunction("onRecycle", onRecycle);
		L.RegFunction("Release", Release);
		L.RegFunction("New", _CreateObjBase);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Target", get_Target, set_Target);
		L.RegVar("radius", get_radius, set_radius);
		L.RegVar("isDestory", get_isDestory, set_isDestory);
		L.RegVar("isDead", get_isDead, set_isDead);
		L.RegVar("gameObject", get_gameObject, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateObjBase(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				ObjBase obj = new ObjBase();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: ObjBase.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
			obj.init();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int initView(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
				obj.initView();
				return 0;
			}
			else if (count == 2)
			{
				ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.initView(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ObjBase.initView");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int getDic(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3)
			{
				ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float o = obj.getDic(arg0, arg1);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else if (count == 4)
			{
				ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
				UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				float o = obj.getDic(arg0, arg1, arg2);
				LuaDLL.lua_pushnumber(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ObjBase.getDic");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEvent(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
			GEventDispatcher o = obj.GetEvent();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMovePart(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
			MovePart o = obj.GetMovePart();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onGet(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
			obj.onGet();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onRecycle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
			obj.onRecycle();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Release(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ObjBase obj = (ObjBase)ToLua.CheckObject<ObjBase>(L, 1);
			obj.Release();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Target(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			ObjBase ret = obj.Target;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Target on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_radius(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			float ret = obj.radius;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index radius on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isDestory(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			bool ret = obj.isDestory;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isDestory on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isDead(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			bool ret = obj.isDead;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isDead on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_gameObject(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			UnityEngine.GameObject ret = obj.gameObject;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index gameObject on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Target(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			ObjBase arg0 = (ObjBase)ToLua.CheckObject<ObjBase>(L, 2);
			obj.Target = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Target on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_radius(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.radius = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index radius on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isDestory(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.isDestory = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isDestory on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isDead(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ObjBase obj = (ObjBase)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.isDead = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isDead on a nil value");
		}
	}
}

