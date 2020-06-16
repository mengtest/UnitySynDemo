---@class BaseController
local BaseController = Class("BaseController")
---@return BaseController
function createController(ctrlName)
    local res = Class(ctrlName, BaseController)
    return res
end

function BaseController: create(uid)
    self.uid = uid
    self.isRun = false
    self:onCreate()
end

function BaseController: begin()
    if self.isRun == true then
        return
    end
    self.isRun = true
    self:onBegin()
end

function BaseController: stop()
    if self.isRun == false then
        return
    end
    self.isRun = false
    self:onStop()
end
---发送协议
---@param msgCmd 协议号
---@param data 协议内容
---@param callback 返回协议的回调
function BaseController: SendMsg(msgCmd, data,callback)
    netWorkMgr:onSocketRequest(msgCmd, data,callback)
end

function BaseController: PushMsgListner()

end

---------------------------可重写的生命周期---------------------------------

function BaseController: onCreate()
    -- 当控制器被创建出来
end

function BaseController: addEventListener()
    -- 添加事件
end

function BaseController: removeEventListener()
    -- 移除事件
end

function BaseController: onBegin()
    -- 当控制器启动，一般在登陆成功后或退出战斗回到大厅时调用
end

function BaseController: onStop()
    -- 当控制器停止，一般在进入战斗时调用，用于停止计时器等
end

---------------------------可重写的生命周期---------------------------------

return BaseController