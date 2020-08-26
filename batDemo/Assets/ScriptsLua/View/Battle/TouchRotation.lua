--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:46
LastEditors: xsddxr909
LastEditTime: 2020-08-26 19:47:39
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
    self.fingerId = nil;
end

function TouchRotation:destory()
    self.rotaeArea=nil
    self.IsDraging=false;
    self.name=nil;
    self.fingerId = nil;
    EventObj.destory(self);
end
function TouchRotation:Reset()
    self.IsDraging=false;
    self.fingerId = nil;
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
    if eventData.pointerId<-1 or self.fingerId~=nil then return end
    self.fingerId = eventData.pointerId;
    self.IsDraging = true;
   --- log("OnDown ")
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,{self.IsDraging });
end
function TouchRotation:OnDrag(eventData)
  ---   log("onDrag "..eventData.position)
     if self.fingerId ~= eventData.pointerId then  return end;
      ---@type Vector2
     local v2 =Vector2(eventData.delta.x*Main.ViewManager:scaleScreen(),eventData.delta.y*Main.ViewManager:scaleScreen());
   --    log(" TouchRotation onDrag x"..eventData.delta.x.." y "..eventData.delta.y);
     EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_MOVE,{v2});
end

function TouchRotation:OnUp(eventData)
    ---正确的手指抬起时重置摇杆
   --- log("OnUp ")
    if self.fingerId ~= eventData.pointerId then  return end;
    self:Reset();
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,{ self.IsDraging });
end

return TouchRotation