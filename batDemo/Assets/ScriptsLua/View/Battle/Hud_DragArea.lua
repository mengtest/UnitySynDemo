---@class Hud_DragArea : ChildView
Hud_DragArea = Class("Hud_DragArea",ChildView)
---@return Hud_DragArea
--自动生成-------------------------------------------
---添加UI引用.
function Hud_DragArea:OnUIInit()
    ---@type UnityEngine.RectTransform
    self.SprintJoyStick=self:Find("MoveArea/BackGround/SprintJoyStick")
    ---@type UnityEngine.RectTransform
    self.Handle=self:Find("MoveArea/BackGround/Handle")
    ---@type UnityEngine.RectTransform
    self.BackGround=self:Find("MoveArea/BackGround")
    ---@type UnityEngine.RectTransform
    self.Sprint=self:Find("Sprint")
    ---@type UnityEngine.RectTransform
    self.RotateArea=self:Find("RotateArea")
    ---@type UnityEngine.RectTransform
    self.MoveArea=self:Find("MoveArea")
end

---移除UI引用.
function Hud_DragArea:OnUIDestory()
self.SprintJoyStick=nil
self.Handle=nil
self.BackGround=nil
self.Sprint=nil
self.RotateArea=nil
self.MoveArea=nil
end

--自动生成-----------end------------------------------
function Hud_DragArea:initialize()
ChildView.initialize(self)
self.viewName= "View.Battle.Hud_DragArea"
self.url="View/Battle/Hud_DragArea"
self.needUpdate=false
self.ViewLayer=ViewLayer.content
--是否为部件
self.isPart=true
end

function Hud_DragArea:OnInit(param)

require("View.Battle.Joystick");
self.joyStick=Joystick:new();
self.joyStick:init(self.MoveArea,self.Handle,self.SprintJoyStick,self.Sprint,self.BackGround);

end

function Hud_DragArea:OnDestory()
--记得nil移除变量
    if self.joyStick then
        self.joyStick:destory();
        self.joyStick=nil;
    end
end

function Hud_DragArea:AddListener()
    self.joyStick:AddListener();
end

function Hud_DragArea:RemoveListener()
    self.joyStick:RemoveListener();
end

function Hud_DragArea:Update()
--- update
end




return Hud_DragArea

