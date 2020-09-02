--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:46
LastEditors: xsddxr909
LastEditTime: 2020-09-01 09:44:08
--]]
---@class Hud_RightBtnArea : ChildView
Hud_RightBtnArea = Class("Hud_RightBtnArea",ChildView)
---@return Hud_RightBtnArea
--自动生成-------------------------------------------
---添加UI引用.
function Hud_RightBtnArea:OnUIInit()
    ---@type UnityEngine.UI.Button
    self.JumpBtn=self:SubGet("JumpBtn","Button")
    ---@type UnityEngine.UI.Text
    self.RotateText=self:SubGet("Test/RotateText","Text")
    ---@type UnityEngine.UI.Button
    self.subRotate=self:SubGet("Test/subRotate","Button")
    ---@type UnityEngine.UI.Button
    self.addRotate=self:SubGet("Test/addRotate","Button")
    ---@type UnityEngine.UI.Text
    self.SpeedText=self:SubGet("Test/SpeedText","Text")
    ---@type UnityEngine.UI.Button
    self.subSpeed=self:SubGet("Test/subSpeed","Button")
    ---@type UnityEngine.UI.Button
    self.addSpeed=self:SubGet("Test/addSpeed","Button")
    ---@type UnityEngine.RectTransform
    self.FreeSeeBtn=self:Find("FreeSeeBtn")
    ---@type UnityEngine.UI.Button
    self.FireBtn=self:SubGet("FireBtn","Button")
    ---@type UnityEngine.UI.Image
    self.Image=self:SubGet("SprintBtn/Image","Image")
    ---@type UnityEngine.UI.Image
    self.SprintBtn=self:SubGet("SprintBtn","Image")
    ---@type UnityEngine.UI.Button
    self.SettingBtn=self:SubGet("SettingBtn","Button")
end

---移除UI引用.
function Hud_RightBtnArea:OnUIDestory()
self.JumpBtn=nil
self.RotateText=nil
self.subRotate=nil
self.addRotate=nil
self.SpeedText=nil
self.subSpeed=nil
self.addSpeed=nil
self.FreeSeeBtn=nil
self.FireBtn=nil
self.Image=nil
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

    self.RotateText.text=CameraManager.Instance.cameraCtrl.Horizontal_Acce_Dic;
    --- Example.playerObj:GetMovePart().rotateSpeed;
    self.SpeedText.text = CameraManager.Instance.cameraCtrl.Horizontal_Acce_Speed;
end

function Hud_RightBtnArea:OnSprintClick(obj)
    ---@type Joystick
    local joyStick =self.panelView.dragArea.joyStick;
    if joyStick.isPress then return end
    self:OnSprintState(not self._isSprinting);
    joyStick:SetSprint(self._isSprinting);
end
function Hud_RightBtnArea:OnJumpClick(obj)
    EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_JUMP);
end

function Hud_RightBtnArea:OnSprintState(isSprinting)
    self._isSprinting = isSprinting;
    self.Image.enabled =  self._isSprinting;
    self.SprintBtn.enabled = not self._isSprinting;
end


function Hud_RightBtnArea:OnSpeedUp(obj)
    local sp= CameraManager.Instance.cameraCtrl.Horizontal_Acce_Speed;
    sp =sp+1;
  --  log(sp);
  CameraManager.Instance.cameraCtrl.Horizontal_Acce_Speed=sp
    self.SpeedText.text= sp;
end
function Hud_RightBtnArea:OnSpeedDown(obj)
    local sp= CameraManager.Instance.cameraCtrl.Horizontal_Acce_Speed;
    sp =sp-1;
   --- log(sp);
   CameraManager.Instance.cameraCtrl.Horizontal_Acce_Speed=sp
    self.SpeedText.text= sp;
end
function Hud_RightBtnArea:OnRotateUp(obj)
    local sp= CameraManager.Instance.cameraCtrl.Horizontal_Acce_Dic;
    sp =sp+1;
    CameraManager.Instance.cameraCtrl.Horizontal_Acce_Dic = sp;
    self.RotateText.text= sp;
end
function Hud_RightBtnArea:OnRotateDown(obj)
    local sp= CameraManager.Instance.cameraCtrl.Horizontal_Acce_Dic;
    sp =sp-1;
    CameraManager.Instance.cameraCtrl.Horizontal_Acce_Dic = sp;
    self.RotateText.text=sp;
end

function Hud_RightBtnArea:OnDestory()
    --记得nil移除变量
end


function Hud_RightBtnArea:AddListener()
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = function(obj)  self:OnSprintClick(obj) end
    UIEventListener.Get(self.JumpBtn.gameObject).onClick = function(obj)  self:OnJumpClick(obj) end

    UIEventListener.Get(self.addSpeed.gameObject).onClick = function(obj)  self:OnSpeedUp(obj) end
    UIEventListener.Get(self.subSpeed.gameObject).onClick = function(obj)  self:OnSpeedDown(obj) end
    UIEventListener.Get(self.addRotate.gameObject).onClick = function(obj)  self:OnRotateUp(obj) end
    UIEventListener.Get(self.subRotate.gameObject).onClick = function(obj)  self:OnRotateDown(obj) end
end

function Hud_RightBtnArea:RemoveListener()
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = nil
    UIEventListener.Get(self.JumpBtn.gameObject).onClick = nil

    UIEventListener.Get(self.addSpeed.gameObject).onClick = nil
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = nil
    UIEventListener.Get(self.addRotate.gameObject).onClick = nil
    UIEventListener.Get(self.subRotate.gameObject).onClick = nil
end

function Hud_RightBtnArea:Update()
--- update
end

return Hud_RightBtnArea

