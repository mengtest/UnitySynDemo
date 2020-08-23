--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:45
LastEditors: xsddxr909
LastEditTime: 2020-08-05 14:48:48
--]]
Main = {}

--lua加载完毕后执行
function Main.Start(str)

    require("Global");
  
    Main.InitLogSwitcher();
    --初始化全局游戏对象
    log("Game start   hello ~~~~~~~~fatherer~~~~~~~~~~~~~~~~")
  
    Main.initManager();

    
    ---@type Example
    require("Example");
    Example.Start();
end


function Main.InitLogSwitcher()
    local openLog= true
    local openErrorLog = true
    local openWarningLog = true
    GameLuaManager.SetLogSwitcher(openLog,openErrorLog,openWarningLog);
end


-- 初始化管理者
function Main.initManager()

    TweenManager.init();
    EventManager.init();
    EventManager.dispatchEventToC(SystemEvent.LUA_INIT_COMPLETE);
    Main.ViewManager = ViewManager:new();
    Main.ViewManager:init();

    
end


return Main
