---@class GameSet
GameSet = Class("GameSet")
---@return GameSet
---

function GameSet:initialize()
    ---@type float 游戏帧率
    self.FrameRate=30;
     ---@type float 刷新时间
    self.DeltaTime=1/self.FrameRate;
end


return GameSet



