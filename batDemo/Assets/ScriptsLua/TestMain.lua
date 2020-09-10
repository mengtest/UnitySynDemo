--[[
Description: 
Version: 1.0
Autor: xsddxr909
Date: 2020-08-03 17:41:45
LastEditors: xsddxr909
LastEditTime: 2020-09-07 19:26:47
--]]
TestMain = {}

--lua加载完毕后执行
function TestMain.Start(str)

    require("Global");
  
    TestMain.InitLogSwitcher();
    --初始化全局游戏对象
    log("Game start   hello ~~~~~~~Test~fatherer~~~~~~~~~~~~~~~~")
  
    Global.initManager();

    Global.ViewManager:Show(ViewType.HuDBatPanel);

    ---@type Example
   --- require("Example");
  --  Example.Start();
end


function TestMain.InitLogSwitcher()
    local openLog= true
    local openErrorLog = true
    local openWarningLog = true
    GameLuaManager.SetLogSwitcher(openLog,openErrorLog,openWarningLog);
end




return TestMain
