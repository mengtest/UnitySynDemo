---@class ViewManager 视图管理类
local ViewManager = Class("ViewManager")

function ViewManager:ctor()
    self._uid = 0
    -- 已初始化的所有view
    ---@type table {string:table}
    self._allViews = {}
    -- 处于显示状态的所有view
    ---@type table {string:Array}
    self._showViews = {}
    -- 层级节点
    ---@type table {string:UnityEngine.GameObject}
    self._layerNodes = {}

    ---@type number @刘海尺寸
    self.bangSize = 0
    ---@type number @指示条尺寸
    self.railSize = 0

    ---@see @遮罩背景
    self._maskBg = nil
    ---@see @遮罩背景的rect
    self._maskRect = nil
    ---@see @遮罩背景所用资源名
    self._maskBgSpriteName = "mask"
    ---@see @遮罩背景偏移大小
    self._maskBgSizeOffset = 20

    ---@type UnityEngine.GameObject
    local UIRoot = GameObject.Find("Root")
    self._uiRoot = UIRoot
    GameObject.DontDestroyOnLoad(self._uiRoot)
    self._layerNodes[ViewLayer.close] = UIRoot.gameObject.Find("closeViewLayer")
    self._layerNodes[ViewLayer.content] = UIRoot.gameObject.Find("content")
    self._layerNodes[ViewLayer.popup] = UIRoot.gameObject.Find("popup")
    self._layerNodes[ViewLayer.info] = UIRoot.gameObject.Find("info")
    self._layerNodes[ViewLayer.guide] = UIRoot.gameObject.Find("guide")
    self._layerNodes[ViewLayer.top] = UIRoot.gameObject.Find("top")

    self:_initCloseViewLayer()
    self:_ResizeScreenSize()
end

---@return UnityEngine.GameObject
function ViewManager: getCloseViewLayer()
    return self._layerNodes[ViewLayer.close]
end

------------------对外方法----------------------

---@return BaseView
---@param viewType string
function ViewManager:show(viewType, ...)
    -- 显示界面
    local cfg = ViewConfig[viewType]
    local isSingle = cfg.isSingle
    ---@type UnityEngine.GameObject
    local layerNode = self._layerNodes[cfg.layer]
    ---@type BaseView
    local view = nil
    if isSingle == true then
        view = self:_getViewFromAll(viewType)
    else
        view = self:_getUnUseViewFromAll(viewType)
    end
    if view == nil then
        log("创建新的view实例", viewType)
        ---@type UnityEngine.GameObject
        local ins = ResHelper.LoadPrefab("view/" .. viewType, layerNode.transform)
        if ins ~= nil then
            ins:SetActive(false)
            ---@type BaseView
            local viewCls = require("view/" .. viewType)
            if viewCls == nil then
                logError("视图逻辑类加载失败", viewType)
                return
            end
            local uid = self:_getUid()
            local _rectTransform = ins:GetComponent(typeof(RectTransform))
            view = viewCls:new()
            view: create(uid, viewType, ins.transform, ins.gameObject, _rectTransform)
            self: _addViewToDict(view, self._allViews)
        else
            logError("视图资源加载失败", viewType)
        end
    end
    if view ~= nil then
        self:_pushViewToCanvas(view, ...)
    else
        logError("视图显示失败", viewType)
    end
    return view
end

---@type  viewType  string
function ViewManager:HideAllAndShowOne(viewType)
    for i, v in pairs(ViewLayer) do
        local viewArr = self._showViews[v]
        if  viewArr~=nil then
            local data = viewArr:getData()
            for k, t in pairs(data) do
                logError("打印颖仓界面",t,t.viewName)
                self:_removeViewFromCanvas(t)
            end
        end
    end
    self:show(viewType)
end


---@param viewType string
function ViewManager:hide(viewType)
    -- 隐藏界面,如果界面有多个实例，只会隐藏一个
    local view = self:getShowView(viewType)
    if view == nil then
        logWarn("不存在已显示的界面", viewType)
        return
    end
    self:_removeViewFromCanvas(view)
end

---@param view BaseView
function ViewManager:hideByViewIns(view)
    -- 直接通过实例，隐藏界面，如果某个界面有多个实例，建议直接使用这个api进行隐藏
    self:_removeViewFromCanvas(view)
end

---@param viewType string
function ViewManager:getShowView(viewType)
    -- 获取正在显示的界面
    local cfg = ViewConfig[viewType]
    ---@type Array
    local viewArr = self._showViews[cfg.layer]
    if viewArr == nil then
        return nil
    end
    ---@type table
    local datas = viewArr:getData()
    for k, v in pairs(datas) do
        local view = v
        if view ~= nil and view.viewName == viewType then
            return view
        end
    end
    return nil
end

---@param viewType string
---@return table
function ViewManager:getShowViewArr(viewType)
    -- 获取正在显示的界面数组
    -- 获取正在显示的界面
    local cfg = ViewConfig[viewType]
    ---@type Array
    local viewArr = self._showViews[cfg.layer]
    if viewArr == nil then
        return nil
    end
    ---@type table
    local res = {}
    ---@type table
    local datas = viewArr:getData()
    for k, v in pairs(datas) do
        local view = v
        if view ~= nil and view.viewName == viewType then
            table.insert(res, view)
        end
    end
    return res
end

---@param viewType string
function ViewManager:destoryView(viewType)
    -- 销毁一个视图的所有实例，如果只想销毁指定的某个，请使用destoryViewIns
    ---@type table
    local viewBox = self._allViews[viewType]
    if viewBox == nil then
        return
    end
    log("销毁所有视图", viewType)
    for k, v in pairs(viewBox) do
        self:destoryViewIns(v, false)
    end
    self._allViews[viewType] = nil
end

---@param view BaseView
---@param needClear boolean
function ViewManager:destoryViewIns(view, needClear)
    -- 销毁指定的View实例
    if view == nil then
        return
    end
    if needClear == nil then
        needClear = true
    end
    log("销毁视图", view.viewName)
    if view.isShow == true then
        -- 先隐藏
        self:hideByViewIns(view)
    end
    view:destory()
    if needClear then
        self:_removeViewFromDict(view, self._allViews)
    end
end

function ViewManager:_ShowLoadingForNetError()
    LuaEventCenter.register(EventType.onReconnect,nil,
    function ()  ViewHelper.showView(ViewType.LoadingView) end)
end

---清理所有界面
function ViewManager:DestroyAllView()
    local tempViewList = {}
    for k,v in pairs(self._allViews) do
        if v ~= nil then
            table.insert(tempViewList,k)
        end
    end
    for _,v in pairs(tempViewList) do
        self:destoryView(v)
    end
end

------------------对外方法----------------------

------------------内部方法----------------------

function ViewManager:_getViewFromAll(viewType)
    local viewBox = self._allViews[viewType]
    if viewBox ~= nil then
        return table.getFirst(viewBox)
    end
    return nil
end

function ViewManager:_getUnUseViewFromAll(viewType)
    local viewBox = self._allViews[viewType]
    if viewBox ~= nil then
        for k,v in pairs(viewBox) do
            if v.isShow == false then
                return v
            end
        end
    end
    return nil
end

function ViewManager:_getUid()
    self._uid = self._uid + 1
    return self._uid
end

---@param view BaseView
---@param dict table
function ViewManager:_addViewToDict(view, dict)
    local uid = view.uid
    local viewName = view.viewName
    local viewBox = dict[viewName]
    if viewBox == nil then
        viewBox = {}
        dict[viewName] = viewBox
    end
    if table.contains(viewBox, view) == false then
        viewBox[uid] = view
    end
end

---@param view BaseView
---@param dict table
function ViewManager:_removeViewFromDict(view, dict)
    local viewName = view.viewName
    ---@type table
    local viewBox = dict[viewName]
    if viewBox == nil then
        return
    end
    table.removeOne(viewBox, view)
end

---@param view BaseView
function ViewManager:_addViewToShowArr(view)
    local type = view.viewName
    local cfg = ViewConfig[type]
    ---@type Array
    local viewArr = self._showViews[cfg.layer]
    if viewArr == nil then
        viewArr = Array:new()
        self._showViews[cfg.layer] = viewArr
    end
    viewArr:push(view)
end

---@param layerNodeTr UnityEngine.Transform
function ViewManager: _refreshShowViewPosZ(layerNodeTr)
    local count = layerNodeTr.childCount
    local index = 0
    for i = 0, count-1 do
        local child = layerNodeTr:GetChild(i)
        if child ~= nil then
            local rectTransform = child:GetComponent(typeof(RectTransform))
            if rectTransform ~= nil and rectTransform.gameObject.activeSelf == true then
                rectTransform.anchoredPosition3D = Vector3.New(0,0, -500*index)
                index = index + 1
            end
        end
    end
end

---@param view BaseView
function ViewManager:_removeViewFormShowArr(view)
    local type = view.viewName
    local cfg = ViewConfig[type]
    ---@type Array
    local viewArr = self._showViews[cfg.layer]
    if viewArr == nil then
        return
    end
    viewArr:remove(view)
end

function ViewManager:_pushViewToCanvas(view, ...)
    if view == nil then
        return
    end
    -- todo 重新被打开的界面index要最大
    log("显示界面", view.viewName)
    local cfg = ViewConfig[view.viewName]
    ---@type UnityEngine.GameObject
    local layerNode = self._layerNodes[cfg.layer]
    view.isShow = true
    view:baseBeforeShow(...)
    view:addListener()
    view.transform:SetParent(layerNode.transform,false)
    view.gameObject:SetActive(true)
    self:_addViewToShowArr(view)
    view: baseAfterShow()
    self:_SortAndAdjustMask()
    self: _refreshShowViewPosZ(layerNode.transform)
end

---@param view BaseView
function ViewManager:_removeViewFromCanvas(view)
    if view == nil then
        return
    end
    log("隐藏界面", view.viewName)
    local cfg = ViewConfig[view.viewName]
    ---@type UnityEngine.GameObject
    local layerNode = self._layerNodes[cfg.layer]
    view.isShow = false
    self:_removeViewFormShowArr(view)
    view:removeListener()
    view:baseBeforeHide()
    view.transform:SetParent(self:getCloseViewLayer().transform)
    --view.gameObject:SetActive(false)
    view:baseAfterHide()
    self:_SortAndAdjustMask()
    self: _refreshShowViewPosZ(layerNode.transform)
end

---@see @屏幕缩放时，刷新所有的需要缩放的
function ViewManager:_ResizeScreenSize()
    self:_ResizeEveryLayer()
end

---@see @将所有层级的recttransform适配屏幕
function ViewManager:_ResizeEveryLayer()
    self.bangSize = ScreenResizeHelper.GetBangSize()
    self.railSize = ScreenResizeHelper.GetRailSize()
    local tempTotalSize = self.bangSize + self.railSize
    local tempMoveValue
    if self.bangSize > self.railSize then
        tempMoveValue = self.bangSize - self.railSize
    else
        tempMoveValue = self.railSize - self.bangSize
    end
    for i=0,ViewLayer.count-1,1 do
        local tempLayer = self._layerNodes[i]
        if not LuaUtil.isNilGameObject(tempLayer) then
            local tempLayerRect = tempLayer:GetComponent(typeof(UnityEngine.RectTransform))
            tempLayerRect.sizeDelta = Vector2(-tempTotalSize, 0)
            local tempLayerTransform = tempLayer:GetComponent(typeof(UnityEngine.Transform))
            tempLayerTransform.localPosition = Vector3.New(tempMoveValue,0, 25000 - i * 5000)
        end
    end
end

---@see @初始化不可见层
function ViewManager:_initCloseViewLayer()
    if not LuaUtil.isNilGameObject(self._layerNodes[ViewLayer.close]) then
        local tempLayer = self._layerNodes[ViewLayer.close]
        local tempLayerRect = tempLayer:GetComponent(typeof(UnityEngine.RectTransform))
        tempLayer:SetActive(false)
        tempLayerRect:SetParent(tempLayer.transform, false)
        tempLayerRect.anchorMin = Vector2(0, 0)
        tempLayerRect.anchorMax = Vector2(1, 1)
        tempLayerRect.sizeDelta = Vector2(0, 0)
        tempLayerRect.anchoredPosition = Vector2(10000, 10000)
    else
        LogError("CloseViewLayer is null")
    end
end

---@see @适配mask显示，只显示最上层的mask
function ViewManager:_SortAndAdjustMask()
    local tempReturnList = {}
    for i = 1, ViewLayer.count, 1 do
        local tempView = self._showViews[i]
        if tempView then
            local datas = tempView:getData()
            for k, v in pairs(datas) do
                table.insert(tempReturnList, v)
            end
        end
    end
    self:_AdjustMaskBg(tempReturnList)
end

---@see @适配mask遮罩背景界面
---@param canList BaseView @排序好的界面
function ViewManager:_AdjustMaskBg(canList)
    -- 从下往上，首个存在的mask的页面显示mask，其他的全部关闭
    local tempMaskBoo = false
    if #canList > 0 then
        for i = #canList, 1, -1 do
            local cfg = ViewConfig[canList[i].viewName]
            if cfg.isMask then
                if LuaUtil.isNilGameObject(self._maskBg) then
                    self:_CreateMask()
                end
                self._maskBg.transform:SetParent(canList[i].transform,false)
                self._maskBg.transform:SetSiblingIndex(0)
                self:_ResizeMaskBg()
                tempMaskBoo = true
                break
            end
        end
    end
    -- 如果没有界面需要maskBg则先放到不可见层
    if not tempMaskBoo then
        if not LuaUtil.isNilGameObject(self._maskBg) then
            self._maskBg.transform:SetParent(self._layerNodes[ViewLayer.close].transform,false)
        end
    end
end

---@see @遮罩背景初始化
function ViewManager:_CreateMask()
    self._maskBg = GameObject("MaskBg")
    local tempMaskBgImg = Util.AddComponent(self._maskBg, "UnityEngine.UI.Image")
    self._maskBgRect = self._maskBg:GetComponent("RectTransform")
    tempMaskBgImg.color = Color(0, 0, 0, 0.7)
end

---@see @屏幕缩放时，遮罩背景进行大小适配回调函数
function ViewManager:_ResizeMaskBg()
    if not LuaUtil.isNilGameObject(self._maskBg) then
        self._maskBg.transform.localPosition = Vector3.zero
        self._maskBg.transform.localScale = Vector3.one
        local tempScreenSize = ScreenResizeHelper.ScreenSize()
        self._maskBgRect.sizeDelta =
            Vector2(tempScreenSize.x + self._maskBgSizeOffset, tempScreenSize.y + self._maskBgSizeOffset)
    end
end


------------------内部方法----------------------

return ViewManager
