---@class GameObjBase:EventObj
GameObjBase = Class("GameObjBase",EventObj)
---@return GameObjBase


-- 类初始化，类似构造函数
function GameObjBase:initialize()
   EventObj.initialize(self);
   self.name="GameObjBase";
end


function GameObjBase:destroy()
    self.name=nil;
    EventObj.destroy(self);
 end

-- 测试event事件.
function GameObjBase:testEvent(EventKey,value)
    self:dispatchEvent(EventKey,value);
    log("GameObjBase test"..self.name);
end


return GameObjBase