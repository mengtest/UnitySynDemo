-- require("protobuf.protobuf")

-- TcpSocketClient = TcpSocket.TcpSocketClient


-- local RequestData = require("Server.RequestData")

-- local NetworkManager = Class("NetworkManager")
-- function NetworkManager:ctor( )
--     networkManager = self
--     -- 请求序号
--     self._reqIndex = 0;
--     -- 请求Map
--     self._reqMap = {}
--     ---监听协议map，<string,RequestData>
--     self._pushMap = {}
--     -- 心跳间隔
--     self._heartReqTime = NetworkEnum.SOCKET_HEART_BEAT_INTERVAL
--     -- 是否使用aes加密
--     self._useAes = false;
--     -- 获取动态密钥间隔
--     self._dynSecretReqTime = NetworkEnum.SOCKET_DYNAMIC_SECRET_INTERVAL
--     -- 心跳timer
--     self._heartTimer = nil
--     -- 动态密钥timer
--     self._dynSecretTimer = nil
--     -- 动态密钥
--     self._dynSecret = nil
--     -- 正在获取动态密钥
--     self._isGettingDynSecret = false
--     -- 等待队列
--     self._waitingReqList = {}
--     -- 网络状态
--     self._status = NetworkEnum.SOCKET_STATUS_ENUM.CLOSED
--     -- 命令号表
--     self._cmdIdMap = {}
--     self._cmdNameMap = {}
--     self:_initCmdMap()
--     -- 解析表
--     self._msgDecodeMap = {}
--     self:_initDecodeMap()
--     -- socket测试地址
--     self._testSocketHost = nil
--     -- socket测试端口
--     self._testSocketPort = 0
--     -- 重设socket
--     self._isResetSocket = false
--     -- socket连接次数
--     self._connectTimes = 8
--     -- socket重连Timer
--     self._reconnectTimer = nil
--     --用于当前服务器返回的状态
--     self._statusCode = nil
--     --self._broadcastManager = BroadcastManager:new()
--     self:_addCustomListener()
--     -- SocketClient.Instance:Initialize()--废弃
--     -- self._useAes = SocketClient.Instance:IsUseCrypt()--废弃


--     --self:onSetTestSocket("192.168.1.147",10020)
--     --self:onSetTestSocket("192.168.1.189",10020)
--     --self:_onSocketBeginConnect()
--     self.isShowLoad =false
-- end

-- function NetworkManager:setData(host, port)
--     self:onSetTestSocket(host, port)
-- end

-- function NetworkManager:destroy()
-- end

-- -- 监听
-- function NetworkManager:_addCustomListener( )
--     -- socket listener
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketConnect", function()self:onSocketConnect()end)
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketException", function()self:onSocketException()end)
--     -- LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketDisconnect", function()self:onSocketDisconnect()end)
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketReceiveMsg", function(msg)self:onSocketReceiveMsg(msg)end)
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onSocketFinishSendMsg", function()self:onSocketFinishSendMsg()end)
--     -- http listener
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpMessageReceived", function(msg)self:onHttpMessageReceived(msg)end)
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpTimedOut", function(index)self:onHttpTimedOut(index)end)
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpException", function(index)self:onHttpException(index)end)
--     LuaFuncManager.Instance:addCustomListener("NetworkManager:onHttpErrorStatus", function(index)self:onHttpErrorStatus(index)end)
-- end

-- function NetworkManager:onSetTestSocket( host, port )
--     log("设置服务器",host,port)
--     if self._testSocketHost ~= host or self._testSocketPort ~= port then
--         self._testSocketHost = host
--         self._testSocketPort = port
--         self._isResetSocket = true
--         -- SocketClient.Instance:Close()
--         -- self:_onSocketBeginConnect()
--     end
-- end

-- --------------------------随机名字和创建角色----------------------
-- function  NetworkManager:randomName(sex,randNameFunc)
--     local rdName = game_pb.RandRoleName()
--     rdName.Sex = 1
--     self._randomName = randNameFunc
--     self:onSocketRequest(rdName,function(...) self:_onRandomName(...)  end)
-- end

-- function NetworkManager:_onRandomName(isSuccess, rspData, reqData)
--     if self._randomName ~=nil  and isSuccess then
--         self._randomName(rspData.RoleName)
--         self._randomName = nil
--     else
--         log("随机名字失败")
--     end
-- end

-- function NetworkManager:createRole(createRolePB,callback)
--     self.createRoleCallback = callback
--     self:onSocketRequest(createRolePB,function(...)  self:_onCreateRole(...) end)
-- end

-- function NetworkManager :_onCreateRole(isSuccess, rspData,reqData)
--     logError(isSuccess)
--     if self.createRoleCallback ~=nil  then
--         self.createRoleCallback(isSuccess,rspData.RoleInfo)
--         self.createRoleCallback =nil
--     else
--         logError("创建角色失败")
--     end
-- end

-- -------------------------------------------------------
-- ------------------------登录处理------------------------------

-- function NetworkManager:beginLogin(loginPb, _loginSuccessFunc, _loginFailedFunc)
--     self.loginPb =loginPb
--     self._loginSuccessFunc = _loginSuccessFunc
--     --self._loginFailedFunc = _loginFailedFunc
--     self:_onSocketBeginConnect()
-- end

-- function NetworkManager: _callLoginSuccess(loginRsq)
--     if self._loginSuccessFunc ~= nil then
--         self._loginSuccessFunc(loginRsq)
--         self._loginSuccessFunc = nil
--     end
-- end

-- function NetworkManager: _callLoginFailed(errorCode)
--     --if self._loginFailedFunc ~= nil then
--     --    self._loginFailedFunc(errorCode)
--     --    self._loginFailedFunc = nil
--     --end
-- end

-- function NetworkManager:_readLoginInfoAfterReConnect()
--     self.loginPb = PlatFormBase:_getCacheInfo()
--     --self.loginPb.LoginType = LOGIN_TYPE.LOGIN_TYPE_GUEST

--     logError("重连信息：",self.loginPb.OpenID,self.loginPb.LoginType)
-- end

-- -- 已经登录游戏，重连后发登录发请求
-- function NetworkManager:_onLoginReq()
--     log("开始登录请求")
--     if NetworkEnum.IsReconnect then
--         --取缓存
--         ViewHelper.closeView(ViewType.LoadingView)
--         ViewHelper.closeView(ViewType.MessageView)
--         self:_readLoginInfoAfterReConnect()
--     end
--     self:onSocketRequest(self.loginPb, function(...) self:_onLoginRsp(...) end)
-- end

-- function NetworkManager:_onLoginRsp( isSuccess, rspData, reqData)
--     if isSuccess then
--         self:onFinishLoginReq()
--         --logError( )
--         --log(tostring( reqData))
--         self:_callLoginSuccess( rspData)
--     else
--         -- todo 3次重试处理
--         self._callLoginFailed(ErrCode.login_loginFailed)
--     end
-- end

-- -- 登录完成后发动态密钥、心跳请求
-- function NetworkManager:onFinishLoginReq()
--     self:_onSecretMessageRequest()
--     self:_beginBeatRequest()
-- end

-- ------------------------登录处理------------------------------

-- ----------------------- 匹配处理 -----------------------------
-- function NetworkManager:doMatch(matchPb)
--     logError("do match============")
--     -- local tenmFunc = self:_onDoMatchResp
--     self:onSocketRequest(matchPb, function(...) 
--         -- logError("-------------------------------------------------------")
--         -- tenmFunc(...) 
--         self:_onDoMatchResp(...)
--         -- logError("11111111111111111111111111111")
--     end)
-- end

-- function NetworkManager:_onDoMatchResp(isSuccess, rspData, reqData)
--     -- logError("--------------------> 匹配协议返回：")
--     -- logError(isSuccess)
--     -- logError("rspData:" .. tostring(rspData))
--     -- logError("reqData:" .. tostring(reqData))

--     if isSuccess then 
--         -- log("本次匹配的请求码为：" .. rspData.MatchRequestId)
--         LuaEventCenter.fire(EventType.onMatchResp, rspData.MatchRequestId)
--     else 
--         LuaEventCenter.fire(EventType.onMatchResp, "")

--     end
-- end

-- function NetworkManager:queryMatch(queryMatchPb)
--     logError("query match============")
--     self:onSocketRequest(queryMatchPb, function(...) 
--         self:_onQueryMatch(...)
--     end)
-- end

-- function NetworkManager:_onQueryMatch(isSuccess, rspData, reqData)
--     log("=====================> 查询匹配结果返回：")
--     --logError(isSuccess)
--     --logError("repData.MatchRequestId:" .. rspData.MatchRequestId)
--     --logError("repData.MatchedPlayerIds:" .. rspData.MatchedPlayerIds)
--     --logError(rspData.Status)
--     --logError("repData.GameServerSessionId:" .. rspData.GameServerSessionId)


--     if isSuccess then
--         log("匹配结果的状态为：" .. rspData.Status)
--         if rspData.Status == MATCH_STATUS.MATCH_COMPLETED then
--             log("匹配成功！")
--             LuaEventCenter.fire(EventType._onQueryMatchResp, rspData.GameServerSessionId)
--         else
--             log("匹配失败！")
--             LuaEventCenter.fire(EventType._onQueryMatchResp, "")
--         end
--     else
--     end
-- end

-- function NetworkManager:cancelMatch(cancelMatchPb)
--     logError("cancel match============")
--     self:onSocketRequest(cancelMatchPb, function(...) 
--         self:_onCancelMatch(...)
--     end)
-- end

-- function NetworkManager:_onCancelMatch(isSuccess, rspData, reqData)
--     log("取消匹配的结果：" .. isSuccess)
--     LuaEventCenter.fire(EventType._onCancelMatchResp, isSuccess)
-- end

-- ------------------------ 房间的处理 ---------------
-- function NetworkManager:joinBattle(joinBattlePb)
--     logError("join battle ============")
    
-- end

-- ------------------------socket处理------------------------------

-- -- 开始连接socket
-- function NetworkManager:_onSocketBeginConnect( )
--     log("开始连接socket",self._connectTimes)
--     if self._reconnectTimer then
--         Timer.clearWithData(self._reconnectTimer)
--         self._reconnectTimer = nil
--     end
--     -- SocketClient.Instance:SendConnect(self._testSocketHost, self._testSocketPort)
--     TcpSocketClient.GetInstance():SendConnect(self._testSocketHost, self._testSocketPort)
--     --if not self:isEqualSocketStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTING) then
--     --    NativeUtils.cleanNetworkInfo()
--     --    self:setStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTING)
--     --    self:_addConnectTimes()
--     --    SocketClient.Instance:SendConnect(self._testSocketHost, self._testSocketPort)
--     --end
-- end

-- -- socket连接成功
-- function NetworkManager:onSocketConnect()
--     log("socket连接成功")
--     if self._isResetSocket then
--         self._isResetSocket = false
--     end
--     --LuaEventCenter.fire(EventType.onReconnect)
--     ViewHelper.closeView(ViewType.LoadingView)
--     self:setStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTED)
--     self:_cleanConnectTimes();
--     -- 重连就清除之前的动态密钥
--     self:_cleanSecretKey()
--     -- 已经登录游戏，socket断线重连的需要发登录请求，然后再发心跳
--     self:_onLoginReq()
-- end

-- -----------------重连处理------------------
-- --function NetworkManager:onReconnect(fireCallBack, unRegisterCallBack)
-- --    self.fireCallBack =fireCallBack
-- --    self.unRegisterCallBack =unRegisterCallBack
-- --end
-- --
-- --function NetworkManager:_onReconnet(isFire)
-- --    if isFire then
-- --        self.fireCallBack()
-- --    else
-- --        self.unRegisterCallBack()
-- --    end
-- --end
-- ------------------------
-- -- socket异常
-- function NetworkManager:onSocketException(msg)
--     log("socket异常:")
--     NetworkEnum.IsReconnect = true
--     self:setStatus(NetworkEnum.SOCKET_STATUS_ENUM.CLOSED)
--     self:_stopTimer()
--     --重连3次
--     --if not self:_isConnectTimeOut() then
--     --    --self:_addConnectTimes()
--     --    if self._reconnectTimer == nil then
--     --        self._reconnectTimer = LuaUtil.delayCall(NetworkEnum.SOCKET_RECONNECT_INTERVAL, self, self._onSocketBeginConnect)
--     --    end
--     --else
--     --    ViewHelper.showView(ViewType.LoadingView)
--     --    self:_cleanConnectTimes()
--     --
--     --    --self:onRetry()
--     --    local data =  LuaUtil.delayCall(NetworkEnum.SOCKET_RECONNECT_INTERVAL, self, self._cleanConnectTimes)
--     --    logError(data.delay)
--     --end
--     --重连3次
--     self:_addConnectTimes()
--     ---3次重连以内不出现网络加载加载圈
--     if self._connectTimes < 8 then
--         if self._reconnectTimer == nil then
--             self._reconnectTimer = LuaUtil.delayCall(NetworkEnum.SOCKET_RECONNECT_INTERVAL, self, self._onSocketBeginConnect)
--         end
--         if self._connectTimes >= 3 and not self.isShowLoad then
--             self.isShowLoad = true
--             ViewHelper.showWaitView(false,"重连中")
--         end
--     else
--         self:_cleanConnectTimes()
--         self.isShowLoad=false
--         ViewHelper.closeView(ViewType.WaitView)
--         local messageView = ViewHelper.showView(ViewType.MessageView)
--         messageView:setDataWithTitle("重连","当前网络异常，请检查您的网络状态",
--         function()
--             log("退出游戏")
--             gGame.viewMgr:HideAllAndShowOne(ViewType.LoginView)
--         end,
--         function()
--             log("重新连接")
--             self:_onSocketBeginConnect()
--         end)
--     end
-- end

-- -- socket掉线
-- -- function NetworkManager:onSocketDisconnect( )
-- --     print("NetworkManager onSocketDisconnect")
-- --     self:_stopTimer()
-- --     Utils.delayCall(function( ) self:_onSocketBeginConnect() end, 10)
-- -- end

-- function NetworkManager:onSocketFinishSendMsg( )
--     -- print("*** NetworkManager:onSocketFinishSendMsg")
-- end

-- -- socket接收消息
-- function NetworkManager:onSocketReceiveMsg( msg )
--     --print("socket接收消息: "..msg)
--     self:onResponse(msg)
-- end

-- -- socket请求超时
-- function NetworkManager:onSocketMsgTimedOut( reqIndex )
--     logError("socket请求超时:" .. reqIndex)
--     self:onRetryRequest(self:getRequestData(reqIndex))
-- end

-- -- 增加socket连接次数
-- function NetworkManager:_addConnectTimes( )
--     self._connectTimes = self._connectTimes + 1
-- end

-- -- 清除socket连接次数
-- function NetworkManager:_cleanConnectTimes( )
--     self._connectTimes = 0
-- end

-- -- socket是否连接超时
-- function NetworkManager:_isConnectTimeOut()
--     if self._connectTimes > NetworkEnum.SOCKET_RECONNECT_TIMES then
--         return true
--     end
--     return false
-- end

-- ------------------------socket处理------------------------------

-- ------------------------http处理------------------------------

-- -- http收到消息
-- function NetworkManager:onHttpMessageReceived( msg )
--     log("http收到消息 : "..msg)
--     self:onResponse(msg)
-- end

-- -- http请求超时
-- function NetworkManager:onHttpTimedOut( reqIndex )
--     log("http请求超时: " .. reqIndex)
--     self:onRetryRequest(self:getRequestData(reqIndex))
-- end

-- -- http请求异常
-- function NetworkManager:onHttpException( reqIndex )
--     log("http请求异常:" .. reqIndex)
--     self:_onRequestError(self:getRequestData(reqIndex))
-- end

-- -- http请求数据异常
-- function NetworkManager:onHttpErrorStatus( reqIndex )
--     log("http请求数据异常: " .. reqIndex)
--     self:_onRequestError(self:getRequestData(reqIndex))
-- end

-- ------------------------http处理------------------------------

-- -- 停止timer
-- function NetworkManager:_stopTimer( )
--     if self._heartTimer ~= nil then
--         Timer.clearWithData(self._heartTimer)
--         --self._heartTimer:Stop()
--     end
--     self._heartTimer = nil

--     if self._dynSecretTimer ~= nil then
--         Timer.clearWithData(self._dynSecretTimer)
--         self._dynSecretTimer = nil
--         --self._dynSecretTimer:Stop()
--     end
--     self._dynSecretTimer = nil
-- end

-- -- 初始化Cmd Map
-- function NetworkManager:_initCmdMap()
--     for k,v in pairs(cmd_pb) do
--         if v and type(v) == "table" then
--             if v.name and type(v.name) == "string" and v.number and type(v.number) == "number" then
--                 --logError("register", v.name)
--                 self._cmdIdMap[v.name] = v.number
--                 self._cmdNameMap[tostring(v.number)] = v.name
--                 local rspName = v.name .."Rsp"
--                 local rspNumber = self:genResponseCMD(v.number)
--                 self._cmdIdMap[rspName] =rspNumber
--                 self._cmdNameMap[tostring(rspNumber)] = rspName
--             end
--         end
--     end
-- end

-- function NetworkManager:genResponseCMD(cmd)
--     return  cmd * 10 + 1
-- end

-- -- 初始化反序列化映射
-- function NetworkManager:_initDecodeMap( )
--     local pbList = {game_pb, broadcast_pb, subnotice_pb, matchbattle_pb}
--     for i=1,#pbList do
--         local data_pb = pbList[i]
--         if data_pb ~= nil then
--             for k,v in pairs(data_pb) do
--                 if v and type(v) == "table" then
--                     local meta = getmetatable(v)
--                     if meta and meta.ParseFromString then
--                         self._msgDecodeMap[string.lower(k)] = v
--                     end
--                 end
--             end
--         end
--     end
-- end

-- -------------------------------心跳---------------------------------

-- function NetworkManager:_beginBeatRequest()
--     log("启动心跳")
--     self._heartTimer = Timer.loop(self._heartReqTime, self, self._heartBeatTimer)
-- end

-- function NetworkManager:_heartBeatTimer()
--     logWarn("dong", self._heartReqTime)
--     local heartBeatReq = game_pb.Heartbeat()
--     heartBeatReq.Counter =0
--     heartBeatReq.TimeDelay =10
--     local reqData = RequestData:new(heartBeatReq, function( ... )self:_onHeartBeatResponse(...)end)
--     self:sendRequestMsg(reqData)
-- end

-- function NetworkManager:_onHeartBeatResponse(isSuccess)
--     if isSuccess then
--         -- body
--     else
--         logError("心跳失败,断开连接")
--         self:_stopHeartBeat()
--     end
-- end

-- function NetworkManager:_stopHeartBeat()
--     log("停止心跳")
--     if self._heartTimer ~= nil then
--         Timer.clearWithData(self._heartTimer)
--     end
-- end

-- --function NetworkManager:_onHeartBeatRequest( )
-- --    local heartBeatReq = game_pb.HeartBeatReq()
-- --    heartBeatReq.Reserved = ""
-- --
-- --    --local reqData = RequestData(heartBeatReq, function( ... )self:_onHeartBeatResponse(...)end)
-- --    local reqData = RequestData:new(heartBeatReq, function( ... )self:_onHeartBeatResponse(...)end)
-- --    self:sendRequestMsg(reqData)
-- --end
-- --
-- --function NetworkManager:_onHeartBeatResponse(isSuccess, rspData, reqData )
-- --    if self._heartTimer ~= nil then
-- --        Timer.clearWithData(self._heartTimer)
-- --        self._heartTimer = nil
-- --        --self._heartTimer:Stop()
-- --    end
-- --    --self._heartTimer = Utils.delayCall(function ( ... )self:_onHeartBeatRequest()end, self._heartReqTime)
-- --    self._heartTimer = LuaUtil.delayCall(self._heartReqTime, self, self._onHeartBeatRequest)
-- --end

-- -------------------------------心跳---------------------------------

-- -------------------------------动态加密密钥---------------------------------

-- function NetworkManager:_onSecretMessageRequest( )
--     if not self._useAes then
--         return
--     end
--     local secretMessageReq = game_pb.SecretMessageReq()
--     --local reqData = RequestData(secretMessageReq, function( ... )self:_onSecretMessageResponse(...)end)
--     local reqData = RequestData:new(secretMessageReq, function( ... )self:_onSecretMessageResponse(...)end)

--     self:sendRequestMsg(reqData)
--     self._isGettingDynSecret = true
-- end

-- function NetworkManager:_onSecretMessageResponse( isSuccess, rspData, reqData )
--     if isSuccess then
--         self._dynSecret = rspData.Key
--         if self._dynSecret then
--             -- SocketClient.Instance:SetDynSymCryptKey(self._dynSecret)
--         end
--     end
--     if self._dynSecretTimer ~= nil then
--         Timer.clearWithData(self._dynSecretTimer);
--         self._dynSecretTimer = nil
--         --self._dynSecretTimer:Stop()
--     end
--     --self._dynSecretTimer = Utils.delayCall(function ( ... )self:_onSecretMessageRequest()end, self._dynSecretReqTime)
--     self._dynSecretTimer = LuaUtil.delayCall(self._dynSecretReqTime, self, self._onSecretMessageRequest)

--     self._isGettingDynSecret = false
--     self:_onSendWaitingMsg()
-- end

-- function NetworkManager:_cleanSecretKey( )
--     -- SocketClient.Instance:CleanDynSymCryptKey()
-- end

-- -------------------------------动态加密密钥---------------------------------

-- -- 获取请求序号
-- function NetworkManager:getReqIndex( )
--     self._reqIndex = self._reqIndex + 1
--     return self._reqIndex
-- end

-- -- 请求数据获取对应cmd号
-- function NetworkManager:getCmdId( name )
--     if self._cmdIdMap[tostring(name)] then
--         return self._cmdIdMap[tostring(name)]
--     else
--         Debugger.LogError("Network ["..tostring(name).."] get Cmd error : Cmd is nil")
--     end
--     return nil
-- end

-- -- 请求数据获取对应cmd号
-- function NetworkManager:getCmdName( id )
--     if self._cmdNameMap[tostring(id)] then
--         return self._cmdNameMap[tostring(id)]
--     end
--     return nil
-- end

-- -- 数据获取对应反序列化pb结构
-- function NetworkManager:getDecodePbWithName( name )
--     -- for k,v in pairs(self._msgDecodeMap) do
--     --     logError("debomap ==== k=",k)
--     --     logError("DEBOMAP ==== V=",v)
--     -- end
--     if self._msgDecodeMap[string.lower(name)] then
--         return self._msgDecodeMap[string.lower(name)]
--     end
--     return nil
-- end

-- -- 数据获取对应反序列化pb结构
-- function NetworkManager:getDecodePbWithCmdId( id )
--     local name = self:getCmdName(id)
--     -- logError("name = ======="..name)
--     if name then
--         return self:getDecodePbWithName(name)
--     end
--     return nil
-- end

-- -- 设置socket状态
-- function NetworkManager:setStatus(status)
--     if status < NetworkEnum.SOCKET_STATUS_ENUM.CONNECTING and status > NetworkEnum.SOCKET_STATUS_ENUM.CLOSED then
--         return
--     end
--     self._status = status
-- end

-- -- socket是否某种状态
-- function NetworkManager:isEqualSocketStatus( status )
--     return self._status == status and true or false
-- end

-- -- 添加请求
-- function NetworkManager:_addRequest( reqData )
--     local reqIndex = reqData:getReqIndex()
--     if reqIndex then
--         logError("_addRequest ===========",reqData,reqIndex,reqData:getCallback())
--         local tempCb = reqData:getCallback()
--         -- if tempCb then
--         --     tempCb(false,1,1)
--         -- end
--         self._reqMap[reqIndex] = reqData
--     end
-- end

-- -- 添加等待请求
-- function NetworkManager:_addWaitingRequest( reqData )
--     if reqData then
--         table.insert( self._waitingReqList, reqData )
--     end
-- end

-- -- 获取请求数据
-- function NetworkManager:getRequestData( reqIndex )
--     return self._reqMap[tostring(reqIndex)]
-- end

-- -- 清除请求数据
-- function NetworkManager:cleanRequestData( reqIndex )
--     if self._reqMap[tostring(reqIndex)] ~= nil then
--         self._reqMap[tostring(reqIndex)]:onDestroy()
--         self._reqMap[tostring(reqIndex)] = nil
--     end
-- end

-- ---以下是有关服务器推送协议的监听添加 start

--     ---添加服务器推送协议侦听请求
--     function NetworkManager:AddPushMsgListner( reqData )
--         self:_addPushMsgListner(reqData)
--     end

--     ---添加服务器推送协议侦听请求
--     function NetworkManager:_addPushMsgListner(reqData)
--         local cmdId = reqData:getPktData():getCmdId()
--         if cmdId then
--             local tempName = self:getCmdName(cmdId)
--             self._pushMap[tempName] = reqData
--         end
--     end

--     ---获取服务器推送侦听协议数据
--     function NetworkManager:getPushDataForMonitor( reqIndex )
--         return self._pushMap[tostring(reqIndex)]
--     end

-- ---以下是有关服务器推送协议的监听添加 end

-- -- 发送请求
-- function NetworkManager:sendRequestMsg( reqData )
--     self:_addRequest(reqData)
--     self:_onSendMsg(reqData)
-- end


-- -- 发送请求
-- function NetworkManager:_onSendMsg( reqData )
--     -- 获取动态密钥时 不允许发其他请求出去，这时候发送的请求会加入等待队列，获取动态密钥后重新发送
--     if self._isGettingDynSecret and not reqData:isHttpRequest() then
--         self:_addWaitingRequest(reqData)
--         return
--     end
--     local sendSuccess = true
--     local seq = reqData:getSeq()

--     if reqData:isHttpRequest() then
--         logError("未支持http请求")
--         --reqData:addRequestTime()
--         --HttpServiceManager.Instance:Send(reqData:getMod(), reqData:getSendMsg(), seq)
--     else
--         if self:isEqualSocketStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTED) then
--             reqData:addRequestTime()
--             local msg = reqData:getSendMsg()
--             -- SocketClient.Instance:Send(msg) --暂时用byte去发送
--             TcpSocketClient.GetInstance():Send(msg)
--         else
--             sendSuccess = false
--             self:_onSocketBeginConnect()
--         end
--     end

--     if sendSuccess then
--         --print("sendMsg Success:", reqData._pktName)
--         --print("[NetworkManager][send]["..seq.."]["..reqData:getPktName().."]: req \nbody : \n"..tostring(reqData:getBodyData()).."\n\npkt : "..tostring(reqData:getPktData():getMsg()).."\nmsg : "..reqData:getSendMsg().."\n")
--     else
--         --print("sendMsg Failed:", reqData)
--         --print("[NetworkManager]socket close & try reconnect")
--         logError("sendError " + reqData._pktName)
--     end
-- end

-- -- 发送等待请求
-- function NetworkManager:_onSendWaitingMsg( )
--     if not self._isGettingDynSecret then
--         for i=1, #self._waitingReqList do
--             local reqData = self._waitingReqList[i]
--             self:_onSendMsg(reqData)
--         end
--         self._waitingReqList = {}
--     end
-- end

-- -- 重发请求
-- function NetworkManager:onRetryRequest( reqData )
--     if reqData == nil then
--         return
--     end

--     if reqData:getRequestTime() <= reqData:getRetryTime() then
--         if not reqData:isHttpRequest() and not self:isEqualSocketStatus(NetworkEnum.SOCKET_STATUS_ENUM.CONNECTED) then
--             self:_onRequestError(reqData)
--         else
--             self:_onSendMsg(reqData)
--         end
--     else
--         self:_onRequestError(reqData)
--     end
-- end

-- function NetworkManager:_onRequestError( reqData )
--     if reqData == nil or not self.isInstanceOf(reqData, RequestData) then
--         return
--     end

--     local callback = reqData:getCallback()
--     callback(false, nil, reqData:getBodyData())
--     self:cleanRequestData(reqData:getSeq())
-- end

-- -- 全部未收到rsp的socket请求重发
-- function NetworkManager:onRetry( )
--     for reqIndex,reqData in pairs(self._reqMap) do
--         if reqData:isHttpRequest() == false then
--             self:onRetryRequest(reqData)
--         end
--     end
-- end

-- -- 发送socket请求
-- function NetworkManager:onSocketRequest(data, callback)
--     --print("发送socket请求:", data)
--     local reqData = RequestData:new(data, callback)
--     --reqData:setSocketReqTimeOut(function( ... )networkManager:onSocketMsgTimedOut(...)end)
--     self:sendRequestMsg(reqData)
-- end

-- -- 发送http请求
-- function NetworkManager:onHttpRequest( mod, data, callback )
--     --local reqData = RequestData(data, callback)
--     local reqData = RequestData:new(data, callback)
--     reqData:setIsHttp(true)
--     reqData:setMod(mod)
--     self:sendRequestMsg(reqData)
-- end

-- -- 收到消息 msg:byte[]
-- function NetworkManager:onResponse(msg)
--     -- 第一步 base64解
--     local rspMsg = LuaUtil.decodeBase64(msg)
--     -- 第二步 解析Packet
--     --cmdpkg_pb.
--     local rspPkt = cmdpkg_pb.CmdPacket()
--     rspPkt:ParseFromString(rspMsg)
--     --try {
--     --    function()
--     --        rspPkt:ParseFromString(rspMsg)
--     --    end,
--     --    catch {
--     --       function(error)
--     --            gLog.warning("[NetworkManager] Decode Rsp Packet pb fail \n caught error: " .. error)
--     --       end
--     --    }
--     --}
--     -- 检查有木有PktInfo 没有的为异常包
--     if rspPkt and rspPkt.Head and rspPkt.Body then
--         -- 通过PktInfo获取cmd号
--         local cmd = rspPkt.Head.Cmd or nil
--         if cmd ~= nil then
--             --TODO 硬编码先解决匹配协议对接不上的问题，后面需要重构！
--             -- logError("S2C111  cmd ="..cmd)
--             -- if cmd >= 20000 then
--             --     cmd = cmd * 10 + 1
--             -- end
--             --response消息通过规则
--             if rspPkt.Head.TransType == cmdpkg_pb.RESPONSE then
--                 cmd = self:genResponseCMD(cmd)
--             end
--             -- logError("S2C222  cmd ="..cmd)
--             -- 通过cmd号获取对应解析pb结构
--             logError("S2C  type = "..rspPkt.Head.TransType.."    type ="..cmdpkg_pb.RESPONSE)
--             local rspName = self:getCmdName(cmd)
--             logError("S2C ",rspName)
--             local bodyMsg = self:getDecodePbWithCmdId(cmd)
--             logError("S2C  body ====",bodyMsg)
--             --local bodyMsg = game_pb.LoginRsp();
--             if bodyMsg then
--                 if bodyMsg._pbname then
--                     rspName = bodyMsg._pbname
--                 end
--                 bodyMsg = bodyMsg()
--               --bodyMsg:ParseFromString(rspPkt.Body)
--                 NetworkManager : try {
--                     function()
--                         bodyMsg:ParseFromString(rspPkt.Body)
--                     end,
--                     catch =
--                         function(error)
--                             bodyMsg = nil
--                             gLog.warning("[NetworkManager] Decode Rsp " .. cmd .. " pb fail \n caught error: " .. error)
--                         end
--                 }
--             end

--             if rspPkt.Head and rspPkt.Head.Serial and rspPkt.Head.Serial ~= 0 then
--                 local isSuccess = true;
--                 if bodyMsg == nil then
--                     isSuccess = false;
--                     logWarn("[NetworkManager][onResponse]["..rspPkt.Head.Serial.."]["..rspName.."][error] decode body fail")
--                     --gGame.topTipsController:showTips("服务器离家出走")
--                 else
--                     --logError(bodyMsg.Head,bodyMsg.Head.StatusCode)
--                     if rspPkt.Head and rspPkt.Head.StatusCode ~= nil and rspPkt.Head.StatusCode ~= 0 then
--                         isSuccess = false;
--                         logWarn("[NetworkManager][onResponse]["..rspPkt.Head.Serial.."]["..rspName.."][error] code : "..rspPkt.Head.StatusCode)
--                     else
--                         logWarn("[NetworkManager][onResponse]["..rspPkt.Head.Serial.."]["..rspName.."] : \nbody : \n".. tostring(bodyMsg).."\n\npacket : "..tostring(rspPkt))
--                     end
--                 end
--                 local reqData = self:getRequestData(rspPkt.Head.Serial)
--                 logError("reqData ====================",reqData)
--                 if reqData ~= nil then
--                     ---- 处理用户资产数据
--                     --if bodyMsg and bodyMsg.Rewards then
--                     --    gGame.propManager:updatePropsWithRsp(bodyMsg.Rewards,nil,reqData:getInstantRefresh())
--                     --end
--                     local callback = reqData:getCallback()
--                     logError("callback ===========",callback)
--                     if callback then
--                         logError("callback isSuccess===========")
--                         callback(isSuccess, bodyMsg, reqData:getBodyData())
--                         logError("callback isSuccess===========2222222222")
--                     end
--                     self:cleanRequestData(rspPkt.Head.Serial)
--                 end
--             else
--                 ---- 处理用户资产数据
--                 --if bodyMsg and bodyMsg.Rewards then
--                 --    gGame.propManager:updatePropsWithRsp(bodyMsg.Rewards,nil,true)
--                 --end

--                 ---没有Seq号的为服务器推送过来消息
--                 logWarn("[NetworkManager][onPushMsg]["..rspName.."] : \nbody : \n".. tostring(bodyMsg).."\n\npacket : "..tostring(rspPkt))
--                 local isSuccess = true;

--                 if bodyMsg == nil then
--                     isSuccess = false;
--                     logWarn("[NetworkManager][onPushMsg]["..rspPkt.Head.Serial.."]["..rspName.."][error] decode body fail")
--                 else
--                     if rspPkt.Head and rspPkt.Head.StatusCode ~= nil and rspPkt.Head.StatusCode ~= 0 then
--                         isSuccess = false;
--                         self._statusCode = rspPkt.Head.StatusCode
--                         logWarn("[NetworkManager][onPushMsg]["..rspPkt.Head.Serial.."]["..rspName.."][error] code : "..rspPkt.Head.StatusCode)
--                     else
--                         logWarn("[NetworkManager][onPushMsg]["..rspPkt.Head.Serial.."]["..rspName.."] : \nbody : \n".. tostring(bodyMsg).."\n\npacket : "..tostring(rspPkt))
--                     end
--                 end
--                 local reqData = self:getPushDataForMonitor(self:getCmdName(cmd))
--                 if reqData ~= nil then
--                     local callback = reqData:getCallback()
--                     if callback then
--                         callback(isSuccess, bodyMsg, reqData:getBodyData())
--                     end
--                 end

--             end
--         end
--     else
--         logError("[NetworkManager][onResponse][error] packet : "..tostring(rspPkt))
--     end
-- end
-- --用于返回服务器状态码
-- function NetworkManager:GetStatusCode()
--     return self._statusCode
-- end

-- function NetworkManager : try(block)
--    -- get the try function
--        local try = block[1]
--          assert(try)
--         local funcs = block[2]
--          local ok, errors = xpcall(try, debug.traceback)
--          if not ok then
--                  -- run the catch function
--                 if funcs and funcs.catch then
--                          funcs.catch(errors)
--                      end
--              end
--         if funcs and funcs.finally then
--                 funcs.finally(ok, errors)
--              end
--          if ok then
--                  return errors
--              end
-- end

-- ------------------------------------------------------------------------
-- ---
-- return NetworkManager