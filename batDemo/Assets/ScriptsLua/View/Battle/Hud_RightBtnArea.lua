--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:46
LastEditors: xsddxr909
LastEditTime: 2020-08-06 16:55:42
--]]
---@class Hud_RightBtnArea : ChildView
Hud_RightBtnArea = Class("Hud_RightBtnArea",ChildView)
---@return Hud_RightBtnArea
--自动生成-------------------------------------------
---添加UI引用.
function Hud_RightBtnArea:OnUIInit()
    ---@type UnityEngine.UI.Button
    self.FireBtn=self:SubGet("FireBtn","Button")
    ---@type UnityEngine.RectTransform
    self.SprintCloseImg=self:Find("SprintBtn/Image")
    ---@type UnityEngine.RectTransform
    self.SprintBtn=self:Find("SprintBtn")
    ---@type UnityEngine.UI.Button
    self.SettingBtn=self:SubGet("SettingBtn","Button")
end

---移除UI引用.
function Hud_RightBtnArea:OnUIDestory()
self.FireBtn=nil
self.SprintCloseImg=nil
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
end

function Hud_RightBtnArea:OnSprintClick(obj)
    ---@type Joystick
    local joyStick =self.panelView.dragArea.joyStick;
    if joyStick.isPress then return end
    self._isSprinting = not self._isSprinting;
    self.SprintCloseImg.enabled = not self._isSprinting;
    self.SprintBtn.enabled =  self._isSprinting;
    joyStick:SetSprint(self._isSprinting);
    EventManager.dispatchEventToC(SystemEvent.UI_BAT_ON_SPRINT_STATE,{self._isSprinting});
end


function Hud_RightBtnArea:OnDestory()
--记得nil移除变量

end

function Hud_RightBtnArea:AddListener()
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = function(obj)  self:OnSprintClick(obj) end
end

function Hud_RightBtnArea:RemoveListener()
    UIEventListener.Get(self.SprintBtn.gameObject).onClick = nil
end

function Hud_RightBtnArea:Update()
--- update
end

return Hud_RightBtnArea

