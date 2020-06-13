--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
if jit then		
	if jit.opt then		
		jit.opt.start(3)				
	end		
	
	print("ver"..jit.version_num.." jit: ", jit.status())
	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
end

if DebugServerIp then  
  require("mobdebug").start(DebugServerIp)
end

require "misc.functions"
---@type UnityEngine.Mathf
Mathf		= require "UnityEngine.Mathf"
---@type UnityEngine.Vector3
Vector3 	= require "UnityEngine.Vector3"
---@type UnityEngine.Quaternion
Quaternion	= require "UnityEngine.Quaternion"
---@type UnityEngine.Vector2
Vector2		= require "UnityEngine.Vector2"
---@type UnityEngine.Vector4
Vector4		= require "UnityEngine.Vector4"
---@type UnityEngine.Color
Color		= require "UnityEngine.Color"
---@type UnityEngine.Ray
Ray			= require "UnityEngine.Ray"
---@type UnityEngine.Bounds
Bounds		= require "UnityEngine.Bounds"
---@type UnityEngine.RaycastHit
RaycastHit	= require "UnityEngine.RaycastHit"
---@type UnityEngine.Touch
Touch		= require "UnityEngine.Touch"
---@type UnityEngine.LayerMask
LayerMask	= require "UnityEngine.LayerMask"
---@type UnityEngine.Plane
Plane		= require "UnityEngine.Plane"
---@type UnityEngine.Time
Time		= reimport "UnityEngine.Time"

list		= require "list"
utf8		= require "misc.utf8"

require "event"
require "typeof"
require "slot"
require "System.Timer"
require "System.coroutine"
require "System.ValueType"
require "System.Reflection.BindingFlags"

--require "misc.strict"