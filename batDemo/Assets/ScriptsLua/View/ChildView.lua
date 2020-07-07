---@class ChildView : BaseView
ChildView = Class("ChildView",BaseView)
---@return ChildView
function ChildView:initialize()
    ---@type PanelView
    self.panelView=nil;
    --是否成为面板部件. 如果是部件 添加到面板上 会跟随隐藏打开.成为面板一份子.
    self.isPart=false;
    BaseView.initialize(self);
end

function ChildView:destory()
   self.panelView=nil;
   self.isPart=false;
   self:UnRegisterEvent();
   self:OnUIDestory();
   BaseView.destory(self);
end

---@param panelView PanelView
function ChildView:setPlaneView(panelView)
    self.panelView=panelView;
    if  self.isPart and self.transform and self.panelView.transform then
        self.transform.SetParent(self.panelView.transform);
    end
end

-----------------------子类重写生命周期----------------------------

function ChildView:Update()
    --- update
end

function ChildView:Close()
    self.gameObject:setActive(false);
end
function ChildView:Show()
    self.gameObject:setActive(true);
end

-- 添加监听
function ChildView: AddListener()
    
end

-- 移除监听
function ChildView: RemoveListener()

end

function ChildView: Init()
  --不是部件 添加到指定层级.
   if self.isPart then
       if  self.panelView then
           self.transform.SetParent(self.panelView.transform);
       end
   else
      Main.ViewManager:addToLayer(self);
   end
   self:OnUIInit();
end
-- 销毁,记得清空变量的引用
function ChildView: OnDestory()

end
----自动生成---
function ChildView: OnUIInit()

end
----自动生成---
function ChildView: OnUIDestory()

end
-----------------------子类重写生命周期----------------------------
return ChildView

