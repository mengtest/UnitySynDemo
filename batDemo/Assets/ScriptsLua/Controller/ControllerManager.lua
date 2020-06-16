-- 引入各控制器
require("Controller.TestController")


---@class ControllerManager
local ControllerManager = Class("ControllerManager")

function ControllerManager: ctor()
    log("ControllerManager ctor")
    self._uid = 0
    self._ctrlMap = {}

    -- 注册控制器,注册后才能使用
    self:_createCtrl(TestController)

    self:addEventListener()
end

function ControllerManager: addEventListener()
    table.doFunc(self._ctrlMap, function (k, v)
        ---@type BaseController
        local ctrl = v
        ctrl: addEventListener()
    end)
end

-- 启动所有控制器
function ControllerManager: beginAllController()
    table.doFunc(self._ctrlMap, function (k, v)
        ---@type BaseController
        local ctrl = v
        ctrl:begin()
    end)
end

-- 停止所有控制器
function ControllerManager: stopAllController()
    table.doFunc(self._ctrlMap, function (k, v)
        ---@type BaseController
        local ctrl = v
        ctrl:stop()
    end)
end

function ControllerManager:_createCtrl(ctrlCls)
    local uid = self:_getUid()
    ---@type BaseController
    local ins = ctrlCls: new()
    ins:create(uid)
    self._ctrlMap[uid] = ins
end

function ControllerManager: _getUid()
    self._uid = self._uid + 1
    return self._uid
end

return ControllerManager