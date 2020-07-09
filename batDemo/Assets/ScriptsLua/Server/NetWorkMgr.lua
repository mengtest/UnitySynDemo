require("protobuf.protobuf")
local pb = require "pb"
pb.option "enum_as_value"
-- local protoc = require "protoc"

TcpSocketClient = TcpSocket.TcpSocketClient

---协议辅助类
local netMsgClass = require("Server.NetMsgHelper")
local RequestData = require("Server.RequestData")

---@class NetWorkMgr
NetWorkMgr = Class("NetWorkMgr")
---@return NetWorkMgr 视图管理类

function NetWorkMgr:initialize( )
    ---@type  NetMsgHelper
    self._netMsgHelper = netMsgClass:new()

    ---请求序号
    self._reqIndex = 0;
    
    ---请求Map
    self._reqMap = {}
    ---监听协议map，<string,RequestData>
    self._pushMap = {}

    ---心跳间隔
    self._heartReqTime = NetworkEnum.SOCKET_HEART_BEAT_INTERVAL
    ---心跳timer
    self._heartTimer = nil

    ---是否使用aes加密
    self._useAes = false;
    ---获取动态密钥间隔
    self._dynSecretReqTime = NetworkEnum.SOCKET_DYNAMIC_SECRET_INTERVAL
    ---动态密钥timer
    self._dynSecretTimer = nil
    ---动态密钥
    self._dynSecret = nil
    ---正在获取动态密钥
    self._isGettingDynSecret = false
    ---等待队列
    self._waitingReqList = {}
    ---网络状态
    self._status = NetworkEnum.SOCKET_STATUS_ENUM.CLOSED
    ---socket地址
    self._socketHost = nil
    ---socket端口
    self._socketPort = 0
    ---重设socket
    self._isResetSocket = false
    ---socket连接次数
    self._connectTimes = 8
    ---socket重连Timer
    self._reconnectTimer = nil
    --用于当前服务器返回的状态
    self._statusCode = nil

    self:_addCustomListener()

    self.isShowLoad =false
end

function NetWorkMgr:destroy()

end

---监听
function NetWorkMgr:_addCustomListener( )
    ---socket listener
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketConnect", function()self:_onSocketConnect()end)
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketException", function()self:_onSocketException()end)
    ---LuaFuncManager.Instance:addCustomListener("NetWorkMgr:onSocketDisconnect", function()self:onSocketDisconnect()end)
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketReceiveMsg", function(msg)self:_onSocketReceiveMsg(msg)end)
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketFinishSendMsg", function()self:_onSocketFinishSendMsg()end)
    ---http listener
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpMessageReceived", function(msg)self:_onHttpMessageReceived(msg)end)
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpTimedOut", function(index)self:_onHttpTimedOut(index)end)
    LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpException", function(index)self:_onHttpException(index)end)
    LuaFuncManager.Instance:addCustomListener("NetWorkMgr:onHttpErrorStatus", function(index)self:_onHttpErrorStatus(index)end)

    LuaEventCenter.register(LuaEventConst.LOGIN_SUCCESS,self,self._onLoginSuccess)
end

---获取请求序号
function NetWorkMgr:getReqIndex( )
    self._reqIndex = self._reqIndex + 1
    return self._reqIndex
end

---用于返回服务器状态码
function NetWorkMgr:GetStatusCode()
    return self._statusCode
end

---设置ScoketIP地址跟端口
function NetWorkMgr:SetSocketIPAndPort(host, port)
    log("设置服务器IP地址跟端口，分别为：",host,port)
    if self._socketHost ~= host or self._socketPort ~= port then
        self._socketHost = host
        self._socketPort = port
        self._isResetSocket = true
    end
end

---开始连接socket
function NetWorkMgr:onSocketBeginConnect( )
    log("开始连接socket",self._connectTimes)
    if self._reconnectTimer then
        Timer.clearWithData(self._reconnectTimer)
        self._reconnectTimer = nil
    end
    TcpSocketClient.GetInstance():SendConnect(self._socketHost, self._socketPort)
end

---socket是否某种状态
function NetWorkMgr:isEqualSocketStatus( status )
    return self._status == status and true or false
end

---外部调用发送http请求
function NetWorkMgr:onHttpRequest(msgCmd, mod, data, callback )
    local reqData = RequestData:new(msgCmd, data, callback)
    reqData:setIsHttp(true)
    reqData:setMod(mod)
    self:_sendRequestMsg(reqData)
end

---外部调用发送socket请求
function NetWorkMgr:onSocketRequest(msgCmd, data, callback)
    if(type(msgCmd) ~= "number") then
        logError("发送协议，协议号不正确，没传，或者协议没导出, msgCmd ===",tostring(msgCmd))
    else
        
        logWarn("C2S",netMsgHelper:getCmdName(msgCmd),LuaUtil.dump2(data))
        local reqData = RequestData:new(msgCmd, data, callback)
        self:_sendRequestMsg(reqData)
    end
end

---添加服务器推送协议侦听请求
function NetWorkMgr:AddPushMsgListner( reqData )
    self:_addPushMsgListner(reqData)
end

-------------------以下均为私有函数，不供外部调用--------------------------------------

-------------------------------socket处理------------------------------

    ---socket连接成功
    function NetWorkMgr:_onSocketConnect()
        log("socket连接成功")
        if self._isResetSocket then
            self._isResetSocket = false
        end
        ViewHelper.closeView(ViewType.LoadingView)
        self:_setStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTED)
        self:_cleanConnectTimes();

        LuaEventCenter.fire(LuaEventConst.SOCKET_CONNECT_SUCCESS)
    end

    ---socket异常
    function NetWorkMgr:_onSocketException(msg)
        log("socket异常:")
        NetworkEnum.IsReconnect = true
        self:_setStatus(NetworkEnum.SOCKET_STATUS_ENUM.CLOSED)
        self:_stopTimer()
        --重连3次
        self:_addConnectTimes()
        ---3次重连以内不出现网络加载加载圈
        if self._connectTimes < 8 then
            if self._reconnectTimer == nil then
                self._reconnectTimer = LuaUtil.delayCall(NetworkEnum.SOCKET_RECONNECT_INTERVAL, self, self.onSocketBeginConnect)
            end
            if self._connectTimes >= 3 and not self.isShowLoad then
                self.isShowLoad = true
                ViewHelper.showWaitView(false,"重连中")
            end
        else
            self:_cleanConnectTimes()
            self.isShowLoad=false
            ViewHelper.closeView(ViewType.WaitView)
            local messageView = ViewHelper.showView(ViewType.MessageView)
            messageView:setDataWithTitle("重连","当前网络异常，请检查您的网络状态",
            function()
                log("退出游戏")
                gGame.viewMgr:HideAllAndShowOne(ViewType.LoginView)
            end,
            function()
                log("重新连接")
                self:onSocketBeginConnect()
            end)
        end
    end

    ---发送协议完成回调
    function NetWorkMgr:_onSocketFinishSendMsg( )
        print("*** NetWorkMgr:onSocketFinishSendMsg")
    end

    ---socket接收消息
    function NetWorkMgr:_onSocketReceiveMsg( msg )
        --print("socket接收消息: "..msg)
        self:onResponse(msg)
    end

    ---socket请求超时
    function NetWorkMgr:_onSocketMsgTimedOut( reqIndex )
        logError("socket请求超时:" .. reqIndex)
        self:_onRetryRequest(self:_getRequestData(reqIndex))
    end

    ---增加socket连接次数
    function NetWorkMgr:_addConnectTimes( )
        self._connectTimes = self._connectTimes + 1
    end

    ---清除socket连接次数
    function NetWorkMgr:_cleanConnectTimes( )
        self._connectTimes = 0
    end

    ---socket是否连接超时
    function NetWorkMgr:_isConnectTimeOut()
        if self._connectTimes > NetworkEnum.SOCKET_RECONNECT_TIMES then
            return true
        end
        return false
    end

    ---设置socket状态
    function NetWorkMgr:_setStatus(status)
        if status < NetworkEnum.SOCKET_STATUS_ENUM.CONNECTING and status > NetworkEnum.SOCKET_STATUS_ENUM.CLOSED then
            return
        end
        self._status = status
    end

-------------------------------socket处理------------------------------

-------------------------------http处理------------------------------

    ---http收到消息
    function NetWorkMgr:_onHttpMessageReceived( msg )
        log("http收到消息 : "..msg)
        self:onResponse(msg)
    end

    ---http请求超时
    function NetWorkMgr:_onHttpTimedOut( reqIndex )
        log("http请求超时: " .. reqIndex)
        self:_onRetryRequest(self:_getRequestData(reqIndex))
    end

    ---http请求异常
    function NetWorkMgr:_onHttpException( reqIndex )
        log("http请求异常:" .. reqIndex)
        self:_onRequestError(self:_getRequestData(reqIndex))
    end

    ---http请求数据异常
    function NetWorkMgr:_onHttpErrorStatus( reqIndex )
        log("http请求数据异常: " .. reqIndex)
        self:_onRequestError(self:_getRequestData(reqIndex))
    end

-------------------------------http处理------------------------------

-------------------------------心跳---------------------------------

    function NetWorkMgr:_beginBeatRequest()
        log("启动心跳")
        self._heartTimer = Timer.loop(self._heartReqTime, self, self._heartBeatTimer)
    end

    function NetWorkMgr:_heartBeatTimer()
        --logWarn("dong", self._heartReqTime)
        local heartBeatReq = {}
        heartBeatReq.Counter =0
        heartBeatReq.TimeDelay =10
        local reqData = RequestData:new(GAME_CMD.GAME_CMD_HEARTBEAT,heartBeatReq, function( ... )self:_onHeartBeatResponse(...)end)
        self:_sendRequestMsg(reqData)
    end

    function NetWorkMgr:_onHeartBeatResponse(isSuccess)
        if isSuccess then
            ---body
        else
            logError("心跳失败,断开连接")
            self:_stopHeartBeat()
        end
    end

    function NetWorkMgr:_stopHeartBeat()
        log("停止心跳")
        if self._heartTimer ~= nil then
            Timer.clearWithData(self._heartTimer)
        end
    end

-------------------------------心跳---------------------------------

-------------------------------动态加密密钥---------------------------------

    function NetWorkMgr:_onSecretMessageRequest( )
        if not self._useAes then
            return
        end
        local secretMessageReq = game_pb.SecretMessageReq()
        local reqData = RequestData:new(secretMessageReq, function( ... )self:_onSecretMessageResponse(...)end)

        self:_sendRequestMsg(reqData)
        self._isGettingDynSecret = true
    end

    function NetWorkMgr:_onSecretMessageResponse( isSuccess, rspData, reqData )
        if isSuccess then
            self._dynSecret = rspData.Key
            if self._dynSecret then
            end
        end
        if self._dynSecretTimer ~= nil then
            Timer.clearWithData(self._dynSecretTimer);
            self._dynSecretTimer = nil
        end
        self._dynSecretTimer = LuaUtil.delayCall(self._dynSecretReqTime, self, self._onSecretMessageRequest)

        self._isGettingDynSecret = false
        self:_onSendWaitingMsg()
    end
-------------------------------动态加密密钥---------------------------------

-------------------------------发送请求相关-------------------------------

    ---发送请求，添加监听，以及发送协议内容
    function NetWorkMgr:_sendRequestMsg( reqData )
        self:_addRequest(reqData)
        self:_onSendMsg(reqData)
    end

    ---添加请求
    function NetWorkMgr:_addRequest( reqData )
        local reqIndex = reqData:getReqIndex()
        if reqIndex then
            self._reqMap[reqIndex] = reqData
        end
    end

---发送请求
---@param reqData RequestData
function NetWorkMgr:_onSendMsg( reqData )
    ---获取动态密钥时 不允许发其他请求出去，这时候发送的请求会加入等待队列，获取动态密钥后重新发送
    if self._isGettingDynSecret and not reqData:isHttpRequest() then
        self:_addWaitingRequest(reqData)
        return
    end
    local sendSuccess = true
    --local seq = reqData:getSeq()

    if reqData:isHttpRequest() then
        logError("未支持http请求")
    else
        if self:isEqualSocketStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTED) then
            reqData:addRequestTime()
            local msg = reqData:getSendMsg()
            local reqName = reqData:getPktName()
            if reqName ~= "md.Heartbeat" then
                -- logWarn("C2S", reqName)
            end
            TcpSocketClient.GetInstance():Send(msg)
        else
            sendSuccess = false
            self:onSocketBeginConnect()
        end
    end

    if sendSuccess then

    else

        logError("sendError " + reqData._pktName)
    end
end

---发送等待请求
function NetWorkMgr:_onSendWaitingMsg( )
    if not self._isGettingDynSecret then
        for i=1, #self._waitingReqList do
            local reqData = self._waitingReqList[i]
            self:_onSendMsg(reqData)
        end
        self._waitingReqList = {}
    end
end

---添加等待请求
function NetWorkMgr:_addWaitingRequest( reqData )
    if reqData then
        table.insert( self._waitingReqList, reqData )
    end
end

---获取请求数据
function NetWorkMgr:_getRequestData( reqIndex )
    return self._reqMap[tostring(reqIndex)]
end

---重发请求
function NetWorkMgr:_onRetryRequest( reqData )
    if reqData == nil then
        return
    end

    if reqData:getRequestTime() <= reqData:getRetryTime() then
        if not reqData:isHttpRequest() and not self:isEqualSocketStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTED) then
            self:_onRequestError(reqData)
        else
            self:_onSendMsg(reqData)
        end
    else
        self:_onRequestError(reqData)
    end
end

---清除请求数据
function NetWorkMgr:cleanRequestData( reqIndex )
    if self._reqMap[tostring(reqIndex)] ~= nil then
        self._reqMap[tostring(reqIndex)]:onDestroy()
        self._reqMap[tostring(reqIndex)] = nil
    end
end
    
-------------------------------发送请求相关-------------------------------

-------------------------------以下是有关服务器推送协议的监听添加 start

---添加服务器推送协议侦听请求
function NetWorkMgr:_addPushMsgListner(reqData)
    local cmdId = reqData:getPktData():getCmdId()
    if cmdId then
        local tempName = self._netMsgHelper:getCmdName(cmdId)
        self._pushMap[tempName] = reqData
    end
end

---获取服务器推送侦听协议数据
function NetWorkMgr:_getPushDataForMonitor( reqIndex )
    return self._pushMap[tostring(reqIndex)]
end

-------------------------------以下是有关服务器推送协议的监听添加 end

function NetWorkMgr:_onRequestError( reqData )
    if reqData == nil or not self.isInstanceOf(reqData, RequestData) then
        return
    end

    local callback = reqData:getCallback()
    callback(false, nil, reqData:getBodyData())
    self:cleanRequestData(reqData:getSeq())
end

---全部未收到rsp的socket请求重发
function NetWorkMgr:onRetry( )
    for reqIndex,reqData in pairs(self._reqMap) do
        if reqData:isHttpRequest() == false then
            self:_onRetryRequest(reqData)
        end
    end
end

---收到消息 msg:byte[]
function NetWorkMgr:onResponse(msg)
    --第一步 base64解
    -- local rspMsg = LuaUtil.decodeBase64(msg)
    local rspMsg = msg
    --第二步 解析Packet
    local rspPkt = assert(pb.decode("cmdpkg.CmdPacket", msg))
    -- rspPkt:ParseFromString(rspMsg)
    --检查有木有PktInfo 没有的为异常包
    if rspPkt and rspPkt.Head and rspPkt.Body then
        --通过PktInfo获取cmd号
        local cmd = rspPkt.Head.Cmd or nil
        if cmd ~= nil then
            --response消息通过规则
            if rspPkt.Head.TransType == TRANS_TYPE.RESPONSE then
                cmd = self._netMsgHelper:genResponseCMD(cmd)
            end
            
            local returnCode = rspPkt.Head.StatusCode
            local bodyMsg
            local reqName = self._netMsgHelper:getCmdName(cmd)

            if returnCode ~= STATUS_CODE.OK then
                logError("协议返回状态错误，错误码为："..returnCode)
                logError("协议错误内容: "..tostring(rspPkt.Body))
                bodyMsg = {}
            else
                bodyMsg = assert(pb.decode(reqName, rspPkt.Body))
            end

            if reqName ~= "md.HeartbeatRsp" then
                local serpent = require("PB.serpent")
                logWarn("S2C", reqName, serpent.block(bodyMsg))
            end
            
            -- ---通过cmd号获取对应解析pb结构
            -- local bodyMsg
            -- local rspName = self._netMsgHelper:getCmdName(cmd)
            -- -- if math.abs(cmd) == GAME_CMD.GAME_CMD_GETTEST then
            -- --     bodyMsg = assert(pb.decode("GetTestRsp", rspPkt.Body))
            -- --     log("bodyMsg ==============",bodyMsg)
            -- -- else
            --     bodyMsg = self._netMsgHelper:getDecodePbWithCmdId(cmd)
            -- -- end
            -- if bodyMsg then
            --     if bodyMsg._pbname then
            --         rspName = bodyMsg._pbname
            --     end
            --     bodyMsg = bodyMsg()
            --     -- bodyMsg:ParseFromString(rspPkt.Body)
                -- LuaUtil.try {
                --     function()
                --         logError("ddddddddddddddddddd")
                --         bodyMsg:ParseFromString(rspPkt.Body)
                --         logError("ttttttttttttttttttt")
                --         logError(tostring(bodyMsg))
                --     end,
                --     LuaUtil.catch {
                --         function(error)
                --             bodyMsg = nil
                --             logError("S2C Decode Rsp", cmd, " pb fail \n caught error: " .. error)
                --             --gLog.warning("[NetWorkMgr] Decode Rsp " .. cmd .. " pb fail \n caught error: " .. error)
                --         end
                --     }
                -- }
            -- end

            if rspPkt.Head and rspPkt.Head.Serial and rspPkt.Head.Serial ~= 0 then
                -----------------------服务器接口回复信息-------------------------
                local returnCode = rspPkt.Head.StatusCode
                -- if bodyMsg == nil then
                --     returnCode = STATUS_CODE.ERR_COMMON
                --     logWarn("S2C ERROR", rspName, "解析body失败")
                -- else
                --     if returnCode == STATUS_CODE.OK then
                --         if rspName ~= "heartbeatRsp" then
                --             logWarn("S2C", rspName, "\n"..tostring(bodyMsg))
                --         end
                --     else
                --         logWarn("S2C", rspName, "错误码: " .. returnCode)
                --     end
                -- end
                local reqData = self:_getRequestData(rspPkt.Head.Serial)
                if reqData ~= nil then
                    local callback = reqData:getCallback()
                    if callback ~= nil then
                        callback(returnCode, bodyMsg, reqData:getBodyData())
                    end
                    self:cleanRequestData(rspPkt.Head.Serial)
                end
            else
                -----------------------服务器推送信息-------------------------
                local returnCode = rspPkt.Head.StatusCode
                self._statusCode = returnCode
                ---没有Seq号的为服务器推送过来消息
                if bodyMsg == nil then
                    returnCode = STATUS_CODE.ERR_COMMON
                    logWarn("S2C 服务器推送 ERROR", rspName, "解析body失败")
                else
                    if returnCode == STATUS_CODE.OK then
                        logWarn("S2C 服务器推送", rspName, "\n"..tostring(bodyMsg))
                    else
                        logWarn("S2C 服务器推送", rspName, "错误码: " .. returnCode)
                    end
                end
                local reqData = self:_getPushDataForMonitor(self._netMsgHelper:getCmdName(cmd))
                if reqData ~= nil then
                    local callback = reqData:getCallback()
                    if callback then
                        callback(returnCode, bodyMsg, reqData:getBodyData())
                    end
                end

            end
        end
    else
        logError("onResponse eror", tostring(rspPkt))
        --logError("[NetWorkMgr][onResponse][error] packet : "..tostring(rspPkt))
    end
end


function NetWorkMgr : try(block)
   ---get the try function
       local try = block[1]
         assert(try)
        local funcs = block[2]
         local ok, errors = xpcall(try, debug.traceback)
         if not ok then
                 ---run the catch function
                if funcs and funcs.catch then
                         funcs.catch(errors)
                     end
             end
        if funcs and funcs.finally then
                funcs.finally(ok, errors)
             end
         if ok then
                 return errors
             end
end

---停止timer
function NetWorkMgr:_stopTimer( )
    if self._heartTimer ~= nil then
        Timer.clearWithData(self._heartTimer)
    end
    self._heartTimer = nil

    if self._dynSecretTimer ~= nil then
        Timer.clearWithData(self._dynSecretTimer)
        self._dynSecretTimer = nil
    end
    self._dynSecretTimer = nil
end
------------------------------------------------------------------------

------------------------登录处理------------------------------

-- 登录完成后发动态密钥、心跳请求
function NetWorkMgr:_onLoginSuccess()
    self:_onSecretMessageRequest()
    self:_beginBeatRequest()
end

------------------------登录处理------------------------------

return NetWorkMgr