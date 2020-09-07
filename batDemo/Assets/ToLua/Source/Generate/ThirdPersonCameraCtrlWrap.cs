﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ThirdPersonCameraCtrlWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ThirdPersonCameraCtrl), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("GetH", GetH);
		L.RegFunction("init", init);
		L.RegFunction("onTouchMove", onTouchMove);
		L.RegFunction("onMouseMove", onMouseMove);
		L.RegFunction("ToggleClampHorizontal", ToggleClampHorizontal);
		L.RegFunction("BounceVertical", BounceVertical);
		L.RegFunction("LockOnDirection", LockOnDirection);
		L.RegFunction("UnlockOnDirection", UnlockOnDirection);
		L.RegFunction("SetTargetOffsets", SetTargetOffsets);
		L.RegFunction("ResetTargetOffsets", ResetTargetOffsets);
		L.RegFunction("ResetYCamOffset", ResetYCamOffset);
		L.RegFunction("SetYCamOffset", SetYCamOffset);
		L.RegFunction("SetXCamOffset", SetXCamOffset);
		L.RegFunction("SetFOV", SetFOV);
		L.RegFunction("ResetFOV", ResetFOV);
		L.RegFunction("SetMaxVerticalAngle", SetMaxVerticalAngle);
		L.RegFunction("ResetMaxVerticalAngle", ResetMaxVerticalAngle);
		L.RegFunction("GetCurrentPivotMagnitude", GetCurrentPivotMagnitude);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("target", get_target, set_target);
		L.RegVar("pivotOffset", get_pivotOffset, set_pivotOffset);
		L.RegVar("camOffset", get_camOffset, set_camOffset);
		L.RegVar("smooth", get_smooth, set_smooth);
		L.RegVar("horizontalAimingSpeed", get_horizontalAimingSpeed, set_horizontalAimingSpeed);
		L.RegVar("verticalAimingSpeed", get_verticalAimingSpeed, set_verticalAimingSpeed);
		L.RegVar("maxVerticalAngle", get_maxVerticalAngle, set_maxVerticalAngle);
		L.RegVar("minVerticalAngle", get_minVerticalAngle, set_minVerticalAngle);
		L.RegVar("XAxis", get_XAxis, set_XAxis);
		L.RegVar("YAxis", get_YAxis, set_YAxis);
		L.RegVar("smoothPivotOffset", get_smoothPivotOffset, set_smoothPivotOffset);
		L.RegVar("smoothCamOffset", get_smoothCamOffset, set_smoothCamOffset);
		L.RegVar("Horizontal_Acce_Dic", get_Horizontal_Acce_Dic, set_Horizontal_Acce_Dic);
		L.RegVar("Horizontal_Acce_Speed", get_Horizontal_Acce_Speed, set_Horizontal_Acce_Speed);
		L.RegVar("isMouseMove", get_isMouseMove, set_isMouseMove);
		L.RegVar("isJoyMove", get_isJoyMove, set_isJoyMove);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetH(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			float o = obj.GetH();
			LuaDLL.lua_pushnumber(L, o);
			return 1;
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
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
				obj.init();
				return 0;
			}
			else if (count == 2)
			{
				ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
				UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 2);
				obj.init(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ThirdPersonCameraCtrl.init");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onTouchMove(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			UnityEngine.Vector2 arg0 = ToLua.ToVector2(L, 2);
			obj.onTouchMove(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int onMouseMove(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			obj.onMouseMove();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToggleClampHorizontal(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
				obj.ToggleClampHorizontal();
				return 0;
			}
			else if (count == 2)
			{
				ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				obj.ToggleClampHorizontal(arg0);
				return 0;
			}
			else if (count == 3)
			{
				ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				obj.ToggleClampHorizontal(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				UnityEngine.Vector3 arg2 = ToLua.ToVector3(L, 4);
				obj.ToggleClampHorizontal(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ThirdPersonCameraCtrl.ToggleClampHorizontal");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BounceVertical(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.BounceVertical(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LockOnDirection(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.LockOnDirection(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnlockOnDirection(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			obj.UnlockOnDirection();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTargetOffsets(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 3);
			obj.SetTargetOffsets(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetTargetOffsets(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			obj.ResetTargetOffsets();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetYCamOffset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			obj.ResetYCamOffset();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetYCamOffset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.SetYCamOffset(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetXCamOffset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.SetXCamOffset(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetFOV(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.SetFOV(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetFOV(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			obj.ResetFOV();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMaxVerticalAngle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.SetMaxVerticalAngle(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetMaxVerticalAngle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			obj.ResetMaxVerticalAngle();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrentPivotMagnitude(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)ToLua.CheckObject<ThirdPersonCameraCtrl>(L, 1);
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			float o = obj.GetCurrentPivotMagnitude(arg0);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_target(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Transform ret = obj.target;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index target on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_pivotOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 ret = obj.pivotOffset;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index pivotOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_camOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 ret = obj.camOffset;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index camOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_smooth(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float ret = obj.smooth;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index smooth on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_horizontalAimingSpeed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float ret = obj.horizontalAimingSpeed;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index horizontalAimingSpeed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_verticalAimingSpeed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float ret = obj.verticalAimingSpeed;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index verticalAimingSpeed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_maxVerticalAngle(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float ret = obj.maxVerticalAngle;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index maxVerticalAngle on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_minVerticalAngle(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float ret = obj.minVerticalAngle;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index minVerticalAngle on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_XAxis(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			string ret = obj.XAxis;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index XAxis on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_YAxis(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			string ret = obj.YAxis;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index YAxis on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_smoothPivotOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 ret = obj.smoothPivotOffset;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index smoothPivotOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_smoothCamOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 ret = obj.smoothCamOffset;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index smoothCamOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Horizontal_Acce_Dic(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			int ret = obj.Horizontal_Acce_Dic;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Horizontal_Acce_Dic on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Horizontal_Acce_Speed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			int ret = obj.Horizontal_Acce_Speed;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Horizontal_Acce_Speed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isMouseMove(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			bool ret = obj.isMouseMove;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isMouseMove on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isJoyMove(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			bool ret = obj.isJoyMove;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isJoyMove on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_target(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Transform arg0 = (UnityEngine.Transform)ToLua.CheckObject<UnityEngine.Transform>(L, 2);
			obj.target = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index target on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_pivotOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.pivotOffset = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index pivotOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_camOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.camOffset = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index camOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_smooth(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.smooth = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index smooth on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_horizontalAimingSpeed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.horizontalAimingSpeed = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index horizontalAimingSpeed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_verticalAimingSpeed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.verticalAimingSpeed = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index verticalAimingSpeed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_maxVerticalAngle(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.maxVerticalAngle = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index maxVerticalAngle on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_minVerticalAngle(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.minVerticalAngle = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index minVerticalAngle on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_XAxis(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.XAxis = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index XAxis on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_YAxis(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.YAxis = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index YAxis on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_smoothPivotOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.smoothPivotOffset = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index smoothPivotOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_smoothCamOffset(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			UnityEngine.Vector3 arg0 = ToLua.ToVector3(L, 2);
			obj.smoothCamOffset = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index smoothCamOffset on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Horizontal_Acce_Dic(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.Horizontal_Acce_Dic = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Horizontal_Acce_Dic on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Horizontal_Acce_Speed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.Horizontal_Acce_Speed = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Horizontal_Acce_Speed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isMouseMove(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.isMouseMove = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isMouseMove on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isJoyMove(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			ThirdPersonCameraCtrl obj = (ThirdPersonCameraCtrl)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.isJoyMove = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isJoyMove on a nil value");
		}
	}
}

