---@class EventObj
EventObj = Class("EventObj")
---@return EventObj



-- 类初始化，类似构造函数
function EventObj:initialize()
    -- 事件表
    self._eventMap = {};
    -- 事件序号
    self._eventIndex = 0;
end

-- 销毁
function EventObj:destroy()
    log("EventObj destroy")
    self._eventMap = nil;
    self._eventIndex = 0;
end


function EventObj:addListener(eventKey, eventFunc)
    if type(eventKey) == "string" and type(eventFunc) == "function" then
        if self._eventMap[eventKey] == nil then
            self._eventMap[eventKey] = {}
        end

        self._eventIndex = self._eventIndex + 1
        self._eventMap[eventKey][tostring(self._eventIndex)] = eventFunc
        -- table.insert( self._eventMap[eventKey], eventFunc )
        return self._eventIndex
    else
        log("EventManager addListener error")
    end
end

function EventObj:removeListener(eventKey, eventId)
    if Utils.isString(eventKey) and eventId ~= nil then
        local map = self._eventMap[eventKey]
        if not map then return end
        if map[tostring(eventId)] then
            map[tostring(eventId)] = nil
        end
    else
        log("EventManager removeListener error",eventKey)
    end
end

function EventObj:dispatchEvent( eventKey, ... )
 
    if type(eventKey) == "string" then
        if self._eventMap[eventKey] ~= nil then
            for eventId, eventFunc in pairs(self._eventMap[eventKey]) do
                eventFunc(...)
             -- gGame.crashHelper:addEventLog(eventKey,...)
            end
        end
    else
        log("EventManager dispatchEvent error")
    end
end

function EventObj:cleanEvent( eventKey )
    if eventKey and type(eventKey) == "string" then
        self._eventMap[eventKey] = nil
    else
        log("EventManager cleanEvent error")
    end
end


return EventObj