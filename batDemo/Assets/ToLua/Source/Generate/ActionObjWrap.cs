﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ActionObjWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ActionObj), typeof(ObjBase));
		L.RegFunction("doActionSkillByLabel", doActionSkillByLabel);
		L.RegFunction("clearAction", clearAction);
		L.RegFunction("onRecycle", onRecycle);
		L.RegFunction("onRelease", onRelease);
		L.RegFunction("New", _CreateActionObj);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("currentBaseAction", get_currentBaseAction, set_currentBaseAction);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateActionObj(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				ActionObj obj = new ActionObj();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: ActionObj.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int doActionSkillByLabel(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				bool o = obj.doActionSkillByLabel(arg0);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 3)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool o = obj.doActionSkillByLabel(arg0, arg1);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 4)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				bool o = obj.doActionSkillByLabel(arg0, arg1, arg2);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 5)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				object[] arg3 = ToLua.CheckObjectArray(L, 5);
				bool o = obj.doActionSkillByLabel(arg0, arg1, arg2, arg3);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 6)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				object[] arg3 = ToLua.CheckObjectArray(L, 5);
				int arg4 = (int)LuaDLL.luaL_checknumber(L, 6);
				bool o = obj.doActionSkillByLabel(arg0, arg1, arg2, arg3, arg4);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ActionObj.doActionSkillByLabel");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int clearAction(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				obj.clearAction();
				return 0;
			}
			else if (count == 2)
			{
				ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				obj.clearAction(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ActionObj.clearAction");
			}
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
			ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
			obj.onRecycle();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onRelease(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ActionObj obj = (ActionObj)ToLua.CheckObject<ActionObj>(L, 1);
			obj.onRelease();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_currentBaseAction(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ActionObj obj = (ActionObj)o;
			ActionBase ret = obj.currentBaseAction;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index currentBaseAction on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_currentBaseAction(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ActionObj obj = (ActionObj)o;
			ActionBase arg0 = (ActionBase)ToLua.CheckObject<ActionBase>(L, 2);
			obj.currentBaseAction = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index currentBaseAction on a nil value");
		}
	}
}

