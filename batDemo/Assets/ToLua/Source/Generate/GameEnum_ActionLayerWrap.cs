﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class GameEnum_ActionLayerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(GameEnum.ActionLayer), typeof(System.Object));
		L.RegFunction("New", _CreateGameEnum_ActionLayer);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegConstant("BaseLayer", 0);
		L.RegConstant("UpLayer", 1);
		L.RegConstant("AddLayer", 2);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameEnum_ActionLayer(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				GameEnum.ActionLayer obj = new GameEnum.ActionLayer();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: GameEnum.ActionLayer.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

