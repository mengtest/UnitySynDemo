-- 视图层级类型，层级高的永远显示在层级低的上面，同层级使用后上原则
---@class ViewLayer
ViewLayer = {
    -- 内容层
    content = 0,
    -- 弹出内容层，弹框，活动等等
    popup = 1,
    -- 提示信息层
    info = 2,
    -- 引导层
    guide = 3,
    -- 顶部层，加载界面等
    top = 4,
    -- 层级数量这个要放最后面哦
    count = 5,
}
---@class ViewShowMode
ViewShowMode={
    Normal="Normal",
    ReverseChange="ReverseChange",
    HideOther="HideOther",
}
---面板状态.
---@class ViewPanelState
ViewPanelState={
      init = 0,
      Open = 1,
      Close = 2,
      Hiding = 3,
      Redisplay =4,
      Freeze = 5,
}

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
    LoginView = "View.Login.LoginView",
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
