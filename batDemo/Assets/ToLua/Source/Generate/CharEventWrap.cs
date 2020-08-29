﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class CharEventWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(CharEvent), typeof(System.Object));
		L.RegFunction("New", _CreateCharEvent);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("MOVE_END", get_MOVE_END, set_MOVE_END);
		L.RegVar("Jump_To_Ground", get_Jump_To_Ground, set_Jump_To_Ground);
		L.RegVar("Jump_Fall", get_Jump_Fall, set_Jump_Fall);
		L.RegVar("Jump_Rise", get_Jump_Rise, set_Jump_Rise);
		L.RegVar("Syn_NormalState", get_Syn_NormalState, set_Syn_NormalState);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCharEvent(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				CharEvent obj = new CharEvent();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: CharEvent.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MOVE_END(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, CharEvent.MOVE_END);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Jump_To_Ground(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, CharEvent.Jump_To_Ground);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Jump_Fall(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, CharEvent.Jump_Fall);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Jump_Rise(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, CharEvent.Jump_Rise);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Syn_NormalState(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, CharEvent.Syn_NormalState);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MOVE_END(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			CharEvent.MOVE_END = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Jump_To_Ground(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			CharEvent.Jump_To_Ground = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Jump_Fall(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			CharEvent.Jump_Fall = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Jump_Rise(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			CharEvent.Jump_Rise = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Syn_NormalState(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			CharEvent.Syn_NormalState = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

