--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:46
LastEditors: xsddxr909
LastEditTime: 2020-09-30 15:01:58
--]]
---@class Hud_RightBtnArea : ChildView
Hud_RightBtnArea = Class("Hud_RightBtnArea",ChildView)
---@return Hud_RightBtnArea
--自动生成-------------------------------------------
---添加UI引用.
function Hud_RightBtnArea:OnUIInit()
    ---@type UnityEngine.UI.Button
    self.ReloadBtn=self:SubGet("ReloadBtn","Button")
    ---@type UnityEngine.UI.Button
    self.AimBtn=self:SubGet("AimBtn","Button")
    ---@type UnityEngine.UI.Button
    self.JumpBtn=self:SubGet("JumpBtn","Button")
    ---@type UnityEngine.RectTransform
    self.FreeSeeBtn=self:Find("FreeSeeBtn")
    ---@type UnityEngine.UI.Button
    self.FireBtn=self:SubGet("FireBtn","Button")
    ---@type UnityEngine.UI.Image
    self.SprintImage=self:SubGet("SprintBtn/Image","Image")
    ---@type UnityEngine.UI.Image
    self.SprintBtn=self:SubGet("SprintBtn","Image")
    ---@type UnityEngine.UI.Button
    self.SettingBtn=self:SubGet("SettingBtn","Button")
end

---移除UI引用.
function Hud_RightBtnArea:OnUIDestory()
self.ReloadBtn=nil
self.AimBtn=nil
self.JumpBtn=nil
self.FreeSeeBtn=nil
self.FireBtn=nil
self.SprintImage=nil
self.SprintBtn=nil
self.SettingBtn=nil
end





--自动生成-----------end------------------------------
function Hud_RightBtnArea:initialize()
ChildView.initialize(self)
self.viewName= "View.Battle.Hud_RightBtnArea"
self.url="View/Battle/Hud_RightBtnArea"
self.needUpdate=true
self.ViewLayer=ViewLayer.content
self.isPart=true;
end

-----UI加载完成---------
function Hud_RightBtnArea:OnInit(param)
    self._isSprinting=false;
    self.Aim_IsDraging = false;

    --self.RotateText.text=CameraManager.Instance.cameraCtrl.Horizontal_Acce_Dic;
    --- Example.playerObj:GetMovePart().rotateSpeed;
   -- self.SpeedText.text = CameraManager.Instance.cameraCtrl.Horizontal_Acce_Speed;
end

function Hud_RightBtnArea:OnSprintClick(obj)
    ---@type Joystick
    local joyStick =self.panelView.dragArea.joyStick;
    if joyStick.isPress then return end
    self:OnSprintState(not self._isSprinting);
    joyStick:SetSprint(self._isSprinting);
end
function Hud_RightBtnArea:OnJumpClick(obj)
    EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_KEYSTATE,{ButtonInput.Jump});
end

function Hud_RightBtnArea:OnSprintState(isSprinting)
    self._isSprinting = isSprinting;
    self.SprintImage.enabled =  self._isSprinting;
    self.SprintBtn.enabled = not self._isSprinting;
end



function Hud_RightBtnArea:OnDestory()
    --记得nil移除变量
end


function Hud_RightBtnArea:AddListener()
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = function(obj)  self:OnSprintClick(obj) end
    UIEventListener.Get(self.JumpBtn.gameObject).onClick = function(obj)  self:OnJumpClick(obj) end

    UIEventListener.Get(self.AimBtn.gameObject).onDown = function(eventData)  self:onAimBtnDown(eventData) end
    UIEventListener.Get(self.AimBtn.gameObject).onDrag = function(eventData)  self:OnAimBtnDrag(eventData) end
    UIEventListener.Get(self.AimBtn.gameObject).onUp = function(eventData)  self:OnAimBtnUp(eventData) end

    UIEventListener.Get(self.FireBtn.gameObject).onDown = function(eventData)  self:onFireBtnDown(eventData) end
    UIEventListener.Get(self.FireBtn.gameObject).onDrag = function(eventData)  self:OnFireBtnDrag(eventData) end
    UIEventListener.Get(self.FireBtn.gameObject).onUp = function(eventData)  self:OnFireBtnUp(eventData) end
end

function Hud_RightBtnArea:RemoveListener()
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = nil
    UIEventListener.Get(self.JumpBtn.gameObject).onClick = nil

    UIEventListener.Get(self.AimBtn.gameObject).onDown = nil
    UIEventListener.Get(self.AimBtn.gameObject).onDrag = nil
    UIEventListener.Get(self.AimBtn.gameObject).onUp = nil

    UIEventListener.Get(self.FireBtn.gameObject).onDown = nil
    UIEventListener.Get(self.FireBtn.gameObject).onDrag = nil
    UIEventListener.Get(self.FireBtn.gameObject).onUp = nil
end

---@param    eventData UnityEngine.EventSystems.PointerEventData
function Hud_RightBtnArea:onAimBtnDown(eventData)
    if eventData.pointerId<-1 or self.Aim_fingerId~=nil then return end
    self.Aim_fingerId = eventData.pointerId;
    if  GameLuaManager.MyPlayer.charData.aimState ~= GameEnum.AimState.Null then
   --     self.Aim_IsDraging = false;
        EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_KEYSTATE,{ButtonInput.Aim,false});
      --  log("onAimBtnDown false ")
    else  
    --    self.Aim_IsDraging = true;
        EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_KEYSTATE,{ButtonInput.Aim,true});
        EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,{ true });
   ---     log("onAimBtnDown true ")
    end
  --   log("onAimBtnDown ")
end
function Hud_RightBtnArea:OnAimBtnDrag(eventData)
    ---   log("onDrag "..eventData.position)
    if self.Aim_fingerId ~= eventData.pointerId then  return end;
    ---@type Vector2
    local v2 =Vector2(eventData.delta.x*Global.ViewManager:scaleScreen(),eventData.delta.y*Global.ViewManager:scaleScreen());
    --    log(" Hud_RightBtnArea onDrag x"..eventData.delta.x.." y "..eventData.delta.y);
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_MOVE,{v2});
end

function Hud_RightBtnArea:OnAimBtnUp(eventData)
    ---正确的手指抬起时重置摇杆
    if self.Aim_fingerId ~= eventData.pointerId then  return end;
 --   self.Aim_IsDraging=false;
    self.Aim_fingerId = nil;
    --- log("OnUp ")
  --  EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_KEYSTATE,{ButtonInput.Aim,false});
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,{ false });
end


---@param    eventData UnityEngine.EventSystems.PointerEventData
function Hud_RightBtnArea:onFireBtnDown(eventData)
    if eventData.pointerId<-1 or self.Fire_fingerId~=nil then return end
    self.Fire_fingerId = eventData.pointerId;
    self.Fire_IsDraging = true;
    --- log("OnDown ")
    EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_KEYSTATE,{ButtonInput.Attack,true});
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,{true });
end
function Hud_RightBtnArea:OnFireBtnDrag(eventData)
    ---   log("onDrag "..eventData.position)
    if self.Fire_fingerId ~= eventData.pointerId then  return end;
    ---@type Vector2
    local v2 =Vector2(eventData.delta.x*Global.ViewManager:scaleScreen(),eventData.delta.y*Global.ViewManager:scaleScreen());
    --    log(" Hud_RightBtnArea onDrag x"..eventData.delta.x.." y "..eventData.delta.y);
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_MOVE,{v2});
end

function Hud_RightBtnArea:OnFireBtnUp(eventData)
    ---正确的手指抬起时重置摇杆
    if self.Fire_fingerId ~= eventData.pointerId then  return end;
    self.Fire_IsDraging=false;
    self.Fire_fingerId = nil;
    --- log("OnUp ")
    EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_KEYSTATE,{ButtonInput.Attack,false});
    EventManager.dispatchEventToC(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,{ false });
end



function Hud_RightBtnArea:Update()
--- update
end

return Hud_RightBtnArea

