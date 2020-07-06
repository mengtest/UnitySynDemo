-- 视图基类
---@class BaseView:EventObj
BaseView = Class("BaseView",EventObj)
---@return BaseView

-- 类初始化，类似构造函数
function BaseView:initialize()
    self.viewName = "BaseView";
    self.needUpdate=false;
    self.isFin=false;
    self.Url="url..";
    self.ViewLayer=ViewLayer.content;

    self._isDestroyed=false;
    ---@type GameAssetRequest
    self._objrequest=nil;

    EventObj.initialize(self);
end

function BaseView:destory()
    --logError("BaseView 销毁")
    self:OnDestory()
    self.viewName = nil;
    self.transform = nil;
    self.needUpdate=false;
    self.ViewLayer=nil;
    
    self.gameObject = nil;
    if self.gameObject~=nil then 
        GameObject.Destroy(self.gameObject);
        self.gameObject = nil;
    end
    if self._objrequest~=nil then 
        self._objrequest:Unload();
        self._objrequest = nil;
    end
    self._isDestroyed=true;
    EventObj.destory(self);
end

---创建预制对象 ..通过prfab加载.
function BaseView: create()
    self._objrequest= GameLuaManager.LoadAsset(self.url,typeof(GameObject),
        function (objs)
            ---如果加载完时,已经销毁.
            if  self._isDestroyed then return end

            if objs then 
                ---@type GameObject
                self.gameObject =GameObject.Instantiate(objs[0]);
                ---@type Transform
                self.transform = self.gameObject:GetComponent(typeof(Transform));
                self.isFin=true;
                
                self:Init();
                
            end
        end
    );
end


---注册监听
function BaseView: RegisterEvent()
    self:AddListener();
end

function BaseView: UnRegisterEvent()
    self:RemoveListener();
end



-----------------------子类重写生命周期----------------------------

-- 创建视图之后，只执行一次
-- 处理适配等
function BaseView: Init()

end

function BaseView: AddListener()
    -- 专门用于添加UI组件的监听事件，只执行一次
end

function BaseView: RemoveListener()
    -- 移除监听
end

function BaseView: OnDestory()
    -- 当被销毁时，被销毁后不可再用
end

-----------------------子类重写生命周期----------------------------

-----------------------工具方法----------------------------

---@param btn UnityEngine.UI.Button
function BaseView: setBtnClick(btn, func)
    -- 设置按钮监听事件
    Utils.BtnAddlistener(btn, function () func(self) end)
end

---@param btn UnityEngine.UI.Toggle
function BaseView:setToggleClick(btnToggle,func)
    Utils.ToggleAddlistener(btnToggle,function(isON) func(self,isON) end)
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
    if Utils.isNilGameObject(canObj) then 
        return 
    end
    return canObj.transform:Find(canPath)
end
-----------------------工具方法----------------------------

return BaseView