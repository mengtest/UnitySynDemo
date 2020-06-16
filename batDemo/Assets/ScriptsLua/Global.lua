--全局变量和函数统一在此添加和require

--导出类简写
Object          = UnityEngine.Object
GameObject      = UnityEngine.GameObject
Transform       = UnityEngine.Transform
RectTransform   = UnityEngine.RectTransform
Application     = UnityEngine.Application
Screen          = UnityEngine.Screen
Input           = UnityEngine.Input
Rect            = UnityEngine.Rect
PlayerPrefs     = UnityEngine.PlayerPrefs

--第三方封装
require("Extension.table")
require("Extension.string")
require("Extension.QueueList")
require("Extension.Array")

--视图基类
require("View.BaseView")
--控制器基类
require("Controller.BaseController")


--以下全局变量在[ToLua/Lua/tolua.lua]已require，可直接使用
--[[
Mathf		= require "UnityEngine.Mathf"
Vector3 	= require "UnityEngine.Vector3"
Quaternion	= require "UnityEngine.Quaternion"
Vector2		= require "UnityEngine.Vector2"
Vector4		= require "UnityEngine.Vector4"
Color		= require "UnityEngine.Color"
Ray			= require "UnityEngine.Ray"
Bounds		= require "UnityEngine.Bounds"
RaycastHit	= require "UnityEngine.RaycastHit"
Touch		= require "UnityEngine.Touch"
LayerMask	= require "UnityEngine.LayerMask"
Plane		= require "UnityEngine.Plane"
Time		= reimport "UnityEngine.Time"

list		= require "list"
utf8		= require "misc.utf8"
--]]


