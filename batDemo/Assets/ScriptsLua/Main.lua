Main = {}

--lua加载完毕后执行
function Main.Start(str)

    require("Global");
  
    Main.InitLogSwitcher();
    --初始化全局游戏对象
    log("Game start   hello ~~~~~~~~fatherer~~~~~~~~~~~~~~~~")
  
    Main.initManager();

    ---@type Example
    local exam= require("Example");
    exam.Start();
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
    Main.ViewManager = ViewManager:new();
    Main.ViewManager:init();
end


return Main
