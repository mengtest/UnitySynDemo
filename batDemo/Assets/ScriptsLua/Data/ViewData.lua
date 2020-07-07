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
    ---登录界面
    LoginView = "View.Login.LoginView",
}
