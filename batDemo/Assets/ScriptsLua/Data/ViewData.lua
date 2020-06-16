-- 视图层级类型，层级高的永远显示在层级低的上面，同层级使用后上原则
---@class ViewLayer
ViewLayer = {
    ---@see @隐藏界面层
    close = 0,
    -- 内容层
    content = 1,
    -- 弹出内容层，弹框，活动等等
    popup = 2,
    -- 提示信息层
    info = 3,
    -- 引导层
    guide = 4,
    -- 顶部层，加载界面等
    top = 5,
    -- 层级数量这个要放最后面哦
    count = 6,
}
-- todo 界面显示动画配置和实现

---@param layer ViewLayer 层级
---@param isSingle boolean 是否单例
---@param autoMask boolean 是否需要遮罩背景
function createViewConfig(layer, isSingle, autoMask)
    if isSingle == nil then
        isSingle = true
    end

    local cfg = {}
    cfg.layer = layer
    cfg.isSingle = isSingle
    cfg.isMask = autoMask
    return cfg
end

--视图类型
---@class ViewType
ViewType = {
    ---GM界面
    GmView = "GmView",
    ---读取等待界面
    WaitView = "WaitView",
    ---漂字信息界面
    FlowMsgView = "FlowMsgView",
    ---登录界面
    LoginView = "LoginView",
    ---大厅主界面
    LobbyView = "LobbyView",
    ---提示框界面
    MessageView = "MessageView",
    ---测试界面
    TestView = "TestView",
    ---角色创建界面
    CreatePlayerView = "CreatePlayerView",
    ---加载界面
    LoadingView ="LoadingView",
    ---匹配界面
    MatchView = "MatchView",
    ---战斗地图界面
    BattleMapView ="BattleMapView",
    ---玩家操作界面
    ControlPlayerView ="ControlPlayerView",
    ---游戏状态界面
    GameStateView ="GameStateView",
    ---战斗背包界面
    BagView = "BagView",
    ----------------------时装相关---------------------
    ---时装界面
    AvatarView = "AvatarView",
    ---时装搭配界面
    AvatarSettingView = "AvatarSettingView",
    ----------------------时装相关---------------------
    ----------------------卡牌相关---------------------
    CardView = "CardView",
    CardDetailView = "CardDetailView",
    CardDebriChangeView = "CardDebriChangeView"
    ----------------------卡牌相关---------------------
}

-- 视图数据配置
---@class ViewConfig
ViewConfig = {}
ViewConfig[ViewType.LoginView] = createViewConfig(ViewLayer.content,true,true)
ViewConfig[ViewType.LobbyView] = createViewConfig(ViewLayer.content)
ViewConfig[ViewType.AvatarView] =createViewConfig(ViewLayer.content)
ViewConfig[ViewType.CardView] =createViewConfig(ViewLayer.content)
ViewConfig[ViewType.ControlPlayerView] =createViewConfig(ViewLayer.content, true)
ViewConfig[ViewType.MatchView] = createViewConfig(ViewLayer.content)

ViewConfig[ViewType.BagView] = createViewConfig(ViewLayer.popup,true,true)
ViewConfig[ViewType.BattleMapView] =createViewConfig(ViewLayer.popup,true)
ViewConfig[ViewType.GameStateView] =createViewConfig(ViewLayer.popup, true)
ViewConfig[ViewType.MessageView] = createViewConfig(ViewLayer.popup, true, true)
ViewConfig[ViewType.TestView] = createViewConfig(ViewLayer.popup, true, true)
ViewConfig[ViewType.CreatePlayerView] = createViewConfig(ViewLayer.popup,true)
ViewConfig[ViewType.AvatarSettingView] = createViewConfig(ViewLayer.popup,true, true)
ViewConfig[ViewType.CardDetailView] = createViewConfig(ViewLayer.popup,true, true)
ViewConfig[ViewType.CardDebriChangeView] = createViewConfig(ViewLayer.popup,true, true)

ViewConfig[ViewType.FlowMsgView] = createViewConfig(ViewLayer.info, false)

ViewConfig[ViewType.WaitView] = createViewConfig(ViewLayer.top)
ViewConfig[ViewType.LoadingView] = createViewConfig(ViewLayer.top)
ViewConfig[ViewType.GmView] = createViewConfig(ViewLayer.top,true, true)


for k,v in pairs(ViewType) do
    if k~=v then
        logError("ViewType的key和value值不同"..k)
    end
    local cfg = ViewConfig[v]
    if cfg == nil then
        logError("ViewType没有配置数据", k)
    end
end