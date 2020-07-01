EventManager = {}

function EventManager.init()
    -- 事件表
    EventManager._eventMap = {}
    -- 事件序号
    EventManager._eventIndex = 0
end

function EventManager.destroy()
    EventManager._eventMap = nil;
    EventManager._eventIndex = 0;
end

function EventManager.addListener(eventKey, eventFunc)
    if type(eventKey) == "string" and type(eventFunc) == "function" then
        if EventManager._eventMap[eventKey] == nil then
            EventManager._eventMap[eventKey] = {}
        end

        EventManager._eventIndex = EventManager._eventIndex + 1
        EventManager._eventMap[eventKey][tostring(EventManager._eventIndex)] = eventFunc
        -- table.insert( self._eventMap[eventKey], eventFunc )
        return EventManager._eventIndex
    else
        log("EventManager addListener error")
    end
end

function EventManager.removeListener(eventKey, eventId)
    if Utils.isString(eventKey) and eventId ~= nil then
        local map = EventManager._eventMap[eventKey]
        if not map then return end
        if map[tostring(eventId)] then
            map[tostring(eventId)] = nil
        end
    else
        log("EventManager removeListener error",eventKey)
    end
end

function EventManager.dispatchEvent( eventKey, ... )
 
    if type(eventKey) == "string" then
        if EventManager._eventMap[eventKey] ~= nil then
            for eventId, eventFunc in pairs(EventManager._eventMap[eventKey]) do
                eventFunc(...)
             -- gGame.crashHelper:addEventLog(eventKey,...)
            end
        end
    else
        log("EventManager dispatchEvent error")
    end
end

function EventManager.cleanEvent( eventKey )
    if eventKey and type(eventKey) == "string" then
        EventManager._eventMap[eventKey] = nil
    else
        log("EventManager cleanEvent error")
    end
end

return EventManager