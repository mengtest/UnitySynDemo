--全局变量和函数统一在此添加和require

--导出类简写
---@class Object
Object          = UnityEngine.Object
---@class GameObject
GameObject      = UnityEngine.GameObject
---@class Transform
Transform       = UnityEngine.Transform
---@class RectTransform
RectTransform   = UnityEngine.RectTransform
---@class Application
Application     = UnityEngine.Application
---@class Screen
Screen          = UnityEngine.Screen
---@class Input
Input           = UnityEngine.Input
---@class Rect
Rect            = UnityEngine.Rect
PlayerPrefs     = UnityEngine.PlayerPrefs


---游戏常用
---@class GameAssetRequest
AssetRequest = GameAssetRequest



require ("LuaUtils");
require ("Class");

--第三方封装
require("Common.Extension.table");
require("Common.Extension.string");
require("Common.Extension.QueueList");
require("Common.Extension.Array");
require("Common.CallbackSequence");


require("GameSet");
require("Data.DataInit");


--管理者类.
require("Manager.TweenManager");
require("Manager.EventManager");


require("Manager.EventObj");
require("Game.GameObjBase");

--视图基类
require("View.BaseView");
require("View.PanelView");
require("View.ChildView");
require("View.ViewManager");

--加载Proto协议脚本
require("PB.pb_list")

--控制器基类
require("Controller.BaseController");



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


