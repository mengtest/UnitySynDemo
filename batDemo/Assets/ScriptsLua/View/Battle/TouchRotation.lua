--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:46
LastEditors: xsddxr909
LastEditTime: 2020-08-05 20:29:47
--]]
---@class TouchRotation:EventObj
TouchRotation = Class("TouchRotation",EventObj)
---@return TouchRotation


-- 类初始化，类似构造函数
function TouchRotation:initialize()
   EventObj.initialize(self);
   self.name="TouchRotation";
end

---@param rotaeArea RectTransform
function TouchRotation:init(rotaeArea)
    self.rotaeArea=rotaeArea
    self.IsDraging=false;
end

function TouchRotation:destory()
    self.rotaeArea=nil
    self.IsDraging=false;
    self.name=nil;
    EventObj.destory(self);
end
function TouchRotation:Reset()
    self.IsDraging=false;
end

function TouchRotation:AddListener()
    log("TouchRotation AddListener");
    UIEventListener.Get(self.rotaeArea.gameObject).onDown = function(eventData)  self:onDown(eventData) end
    UIEventListener.Get(self.rotaeArea.gameObject).onDrag = function(eventData)  self:OnDrag(eventData) end
    UIEventListener.Get(self.rotaeArea.gameObject).onUp = function(eventData)  self:OnUp(eventData) end
end

function TouchRotation:RemoveListener()
  --  log("Joystick RemoveListener");
    UIEventListener.Get(self.rotaeArea.gameObject).onDown = nil
    UIEventListener.Get(self.rotaeArea.gameObject).onDrag = nil
    UIEventListener.Get(self.rotaeArea.gameObject).onUp = nil
end

---@param    eventData UnityEngine.EventSystems.PointerEventData
function TouchRotation:onDown(eventData)
    self.IsDraging = true;
end
function TouchRotation:OnDrag(eventData)
  ---   log("onDrag "..eventData.position)
      ---@type Vector2
     local v2 = eventData.delta;
 ---    log("onDrag "..eventData.delta.x.." y "..eventData.delta.y);
     EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_TOUCH_MOVE,{eventData.delta});
end

function TouchRotation:OnUp(eventData)
    ---正确的手指抬起时重置摇杆
  --  log("OnUp ")
    self:Reset();
end

return TouchRotation