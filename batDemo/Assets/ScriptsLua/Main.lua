local Main = {}

--lua加载完毕后执行
function Main.Start(str)

    require("Global")

  
    Main.initLogSwitcher()
    --初始化全局游戏对象
    log("Game start   hello ~~~~~~~~father~~~~~~~~~~~~~~~~")
 --   local gameCls = require("Game.Game")
 --   gGame = gameCls:new()
 --   gGame:start()
end

function Main.initLogSwitcher()
    local openLog= true
    local openErrorLog = true
    local openWarningLog = true
    Utils.SetLogSwitcher(openLog,openErrorLog,openWarningLog)
end

return Main
