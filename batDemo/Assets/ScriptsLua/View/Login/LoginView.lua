---@class LoginView : PanelView
LoginView = Class("LoginView",PanelView)
---@return LoginView
--自动生成-------------------------------------------
---添加UI引用.
function LoginView:OnUIInit()
    ---@type UnityEngine.RectTransform
    self.Toggle=self:Find("Toggle")
    ---@type UnityEngine.RectTransform
    self.btnClose=self:Find("btnClose")
    ---@type UnityEngine.RectTransform
    self.Image1=self:Find("Image1")
end

---移除UI引用.
function LoginView:OnUIDestory()
self.Toggle=nil
self.btnClose=nil
self.Image1=nil

end

--自动生成-----------end------------------------------
function LoginView:initialize()
PanelView.initialize(self)
self.viewName= "View.Login.LoginView"
self.url="View/Login/LoginView"
self.needUpdate=false
self.ViewLayer=ViewLayer.content
self.showMode=ViewShowMode.Normal
end

function LoginView:Init()
PanelView.Init(self)

end

function LoginView:OnDestory()
--记得nil移除变量

end

function LoginView:AddListener()

end

function LoginView:RemoveListener()

end

function LoginView:Update()
--- update
end

return LoginView

