---@class ViewManager 视图管理类
ViewManager = Class("ViewManager")
---@return ViewManager

function ViewManager:initialize()

    -- 层级节点
    ---@type table {string:UnityEngine.GameObject}
    self._layerNodes = {}

    ---已经 创建处理的界面 包括 关闭的 隐藏的 打开的
    self._mapUIPool = {}
    --正在显示的队列 PanelView
    self._mapShowingUI = {}
    -- 需要反向的队列 PanelView
    self._arrReverseChangeUI = Array();
    self._delUiTime=20;
    self._releaseUITime=2;
    self._releaseUITempTime=0;

    ---@type UnityEngine.GameObject
    local UIRoot = GameObject.Find("UIRoot");
    self._uiRoot = UIRoot;
    GameObject.DontDestroyOnLoad(self._uiRoot);
    self._layerNodes[ViewLayer.content] = UIRoot.Find("content");
    self._layerNodes[ViewLayer.popup] = UIRoot.Find("popup");
    self._layerNodes[ViewLayer.info] = UIRoot.Find("info");
    self._layerNodes[ViewLayer.guide] = UIRoot.Find("guide");
    self._layerNodes[ViewLayer.top] = UIRoot.Find("top");

end

function ViewManager: Release()
    self:CloseAllUI();
    UpdateBeat:RemoveListener(self._OnUpdateHandle);
    for k, v in ipairs(self._mapUIPool) do
        v:Close();
        v:destory();
    end
    self._mapUIPool=nil;
    self._arrReverseChangeUI =nil;
    self._layerNodes =nil;
    self._mapShowingUI =nil;
    self._OnUpdateHandle=nil;
end


--添加到显示层.
---@param  baseView BaseView
function ViewManager:addToLayer(baseView)
    if   self._layerNodes[baseView.ViewLayer] then
        local parentObj= self._layerNodes[baseView.ViewLayer];
        baseView.transform:SetParent(parentObj.transform);
    end
end


function ViewManager: init()
    self._OnUpdateHandle = UpdateBeat:CreateListener(self.Update,self)
    UpdateBeat:AddListener(self._OnUpdateHandle);
end

----获取其他面板.
---@param viewType ViewType
function ViewManager: getPanelView(viewType)
    if self._mapUIPool[viewType] then
       return self._mapUIPool[viewType];
    end
end

---@param viewType ViewType
function ViewManager:Show(viewType, ...)
    ---@type PanelView
    local panelView =nil;
    -- 已经加载过.
    if self._mapUIPool[viewType] then
        panelView = self._mapUIPool[viewType];
        if panelView.isFin then 
            self:AddUIToRoot(panelView,...);
        end
        return panelView;
    end
    local viewCls = require(viewType);
    if viewCls == nil then
        logError("视图逻辑类加载失败", viewType)
        return nil;
    end
    panelView = viewCls:new();
    panelView.viewType=viewType;
    self._mapUIPool[viewType]=panelView;
    panelView:create();
    self.AddUIToRoot(panelView,...);
    return panelView
end

---@param viewType ViewType
---@param Del bool
function ViewManager:Close(viewType,Del)
    if  not self._mapUIPool[viewType] then return end
    ---@type PanelView
    local panelView=self._mapUIPool[viewType];
    local switch = {
        Normal = function ()
            if self._mapShowingUI[viewType] then
                panelView:Close();
                self._mapShowingUI[viewType]=nil;
            end
        end,
        ReverseChange = function ()
            local arrLen = self._arrReverseChangeUI:len();
            if self._arrReverseChangeUI[arrLen].viewType ~= viewType then return end
            if arrLen >= 2 then
                self._arrReverseChangeUI[arrLen]:Close();
                self._arrReverseChangeUI:remove(arrLen);
                self._arrReverseChangeUI[arrLen-1]:Redisplay();
            elseif arrLen == 1 then
                self._arrReverseChangeUI[1]:Close();
                self._arrReverseChangeUI:remove(1);
            end
        end,
        HideOther = function ()
            if self._mapShowingUI[viewType] then
                panelView:Close();
                table.remove(self._mapShowingUI, viewType);

                for k, v in ipairs(self._mapShowingUI) do
                    v:Redisplay();
                end

                for k, v in ipairs(self._arrReverseChangeUI) do
                    v:Redisplay();
                end
            end
        end,
    }
    if switch[panelView.showMode] then
        switch[panelView.showMode]();
    end
    if Del then
        ViewManager:_delUI(viewType);
    end
end

---销毁一个面板.  内部用
---@param viewType ViewType
function ViewManager:_delUI(viewType)
    if not self._mapUIPool[viewType] then return end
    local panelView=self._mapUIPool[viewType];
    table.remove(self._mapUIPool, viewType);
    panelView:destory();
end


function ViewManager:CloseAllUI()
    local arrLen = self._arrReverseChangeUI:len();
    if arrLen>0 then
        for i = 1, arrLen, 1 do
            self._arrReverseChangeUI[i]:Close();
        end
    end
    self._arrReverseChangeUI=Array();
    for k, v in ipairs(self._mapShowingUI) do
        v:Close();
    end
    self._mapShowingUI={};
end

--每帧更新 渲染帧 2秒一次
function ViewManager:Update()
    --- log(Time.deltaTime);
    self._releaseUITempTime = Time.deltaTime + self._releaseUITempTime;
    if self._releaseUITempTime>=self._releaseUITime then
        for k, v in ipairs(self._mapUIPool) do
            if (not v.isOpen) and (Time.GetTimestamp()-v.closeTime >self._delUiTime) then
                    table.remove(self._mapUIPool,k);
                    v.destory();
            end
        end
        self._releaseUITempTime = 0;
    end
end


---添加UI到UI节点
---@param panelView PanelView
function ViewManager:AddUIToRoot(panelView,...)
    --log("AddUIToRoot: "..panelView.viewName);
    local tempTable =table.unpack(...);
    local switch = {
        Normal = function ()
            if not self._mapShowingUI[panelView.viewType] then
                self._mapShowingUI[panelView.viewType]=panelView;
                panelView.openParam=tempTable;
                panelView:Open();
            end
        end,
        ReverseChange = function ()
              local arrLen = self._arrReverseChangeUI:len();
              if arrLen>0 then 
                self._arrReverseChangeUI[arrLen]:Freeze();
              end
              self._arrReverseChangeUI:insert(panelView);
              panelView.openParam=tempTable;
              panelView:Open();
        end,
        HideOther = function ()
            if not self._mapShowingUI[panelView.viewType] then

                for k, v in ipairs(self._mapShowingUI) do
                    v:Hiding();
                end

                for k, v in ipairs(self._arrReverseChangeUI) do
                    v:Hiding();
                end

                self._mapShowingUI[panelView.viewType]=panelView;
                panelView.openParam=tempTable;
                panelView:Open();
            end
        end,
    }
    if switch[panelView.showMode] then
        switch[panelView.showMode]();
    end
end

return ViewManager
