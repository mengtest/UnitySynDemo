﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class GameEnum_CtrlTypeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(GameEnum.CtrlType));
		L.RegVar("Null", get_Null, null);
		L.RegVar("JoyCtrl", get_JoyCtrl, null);
		L.RegVar("keyBordCtrl", get_keyBordCtrl, null);
		L.RegVar("AiCtrl", get_AiCtrl, null);
		L.RegVar("NetCtrl", get_NetCtrl, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
		TypeTraits<GameEnum.CtrlType>.Check = CheckType;
		StackTraits<GameEnum.CtrlType>.Push = Push;
	}

	static void Push(IntPtr L, GameEnum.CtrlType arg)
	{
		ToLua.Push(L, arg);
	}

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(typeof(GameEnum.CtrlType), L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Null(IntPtr L)
	{
		ToLua.Push(L, GameEnum.CtrlType.Null);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_JoyCtrl(IntPtr L)
	{
		ToLua.Push(L, GameEnum.CtrlType.JoyCtrl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_keyBordCtrl(IntPtr L)
	{
		ToLua.Push(L, GameEnum.CtrlType.keyBordCtrl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AiCtrl(IntPtr L)
	{
		ToLua.Push(L, GameEnum.CtrlType.AiCtrl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NetCtrl(IntPtr L)
	{
		ToLua.Push(L, GameEnum.CtrlType.NetCtrl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		GameEnum.CtrlType o = (GameEnum.CtrlType)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

