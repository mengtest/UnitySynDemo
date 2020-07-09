local PacketBuilder = require("Server.PacketBuilder")
---@class RequestData
RequestData = Class("RequestData")
---@return RequestData

RequestData.create = function(msgCmd, data, callback)
    return RequestData: new(msgCmd, data, callback)
end

-- 请求数据初始化
function RequestData:initialize(msgCmd, data, callback )
    -- body数据
    self._bodyData = data;
    -- 请求序号
    self._reqIndex = netWorkMgr:getReqIndex()
    -- 包体
    self._pktData = PacketBuilder:new(msgCmd, {data}, self._reqIndex)
    -- 包名
    self._pktName = "Packet"
    -- 包体数据
    self._sendMsg = self._pktData:getRequestMsg()
    -- 回调方法
    self._callback = callback
    -- 是否http请求
    self._isHttp = false
    -- 请求发送次数
    self._requestTimes = 0
    -- 请求重试次数 默认1次
    self._retryTimes = 1
    -- socket请求超时回调
    self._socketTimeoutCallback = nil
    -- 超时计时器
    self._timeoutTimer = nil
    -- http请求模块
    self._mod = nil

    self:setPktName()
end

function RequestData:setPktName( )
    -- local meta = getmetatable(self._bodyData)
    -- if meta and meta._descriptor and meta._descriptor.name then
    --     self._pktName = meta._descriptor.name
    -- end
    self._pktName = netMsgHelper:getCmdName(self._pktData._msg.Head.Cmd)
end

function RequestData:getPktName( )
    return self._pktName
end

function RequestData:getReqIndex( )
    return tostring(self._reqIndex)
end

function RequestData:getBodyData( )
    return self._bodyData
end

function RequestData:getPktData( )
    return self._pktData
end

function RequestData:getSendMsg( )
    return self._sendMsg
end

function RequestData:getCallback( )
    return self._callback
end

function RequestData:getSeq( )
    return tostring(self:getPktData():getSeq())
end

function RequestData:getCmdId( )
    if self._pktData then
        return self._pktData:getCmdId()
    end
    return nil
end

function RequestData:setIsHttp( isHttp )
    self._isHttp = isHttp
end

function RequestData:isHttpRequest( )
    return self._isHttp
end

function RequestData:setMod( mod )
    self._mod = mod
end

function RequestData:getMod( )
    return self._mod
end

-- 增加请求次数
function RequestData:addRequestTime( )
    self._requestTimes = self._requestTimes + 1
    if self._isHttp == false then
        -- socket请求 自己做超时判断
        if self._timeoutTimer ~= nil then
            Timer.clearWithData(self._timeoutTimer)
            --self._timeoutTimer:Stop()
        end
        --self._timeoutTimer = Utils.delayCall(function( )
        --    if self._socketTimeoutCallback then
        --        self._socketTimeoutCallback(self:getSeq())
        --    end
        --end, NetworkEnum.SOCKET_MSG_TIME_OUT_INTERVAL)
        self._timeoutTimer = LuaUtil.delayCall(NetworkEnum.SOCKET_MSG_TIME_OUT_INTERVAL, self,
                function ()
                    if self._socketTimeoutCallback then
                        self._socketTimeoutCallback(self:getSeq())
                    end
                end)
    end
end

function RequestData:getRequestTime( )
    return self._requestTimes
end

function RequestData:setRetryTime( times )
    self._retryTimes = times
end

function RequestData:getRetryTime( )
    return self._retryTimes
end

function RequestData:setSocketReqTimeOut( timeoutCallback )
    self._socketTimeoutCallback = timeoutCallback
end

-- 销毁
function RequestData:onDestroy( )
    -- 清除timer
    if self._timeoutTimer ~= nil then
        --self._timeoutTimer:Stop()
        Timer.clearWithData(self._timeoutTimer)
        self._timeoutTimer = nil
    end
end

return RequestData