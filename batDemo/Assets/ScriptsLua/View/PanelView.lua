---@class PanelView : BaseView
PanelView = Class("PanelView",BaseView)
---@return PanelView
--- childView 跟随父类一起隐藏 自动销毁 放空引用便可----------------------------
--- PanelView创建完后 会在init 中 调用创建childView   允许 多个childView相同 ChildView 里面可以创建各种baseView 自己管理 自己销毁
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
      self._OnUpdateHandle=nil;
    end
   for i, v in ipairs(self._childViews) do
        v:destory()
   end
   self._childViews = nil;
   self.openParam=nil;
   self.closeTime=0;
   self.isstateCg=false;
   

   self:OnUIDestory();
   BaseView.destory(self);
end

---@param childViewType ChildViewType
---@param addchild bool 是否添加到父节点
function PanelView:creatChildView(childViewType,...)
    local viewCls = require(childViewType);
    if viewCls == nil then
        logError("视图逻辑类加载失败", childViewType)
        return nil;
    end
    ---@type ChildView
    local childView = viewCls:new();
    self:addChildView(childView);
    local args = {...};
    childView:create(args);
    return childView
end

---@param childView ChildView
function PanelView:addChildView(childView)
    childView:setPlaneView(self);
    table.insert(self._childViews, childView)
end

---获取chidView 如果viewName相同请自己缓存
---@param viewName string
function PanelView:getChildView(viewName)
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
    self.canvasGroup.alpha=0;
    self.gameObject:SetActive(false);
    self:UnRegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:UnRegisterEvent()
    end
    self.closeTime=Time.GetTimestamp();
    if self._OnUpdateHandle then
         UpdateBeat:RemoveListener(self._OnUpdateHandle);
         self._OnUpdateHandle=nil
    end
    self.isstateCg=false;
    self:onClose();
end
function PanelView:Redisplay()
    if not self:changeState(ViewPanelState.Redisplay) then return end
    self.canvasGroup.alpha=1;
    self.gameObject:SetActive(true);
    self:RegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:RegisterEvent()
    end
    self.isstateCg=false;
end
function PanelView:Hiding()
    if not self:changeState(ViewPanelState.Hiding) then return end
    self.canvasGroup.alpha=0;
    self.gameObject:SetActive(false);
    self:UnRegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:UnRegisterEvent()
    end
    self.isstateCg=false;
end
function PanelView:Open()
    if not self:changeState(ViewPanelState.Open) then return end
    self.canvasGroup.alpha=1;
    self.gameObject:SetActive(true);
    self._OnUpdateHandle = UpdateBeat:CreateListener(self.Update,self)
    UpdateBeat:AddListener(self._OnUpdateHandle);
    self:RegisterEvent();
    for i, v in ipairs(self._childViews) do
        v:RegisterEvent()
    end
    self.isstateCg=false;
    self:onOpen();
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
        ----    log("update "..v.viewName);
        end
   end
end






function PanelView: Init(param)
    self.canvasGroup = self.gameObject:GetComponent(typeof(CanvasGroup));
    if not self.canvasGroup then
        logError(" self.canvasGroup not found >>> "..self.viewName);
    end
    self.canvasGroup.alpha=1;
    Main.ViewManager:addToLayer(self);
    self.gameObject:SetActive(true);
    self:OnUIInit();
    self:OnInit(param);
    if self.panelstate~=ViewPanelState.Init and self[self.panelstate] then
        local switch = {
            Open = function ()
               self:Open();
            end,
            Close = function ()
                self:Close();
            end,
            Hiding = function ()
                self:Hiding();
            end,
            Redisplay = function ()
                self:Redisplay();
            end,
            Freeze = function ()
                self:Freeze();
            end,
        }
        if switch[self.panelstate] then
            switch[self.panelstate]();
        end
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
function PanelView: onOpen()
    
end

-- 首次关闭
function PanelView: onClose()
    
end



----自动生成---
function PanelView: OnUIInit()

end
----自动生成---
function PanelView: OnInit(param)

end

----自动生成---
function PanelView: OnUIDestory()

end
-----------------------子类重写生命周期----------------------------
return PanelView

