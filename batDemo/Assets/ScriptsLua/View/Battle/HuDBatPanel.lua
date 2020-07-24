---@class HuDBatPanel : PanelView
HuDBatPanel = Class("HuDBatPanel",PanelView)
---@return HuDBatPanel
--自动生成-------------------------------------------
---添加UI引用.
function HuDBatPanel:OnUIInit()
    ---@type UnityEngine.RectTransform
    self.HTKBox=self:Find("HTKBox")
    ---@type UnityEngine.RectTransform
    self.BoxScroBox=self:Find("BoxScroBox")
    ---@type UnityEngine.RectTransform
    self.BoxBtnCloseImg=self:Find("BoxBtn/CloseImg")
    ---@type UnityEngine.RectTransform
    self.BoxBtnOpenImg=self:Find("BoxBtn/OpenImg")
    ---@type UnityEngine.UI.Button
    self.BoxBtn=self:SubGet("BoxBtn","Button")
    ---@type UnityEngine.RectTransform
    self.NearScroBox=self:Find("NearScroBox")
    ---@type UnityEngine.RectTransform
    self.NearBtnCloseImg=self:Find("NearBtn/CloseImg")
    ---@type UnityEngine.RectTransform
    self.NearBtnOpenImg=self:Find("NearBtn/OpenImg")
    ---@type UnityEngine.UI.Button
    self.NearBtn=self:SubGet("NearBtn","Button")
end

---移除UI引用.
function HuDBatPanel:OnUIDestory()
self.HTKBox=nil
self.BoxScroBox=nil
self.BoxBtnCloseImg=nil
self.BoxBtnOpenImg=nil
self.BoxBtn=nil
self.NearScroBox=nil
self.NearBtnCloseImg=nil
self.NearBtnOpenImg=nil
self.NearBtn=nil
end

--自动生成-----------end------------------------------
function HuDBatPanel:initialize()
PanelView.initialize(self)
self.viewName= "View.Battle.HuDBatPanel"
self.url="View/Battle/HuDBatPanel"
self.needUpdate=false
self.ViewLayer=ViewLayer.content
self.showMode=ViewShowMode.Normal
end

-----UI加载完成---------
function HuDBatPanel:Init(param)
PanelView.Init(self,param)
 ---@type Hud_RightBtnArea
 self.rightBtnArea= self:creatChildView(ChildViewType.Hud_RightBtnArea,true,true)
 ---self.rightBtnArea:Close();
 log("HuDBatPanel init "..param);
end

function HuDBatPanel:OnDestory()
--记得nil移除变量
self.rightBtnArea=nil

end

function HuDBatPanel:AddListener()

end

function HuDBatPanel:RemoveListener()

end

--- update
function HuDBatPanel:Update()
     PanelView.Update(self)
     
end

-- 首次打开
function HuDBatPanel: onOpen()

end

-- 首次关闭
function HuDBatPanel: onClose()
    
end


return HuDBatPanel

