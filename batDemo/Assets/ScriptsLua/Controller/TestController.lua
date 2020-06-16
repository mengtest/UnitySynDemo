---@class TestController : BaseController  测试用控制器
TestController = createController("TestController")
---@type TestController 单例，在ControllerManager中注册之后才能使用
TestController.instance = nil

function TestController: onCreate()
    -- 单例赋值，方便代码提示
    TestController.instance = self
end

function TestController: onBegin()
    -- 当控制器启动，一般在登陆成功后或退出战斗回到大厅时调用
end

function TestController: onStop()
    -- 当控制器停止，一般在进入战斗时调用，用于停止计时器等
end

function TestController: testFunc(callBack)
    -- 同服务器交互
    --local data = game_pb.HelloReq()
    --gGame.serverMgr.requestServer(data, callBack)
end
