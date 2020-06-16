--这里存放枚举定义数据

--场景类型
SceneType = {
    None = "None",
    Login = "Login",
    Lobby = "Lobby",
    Battle = "Battle"
}

LayerType =
{
    UI = "UI"
}

EAvatarPart =
{
    -- 全部
    all = -1,
    -- 基础裸模
    baseBody = 0,
    -- 头饰
    headWear = 1,
    -- 表情
    face = 2,
    -- 脸饰
    faceWear = 3,
    -- 上衣
    topCloth = 4,
    -- 手饰
    handWear = 5,
    -- 裤子
    downCloth = 6,
    -- 鞋子
    shoe = 7,
    -- 背饰
    backWear = 8,
    -- 脚印
    footWear = 9,
    -- 套装，套装是一个模型整体
    suit = 10
}

-- 前端错误码
ErrCode = {
    login_socketErr = 1,
    login_loginFailed = 2,
}

--- 货币类型,和服务端保存一致
CURRENCY =
{
    --人民币
    CURRENCY_RMB = 0,
    --金币
    CURRENCY_GOLD =1,
    --钻石
    CURRENCY_DIAMOND =2,
}

serverList=
{
    {
        name = "147内网",
        ip = "192.168.1.147",
        port = 9320
    },
    {
        name ="树江",
        ip = "192.168.1.160",
        port = 9320
    },
	{
        name ="树江_linux",
        ip = "192.168.231.128",
        port = 9320
    },
    {
       name ="彭琛",
       ip = "192.168.1.243",
       port = 9320
    },
    {
        name ="桑至洪",
        ip = "192.168.1.153",
        port = 9320
    },
    {
        name ="本机",
        ip = "127.0.0.1",
        port = 9320
    },
    {
        name ="外网",
        ip = "182.254.181.66",
        port = 9320
    }
}

---卡牌解锁状态
CardLockStatus =
{
    lock = 0,
    unlock = 1
}

---卡牌界面4个槽位状态
LockCardDataStatus =
{
    lock = 0,
    unlockEmpty = 1,
    unlockNotEmpty = 2
}

---卡牌详情界面3中状态
CardDetailDataStatue =
{
    lock = 0,
    unLockNotEquip = 1,
    unlockEquip = 2
}

-- 用户信息
UserInfo =
{
    openid,
    uid,
    name,
    guestid,
    logintype,
}
--服务器id索引
serverIndex = 1

--记录掉线前一个状态(记录最近的一个pb请求是啥)
--默认登陆掉线
--lastLoseConnectionReq={}
lastLoseConnectionReq = nil
--lastLoseConnectionReq.OpenID="102"
--lastLoseConnectionReq.OpenID="13"


