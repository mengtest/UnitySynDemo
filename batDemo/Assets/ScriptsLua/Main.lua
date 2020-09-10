--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:45
LastEditors: xsddxr909
LastEditTime: 2020-09-07 19:29:02
--]]
Main = {}

--lua加载完毕后执行
function Main.Start(str)

    require("Global");
  
    Main.InitLogSwitcher();
    --初始化全局游戏对象
    log("Game start   hello ~~~~~~~~Main~~~~~~~~~~~~~~~~")
  
    Global.initManager();

    
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



return Main
