---@class PanelView : BaseView
PanelView = Class("PanelView",BaseView)
---@return PanelView
function PanelView:initialize()
    self.showMode=ViewShowMode.Normal
    self._childViews = {}
    self.panelstate=ViewPanelState.Init;
    self.openParam=nil;
    self.closeTime=0;
    self.isstateCg=false;
    self._OnUpdateHandle=nil;
    BaseView.initialize(self);
end

function PanelView:destory()
    
    self:UnRegisterEvent();
    if self._OnUpdateHandle then
      UpdateBeat:RemoveListener(self._OnUpdateHandle);
    end
   for i, v in ipairs(self._childViews) do
        v:destroy()
   end
   self._childViews = nil;
   self.openParam=nil;
   self._OnUpdateHandle=nil;
   self.closeTime=0;
   self.isstateCg=false;
   

   self:OnUIDestory();
   BaseView.destory(self);
end


---@param childView ChildView
function PanelView:addChildView(childView)
    childView:setPlaneView(self);
    table.insert(self._childViews, childView)
end

---获取chidView
---@param ViewType ViewType
function PanelView:getChildView(ViewType)
    for i, v in ipairs(self._childViews) do
        if v.viewName == ViewType then
            return v;
        end
    end
    return nil;
end

---@param childView ChildView
function PanelView:destroyChildView(childView)
    if self._isDestroyed then return end
    for i, v in ipairs(self._childViews) do
        if v == childView then
            table.remove(self._childViews, i)
            break
        end
    end
    childView:destroy()
end
---改变状态
---@param  ViewPanelState ViewPanelState
function PanelView:changeState(ViewPanelState)
    if self.panelstate~=ViewPanelState or self.isstateCg then
        self.panelstate=ViewPanelState;
        self.isstateCg=true;
        if self.isFin then 
           return true;
        else
            return false;
        end
    end
end

function PanelView:Close()
    if not self:changeState(ViewPanelState.Close) then return end
    self.canvasGroup.Alpha=0;
    self.gameObject:setActive(false);
    self:UnRegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:UnRegisterEvent()
    end
    self.closeTime=Time.GetTimestamp();
    UpdateBeat:RemoveListener(self._OnUpdateHandle);
    self.isstateCg=false;
    self:onClose();
end
function PanelView:Redisplay()
    if not self:changeState(ViewPanelState.Redisplay) then return end
    self.canvasGroup.Alpha=1;
    self.gameObject:setActive(true);
    self:RegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:RegisterEvent()
    end
    self.isstateCg=false;
end
function PanelView:Hiding()
    if not self:changeState(ViewPanelState.Hiding) then return end
    self.canvasGroup.Alpha=0;
    self.gameObject:setActive(false);
    self:UnRegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:UnRegisterEvent()
    end
    self.isstateCg=false;
end
function PanelView:Open()
    if not self:changeState(ViewPanelState.Open) then return end
    self.canvasGroup.Alpha=1;
    self.gameObject:setActive(true);
    self._OnUpdateHandle = UpdateBeat:CreateListener(self.Update,self)
    UpdateBeat:AddListener(self._OnUpdateHandle);
    self:RegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:RegisterEvent()
    end
    self.isstateCg=false;
   if self.openParam~=nil then
     self:onOpen(self.openParam);
     self.openParam=nil;
   end
end
---冻结 仍然显示 但没有监听
function PanelView:Freeze()
    if not self:changeState(ViewPanelState.Freeze) then return end
    self:UnRegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:UnRegisterEvent()
    end
    self.isstateCg=false;
end


-----------------------子类重写生命周期----------------------------

function PanelView:Update()
    --- update
    for i, v in ipairs(self._childViews) do
        if v.needUpdate then
            v:Update()
        end
   end
end






function PanelView: Init()
    self.canvasGroup = self.gameObject:GetComponent(typeof(CanvasGroup));
    if not self.canvasGroup then
        logError(" self.canvasGroup not found >>> "..self.viewName);
    end
    self.canvasGroup.Alpha=1;
    self.gameObject:setActive(true);
    Main.ViewManager:addToLayer(self);
    self:OnUIInit();
    if self.panelstate~=ViewPanelState.Init and self[self.panelstate] then
        self[self.panelstate]();
    end
end



-- 添加监听
function PanelView: AddListener()
    
end

-- 移除监听
function PanelView: RemoveListener()

end

-- 销毁,记得清空变量的引用
function PanelView: OnDestory()

end

-- 首次打开
function PanelView: onOpen(...)
    
end

-- 首次关闭
function PanelView: onClose()
    
end



----自动生成---
function PanelView: OnUIInit()

end
----自动生成---
function PanelView: OnUIDestory()

end
-----------------------子类重写生命周期----------------------------
return PanelView

