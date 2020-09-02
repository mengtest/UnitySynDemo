﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class GameEnum_KeyInputWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(GameEnum.KeyInput), typeof(System.Object));
		L.RegFunction("New", _CreateGameEnum_KeyInput);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Jump", get_Jump, null);
		L.RegVar("Aim", get_Aim, null);
		L.RegVar("Attack", get_Attack, null);
		L.RegVar("Squat", get_Squat, null);
		L.RegVar("Reload", get_Reload, null);
		L.RegVar("Roll", get_Roll, null);
		L.RegVar("Blink", get_Blink, null);
		L.RegVar("Skill_1", get_Skill_1, null);
		L.RegVar("Skill_2", get_Skill_2, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameEnum_KeyInput(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				GameEnum.KeyInput obj = new GameEnum.KeyInput();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: GameEnum.KeyInput.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Jump(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Jump);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Aim(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Aim);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Attack(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Attack);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Squat(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Squat);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Reload(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Reload);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Roll(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Roll);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Blink(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Blink);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Skill_1(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Skill_1);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Skill_2(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, GameEnum.KeyInput.Skill_2);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

