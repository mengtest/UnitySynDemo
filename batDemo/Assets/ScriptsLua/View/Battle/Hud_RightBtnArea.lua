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


end

function Hud_RightBtnArea:OnDestory()
--记得nil移除变量

end

function Hud_RightBtnArea:AddListener()

end

function Hud_RightBtnArea:RemoveListener()

end

function Hud_RightBtnArea:Update()
--- update
end

return Hud_RightBtnArea

