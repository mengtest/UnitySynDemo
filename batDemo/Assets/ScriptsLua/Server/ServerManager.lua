require("Server.NetworkEnum")
require("common.config")
local netMgrClass = require("Server.NetWorkMgr")

-- 服务器管理类
---@class ServerManager
local ServerManager = Class("SceneManager")

-- 初始化
function ServerManager:initialize()
    print("ServerManager ctor")
    --测试服
    --self.host = "192.168.1.147"
    --self.port = 9320
    --Jiangge
    --self.host = "192.168.1.189"
    --self.port = 9320

    self.netMgr = netMgrClass:new()
    --self.netMgr:setData(self.host, self.port)
    self.netMgr:SetSocketIPAndPort(serverList[serverIndex].ip,serverList[serverIndex].port)
end

function ServerManager:reConnect()
   self.netMgr:onReconnect(function()
       self:_onFire()
   end, function()
       self:_onUnResgister()

    end)
end
local loadView
function ServerManager:_onFire()
    --LuaEventCenter.fire(EventType.onReconnect)
    --ViewHelper.showView(ViewType.onReconnect)
    loadView = ViewHelper.showView(ViewType.LoadingView)
end
function ServerManager:_onUnResgister()
    loadView:HideLoadingView()
end
--------------------------登录---------------------------------
---
function ServerManager:Login(loginPb)
    LoginController.instance:beginLogin(loginPb,
            function (loginRsq) self:_onLoginSuccess(loginRsq) end,
            function (errorCode) self:_onLoginFailed(errorCode) end
    )
end
------------------------随机名字和创建角色---------------
function ServerManager:RandomRoleName(sex)
    LoginController.instance:randomName(
        sex,
            function(roleName) self:_onRandomRoleName(roleName) end
    )
end
function  ServerManager:CreateRole(createRolePb)
    LoginController.instance:createRole(createRolePb,function (isSuccess, roleInfo)
        self:_onCreateRole(isSuccess, roleInfo)
    end)
end

---@param roleName string
function ServerManager:_onRandomRoleName(roleName)
    LuaEventCenter.fire(EventType.onRandomRoleName,roleName)
end

---@param roleinfo table(RoleInfo)
function ServerManager: _onCreateRole(isSuccess, roleInfo)
    LuaEventCenter.fire(EventType.onCreateRole,isSuccess, roleInfo)
end
----------------------
function ServerManager:_onLoginSuccess(loginRsq)
    -- todo 基础数据同步
    gGame.player:syncDataOnLogin(loginRsq)
    LuaEventCenter.fire(EventType.onLoginSuccess,loginRsq)
end

function ServerManager:_onLoginFailed(errorCode)
    LuaEventCenter.fire(EventType.onLoginFailed, errorCode)
end

--------------------------登录---------------------------------

-- 匹配 --



--------------------------对外工具封装---------------------------------

---@param data table
---@param callBack function
function ServerManager: requestServer(data, callBack)
    self.netMgr:onSocketRequest(data, callBack)
end

--------------------------对外工具封装---------------------------------

return ServerManager