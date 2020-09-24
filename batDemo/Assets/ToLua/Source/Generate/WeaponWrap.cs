﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class WeaponWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(Weapon), typeof(Item));
		L.RegFunction("initData", initData);
		L.RegFunction("EquipWeaponRightHand", EquipWeaponRightHand);
		L.RegFunction("EquipWeaponBackChest", EquipWeaponBackChest);
		L.RegFunction("Fire", Fire);
		L.RegFunction("onViewLoadFin", onViewLoadFin);
		L.RegFunction("onGet", onGet);
		L.RegFunction("onRecycle", onRecycle);
		L.RegFunction("onRelease", onRelease);
		L.RegFunction("New", _CreateWeapon);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateWeapon(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				Weapon obj = new Weapon();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: Weapon.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int initData(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
			obj.initData();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EquipWeaponRightHand(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
			Player arg0 = (Player)ToLua.CheckObject<Player>(L, 2);
			obj.EquipWeaponRightHand(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EquipWeaponBackChest(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
			Player arg0 = (Player)ToLua.CheckObject<Player>(L, 2);
			obj.EquipWeaponBackChest(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Fire(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
			obj.Fire();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onViewLoadFin(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
			obj.onViewLoadFin();
			return 0;
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
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
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
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
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
			Weapon obj = (Weapon)ToLua.CheckObject<Weapon>(L, 1);
			obj.onRelease();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

