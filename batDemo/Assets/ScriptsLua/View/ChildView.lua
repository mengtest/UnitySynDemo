---@class ChildView : BaseView
ChildView = Class("ChildView",BaseView)
---@return ChildView
function ChildView:initialize()
    ---@type PanelView
    self.panelView=nil;
    --是否成为面板部件. 如果是部件 添加到面板上 会跟随隐藏打开.成为面板一份子.
    self.isPart=false;
    self.needListen=false;
    BaseView.initialize(self);
end

function ChildView:destory()
   self.panelView=nil;
   self.isPart=false;
   self.needListen=false;
   self:UnRegisterEvent();
   self:OnUIDestory();
   BaseView.destory(self);
end

---@param panelView PanelView
function ChildView:setPlaneView(panelView)
    self.panelView=panelView;
end

-----------------------子类重写生命周期----------------------------

function ChildView:Update()
    --- update
end

function ChildView:Close()
    self.gameObject:SetActive(false);
end
function ChildView:Show()
    self.gameObject:SetActive(true);
end

---注册监听
function ChildView: RegisterEvent()
    self.needListen=true;
    if self.isFin then
       self:AddListener();
       self.needListen=false;
    end
end

function ChildView: UnRegisterEvent()
    self.needListen=false;
    if self.isFin then
      self:RemoveListener();
    end
end



function ChildView: Init(param)
  --不是部件 添加到指定层级.
   if self.isPart then
       if  self.panelView then
           self.transform:SetParent(self.panelView.transform,false);
           self.transform:SetAsLastSibling();
       end
   else
      Main.ViewManager:addToLayer(self);
   end
   self:OnUIInit();
   self:OnInit(param);
   if  self.needListen then
      self:AddListener();
      self.needListen=false;
   end
end
-- 销毁,记得清空变量的引用
function ChildView: OnDestory()

end
----自动生成---
function ChildView: OnUIInit()

end

----自动生成---
function ChildView: OnInit(param)

end
----自动生成---
function ChildView: OnUIDestory()

end

-- 添加监听
function ChildView: AddListener()

end

-- 移除监听
function ChildView: RemoveListener()

end

-----------------------子类重写生命周期----------------------------
return ChildView

