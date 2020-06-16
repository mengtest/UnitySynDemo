-- 视图基类
---@class BaseView
local BaseView = Class("BaseView")
---@return BaseView
function createView(viewName)
    local c = Class(viewName, BaseView)
    return c
end

function BaseView: create(uid, viewName, transform, gameObject, rectTransform)
    self.uid = uid
    self.viewName = viewName
    ---@type UnityEngine.Transform
    self.transform = transform
    ---@type UnityEngine.GameObject
    self.gameObject = gameObject
    ---@type UnityEngine.RectTransform
    self.rectTransform = rectTransform
    self.isShow = false

    local e=self.transform:GetComponentsInChildren(typeof(UnityEngine.UI.Text),true):GetEnumerator()
    while e:MoveNext() do
        local txt=e.Current
        local name=txt.name
        name=name:gsub("lg_","",1)
        if txt.resizeTextForBestFit == false then
            txt.resizeTextForBestFit = true
            txt.resizeTextMinSize = 14
            txt.resizeTextMaxSize = txt.fontSize
        end
    end
    e=self.transform:GetComponentsInChildren(typeof(UnityEngine.UI.Button),true):GetEnumerator()
    while e:MoveNext() do
        ---@type UnityEngine.UI.Button
        local btn=e.Current
        if btn.onClick:GetPersistentEventCount()<1 and self["on_"..btn.name] then
            Util.BtnAddlistener(btn,function()
                SoundHelper.playClick()
                self["on_"..btn.name](self)
            end,true)
        end
    end
    --监听toggle
    --e= self.transform:GetComponentsInChildren(typeof(UnityEngine.UI.Toggle),true):GetEnumerator()
    --while e:MoveNext() do
    --    local toggle= e.Current
    --    if toggle.onValueChanged(isOn)and self["on_"..toggle.name] then
    --        Util.ToggleAddlistener(toggle,function (isOn) self["on"..toggle.name](self,isOn)
    --        end,true)
    --    end
    --end

    if self["Init"] then
        self:Init()
    end
    self:baseCreateView()
    self:addUIListener()
end

-----------------------子类重写生命周期----------------------------

function BaseView: baseCreateView()
    -- 创建视图之后，只执行一次
    -- 处理适配等
end

function BaseView: addUIListener()
    -- 专门用于添加UI组件的监听事件，只执行一次
end

function BaseView: baseBeforeShow(...)
    -- 视图显示之前，这个时候已经创建好了
end

function BaseView: baseAfterShow()
    -- 视图显示之后
end

function BaseView: baseBeforeHide()
    -- 视图隐藏之前
end

function BaseView: baseAfterHide()
    -- 视图隐藏之后
end

function BaseView: addListener()
    -- 添加监听
end

function BaseView: removeListener()
    -- 移除监听
end

function BaseView: baseOnDestory()
    -- 当被销毁时，被销毁后不可再用
end

-----------------------子类重写生命周期----------------------------

-----------------------工具方法----------------------------

---@param btn UnityEngine.UI.Button
function BaseView: setBtnClick(btn, func)
    -- 设置按钮监听事件
    Util.BtnAddlistener(btn, function () func(self) end)
end

---@param btn UnityEngine.UI.Toggle
function BaseView:setToggleClick(btnToggle,func)
    Util.ToggleAddlistener(btnToggle,function(isON) func(self,isON) end)
end
---@param name string
function BaseView: Find(name)
    return self.transform:Find(name)
end

---@param name string
---@param compname string
function BaseView: SubGet(name,compname)
    return self:Find(name):GetComponent(compname)
end

---从某个对象中的子对象中，寻找路径为canPath的对象
function BaseView:FindChild(canObj,canPath)
    if LuaUtil.isNilGameObject(canObj) then 
        return 
    end
    return canObj.transform:Find(canPath)
end
-----------------------工具方法----------------------------

function BaseView: destory()
    --logError("BaseView 销毁")
    if self.gameObject == nil then
        return
    end
    self: baseOnDestory()
    -- todo
    UnityEngine.GameObject.Destroy(self.gameObject)
end

return BaseView