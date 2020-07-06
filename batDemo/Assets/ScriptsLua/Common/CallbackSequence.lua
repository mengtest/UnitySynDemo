CallbackSequence = Class("CallbackSequence")

function CallbackSequence:initialize()
    self._callbackList = {}
    self._index = 1
    self._isStarted = false
    self._onStart = nil
    self._onComplete = nil
end

-- isSync: �Ƿ�Ϊͬ���ص���ͬ���ص�����ִ�к�����ִ����һ���ص�
function CallbackSequence:append(callback, isSync)
    if type(callback) ~= "function" then
        Debugger.LogError("Only can append function.")
        return
    end
    self._callbackList[#self._callbackList + 1] = {
        callback = callback,
        isSync = isSync,
    }
end

function CallbackSequence:start()
    if self._isStarted then return end
    self._isStarted = true
    self._index = 1
    if self._onStart ~= nil then
        self._onStart()
    end
    self:execute()
end

function CallbackSequence:stop()
    self._isStarted = false
    self._callbackList = {}
end

function CallbackSequence:execute()
    if self._index <= #self._callbackList then
        local callback = self._callbackList[self._index]
        self._index = self._index + 1
        local index = self._index
        if callback.isSync then
            local ok, msg = pcall(callback.callback)
            if not ok then
                logError(tostring(msg))
            end
            self:execute()
        else
            local ok, msg = pcall(callback.callback, function() self:execute() end)
            
            if not ok then
                logError(tostring(msg))
                --self._indexδ�ı�˵����������ʱ���ص���δִ�У��ɼ���ִ�ж���
                if index == self._index and index <= #self._callbackList then
                    self:execute()
                end
            end
        end
    else 
        self._index = self._index + 1
        self:stop()
    end
end

return CallbackSequence



----------------local sequence = CallbackSequence()
--需要callback--sequence:append(function(callback) self:loadFonts(callback) end)
--需要callback--sequence:append(function(callback) self.loadingPageController:showView(callback) end)
--需要callback--sequence:append(function(callback) self.viewManager:loadRoot(callback) end)
--直接执行完-----sequence:append(function() self:destroyStartScene() end, true)
--直接执行完-----sequence:append(function() self:onHelloReq() end, true)
-----------------sequence:start()