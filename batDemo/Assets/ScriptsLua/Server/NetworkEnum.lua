---登录枚举
NetworkEnum = {}

-- 测试环境端口
NetworkEnum.SOCKET_TEST_PORT = {
    TEST_ENV    = 9800,
    KSANA_ENV   = 9801,
    STAR_ENV    = 9802,
    TOM_ENV     = 10011
}

---网络状态
NetworkEnum.SOCKET_STATUS_ENUM = {
    ---正在连接
    CONNECTING = 0,
    ---已经连接
    CONNECTED  = 1,
    ---连接已经关闭/打开连接失败
    CLOSED     = 2,
    ---重连中
    Reconnect   = 3,
}

NetworkEnum.IsReconnect = false

---socket重连尝试次数
NetworkEnum.SOCKET_RECONNECT_TIMES = 3
---socket重连间隔
NetworkEnum.SOCKET_RECONNECT_INTERVAL = 3
---socket请求超时时间
NetworkEnum.SOCKET_MSG_TIME_OUT_INTERVAL = 6
---心跳间隔时间
NetworkEnum.SOCKET_HEART_BEAT_INTERVAL = 15000
---动态密钥间隔时间
NetworkEnum.SOCKET_DYNAMIC_SECRET_INTERVAL = 300