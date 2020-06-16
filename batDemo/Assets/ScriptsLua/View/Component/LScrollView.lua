--------------------------------------------------------------------------------------
--- 支持横向和竖向，包括单行，单列，多行多列
--- 
--- 注意：ScrollRect必须设置Viewport，否则初始化会报错
--- 注意：Content必须加入GridLayoutGroup,否则初始化会报错（不强制，但建议padding都设置为0）
--- 注意：Content 不能加入 ContentSizeFitter,否者初始化会报错
--- 注意：Item不能加入LayoutElement 否则初始化会报错
--- 注意：Scrollview 必须设置一个 horizontal 或 vertical
--- 注意：建议界面销毁时，调用Destroy方法
---
---
--- 以下是为基本使用列子
--- 
--- 数据模型列表
--- local tempArr = {}
--- for i=1,8 do
---     local tempItem = {}
---     tempItem.str = "低级"..tostring(i)
---     table.insert(tempArr,tempItem)
--- end
--- 
--- 注意：itemMaker中自动带有Obj，Data，Index等3个属性，会自动绑定Item的GameObject对象，
---       以及自动将数据存到Data中，会自动存储索引值Index
--- 注意：如果Item是自带Button组件，scrollview会自动将button跟public.OnClick绑定
--- 注意：最好将组件获取，统一写在Init函数中
--- 
--- local itemMaker = function()
---     local this = {}
--- 
---     this.Init = function(canView,canGameObj,canScrollView,canParamList)
---         this.tempText = Util.GetChildComponent(this.Obj,"Text","Text")
---     end
---     this.Render = function(canData,canIndex)
---         this.tempText.text = canData.str
---     end
---     this.OnDestory = function(canIndex,canData)
--- 
---     end
---     this.OnRecycle = function(canIndex,canData)
--- 
---     end
--- 
---     this.OnClick = function(canIndex,canData)
---         print("on ddddddddddddddddddddddddd"..this.Data.str)
---     end
---     return this
--- end
--- 
--- local scrollView = LScrollView:new(self,ScrollRect组件,itemMaker)
--- self.scro.dataSource = tempArr
--- self.scro:RefreshAll()
--- Timer.once(2000,self,function()
---     table.insert(self.scro.dataSource,2,{str=2222})
---     self.scro:RefreshAll()
--- end)
--------------------------------------------------------------------------------------
---
--- 以下是scrollview提供的接口
---
--- 添加一个数据在最后
--- scrollView:Add(canData)
---
--- 在第3个位置插入一个，如果原长度不够2个，则添加到最后并警告
--- scrollView:Add(canData,3)
---
--- 在最后添加一个数组
--- scrollView:AddArray(canDataAry)
--- 
--- 添加Item数组，并且移除所有其他item数据
--- scrollView:AddAndRemoveAll(canDataAry)
---
--- 移除第3个
--- scrollView:Remove(3)
---
--- 移除全部
--- scrollView:RemoveAll()
---
--- 获取数据源的数量
--- scrollView:GetDataCount()
---
--- 刷新第3个，触发调用ItemMaker的show犯法 
--- scrollView:Refresh(3)
--- 
--- 使用新的数据刷新第3个，触发调用itemmaker的show方法
--- scrollView:RefreshWithData(3,canData)
--- 
--- 刷新全部，触发全部itemmaker的show方法
--- scrollView:RefreshAll()
---  
--- 获取当前位置，只返回单个数值，自动处理横向或竖向
--- scrollView:GetPosition()
--- 
--- 设置当前位置
--- scrollView:SetPosition(canValue)
--- 
--- 显示第3个，如果第3个完整显示在可显示区域，则不动，如果不在或不完整显示在可显示区域，则向下或向上滚动到显示第3个
--- scrollView:ShowIndex(3)
--- 
--- 将第3个置顶，如果内容长度不足以让第3个指定则不动（如：scrollview能显示4个，目前内容有4个，则scrollview只会滚动到最底部）
--- scrollView:SetIndex(3)
---
--- （已废弃）设置选中第3个，触发itemmaker的setselected的函数，参数为true
--- scrollView:SetSelectedIndex(3)
---
--- （已废弃）获取当前选中
--- scrollView:GetSelectIndex()
--- 
--- 屏幕滚动是否开启
--- scrollView:SetScrollEnabled(canActiveBoo)
--- 
--- 获取第3个数据对应的gameobject，如果第3个不在显示区域列表或列表不足3个，则返回false
--- scrollView:GetItemGameObjectByIndex(3)
--- 
--- 获取第3个数据
--- scrollView:GetItemByIndex(3)
--- 
--- 设置排序的方法
--- scrollView:Sort(canFunc)
--- 
--- 调用第3个的itemmaker的itemfuncname函数，并传递par1和par2参数
--- scrollView:CallItemFunction(3,itemfuncname,par1,par2)
--- 
--- 调用全部itemmaker的itemfuncname函数，并传递par1和par2参数
--- scrollView:CallAllItemFunction(itemfuncname,par1,par2)
--- 
--- 页面销毁时请调用
--- scrollView:Destroy()
---
--------------------------------------------------------------------------------------


local unpack = unpack or table.unpack

---滑动列表内容桥接类
---@class LScrollItemMaker
LScrollItemMaker = {}
LScrollItemMaker = Class("LScrollItemMaker")

function LScrollItemMaker:ctor(canView,canGameObject,canScrollView,canItemHandlerMaker,canItemInitParamList) 
    ---索引
    self.m_Index = 0
    ---内容数据
    self.m_Data = nil
    ---是否选中
    self.m_IsSelected = false
    
    ---ScrollView所在界面类
    self.m_View = canView
    ---此内容对应的Obj
    self.m_GameObject = canGameObject
    ---ScrollView类
    self.m_ScrollView = canScrollView
    ---对应外部内容逻辑类
    self.m_ItemHandlerMaker = canItemHandlerMaker()
    ---传入的参数列表
    self.m_ItemInitParamList = canItemInitParamList
end

function LScrollItemMaker:Reset()

end

function LScrollItemMaker:GetGameObject()
    return self.m_GameObject
end

function LScrollItemMaker:GetIndex()
    return self.m_Index
end

function LScrollItemMaker:GetData()
    return self.m_Data
end

---设置index和Data，将触发外部响应Show
function LScrollItemMaker:SetIndexAndData(canIndex,canData)
    self.m_Index = canIndex
    self.m_Data = canData
    if canIndex and canIndex > 0 then
        self:CallShow()
    end
end

---设置index和Data，将触发外部响应Show
function LScrollItemMaker:SetIndexAndDataNoShow(canIndex,canData)
    self.m_Index = canIndex
    self.m_Data = canData
end

-- ---设置选中状态，将触发外部响应SetSelected
-- function LScrollItemMaker:SetSelected(canSelectBoo)
--     if self.m_IsSelected ~= canSelectBoo then
--         self.m_IsSelected = canSelectBoo
--         self._CallSetSelected()
--     end
-- end

---设置显示或隐藏
function LScrollItemMaker:SetActive(canAtiveBoo)
    if self.m_GameObject then
        self.m_GameObject:SetActive(canAtiveBoo)
    end
end

---设置GameObject位置
function LScrollItemMaker:SetPosition(canX,canY)
    if self.m_GameObject then
        local tempNowPos = self.m_GameObject.transform.localPosition
        self.m_GameObject.transform.localPosition = Vector3(canX,canY,tempNowPos.z)
    end
end

---调用外部响应函数Init
function LScrollItemMaker:CallInit()
    self.m_ItemHandlerMaker.Obj = self.m_GameObject

    local tempButton = self.m_ItemHandlerMaker.Obj:GetComponent("Button")
    if tempButton and self.m_ItemHandlerMaker.OnClick then
        Util.BtnAddlistener(tempButton,function()
            self.m_ItemHandlerMaker.OnClick(self.m_Index,self.m_Data)
            -- self.m_ScrollView:SetSelectedIndex(self.m_ItemHandlerMaker.Index)
        end)
    elseif tempButton and not self.m_ItemHandlerMaker.OnClick then
        logWarn("组件item带有button，但是却不带有public.OnClick方法")
    end

    self:_CallItemHandler("Init", self.m_View, self.m_GameObject, self.m_ScrollView, unpack(self.m_ItemInitParamList))
end

---调用外部响应函数Show
function LScrollItemMaker:CallShow()

    self.m_ItemHandlerMaker.Data = self.m_Data
    self.m_ItemHandlerMaker.Index = self.m_Index

    self:_CallItemHandler("Render",self.m_Index,self.m_Data)
end

-- ---调用外部响应函数Selected
-- function LScrollItemMaker:_CallSetSelected()
--     self:_CallItemHandler("OnSelected",self.m_IsSelected)
-- end

---调用外部响应函数Destroy
function LScrollItemMaker:_CallDestroy()
    self:_CallItemHandler("OnDestory",self.m_Index,self.m_Data)
end

---调用外部响应函数Recycle
function LScrollItemMaker:CallRecycle()
    self:_CallItemHandler("OnRecycle",self.m_Index,self.m_Data)
end

---调用外部响应函数的方法
function LScrollItemMaker:_CallItemHandler(canFuncName,...)
    if self.m_ItemHandlerMaker and self.m_ItemHandlerMaker[canFuncName] then
        local arg = {...}
        return self.m_ItemHandlerMaker[canFuncName](unpack(arg))
    end
end

---滑动列表组件
--LScrollView = {}
---@class LScrollView
LScrollView = Class("LScrollView")

function LScrollView:ctor(canView,canScrollRect,canItemHandlerMaker,...)
    ---ScrollView所在界面类
    self.m_View = canView
    ---ScrollView所对应的列表Rect组件
    self.m_ScrollRect = canScrollRect
    ---对应外部内容逻辑类
    self.m_ItemHandlerMaker = canItemHandlerMaker

    ---第一个Item
    self.m_FirstItemGo = nil
    ---是否Vertical
    ---@type boolean
    self.m_IsVertical = nil
    ---初始化位置
    ---@type Vector2
    self.m_InitPosition = nil
    ---展示尺寸
    ---@type Vector2
    self.m_ShowSize = nil
    ---Cell尺寸
    ---@type Vector2
    self.m_CellSize = nil
    ---Spacing
    ---@type Vector2
    self.m_Spacing = nil
    ---Padding
    self.m_Padding = nil
    ---只有 isVertical = true 才会有数值
    self.m_MaxRow = nil
    ---只有 isVertical = false 才会有数值
    self.m_MaxColumn = nil


    ---显示中的Item桥接类列表
    self.m_ItemList = {}
    ---回收池中的Item桥接类列表
    self.m_RecycleItemList = {}
    ---Item数据列表，数据结构取决于使用者传入的数据
    self.dataSource = {}
    ---数据列表长度，用于判断是否要刷新content大小
    ---@type number
    self.m_ItemDataLastNum = 0

    ---开始索引
    self.m_BeginIndex = nil
    ---结束索引
    self.m_EndIndex = nil
    ---当前选中高亮索引
    self.m_SelectedIndex = nil
    ---拖动到最后时显示个数上限
    self.m_tailItemCount = nil

    ---外部传递给Item的Init函数
    self.m_ItemInitParamList = {...}

    ---Content
    self.m_Content = canScrollRect.content
    ---ContentObj
    self.m_ContentGo = self.m_Content.gameObject

    local tempViewport = self.m_ScrollRect.viewport
    local tempGridLayoutGroup = Util.GetChildComponent(self.m_ContentGo, "","GridLayoutGroup")

    if not tempViewport then
        return logError("[ScrollView] Init GridView的ScrollRect必须设置viewport")
    end
    if not tempGridLayoutGroup then
        return logError("[ScrollView] Init Content下必须加入GridLayoutGroup")
    end
    
    local tempContentChildNum = self.m_ContentGo.transform.childCount
    if tempContentChildNum > 1 then
        while self.m_ContentGo.transform.childCount > 1 do
            GameObject.DestroyImmediate(self.m_ContentGo.transform:GetChild(1).gameObject)
        end
    elseif tempContentChildNum < 1 then
        return logError("[ScrollView] Init Content下必须有一个对象")
    end

    self.m_FirstItemGo = self.m_ContentGo.transform:GetChild(0).gameObject

    if Util.GetChildComponent(self.m_FirstItemGo,"", "LayoutElement") then
        return logError("[ScrollView] Init Item下不要加入LayoutElement")
    end
    if Util.GetChildComponent(self.m_ContentGo , "", "ContentSizeFitter") then
        return logError("[ScrollView] Init content下不要加入ContentSizeFitter")
    end
    if self.m_ScrollRect.vertical == self.m_ScrollRect.horizontal then
        return logError("[ScrollView] Init GridView的ScrollRect设置vertical或horizontal有且只能有一个为true")
    end

    --第一个Item的GameObject不给使用者使用
    self.m_FirstItemGo:SetActive(false)

    self.m_InitPosition = {x = self.m_ContentGo.transform.localPosition.x, y = self.m_ContentGo.transform.localPosition.y}

    self:RefreshSize()

    -- self.m_View:AddComponentCallbackListnener(self.m_ScrollRect,"onValueChanged",
    -- function(canVector2) 
    --     self:_OnValueChanged() 
    -- end)
    Util.AddScrollViewOnValueChangeListener(self.m_ScrollRect,
    function(canVector2) 
        self:_OnValueChanged()
    end)

    --PC端调节滚轮灵敏度
    if AppConst.UNITY_STANDALONE then
        self.m_ScrollRect.scrollSensitivity = 40
    end
end

---获得数据的数量
function LScrollView:GetDataCount()
    if self.dataSource then
        return #self.dataSource
    else
        return 0
    end
end

---刷新尺寸（方法有点问题，暂时不方便使用）
function LScrollView:RefreshSize()
    local tempViewport = self.m_ScrollRect.viewport
    local tempGridLayoutGroup = Util.GetChildComponent(self.m_ContentGo,"","GridLayoutGroup")
    local cellSize = Util.GetGridLayoutGroupCellSize(tempGridLayoutGroup)
    local padding = Util.GetGridLayoutGroupPadding(tempGridLayoutGroup)
    --整理数量
    local tempViewportRect = tempViewport.gameObject.transform.rect
    self.m_IsVertical = self.m_ScrollRect.vertical
    self.m_ShowSize = {x=tempViewportRect.width,y=tempViewportRect.height}
    self.m_CellSize = {x=cellSize.x,y=cellSize.y}
    self.m_Spacing = {x=tempGridLayoutGroup.spacing.x,y=tempGridLayoutGroup.spacing.y}
    -- self.m_Padding = {left= tempGridLayoutGroup.padding.left,
    -- right=tempGridLayoutGroup.padding.right,
    -- top=tempGridLayoutGroup.padding.top,
    -- bottom =tempGridLayoutGroup.padding.bottom}
    self.m_Padding = {left= padding.x,
    right=padding.y,
    top=padding.z,
    bottom =padding.w}

    --初始化组件默认值
    tempGridLayoutGroup.enabled = false
    local firstItemTransform = self.m_FirstItemGo.transform
    firstItemTransform.sizeDelta = Vector2(self.m_CellSize.x,self.m_CellSize.y)
    firstItemTransform.anchorMin = Vector2(0,1)
    firstItemTransform.anchorMax = Vector2(0,1)
    firstItemTransform.pivot = Vector2(0,1)

    if self.m_IsVertical then
        self.m_Content.anchorMin = Vector2(0,1)
        self.m_Content.anchorMax = Vector2(1,1)
    else
        self.m_Content.anchorMin = Vector2(0,0)
        self.m_Content.anchorMax = Vector2(0,1)
    end
    self.m_Content.pivot = Vector2(0,1)

    ---计算显示Item数量(包括多显示一行或一列)
    if self.m_IsVertical then
        self.m_MaxColumn = math.max(1,math.floor((self.m_ShowSize.x - self.m_Padding.left - self.m_Padding.right + self.m_Spacing.x)/(self.m_CellSize.x + self.m_Spacing.x)))
        self.m_MaxRow = false
        self.m_tailItemCount = math.ceil((self.m_ShowSize.y - self.m_Padding.bottom + self.m_Spacing.y)/(self.m_CellSize.y+self.m_Spacing.y))*self.m_MaxColumn
    else
        self.m_MaxRow = math.max(1,math.floor((self.m_ShowSize.y - self.m_Padding.top - self.m_Padding.bottom + self.m_Spacing.y)/(self.m_CellSize.y+self.m_Spacing.y)))
        self.m_MaxColumn = false
        self.m_tailItemCount = math.ceil((self.m_ShowSize.x - self.m_Padding.right + self.m_Spacing.x)/(self.m_CellSize.x + self.m_Spacing.x)) * self.m_MaxRow
    end

    --刷新一次界面
    self:_OnValueChanged()
end

---构造Item对象
function LScrollView:_NewItem(canGameObject)
    return LScrollItemMaker:new(self.m_View,canGameObject,self,self.m_ItemHandlerMaker,self.m_ItemInitParamList)
end

---优先从回收列表中获取，如果没有就创建出来，并设置显示
function LScrollView:_GetFreeItem()
    ---@type LScrollItemMaker
    local tempItem
    if #self.m_RecycleItemList > 0 then
        tempItem = table.remove(self.m_RecycleItemList,1)
    else
        tempItem = self:_NewItem(Util.AddChild(self.m_ContentGo,self.m_FirstItemGo))
        tempItem:CallInit()
    end
    tempItem:SetIndexAndData(nil,nil)
    tempItem:SetActive(true)
    return tempItem
end

---回收Item,并设置隐藏，提供下次使用
---@param canItem LScrollItemMaker
function LScrollView:_RecycleItem(canItem)
    canItem:SetActive(false)
    for tempIndex, tempItem in ipairs(self.m_ItemList) do
        if tempItem == canItem then
            canItem:CallRecycle()
            table.remove(self.m_ItemList,tempIndex)
            break
        end
    end
    self.m_RecycleItemList[#self.m_RecycleItemList+1] = canItem
end

---滚动响应处理
function LScrollView:_OnValueChanged()
    --计算并更新Content大小
    local tempItemDataListCount = #self.dataSource
    if tempItemDataListCount ~= self.m_ItemDataLastNum then
        self.m_ItemDataLastNum = tempItemDataListCount
        if self.m_IsVertical then
            local tempRowNum = math.ceil(tempItemDataListCount/self.m_MaxColumn)
            local tempContentHeight = tempRowNum *  self.m_CellSize.y + math.max(tempRowNum - 1,0)*self.m_Spacing.y+self.m_Padding.top+self.m_Padding.bottom
            self.m_ContentGo.transform.sizeDelta = Vector2(0,tempContentHeight)
        else
            local tempColumn = math.ceil(tempItemDataListCount / self.m_MaxRow)
            local tempContentWidth = tempColumn * self.m_CellSize.x + math.max(tempColumn-1,0) * self.m_Spacing.x + self.m_Padding.left +self.m_Padding.right
            self.m_ContentGo.transform.sizeDelta = Vector2(tempContentWidth,0)
        end
    end
    
    if tempItemDataListCount == 0 then
        self.m_BeginIndex = 0
        self.m_EndIndex = 0
        self.m_SelectedIndex = false
        while #self.m_ItemList > 0 do
            self:_RecycleItem(self.m_ItemList[1])
        end
    else
        local contentPosition = self.m_ContentGo.transform.localPosition
        local contentX = contentPosition.x
        local contentY = contentPosition.y
        local contenRect = self.m_ContentGo.transform.rect
        local contentWidth = contenRect.width
        local contentHeight = contenRect.height

        --计算核心部分，计算得到显示第一个和最后一个的索引
        if self.m_IsVertical then
            if (contentY - contentHeight) > (self.m_InitPosition.y - self.m_ShowSize.y) then
                self.m_EndIndex = tempItemDataListCount
                self.m_BeginIndex = math.max(1,math.ceil(tempItemDataListCount/self.m_MaxColumn)*self.m_MaxColumn - self.m_tailItemCount + 1)
            else
                local overTopHeight = math.max(0,contentY - self.m_InitPosition.y - self.m_Padding.top)
                local hideRow = math.floor((overTopHeight + self.m_Spacing.y) / (self.m_CellSize.y + self.m_Spacing.y))
                local showRow = math.ceil((overTopHeight + self.m_ShowSize.y) / (self.m_CellSize.y + self.m_Spacing.y))
                self.m_BeginIndex = hideRow * self.m_MaxColumn + 1
                self.m_EndIndex = math.min(showRow * self.m_MaxColumn,tempItemDataListCount)
            end
        else
            if (contentX + contentWidth) < (self.m_InitPosition.x + self.m_ShowSize.x) then
                self.m_EndIndex = tempItemDataListCount
                self.m_BeginIndex = math.max(1,math.ceil(tempItemDataListCount/self.m_MaxRow)*self.m_MaxRow - self.m_tailItemCount + 1)
            else
                local overLeftWidth = math.max(0,self.m_InitPosition.x - contentX - self.m_Padding.left)
                local hideColumn = math.floor((overLeftWidth + self.m_Spacing.x) /(self.m_CellSize.x +self.m_Spacing.x))
                local showColumn = math.ceil((overLeftWidth+self.m_ShowSize.x)/(self.m_CellSize.x+self.m_Spacing.x))
                self.m_BeginIndex = hideColumn * self.m_MaxRow + 1
                self.m_EndIndex = math.min(showColumn * self.m_MaxRow, tempItemDataListCount)
            end
        end

        --先删除不符合条件的Item
        for index = #self.m_ItemList, 1, -1 do
            local itemIndex = self.m_ItemList[index]:GetIndex()
            if itemIndex < self.m_BeginIndex or itemIndex > self.m_EndIndex then
                self:_RecycleItem(self.m_ItemList[index])
            end
        end

        --再添加需要显示的内容
        local itemIndex = 1
        for itemDataIndex = self.m_BeginIndex, self.m_EndIndex do
            local itemData = self.dataSource[itemDataIndex]
            ---@type LScrollItemMaker
            local item = self.m_ItemList[itemIndex]
            if item and item:GetIndex() == itemDataIndex then

            else
                item = self:_GetFreeItem()
                table.insert(self.m_ItemList,itemIndex,item)
                item:SetIndexAndData(itemDataIndex,itemData)
            end
            -- item:SetSelected(itemDataIndex == self.m_SelectedIndex)
            itemIndex = itemIndex + 1
        end

        --遍历整理item位置
        for _,item in ipairs(self.m_ItemList) do
            local index = item:GetIndex()
            local row, column
            if self.m_IsVertical then
                row = math.ceil(index / self.m_MaxColumn)
                column = index % self.m_MaxColumn
                if column == 0 then 
                    column = self.m_MaxColumn 
                end
            else
                column = math.ceil(index/self.m_MaxRow)
                row = index % self.m_MaxRow
                if row == 0 then
                    row = self.m_MaxRow
                end
            end
            local itemX = self.m_Padding.left + (column-1) * (self.m_CellSize.x + self.m_Spacing.x)
            local itemY = - self.m_Padding.top - (row - 1) * (self.m_CellSize.y + self.m_Spacing.y)
            
            if self.Getpos then
                itemX,itemY = self.Getpos(index)
            end

            item:SetPosition(itemX,itemY)
        end

    end

end

function LScrollView:_OnValueChangedNoShow()
    --计算并更新Content大小
    local tempItemDataListCount = #self.dataSource
    if tempItemDataListCount ~= self.m_ItemDataLastNum then
        self.m_ItemDataLastNum = tempItemDataListCount
        if self.m_IsVertical then
            local tempRowNum = math.ceil(tempItemDataListCount/self.m_MaxColumn)
            local tempContentHeight = tempRowNum *  self.m_CellSize.y + math.max(tempRowNum - 1,0)*self.m_Spacing.y+self.m_Padding.top+self.m_Padding.bottom
            self.m_ContentGo.transform.sizeDelta = Vector2(0,tempContentHeight)
        else
            local tempColumn = math.ceil(tempItemDataListCount / self.m_MaxRow)
            local tempContentWidth = tempColumn * self.m_CellSize.x + math.max(tempColumn-1,0) * self.m_Spacing.x + self.m_Padding.left +self.m_Padding.right
            self.m_ContentGo.transform.sizeDelta = Vector2(tempContentWidth,0)
        end
    end
    
    if tempItemDataListCount == 0 then
        self.m_BeginIndex = 0
        self.m_EndIndex = 0
        self.m_SelectedIndex = false
        while #self.m_ItemList > 0 do
            self:_RecycleItem(self.m_ItemList[1])
        end
    else
        local contentPosition = self.m_ContentGo.transform.localPosition
        local contentX = contentPosition.x
        local contentY = contentPosition.y
        local contenRect = self.m_ContentGo.transform.rect
        local contentWidth = contenRect.width
        local contentHeight = contenRect.height

        --计算核心部分，计算得到显示第一个和最后一个的索引
        if self.m_IsVertical then
            if (contentY - contentHeight) > (self.m_InitPosition.y - self.m_ShowSize.y) then
                self.m_EndIndex = tempItemDataListCount
                self.m_BeginIndex = math.max(1,math.ceil(tempItemDataListCount/self.m_MaxColumn)*self.m_MaxColumn - self.m_tailItemCount + 1)
            else
                local overTopHeight = math.max(0,contentY - self.m_InitPosition.y - self.m_Padding.top)
                local hideRow = math.floor((overTopHeight + self.m_Spacing.y) / (self.m_CellSize.y + self.m_Spacing.y))
                local showRow = math.ceil((overTopHeight + self.m_ShowSize.y) / (self.m_CellSize.y + self.m_Spacing.y))
                self.m_BeginIndex = hideRow * self.m_MaxColumn + 1
                self.m_EndIndex = math.min(showRow * self.m_MaxColumn,tempItemDataListCount)
            end
        else
            if (contentX + contentWidth) < (self.m_InitPosition.x + self.m_ShowSize.x) then
                self.m_EndIndex = tempItemDataListCount
                self.m_BeginIndex = math.max(1,math.ceil(tempItemDataListCount/self.m_MaxRow)*self.m_MaxRow - self.m_tailItemCount + 1)
            else
                local overLeftWidth = math.max(0,self.m_InitPosition.x - contentX - self.m_Padding.left)
                local hideColumn = math.floor((overLeftWidth + self.m_Spacing.x) /(self.m_CellSize.x +self.m_Spacing.x))
                local showColumn = math.ceil((overLeftWidth+self.m_ShowSize.x)/(self.m_CellSize.x+self.m_Spacing.x))
                self.m_BeginIndex = hideColumn * self.m_MaxRow + 1
                self.m_EndIndex = math.min(showColumn * self.m_MaxRow, tempItemDataListCount)
            end
        end

        --先删除不符合条件的Item
        for index = #self.m_ItemList, 1, -1 do
            local itemIndex = self.m_ItemList[index]:GetIndex()
            if itemIndex < self.m_BeginIndex or itemIndex > self.m_EndIndex then
                self:_RecycleItem(self.m_ItemList[index])
            end
        end

        --再添加需要显示的内容
        local itemIndex = 1
        for itemDataIndex = self.m_BeginIndex, self.m_EndIndex do
            local itemData = self.dataSource[itemDataIndex]
            ---@type LScrollItemMaker
            local item = self.m_ItemList[itemIndex]
            if item and item:GetIndex() == itemDataIndex then

            else
                item = self:_GetFreeItem()
                table.insert(self.m_ItemList,itemIndex,item)
                item:SetIndexAndDataNoShow(itemDataIndex,itemData)
            end
            -- item:SetSelected(itemDataIndex == self.m_SelectedIndex)
            itemIndex = itemIndex + 1
        end

        --遍历整理item位置
        for _,item in ipairs(self.m_ItemList) do
            local index = item:GetIndex()
            local row, column
            if self.m_IsVertical then
                row = math.ceil(index / self.m_MaxColumn)
                column = index % self.m_MaxColumn
                if column == 0 then 
                    column = self.m_MaxColumn 
                end
            else
                column = math.ceil(index/self.m_MaxRow)
                row = index % self.m_MaxRow
                if row == 0 then
                    row = self.m_MaxRow
                end
            end
            local itemX = self.m_Padding.left + (column-1) * (self.m_CellSize.x + self.m_Spacing.x)
            local itemY = - self.m_Padding.top - (row - 1) * (self.m_CellSize.y + self.m_Spacing.y)
            
            if self.Getpos then
                itemX,itemY = self.Getpos(index)
            end

            item:SetPosition(itemX,itemY)
        end

    end

end

---Getpos 外部传入的方法，返回第index个item的位置
---scrollView.Getpos(index)

---添加Item
---参数index为可选参数，不填将在当前的最后添加
function LScrollView:Add(data,index)
    local tempItemDataListCount = #self.dataSource
    if type(index) ~= "number" then
        index = tempItemDataListCount + 1
    end

    if index < 1 then
        logWarn("[ScrollView] Add 添加的索引小于1，后续将重置为1后执行，index={0}",index)
        index = 1
    elseif index > tempItemDataListCount + 1 then
        logWarn("[ScrollView] Add 添加的索引大于列表数量+1，后续将重置为列表数量+1后执行，index={0}",index)
        index = tempItemDataListCount + 1
    end
    table.insert(self.dataSource,index,data)
    for _,item in ipairs(self.m_ItemList) do
        local itemIndex = item:GetIndex()
        if itemIndex >= index then
            item:SetIndexAndData(itemIndex,self.dataSource[itemIndex])
        end
    end
    self:_OnValueChanged()
end

--播放渐入动画 no edit
function LScrollView:PlayAnimation()

end

---添加Item数组，接入到Item尾部
function LScrollView:AddArray(canArray)
    local tempCount = #self.dataSource
    for _,data in ipairs(canArray) do
        self.dataSource[tempCount+1] = data
        tempCount = tempCount + 1
    end
    self:_OnValueChanged()
end

---添加Item数组，并且移除所有其他item数据
function LScrollView:AddAndRemoveAll(canArray)
    self:RemoveAll()
    local tempCount = #self.dataSource
    for _,data in ipairs(canArray) do
        self.dataSource[tempCount+1] = data
        tempCount = tempCount + 1
    end
    self:_OnValueChanged()
end

---移除Item
function LScrollView:Remove(canIndex)
    if canIndex < 1 or canIndex > #self.dataSource then
        return logWarn("[ScrollView] Remove 移除的索引超出范围 index = {0}",canIndex)
    end
    table.remove(self.dataSource,canIndex)
    local todoRecycleItemAry = {}
    for _,tempItem in ipairs(self.m_ItemList) do
        local tempItemIndex = tempItem:GetIndex()
        if tempItemIndex == canIndex then
            todoRecycleItemAry[#todoRecycleItemAry+1] = tempItem
        elseif tempItemIndex > #self.dataSource then
            todoRecycleItemAry[#todoRecycleItemAry+1] = tempItem
        elseif tempItemIndex > canIndex then
            tempItem:SetIndexAndData(tempItemIndex,self.dataSource[tempItemIndex])
        end
    end
    for _,item in ipairs(todoRecycleItemAry) do
        self:_RecycleItem(item)
    end
    self:_OnValueChanged()
end

---移除全部Item
function LScrollView:RemoveAll()
    self.dataSource = {}
    if self.m_IsVertical then
        self:SetPosition(self.m_InitPosition.y)
    else
        self:SetPosition(self.m_InitPosition.x)
    end
end

---刷新指定Item
function LScrollView:Refresh(canIndex)
    if canIndex < 1 or canIndex > #self.dataSource then
        return logWarn("[ScrollView] Refresh 刷新索引超出范围 index = {0}",canIndex)
    end
    for _,tempItem in ipairs(self.m_ItemList) do
        if tempItem:GetIndex() == canIndex then
            tempItem:CallShow()
            break
        end
    end
end

---刷新指定Item并且更新数据
function LScrollView:RefreshWithData(canIndex,canData)
    if canIndex < 1 or canIndex > #self.dataSource then
        return logWarn("[ScrollView] Refresh 刷新索引超出范围 index = {0}",canIndex)
    end
    self.dataSource[canIndex] = canData
    for _,tempItem in ipairs(self.m_ItemList) do
        if tempItem:GetIndex() == canIndex then
            tempItem:SetIndexAndData(canIndex,canData)
            break
        end
    end
end

---移除原有的所有项，根据新数据，重新计算生成，慎用
function LScrollView:RefreshAll()
    self:_OnValueChangedNoShow()
    self:_RefreshAllData()
    -- for _,tempItem in ipairs(self.m_ItemList) do
    --     tempItem:CallShow()
    -- end
end

---重新调用一遍所有项的Render，不会重新生成项
function LScrollView: RefreshAllRender()
    self: _RefreshAllData()
end

function LScrollView:_RefreshAllData()
    for _,item in ipairs(self.m_ItemList) do
        local itemIndex = item:GetIndex()
        local tempData = self.dataSource[itemIndex]
        if tempData then
            item:SetIndexAndData(itemIndex,tempData)
        end
    end
end

---获取滚动位置，根据isVertical获取横向或竖向的坐标
function LScrollView:GetPosition()
    if self.m_IsVertical then
        return self.m_ContentGo.transform.localPosition.y
    else
        return self.m_ContentGo.transform.localPosition.x
    end
end

---设置滚动位置，根据isVertical设置横向或者竖向的坐标
function LScrollView:SetPosition(canValue)
    self.m_ScrollRect:StopMovement()
    local tempOldPos = self.m_ContentGo.transform.localPosition
    local tempNewPos
    if self.m_IsVertical then
        canValue = math.max(self.m_InitPosition.y,canValue)
        canValue = math.min(self.m_InitPosition.y+math.max(self.m_ContentGo.transform.rect.height-self.m_ShowSize.y,0),canValue)
        tempNewPos = Vector3(tempOldPos.x,canValue,tempOldPos.z)
    else
        canValue = math.min(self.m_InitPosition.x,canValue)
        canValue = math.max(self.m_InitPosition.x - math.max(self.m_ContentGo.transform.rect.width-self.m_ShowSize.x,0),canValue)
        tempNewPos = Vector3(canValue,tempOldPos.y,tempOldPos.z)
    end
    self.m_ContentGo.transform.localPosition = tempNewPos
    self:_OnValueChanged()
end

---强制设置滚动位置，不考虑滚动条长度
function LScrollView:SetPositionForce(canValue)
    self.m_ScrollRect:StopMovement()
    local tempOldPos = self.m_ContentGo.transform.localPosition
    local tempNewPos
    if self.m_IsVertical then
        tempNewPos = Vector3(tempOldPos.x,canValue,tempOldPos.z)
    else
        tempNewPos = Vector3(canValue,tempOldPos.y,tempOldPos.z)
    end
    self.m_ContentGo.transform.localPosition = tempNewPos
    self:_OnValueChanged()
end

---设置显示指定Item
function LScrollView:ShowIndex(canIndex)
    if canIndex < 1 or canIndex > #self.dataSource then
        return logWarn("[ScrollView] ShowIndex 索引超出范围 index = {0}",canIndex)
    end
    self.m_ScrollRect:StopMovement()
    local tempContentPosition = self.m_ContentGo.transform.localPosition
    local tempContenX = tempContentPosition.x
    local tempContenY = tempContentPosition.y
    if self.m_IsVertical then
        local tempBeginIndex = math.ceil((tempContenY - self.m_InitPosition.y - self.m_Padding.top + self.m_Spacing.y)/(self.m_CellSize.y+self.m_Spacing.y)) + 1
        local tempEndRow = math.floor((tempContenY - self.m_InitPosition.y + self.m_ShowSize.y - self.m_Padding.top + self.m_Spacing.y)/(self.m_CellSize.y + self.m_Spacing.y))
        local tempShowRow = math.ceil(canIndex/self.m_MaxColumn) * self.m_MaxColumn / self.m_MaxColumn
        if tempShowRow < tempBeginIndex then
            self:SetPosition(self.m_InitPosition.y+(self.m_Padding.top+math.max(tempShowRow-1,0)*self.m_CellSize.y+math.max(tempShowRow-1,0)*self.m_Spacing.y))
        elseif tempShowRow > tempEndRow then
            self:SetPosition(self.m_InitPosition.y+(self.m_Padding.top+tempShowRow*self.m_CellSize.y+math.max(tempShowRow-1,0)*self.m_Spacing.y+
            (canIndex == #self.dataSource and self.m_Padding.bottom or 0)- self.m_ShowSize.y))
        end
    else
        local tempBeginColumn = math.ceil((self.m_InitPosition.x - tempContenX - self.m_Padding.left + self.m_Spacing.x) / (self.m_CellSize.x+self.m_Spacing.x)) + 1
        local tempEndColumn = math.floor((self.m_InitPosition.x - tempContenX + self.m_ShowSize.x - self.m_Padding.left + self.m_Spacing.x)/(self.m_CellSize.x + self.m_Spacing.x))
        local tempShowColumn = math.ceil(canIndex/self.m_MaxRow) * self.m_MaxRow / self.m_MaxRow
        if tempShowColumn < tempBeginColumn then
            self:SetPosition(self.m_InitPosition.x - (self.m_Padding.left + math.max(tempShowColumn -1,0)*self.m_CellSize.x+math.max(tempShowColumn-1,0)*self.m_Spacing.x))
        elseif tempShowColumn > tempEndColumn then
            self:SetPosition(self.m_InitPosition.x - (self.m_Padding.left + tempShowColumn * self.m_CellSize.x + math.max(tempShowColumn-1,0)*
            self.m_Spacing.x+(canIndex == #self.dataSource and self.m_Padding.right or 0) - self.m_ShowSize.x))
        end
    end
end

---设置指定Item置顶
function LScrollView:SetIndex(canIndex)
    if #self.dataSource > 0 then
        if canIndex < 1 then
            canIndex = 1
        elseif canIndex > #self.dataSource then
            canIndex = #self.dataSource
        end
        if self.m_IsVertical then
            local tempShowRow = math.ceil(canIndex/self.m_MaxColumn)*self.m_MaxColumn/self.m_MaxColumn
            self:SetPosition(self.m_InitPosition.y+(self.m_Padding.top+math.max(tempShowRow-1,0)*self.m_CellSize.y+math.max(tempShowRow-1,0)*self.m_Spacing.y))

        else
            local tempShowColumn = math.ceil(canIndex/self.m_MaxRow)*self.m_MaxRow/self.m_MaxRow
            self:SetPosition(self.m_InitPosition.x - (self.m_Padding.left+math.max(tempShowColumn-1,0)*self.m_CellSize.x+math.max(tempShowColumn-1,0)*self.m_Spacing.x))
        end
    end
end

-- ---设置选中
-- function LScrollView:SetSelectedIndex(canIndex)
--     self.m_SelectedIndex = canIndex
--     for _,tempItem in ipairs(self.m_ItemList) do
--         tempItem:SetSelected(tempItem:GetIndex() == self.m_SelectedIndex)
--     end
-- end

-- ---获取选中
-- function LScrollView:GetSelectIndex()
--     return self.m_SelectedIndex
-- end

---设置是否允许滚动
function LScrollView:SetScrollEnabled(canBoo)
    self.m_ScrollRect.vertical = canBoo and self.m_IsVertical
    self.m_ScrollRect.horizontal = canBoo and not self.m_IsVertical
end

---获取某个index的item或者itemObj
function LScrollView:GetItemGameObjectByIndex(canIndex,canIsReturnItem)
    for _,tempItem in ipairs(self.m_ItemList) do
        if tempItem:GetIndex() == canIndex then
            if canIsReturnItem then
                return tempItem
            else
                return tempItem:GetGameObject()
            end
        end
    end
    return false
end

---获取某个index的数据
function LScrollView:GetItemByIndex(canIndex)
    return self.dataSource[canIndex] or false
end

---排序
function LScrollView:Sort(canFunc)
    table.Sort(self.dataSource,canFunc)
    for _,tempItem in ipairs(self.m_ItemList) do
        local tempItemIndex = tempItem:GetIndex()
        tempItem:SetIndexAndData(tempItemIndex,self.dataSource[tempItemIndex])
    end
    self:_OnValueChanged()
end

---获取指定指引的数据
function LScrollView:GetDataByIndex(canIndex)
    if type(canIndex) == "number" then
        return self.dataSource[canIndex]
    end
end

---调用第3个的itemmaker的itemfuncname函数，并传递par1和par2参数
function LScrollView:CallItemFunction(canIndex,canFuncName,...)
    local tempPartList = {...}
    local tempItem = self.m_ItemList[canIndex]
    if tempItem then
        tempItem:CallItemHandler(canFuncName,unpack(tempPartList))
    end
end

---调用指定Item的指定方法
function LScrollView:CallAllItemFunction(canFuncName,...)
    local tempPartList = {...}
    for _,tempItem in ipairs(self.m_ItemList) do
        tempItem:CallItemHandler(canFuncName,unpack(tempPartList))
    end
end

---外部在页面销毁时候调用
function LScrollView:Destroy()
    self:RemoveAll()
    for _,tempItem in ipairs(self.m_ItemList) do
        tempItem:CallDestroy()
    end
    self.m_ItemList = {}
    for _,tempItem in ipairs(self.m_RecycleItemList) do
        tempItem:CallDestroy()
    end
    self.m_RecycleItemList = {}
end