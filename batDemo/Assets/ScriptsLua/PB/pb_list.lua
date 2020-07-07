--Auto Create , Don't Edit--
--If you have any questions, please contact ma huibao--

local tempPath2 = GameLuaManager.GetProtoBytesPath()
assert(pb.loadfile(tempPath2))
---战斗回合状态
---@class ROUND_STATUS
ROUND_STATUS = {
    ---@type number 准备就续-未开始
    ROUND_READY = 0,
    ---@type number 战斗中-已开始
    ROUND_BATTLING = 1,
    ---@type number 战斗结束
    ROUND_COMPLETE = 2,
}
---
---@class ROUND_PLAYER_STATUS
ROUND_PLAYER_STATUS = {
    ---@type number 准备就续-未开始
    PLAYER_READY = 0,
    ---@type number 准备就续-未开始
    PLAYER_JOINED = 1,
    ---@type number 战斗中-已开始
    PLAYER_BATTLING = 2,
    ---@type number 提前退出
    PLAYER_EARLY_QUIT = 3,
    ---@type number 战斗结束
    PLAYER_COMPLETE = 4,
}
---cmd组成规则，服务器mod*10000+cmd
---@class GAME_CMD
GAME_CMD = {
    ---@type number 
    INVALID_CMD = 0,
    ---@type number 测试用例
    GAME_CMD_GETTEST = 99,
    ---@type number 
    GAME_CMD_HELLO = 1,
    ---@type number 登录系统
    GAME_CMD_LOGIN = 10000,
    ---@type number 心跳Heartbeat
    GAME_CMD_HEARTBEAT = 10001,
    ---@type number 获取随机角色名字
    GAME_CMD_RANDROLENAME = 10002,
    ---@type number 创建角色
    GAME_CMD_CREATEROLE = 10003,
    ---@type number 角色信息
    GAME_CMD_GETROLE = 10004,
    ---@type number gm命令
    GAME_CMD_GM = 10005,
    ---@type number 通知更新某一对象NoticeObjUpdate
    GAME_CMD_NOTICEOBJUPDATE = 19000,
    ---@type number 通知用户下线，（被踢下线，或，被挤下线）的通知
    GAME_CMD_NOTICEOFFLINE = 19999,
    ---@type number 通知战斗结算
    GAME_CMD_NOTICESUMMARY = 19001,
    ---@type number 通知升级
    GAME_CMD_NOTICEUPLV = 19002,
    ---@type number 开始匹配
    GAME_CMD_DOMATCH = 20001,
    ---@type number 查询匹配结果
    GAME_CMD_QUERYMATCH = 20002,
    ---@type number 取消匹配
    GAME_CMD_CANCELMATCH = 20003,
    ---@type number 加入战斗房间
    GAME_CMD_JOINBATTLE = 20004,
    ---@type number avatar:获取数据
    GAME_CMD_GETAVATARSLOT = 30000,
    ---@type number avatar:选择换装
    GAME_CMD_CHANGEAVATAR = 30001,
    ---@type number avatar:历史换装方案
    GAME_CMD_GETAVATARSETTING = 30002,
    ---@type number avatar:保存换装方案
    GAME_CMD_SAVEAVATARSETTING = 30003,
    ---@type number avatar:获取装备数据
    GAME_CMD_GETAVATARITEMDATA = 30005,
    ---@type number avatar:解锁搭配方案
    GAME_CMD_UNLOCKAVATARSETTING = 30006,
    ---@type number avatar:卸载装备
    GAME_CMD_UNEQUIPAVATAR = 30007,
    ---@type number avatar:切换方案
    GAME_CMD_CHANGESETTING = 30008,
    ---@type number 卡牌系统：获取卡槽数据
    GAME_CMD_GETCARDPAGEDATA = 40000,
    ---@type number 卡牌系统：获取卡牌数据
    GAME_CMD_GETCARDDATA = 40001,
    ---@type number 卡牌系统:卸载装上卡牌
    GAME_CMD_HANDLECARD = 40002,
    ---@type number 卡牌系统:升级卡片
    GAME_CMD_UPGRADECARD = 40003,
    ---@type number 卡牌系统:兑换卡牌核心
    GAME_CMD_EXCHANGECARDDEBRIS = 40004,
    ---@type number 卡牌系统:保存當前頁
    GAME_CMD_CHANGECARDPAGE = 40005,
    ---@type number 卡牌系统:解锁卡槽
    GAME_CMD_UNLOCKCARDSLOT = 40006,
    ---@type number 卡牌系统:获取某张卡牌数据
    GAME_CMD_GETCARDDATABYID = 40007,
    ---@type number 卡牌系统：获取当前页数据
    GAME_CMD_GETCARDCURPAGEDATA = 40008,
    ---@type number 卡牌系统：获取某页数据
    GAME_CMD_GETCARDONEPAGEDATA = 40009,
    ---@type number 卡牌系统：获取某页数据
    GAME_CMD_GETCARDCURPAGEALLDATA = 40010,
    ---@type number Batle数据
    GAME_CMD_GETBATTLEDATAINFO = 50000,
}
---
---@class TRANS_TYPE
TRANS_TYPE = {
    ---@type number requesttag
    REQUEST = 0,
    ---@type number responsetag
    RESPONSE = 1,
}
---登录类型
---@class LOGIN_TYPE
LOGIN_TYPE = {
    ---@type number 
    LOGIN_TYPE_KNOWN = 0,
    ---@type number 游客登录
    LOGIN_TYPE_GUEST = 1,
    ---@type number 自建账号登录
    LOGIN_TYPE_SELF_ACCOUNT = 2,
    ---@type number 第三方sdk登入
    LOGIN_TYPE_SDK = 3,
}
---Server:账号openid由服务器去获取
---@class ACCOUNT_GAIN_TYPE
ACCOUNT_GAIN_TYPE = {
    ---@type number 
    ACCOUNT_GAIN_TYPE_CLIENT = 0,
    ---@type number 
    ACCOUNT_GAIN_TYPE_SERVER = 1,
}
---
---@class CHANNEL
CHANNEL = {
    ---@type number 无来源,包括自建账号,游客账号等等
    CHANNEL_NONE = 0,
}
---渠道来源：账号来源：QQ、微信、facebook等等
---@class SDK_TYPE
SDK_TYPE = {
    ---@type number 
    SDK_TYPE_NONE = 0,
    ---@type number 
    SDK_TYPE_QQ = 1,
    ---@type number 
    SDK_TYPE_WCHAT = 2,
}
---包区域来源
---@class REGION
REGION = {
    ---@type number 中国区
    REGION_CN = 0,
}
---操作系统
---@class PHONE_OS
PHONE_OS = {
    ---@type number android
    PHONE_OS_ANDROID = 0,
    ---@type number IOS
    PHONE_OS_IOS = 1,
    ---@type number window等其他
    PHONE_OS_OTHERS = 2,
}
---
---@class SEX
SEX = {
    ---@type number 未知
    UNKNOW = 0,
    ---@type number 男
    MALE = 1,
    ---@type number 女
    FEMAILE = 2,
}
---货币类型
---@class CURRENCY
CURRENCY = {
    ---@type number 人民币
    CURRENCY_RMB = 0,
    ---@type number 金币
    CURRENCY_GOLD = 1,
    ---@type number 钻石
    CURRENCY_DIAMOND = 2,
}
---时装类型
---@class AVATAR_TYPE_TYPE
AVATAR_TYPE_TYPE = {
    ---@type number 0裸模整体
    AVATAR_TYPE_BARE_MOLD = 0,
    ---@type number 1头饰
    AVATAR_TYPE_HEAD = 1,
    ---@type number 2表情
    AVATAR_TYPE_EXPRESSION = 2,
    ---@type number 3面饰
    AVATAR_TYPE_FACE = 3,
    ---@type number 4上衣
    AVATAR_TYPE_COAT = 4,
    ---@type number 5手饰,
    AVATAR_TYPE_HAND = 5,
    ---@type number 6裤子,
    AVATAR_TYPE_PANTS = 6,
    ---@type number 7鞋子,
    AVATAR_TYPE_SHOES = 7,
    ---@type number 8背饰,
    AVATAR_TYPE_BAG = 8,
    ---@type number 9脚印
    AVATAR_TYPE_FOOTPRINT = 9,
}
---
---@class AVATAR_SETTING_TYPE
AVATAR_SETTING_TYPE = {
    ---@type number 保存方案
    AVATAR_SETTING_TYPE_SAVE = 0,
    ---@type number 解锁方案
    AVATAR_SETTING_TYPE_UNLOCK = 1,
}
---gm命令
---@class GM_COMMAND
GM_COMMAND = {
    ---@type number 更新货币
    GM_COMMAND_UPDATE_CURRENCY = 0,
    ---@type number 添加时装
    GM_COMMAND_ADD_AVATAR_ITEM = 1,
    ---@type number 删除时装
    GM_COMMAND_DELETE_AVATAR_ITEM = 2,
    ---@type number 一键添加所有时装
    GM_COMMAND_ADD_ALL_AVATAR_ITEM = 3,
    ---@type number 添加卡牌
    GM_COMMAND_ADD_CARD = 4,
    ---@type number 一键添加所有卡牌
    GM_COMMAND_ADD_ALL_CARD = 5,
    ---@type number 添加通用碎片核心
    GM_COMMAND_ADD_CARD_COMMON_DEBRIS = 6,
    ---@type number 添加卡牌碎片：{Id:7,Args:60100100}
    GM_COMMAND_ADD_CARD_DEBRIS = 7,
}
---avatar方法搭配状态
---@class AVATAR_SETTING_STATUS
AVATAR_SETTING_STATUS = {
    ---@type number 锁住
    AVATAR_SETTING_STATUS_LOCK = 0,
    ---@type number 已经开启
    AVATAR_SETTING_STATUS_OPEN = 1,
}
---卡牌操作状态
---@class CARD_HANDLE
CARD_HANDLE = {
    ---@type number 卡牌装上
    CARD_HANDLE_EQUIP = 0,
    ---@type number 开拍卸载
    CARD_HANDLE_UNEQUIP = 1,
}
---匹配状态
---@class MATCH_STATUS
MATCH_STATUS = {
    ---@type number 未知，即默认值
    MATCH_UNKNOWN = 0,
    ---@type number 正在等待匹配
    MATCH_WAITTING = 1,
    ---@type number 匹配完成
    MATCH_COMPLETED = 2,
    ---@type number 匹配取消
    MATCH_CANCELED = 3,
    ---@type number 匹配超时
    MATCH_TIMEOUT = 4,
}
---1000~1999登录及账号相关错误
---@class STATUS_CODE
STATUS_CODE = {
    ---@type number 通用成功
    OK = 0,
    ---@type number 通用错误
    ERR_COMMON = 1,
    ---@type number 配置表数据不存在
    ERR_CONFIG_DATA_NOT_EXIST = 2,
    ---@type number 通用获取db数据error
    ERR_GET_DB_DATA_ERR = 3,
    ---@type number 通用获取db数据error
    ERR_GET_SAVE_DATA_ERR = 4,
    ---@type number 通用货币不足
    ERR_CURRENCY_NOT_ENOUGH = 5,
    ---@type number 没有登录
    ERR_NO_LOGIN = 1000,
    ---@type number 已经登录无需再次登录
    ERR_ALREALY_LOGIN = 1001,
    ---@type number 登录账号为空
    ERR_LOGIN_ACCOUNT_EMPTY = 1002,
    ---@type number 创建账号失败
    ERR_LOGIN_CREATEACCOUNT_FAIL = 1003,
    ---@type number 无效OPEN_ID
    ERR_INVALID_OPENID = 1004,
    ---@type number 创建角色失败
    ERR_ROLE_CREATE_FAIL = 1005,
    ---@type number 创建角色名字已经存在
    ERR_ROLE_CREATE_NAME_EXIST = 1006,
    ---@type number 玩家角色重名
    ERR_CREATEROLE_REPEATEDNAME = 1007,
    ---@type number 玩家角色已经存在
    ERR_CREATEROLE_EXIST = 1008,
    ---@type number 创建角色名字太长
    ERR_ROLE_NAME_TOO_LONG = 1009,
    ---@type number 创建角色名字太短
    ERR_ROLE_NAME_TOO_SHORT = 1010,
    ---@type number 账号被禁用
    ERR_ACCOUNT_DISABLE = 1011,
    ---@type number 角色消耗货币失败
    ERR_ROLE_COST_CURRENCY_FAIL = 1012,
    ---@type number 匹配失败
    ERR_DO_MATCH = 1101,
    ---@type number 查询结果失败
    ERR_QUERY_MATCH = 1102,
    ---@type number 取消匹配失败
    ERR_CANCEL_MATCH = 1103,
    ---@type number 加入房间失败
    ERR_JOIN_BATTLE = 1104,
    ---@type number avatar:相同装备不用装备
    ERR_AVATAR_CHANGE_SAME_EQUIP = 1201,
    ---@type number avatar:槽位数据为空
    ERR_AVATAR_CHANGE_SLOT_DATA_EMPTY = 1202,
    ---@type number avatar:方案保存失败,未解锁
    ERR_AVATAR_SAVE_SETTING_FAIL_UNLOCK = 1203,
    ---@type number avatar:添加时装，已拥有相同永久
    ERR_AVATAR_HAD_SAME_PERMANENT_ITEM = 1204,
    ---@type number avatar:方案解锁失败:缺少货币ERR_AVATAR_UNLOCK_SETTING_FAIL_LACK_CURRENCY1205;avatar:方案存档失败
    ERR_AVATAR_DB_SAVE_SETTING_FAIL = 1206,
    ---@type number avatar:方案卸载装备失败：没有装备过此装备
    ERR_AVATAR_UNEQUIP_FAIL_SLOT_EMPTY = 1207,
    ---@type number avatar:搭配解锁失败：已经解锁了
    ERR_AVATAR_UNLOCK_SETTING_FAIL_HAD_UNLOCK = 1208,
    ---@type number avatar:换装失败：槽位不匹配
    ERR_AVATAR_UNLOCK_SETTING_FAIL_PART_NOT_MATCH = 1209,
    ---@type number avatar:换装通用失败
    ERR_AVATAR_CHANGE_ERROR = 1230,
    ---@type number avatar:解锁失败,没有找到玩家数据
    ERR_AVATAR_UNLOCK_SETTING_FAIL_PLAYER_DATA_MISS = 1231,
    ---@type number avatar:换装失败：没有此装备
    ERR_AVATAR_CHANGE_FAIL_NOT_ITEM = 1232,
    ---@type number 卡牌系统：物品不存在
    ERR_CARD_UPGRADE_FAIL_CARD_NOT_EXIT = 1300,
    ---@type number 卡牌系统：物品最大等级
    ERR_CARD_UPGRADE_FAIL_MAX_LEVEL = 1301,
    ---@type number 卡牌系统：货币不足ERR_CARD_UPGRADE_FAIL_LACK_CURRENCY1302;卡牌系统：碎片不足
    ERR_CARD_UPGRADE_FAIL_LACK_DEBRIS = 1303,
    ---@type number 卡牌系统：卡槽锁住中
    ERR_CARD_EQUIP_SLOT_LOCK = 1304,
    ---@type number 卡牌系统：卡槽不存在
    ERR_CARD_UPEQUIP_NOT_EXIST = 1305,
    ---@type number 卡牌系统：卡片还没开放
    ERR_CARD_UPGRADE_FAIL_NOT_OPEN = 1306,
    ---@type number 卡牌系统：通用核心兑换数量不足
    ERR_CARD_EXCHANGE_DEBRIS_LACK_ITEM = 1307,
    ---@type number 卡牌系统:添加卡牌,已经拥有无需添加
    ERR_CARD_ADD_FAIL_HAD = 1308,
    ---@type number 卡牌系统:添加卡牌,无需解锁
    ERR_CARD_SLOT_HAD_UNLOCK = 1309,
    ---@type number 卡牌系统:前一个卡槽还处于锁住中
    ERR_CARD_SLOT_PRE_SLOT_LOCK = 1310,
    ---@type number 卡牌系统:卡页锁住中
    ERR_CARD_PAGE_LOCK = 1311,
}
---通知更新对象
---@class NOTICE_UPDATE_OBJ
NOTICE_UPDATE_OBJ = {
    ---@type number 未知，即默认值
    UPDATE_UNKNOWN = 0,
    ---@type number 更新角色
    UPDATE_ROLE = 1,
}
---
---@class Gender
Gender = {
    ---@type number 
    MAN = 0,
    ---@type number 
    WOMAN = 1,
}
MsgIDMap = {
    ---@type string 测试用例
    [99] = "md.GetTest",
    ---@type string 测试用例 服务端返回
    [-99] = "md.GetTestRsp",
    ---@type string 
    [1] = "md.Hello",
    ---@type string  服务端返回
    [-1] = "md.HelloRsp",
    ---@type string 登录系统
    [10000] = "md.Login",
    ---@type string 登录系统 服务端返回
    [-10000] = "md.LoginRsp",
    ---@type string 心跳Heartbeat
    [10001] = "md.Heartbeat",
    ---@type string 心跳Heartbeat 服务端返回
    [-10001] = "md.HeartbeatRsp",
    ---@type string 获取随机角色名字
    [10002] = "md.RandRoleName",
    ---@type string 获取随机角色名字 服务端返回
    [-10002] = "md.RandRoleNameRsp",
    ---@type string 创建角色
    [10003] = "md.CreateRole",
    ---@type string 创建角色 服务端返回
    [-10003] = "md.CreateRoleRsp",
    ---@type string 角色信息
    [10004] = "md.GetRole",
    ---@type string 角色信息 服务端返回
    [-10004] = "md.GetRoleRsp",
    ---@type string gm命令
    [10005] = "md.GM",
    ---@type string gm命令 服务端返回
    [-10005] = "md.GMRsp",
    ---@type string 通知更新某一对象NoticeObjUpdate
    [19000] = "md.NoticeObjUpdate",
    ---@type string 通知更新某一对象NoticeObjUpdate 服务端返回
    [-19000] = "md.NoticeObjUpdateRsp",
    ---@type string 通知用户下线，（被踢下线，或，被挤下线）的通知
    [19999] = "md.NoticeOffLine",
    ---@type string 通知用户下线，（被踢下线，或，被挤下线）的通知 服务端返回
    [-19999] = "md.NoticeOffLineRsp",
    ---@type string 通知战斗结算
    [19001] = "md.NoticeSummary",
    ---@type string 通知战斗结算 服务端返回
    [-19001] = "md.NoticeSummaryRsp",
    ---@type string 开始匹配
    [20001] = "md.DoMatch",
    ---@type string 开始匹配 服务端返回
    [-20001] = "md.DoMatchRsp",
    ---@type string 查询匹配结果
    [20002] = "md.QueryMatch",
    ---@type string 查询匹配结果 服务端返回
    [-20002] = "md.QueryMatchRsp",
    ---@type string 取消匹配
    [20003] = "md.CancelMatch",
    ---@type string 取消匹配 服务端返回
    [-20003] = "md.CancelMatchRsp",
    ---@type string 加入战斗房间
    [20004] = "md.JoinBattle",
    ---@type string 加入战斗房间 服务端返回
    [-20004] = "md.JoinBattleRsp",
    ---@type string avatar:获取数据
    [30000] = "md.GetAvatarSlot",
    ---@type string avatar:获取数据 服务端返回
    [-30000] = "md.GetAvatarSlotRsp",
    ---@type string avatar:选择换装
    [30001] = "md.ChangeAvatar",
    ---@type string avatar:选择换装 服务端返回
    [-30001] = "md.ChangeAvatarRsp",
    ---@type string avatar:历史换装方案
    [30002] = "md.GetAvatarSetting",
    ---@type string avatar:历史换装方案 服务端返回
    [-30002] = "md.GetAvatarSettingRsp",
    ---@type string avatar:保存换装方案
    [30003] = "md.SaveAvatarSetting",
    ---@type string avatar:保存换装方案 服务端返回
    [-30003] = "md.SaveAvatarSettingRsp",
    ---@type string avatar:获取装备数据
    [30005] = "md.GetAvatarItemData",
    ---@type string avatar:获取装备数据 服务端返回
    [-30005] = "md.GetAvatarItemDataRsp",
    ---@type string avatar:解锁搭配方案
    [30006] = "md.UnLockAvatarSetting",
    ---@type string avatar:解锁搭配方案 服务端返回
    [-30006] = "md.UnLockAvatarSettingRsp",
    ---@type string avatar:卸载装备
    [30007] = "md.UnEquipAvatar",
    ---@type string avatar:卸载装备 服务端返回
    [-30007] = "md.UnEquipAvatarRsp",
    ---@type string avatar:切换方案
    [30008] = "md.ChangeSetting",
    ---@type string avatar:切换方案 服务端返回
    [-30008] = "md.ChangeSettingRsp",
    ---@type string 卡牌系统：获取卡槽数据
    [40000] = "md.GetCardPageData",
    ---@type string 卡牌系统：获取卡槽数据 服务端返回
    [-40000] = "md.GetCardPageDataRsp",
    ---@type string 卡牌系统：获取卡牌数据
    [40001] = "md.GetCardData",
    ---@type string 卡牌系统：获取卡牌数据 服务端返回
    [-40001] = "md.GetCardDataRsp",
    ---@type string 卡牌系统:卸载装上卡牌
    [40002] = "md.HandleCard",
    ---@type string 卡牌系统:卸载装上卡牌 服务端返回
    [-40002] = "md.HandleCardRsp",
    ---@type string 卡牌系统:升级卡片
    [40003] = "md.UpgradeCard",
    ---@type string 卡牌系统:升级卡片 服务端返回
    [-40003] = "md.UpgradeCardRsp",
    ---@type string 卡牌系统:兑换卡牌核心
    [40004] = "md.ExchangeCardDebris",
    ---@type string 卡牌系统:兑换卡牌核心 服务端返回
    [-40004] = "md.ExchangeCardDebrisRsp",
    ---@type string 卡牌系统:保存當前頁
    [40005] = "md.ChangeCardPage",
    ---@type string 卡牌系统:保存當前頁 服务端返回
    [-40005] = "md.ChangeCardPageRsp",
    ---@type string 卡牌系统:解锁卡槽
    [40006] = "md.UnlockCardSlot",
    ---@type string 卡牌系统:解锁卡槽 服务端返回
    [-40006] = "md.UnlockCardSlotRsp",
    ---@type string 卡牌系统:获取某张卡牌数据
    [40007] = "md.GetCardDataByID",
    ---@type string 卡牌系统:获取某张卡牌数据 服务端返回
    [-40007] = "md.GetCardDataByIDRsp",
    ---@type string 卡牌系统：获取当前页数据
    [40008] = "md.GetCardCurPageData",
    ---@type string 卡牌系统：获取当前页数据 服务端返回
    [-40008] = "md.GetCardCurPageDataRsp",
    ---@type string 卡牌系统：获取某页数据
    [40009] = "md.GetCardOnePageData",
    ---@type string 卡牌系统：获取某页数据 服务端返回
    [-40009] = "md.GetCardOnePageDataRsp",
    ---@type string 卡牌系统：获取某页数据
    [40010] = "md.GetCardCurPageAllData",
    ---@type string 卡牌系统：获取某页数据 服务端返回
    [-40010] = "md.GetCardCurPageAllDataRsp",
    ---@type string Batle数据
    [50000] = "md.GetBattleDataInfo",
    ---@type string Batle数据 服务端返回
    [-50000] = "md.GetBattleDataInfoRsp",
}

if true then return  end

---@class battle_round_db_pb
battle_round_db_pb = {}
---@class cmd_pb
cmd_pb = {}
---@class cmdpkg_pb
cmdpkg_pb = {}
---@class db_pb
db_pb = {}
---@class enum_pb
enum_pb = {}
---@class game_pb
game_pb = {}
---@class matchbattle_pb
matchbattle_pb = {}
---@class matchbattle_db_pb
matchbattle_db_pb = {}
---@class reward_db_pb
reward_db_pb = {}
---@class statuscode_pb
statuscode_pb = {}
---@class subnotice_pb
subnotice_pb = {}
---@class testcase_pb
testcase_pb = {}
---@class battle_round_db_pb_UserCareerScoreData
battle_round_db_pb_UserCareerScoreData = {
    ---@type string 账号ID
    UID = nil,
    ---@type int32 总(整个职业生涯)共参于（战局）的回合数
    TotalRound = nil,
    ---@type int32 总(整个职业生涯)胜利次数
    TotalVictory = nil,
    ---@type int32 总(整个职业生涯)击杀数
    TotalKillNum = nil,
    ---@type CareerScore 总(整个职业生涯)评分,目前有7个维度
    TotalCareerScore = nil,
    ---@type int64 最近更新时间
    UpdateTime = nil,
}
---战斗玩家状态玩家维度(职业)评分
---@return battle_round_db_pb_UserCareerScoreData
function battle_round_db_pb.UserCareerScore()
end
---@class battle_round_db_pb_CareerScoreData
battle_round_db_pb_CareerScoreData = {
    ---@type float 淘汰评分
    Elimination = nil,
    ---@type float 胜率评分
    Victory = nil,
    ---@type float 生存评分
    Survival = nil,
    ---@type float 探索评分
    Discovery = nil,
    ---@type float 技巧评分
    Skill = nil,
    ---@type float 治疗评分
    Heal = nil,
    ---@type float 救援评分
    Rescue = nil,
}
---维度(职业)评分
---@return battle_round_db_pb_CareerScoreData
function battle_round_db_pb.CareerScore()
end
---@class battle_round_db_pb_BattleRoundLogData
battle_round_db_pb_BattleRoundLogData = {
    ---@type string 局唯一ID
    RoundId = nil,
    ---@type string 匹配码
    MatchCode = nil,
    ---@type string GSE的唯一sessionID
    GameServerSessionId = nil,
    ---@type int32 总玩家数
    PlayerCount = nil,
    ---@type int32 总队伍数
    TeamCount = nil,
    ---@type int64 创建时间(Unix时间戳)
    CreateTime = nil,
    ---@type int64 开局时间(Unix时间戳)
    BeginTime = nil,
    ---@type int64 结束时间(Unix时间戳)
    EndTime = nil,
    ---@type ROUND_STATUS 状态[未开始,已开始,已结束]
    RoundStatus = nil,
    ---@type BattleRoundTeam 队伍
    Teams = nil,
}
---战斗回合
---@return battle_round_db_pb_BattleRoundLogData
function battle_round_db_pb.BattleRoundLog()
end
---@class battle_round_db_pb_BattleRoundTeamData
battle_round_db_pb_BattleRoundTeamData = {
    ---@type string 队伍ID
    TeamId = nil,
    ---@type int32 队伍名次
    Rank = nil,
    ---@type int32 玩家数
    PlayerCount = nil,
    ---@type int32 评级[1冠军,2胜利,3优秀,4再接再厉,5淘汰,6落地成盒]
    Rating = nil,
    ---@type BattleRoundPlayer 玩家
    Players = nil,
}
---战斗队伍记录
---@return battle_round_db_pb_BattleRoundTeamData
function battle_round_db_pb.BattleRoundTeam()
end
---@class battle_round_db_pb_BattleRoundPlayerData
battle_round_db_pb_BattleRoundPlayerData = {
    ---@type string 玩家ID
    PlayerId = nil,
    ---@type string 段位
    Segment = nil,
    ---@type int64 创建时间(Unix时间戳)
    JoinTime = nil,
    ---@type int64 开局时间(Unix时间戳)
    BeginTime = nil,
    ---@type int64 结束时间(Unix时间戳)
    EndTime = nil,
    ---@type int32 击杀人数
    KillNum = nil,
    ---@type int32 伤害值
    HurtVal = nil,
    ---@type int32 治疗值
    CureVal = nil,
    ---@type int32 救援值
    RescueVal = nil,
    ---@type int32 探索分值
    DiscoveryVal = nil,
    ---@type float 评分
    Score = nil,
    ---@type ROUND_PLAYER_STATUS 状态[未进入战斗,已进入战斗,提前退出,完成战斗]
    Status = nil,
    ---@type int32 评级[1冠军,2胜利,3优秀,4再接再厉,5淘汰,6落地成盒]
    Rating = nil,
    ---@type bool 是否MVP
    Mvp = nil,
    ---@type int32 队伍名次
    TeamRank = nil,
    ---@type int32 存活时间(秒数：BeginTime-EndTime)
    Ttl = nil,
    ---@type SummaryReward 结算奖励(金币，经验等结算固定奖励)
    SummaryRewards = nil,
    ---@type CareerScore 维度评分,目前有7个维度
    CareerScore = nil,
    ---@type Reward 奖励(此字段暂时用不到,除金币，经验外的额外奖励)
    Rewards = nil,
}
---战斗玩家记录
---@return battle_round_db_pb_BattleRoundPlayerData
function battle_round_db_pb.BattleRoundPlayer()
end
---@class cmdpkg_pb_PkgHeadData
cmdpkg_pb_PkgHeadData = {
    ---@type uint32 CMD
    Cmd = nil,
    ---@type TRANS_TYPE 0表示request,1表示response
    TransType = nil,
    ---@type uint32 序列号
    Serial = nil,
    ---@type uint32 状态码
    StatusCode = nil,
    ---@type uint32 超时包设置为1用于前端重试
    Timeout = nil,
    ---@type map<string,string> 自定义Options是Map对象
    Options = nil,
}
---
---@return cmdpkg_pb_PkgHeadData
function cmdpkg_pb.PkgHead()
end
---@class cmdpkg_pb_CmdPacketData
cmdpkg_pb_CmdPacketData = {
    ---@type PkgHead 
    Head = nil,
    ---@type bytes 
    Body = nil,
}
---
---@return cmdpkg_pb_CmdPacketData
function cmdpkg_pb.CmdPacket()
end
---@class db_pb_UserInfoData
db_pb_UserInfoData = {
    ---@type string 第三方ID
    OpenID = nil,
    ---@type string 唯一id,展示给用户
    UID = nil,
    ---@type string 玩家名称
    Name = nil,
    ---@type REGION 区域
    Region = nil,
    ---@type bool 是否有创建角色，如果没有则创建
    HasRole = nil,
    ---@type int32 创建时间@inject_tag:response:"-"
    CreateTime = nil,
    ---@type CHANNEL 游戏渠道
    ChannelID = nil,
    ---@type LOGIN_TYPE 登录类型
    LoginType = nil,
    ---@type ACCOUNT_GAIN_TYPE 账号获取类型
    AccountGainType = nil,
    ---@type SDK_TYPE 账号类型：QQ微信等
    SdkType = nil,
    ---@type PHONE_OS 手机操作系统
    Phone_OS = nil,
    ---@type string 游客ID
    GuestId = nil,
    ---@type bool 是否禁用
    IsDisable = nil,
}
---用户信息
---@return db_pb_UserInfoData
function db_pb.UserInfo()
end
---@class db_pb_EquipmentInfoData
db_pb_EquipmentInfoData = {
    ---@type uint32 装备id
    EquipID = nil,
    ---@type uint32 装备等级
    Level = nil,
}
---装备信息
---@return db_pb_EquipmentInfoData
function db_pb.EquipmentInfo()
end
---@class db_pb_RoleInfoData
db_pb_RoleInfoData = {
    ---@type string stringRoleId1;账号ID
    UID = nil,
    ---@type string 角色名字
    RoleName = nil,
    ---@type SEX 性别0:未设置，1：男，2：女
    Sex = nil,
    ---@type int32 创建时间
    CreateTime = nil,
    ---@type uint32 金币数量
    CoinNum = nil,
    ---@type uint32 钻石
    DiamondNum = nil,
    ---@type uint32 当前等级
    Level = nil,
    ---@type int64 当前经验
    CurrentExp = nil,
    ---@type int64 升级所需经验
    UpLvNeedExp = nil,
    ---@type string 头像
    HeadImg = nil,
}
---角名信息
---@return db_pb_RoleInfoData
function db_pb.RoleInfo()
end
---@class db_pb_AvatarItemDataData
db_pb_AvatarItemDataData = {
    ---@type uint32 装备唯一id
    EquipID = nil,
    ---@type int32 装备类型时间:-1：永久装备0：到期时间,避免冲突>0到期时间戳
    Time = nil,
}
---装备数据
---@return db_pb_AvatarItemDataData
function db_pb.AvatarItemData()
end
---@class db_pb_AvatarItemsData
db_pb_AvatarItemsData = {
    ---@type map<uint32,AvatarItemData> 时装列表type:data
    Items = nil,
}
---avatar:所有时装数据
---@return db_pb_AvatarItemsData
function db_pb.AvatarItems()
end
---@class db_pb_AvatarSlotDataData
db_pb_AvatarSlotDataData = {
    ---@type uint32 
    Slot = nil,
    ---@type uint32 
    Id = nil,
}
---avatar:槽数据
---@return db_pb_AvatarSlotDataData
function db_pb.AvatarSlotData()
end
---@class db_pb_AvatarSlotData
db_pb_AvatarSlotData = {
    ---@type map<uint32,AvatarSlotData> 
    SlotData = nil,
}
---avatar:所有槽数据
---@return db_pb_AvatarSlotData
function db_pb.AvatarSlot()
end
---@class db_pb_CurrencyInfoData
db_pb_CurrencyInfoData = {
    ---@type CURRENCY 货币类型
    Currency = nil,
    ---@type uint32 货币数量
    Num = nil,
}
---
---@return db_pb_CurrencyInfoData
function db_pb.CurrencyInfo()
end
---@class db_pb_SettingLockDataData
db_pb_SettingLockDataData = {
    ---@type uint32 状态1开0是锁住
    Status = nil,
    ---@type CurrencyInfo 解锁货币相关
    Info = nil,
}
---搭配配置数据
---@return db_pb_SettingLockDataData
function db_pb.SettingLockData()
end
---@class db_pb_AvatarSettingDataData
db_pb_AvatarSettingDataData = {
    ---@type uint32 方案index
    Index = nil,
    ---@type uint32 方案列表
    EquipID = nil,
    ---@type uint32 状态1开0是锁住
    Status = nil,
    ---@type CurrencyInfo 解锁货币相关
    Info = nil,
}
---
---@return db_pb_AvatarSettingDataData
function db_pb.AvatarSettingData()
end
---@class db_pb_AvatarSettingRspDataData
db_pb_AvatarSettingRspDataData = {
    ---@type uint32 方案index
    Index = nil,
    ---@type string 方案列表repeateduint32EquipID2;
    EquipID = nil,
    ---@type uint32 状态1开0是锁住
    Status = nil,
    ---@type string 解锁货币相关
    Info = nil,
}
---
---@return db_pb_AvatarSettingRspDataData
function db_pb.AvatarSettingRspData()
end
---@class db_pb_AvatarSettingData
db_pb_AvatarSettingData = {
    ---@type AvatarSettingData 
    Setting = nil,
}
---avatar:avatar:avatar:所有搭配数据
---@return db_pb_AvatarSettingData
function db_pb.AvatarSetting()
end
---@class db_pb_CardPageDataData
db_pb_CardPageDataData = {
    ---@type map<uint32,uint32> index:cardid方便查找卡牌数据
    PageData = nil,
}
---卡分页数据
---@return db_pb_CardPageDataData
function db_pb.CardPageData()
end
---@class db_pb_CardDataData
db_pb_CardDataData = {
    ---@type uint32 卡牌唯一id
    Id = nil,
    ---@type uint32 level
    Level = nil,
    ---@type uint32 碎片数量
    DebrisNum = nil,
    ---@type uint32 状态
    Status = nil,
}
---卡牌数据
---@return db_pb_CardDataData
function db_pb.CardData()
end
---@class db_pb_PlayerBaseInfoData
db_pb_PlayerBaseInfoData = {
    ---@type RoleInfo 角色信息
    Player = nil,
    ---@type uint32 avatar时装
    EquipID = nil,
}
---玩家基础信息
---@return db_pb_PlayerBaseInfoData
function db_pb.PlayerBaseInfo()
end
---@class db_pb_ItemLogData
db_pb_ItemLogData = {
    ---@type uint32 物品id
    ItemID = nil,
    ---@type uint32 获取时间
    Time = nil,
}
---道具系统start
---@return db_pb_ItemLogData
function db_pb.ItemLog()
end
---@class db_pb_ItemDataData
db_pb_ItemDataData = {
    ---@type uint32 物品id
    ItemID = nil,
    ---@type uint32 获取时间
    EndTime = nil,
}
---
---@return db_pb_ItemDataData
function db_pb.ItemData()
end
---@class game_pb_EmptyRspData
game_pb_EmptyRspData = {
}
---messageEmptyRsp{
---@return game_pb_EmptyRspData
function game_pb.EmptyRsp()
end
---@class game_pb_HelloData
game_pb_HelloData = {
}
---握手请求
---@return game_pb_HelloData
function game_pb.Hello()
end
---@class game_pb_HelloRspData
game_pb_HelloRspData = {
}
---
---@return game_pb_HelloRspData
function game_pb.HelloRsp()
end
---@class game_pb_LoginData
game_pb_LoginData = {
    ---@type REGION stringVersion1;区域
    Region = nil,
    ---@type string 第三账号ID
    OpenID = nil,
    ---@type string 登录TOKEN
    Token = nil,
    ---@type CHANNEL 游戏渠道
    ChannelID = nil,
    ---@type LOGIN_TYPE 登录类型
    LoginType = nil,
    ---@type ACCOUNT_GAIN_TYPE 账号获取类型
    AccountGainType = nil,
    ---@type string 额外参数：用于一些特殊传递的参数,例如客户端要求服务器获取openid
    ExtraParams = nil,
    ---@type SDK_TYPE 账号来源
    SdkType = nil,
    ---@type PHONE_OS 操作系统
    Phone_Os = nil,
}
---登录请求
---@return game_pb_LoginData
function game_pb.Login()
end
---@class game_pb_LoginRspData
game_pb_LoginRspData = {
    ---@type string 用户唯一ID
    UID = nil,
    ---@type UserInfo 用户信息
    UseInfo = nil,
}
---登录返回
---@return game_pb_LoginRspData
function game_pb.LoginRsp()
end
---@class game_pb_HeartbeatData
game_pb_HeartbeatData = {
    ---@type int32 心跳包数量
    Counter = nil,
    ---@type int32 网络延迟
    TimeDelay = nil,
}
---心跳包
---@return game_pb_HeartbeatData
function game_pb.Heartbeat()
end
---@class game_pb_HeartbeatRspData
game_pb_HeartbeatRspData = {
    ---@type int32 心跳包数量
    Counter = nil,
}
---
---@return game_pb_HeartbeatRspData
function game_pb.HeartbeatRsp()
end
---@class game_pb_RandRoleNameData
game_pb_RandRoleNameData = {
    ---@type SEX 性别0:未设置，1：男，2：女
    Sex = nil,
}
---随机角色名称
---@return game_pb_RandRoleNameData
function game_pb.RandRoleName()
end
---@class game_pb_RandRoleNameRspData
game_pb_RandRoleNameRspData = {
    ---@type string 角色名称
    RoleName = nil,
}
---
---@return game_pb_RandRoleNameRspData
function game_pb.RandRoleNameRsp()
end
---@class game_pb_CreateRoleData
game_pb_CreateRoleData = {
    ---@type string 角色名称
    RoleName = nil,
    ---@type SEX 性别0:未设置，1：男，2：女
    Sex = nil,
    ---@type string 头像
    HeadImg = nil,
}
---创建角色
---@return game_pb_CreateRoleData
function game_pb.CreateRole()
end
---@class game_pb_CreateRoleRspData
game_pb_CreateRoleRspData = {
    ---@type RoleInfo 角色信息
    RoleInfo = nil,
}
---
---@return game_pb_CreateRoleRspData
function game_pb.CreateRoleRsp()
end
---@class game_pb_GetRoleData
game_pb_GetRoleData = {
}
---获取角色信息
---@return game_pb_GetRoleData
function game_pb.GetRole()
end
---@class game_pb_GetRoleRspData
game_pb_GetRoleRspData = {
    ---@type RoleInfo 角色信息
    RoleInfo = nil,
}
---
---@return game_pb_GetRoleRspData
function game_pb.GetRoleRsp()
end
---@class game_pb_GetAvatarSlotData
game_pb_GetAvatarSlotData = {
}
---avatar:获取槽位数据
---@return game_pb_GetAvatarSlotData
function game_pb.GetAvatarSlot()
end
---@class game_pb_GetAvatarSlotRspData
game_pb_GetAvatarSlotRspData = {
    ---@type map<uint32,AvatarSlotData> 
    SlotData = nil,
}
---avatar:获取槽位回调
---@return game_pb_GetAvatarSlotRspData
function game_pb.GetAvatarSlotRsp()
end
---@class game_pb_ChangeAvatarData
game_pb_ChangeAvatarData = {
    ---@type uint32 槽位
    Slot = nil,
    ---@type uint32 UID
    UID = nil,
}
---avatar:换装
---@return game_pb_ChangeAvatarData
function game_pb.ChangeAvatar()
end
---@class game_pb_ChangeAvatarRspData
game_pb_ChangeAvatarRspData = {
    ---@type uint32 槽位
    Slot = nil,
    ---@type uint32 UID
    UID = nil,
}
---avatar:换装回调
---@return game_pb_ChangeAvatarRspData
function game_pb.ChangeAvatarRsp()
end
---@class game_pb_UnEquipAvatarData
game_pb_UnEquipAvatarData = {
    ---@type uint32 卸载槽位装备
    Slot = nil,
}
---avatar:卸载装备
---@return game_pb_UnEquipAvatarData
function game_pb.UnEquipAvatar()
end
---@class game_pb_UnEquipAvatarRspData
game_pb_UnEquipAvatarRspData = {
    ---@type uint32 卸载槽位装备
    Slot = nil,
    ---@type uint32 UID
    UID = nil,
}
---avatar:卸载装备回调
---@return game_pb_UnEquipAvatarRspData
function game_pb.UnEquipAvatarRsp()
end
---@class game_pb_GetAvatarSettingData
game_pb_GetAvatarSettingData = {
}
---
---@return game_pb_GetAvatarSettingData
function game_pb.GetAvatarSetting()
end
---@class game_pb_GetAvatarSettingRspData
game_pb_GetAvatarSettingRspData = {
    ---@type uint32 当前选择
    CurrSetting = nil,
    ---@type AvatarSettingRspData 所有搭配方案
    SettingData = nil,
}
---
---@return game_pb_GetAvatarSettingRspData
function game_pb.GetAvatarSettingRsp()
end
---@class game_pb_GetAvatarItemDataData
game_pb_GetAvatarItemDataData = {
}
---avatar:avatar:avatar:获取装备数据
---@return game_pb_GetAvatarItemDataData
function game_pb.GetAvatarItemData()
end
---@class game_pb_GetAvatarItemDataRspData
game_pb_GetAvatarItemDataRspData = {
    ---@type map<uint32,AvatarItemData> 
    Items = nil,
}
---avatar:获取装备数据回调
---@return game_pb_GetAvatarItemDataRspData
function game_pb.GetAvatarItemDataRsp()
end
---@class game_pb_SaveAvatarSettingData
game_pb_SaveAvatarSettingData = {
}
---avatar:修改搭配方案
---@return game_pb_SaveAvatarSettingData
function game_pb.SaveAvatarSetting()
end
---@class game_pb_SaveAvatarSettingRspData
game_pb_SaveAvatarSettingRspData = {
}
---avatar:修改搭配方案回调
---@return game_pb_SaveAvatarSettingRspData
function game_pb.SaveAvatarSettingRsp()
end
---@class game_pb_UnLockAvatarSettingData
game_pb_UnLockAvatarSettingData = {
    ---@type uint32 方案index
    Index = nil,
    ---@type uint32 货币类型
    CurrencyType = nil,
}
---avatar:解锁方案
---@return game_pb_UnLockAvatarSettingData
function game_pb.UnLockAvatarSetting()
end
---@class game_pb_UnLockAvatarSettingRspData
game_pb_UnLockAvatarSettingRspData = {
    ---@type uint32 方案index
    Index = nil,
    ---@type AvatarSettingRspData 方案数据AvatarSettingDataSettingData2;
    SettingData = nil,
}
---avatar:解锁方案回调
---@return game_pb_UnLockAvatarSettingRspData
function game_pb.UnLockAvatarSettingRsp()
end
---@class game_pb_ChangeSettingData
game_pb_ChangeSettingData = {
    ---@type uint32 方案index
    Index = nil,
}
---avatar:切换搭配方案
---@return game_pb_ChangeSettingData
function game_pb.ChangeSetting()
end
---@class game_pb_ChangeSettingRspData
game_pb_ChangeSettingRspData = {
    ---@type uint32 方案index
    Index = nil,
    ---@type AvatarSettingRspData 方案数据AvatarSettingDataSettingData2;
    SettingData = nil,
}
---avatar:切换搭配方案回调
---@return game_pb_ChangeSettingRspData
function game_pb.ChangeSettingRsp()
end
---@class game_pb_GMData
game_pb_GMData = {
    ---@type uint32 GM_COMMAND枚举
    Id = nil,
    ---@type string 
    Args = nil,
}
---role:gm命令
---@return game_pb_GMData
function game_pb.GM()
end
---@class game_pb_GMRspData
game_pb_GMRspData = {
}
---role:gm命令
---@return game_pb_GMRspData
function game_pb.GMRsp()
end
---@class game_pb_GetBattleDataInfoData
game_pb_GetBattleDataInfoData = {
}
---战斗需求数据
---@return game_pb_GetBattleDataInfoData
function game_pb.GetBattleDataInfo()
end
---@class game_pb_GetBattleDataInfoRspData
game_pb_GetBattleDataInfoRspData = {
    ---@type uint32 
    avatarData = nil,
}
---战斗需求数据回调
---@return game_pb_GetBattleDataInfoRspData
function game_pb.GetBattleDataInfoRsp()
end
---@class game_pb_GetCardPageDataData
game_pb_GetCardPageDataData = {
}
---卡牌系统start
---@return game_pb_GetCardPageDataData
function game_pb.GetCardPageData()
end
---@class game_pb_GetCardPageDataRspData
game_pb_GetCardPageDataRspData = {
    ---@type uint32 分頁
    CurPage = nil,
    ---@type CardPageData 頁數據
    PageData = nil,
}
---卡牌系统：卡牌系统：卡槽数据回调
---@return game_pb_GetCardPageDataRspData
function game_pb.GetCardPageDataRsp()
end
---@class game_pb_GetCardCurPageDataData
game_pb_GetCardCurPageDataData = {
}
---卡牌系统：當前頁數據
---@return game_pb_GetCardCurPageDataData
function game_pb.GetCardCurPageData()
end
---@class game_pb_GetCardCurPageDataRspData
game_pb_GetCardCurPageDataRspData = {
    ---@type uint32 当前页
    CurPage = nil,
    ---@type CardPageData 当前页数据
    PageData = nil,
    ---@type uint32 总页
    TotalPage = nil,
}
---卡牌系统：當前頁數據回調
---@return game_pb_GetCardCurPageDataRspData
function game_pb.GetCardCurPageDataRsp()
end
---@class game_pb_GetCardCurPageAllDataData
game_pb_GetCardCurPageAllDataData = {
}
---卡牌系统：當前頁數據卡牌具体数据
---@return game_pb_GetCardCurPageAllDataData
function game_pb.GetCardCurPageAllData()
end
---@class game_pb_GetCardCurPageAllDataRspData
game_pb_GetCardCurPageAllDataRspData = {
    ---@type uint32 当前页
    CurPage = nil,
    ---@type map<uint32,CardData> 所有卡牌数据
    CardsData = nil,
}
---卡牌系统：當前頁數據回調
---@return game_pb_GetCardCurPageAllDataRspData
function game_pb.GetCardCurPageAllDataRsp()
end
---@class game_pb_GetCardOnePageDataData
game_pb_GetCardOnePageDataData = {
    ---@type uint32 
    Page = nil,
}
---卡牌系统：頁數據
---@return game_pb_GetCardOnePageDataData
function game_pb.GetCardOnePageData()
end
---@class game_pb_GetCardOnePageDataRspData
game_pb_GetCardOnePageDataRspData = {
    ---@type uint32 当前页
    Page = nil,
    ---@type CardPageData 当前页数据
    PageData = nil,
}
---卡牌系统：頁數據回調
---@return game_pb_GetCardOnePageDataRspData
function game_pb.GetCardOnePageDataRsp()
end
---@class game_pb_ChangeCardPageData
game_pb_ChangeCardPageData = {
    ---@type uint32 
    Page = nil,
}
---
---@return game_pb_ChangeCardPageData
function game_pb.ChangeCardPage()
end
---@class game_pb_ChangeCardPageRspData
game_pb_ChangeCardPageRspData = {
    ---@type uint32 分頁
    CurPage = nil,
}
---
---@return game_pb_ChangeCardPageRspData
function game_pb.ChangeCardPageRsp()
end
---@class game_pb_GetCardDataData
game_pb_GetCardDataData = {
}
---卡牌系統:卡牌系統:卡牌系统：卡片数据
---@return game_pb_GetCardDataData
function game_pb.GetCardData()
end
---@class game_pb_GetCardDataRspData
game_pb_GetCardDataRspData = {
    ---@type map<uint32,CardData> 
    PageData = nil,
}
---卡牌系统：卡片数据回调
---@return game_pb_GetCardDataRspData
function game_pb.GetCardDataRsp()
end
---@class game_pb_GetCardDataByIDData
game_pb_GetCardDataByIDData = {
    ---@type uint32 卡牌id
    CardID = nil,
    ---@type uint32 当前选中页
    SelectPage = nil,
}
---卡牌系统：获取某个卡牌
---@return game_pb_GetCardDataByIDData
function game_pb.GetCardDataByID()
end
---@class game_pb_GetCardDataByIDRspData
game_pb_GetCardDataByIDRspData = {
    ---@type uint32 cardID
    CardID = nil,
    ---@type CardData 卡片数据
    CardData = nil,
    ---@type uint32 通用碎片核心
    CommonDebris = nil,
    ---@type uint32 当前选中页
    SelectPage = nil,
    ---@type bool 是否被装备
    isOnSelectPage = nil,
}
---卡牌系统：获取某个卡牌回调
---@return game_pb_GetCardDataByIDRspData
function game_pb.GetCardDataByIDRsp()
end
---@class game_pb_UpgradeCardData
game_pb_UpgradeCardData = {
    ---@type uint32 卡片ID
    CardId = nil,
    ---@type CURRENCY 货币类型
    CurrencyType = nil,
}
---卡牌系统：升级卡片
---@return game_pb_UpgradeCardData
function game_pb.UpgradeCard()
end
---@class game_pb_UpgradeCardRspData
game_pb_UpgradeCardRspData = {
    ---@type uint32 卡片ID
    CardId = nil,
    ---@type CURRENCY 货币类型
    CurrencyType = nil,
    ---@type uint32 当前等级
    Level = nil,
    ---@type uint32 碎片数量
    DebrisNum = nil,
}
---卡牌系统：升级卡片回调
---@return game_pb_UpgradeCardRspData
function game_pb.UpgradeCardRsp()
end
---@class game_pb_ExchangeCardDebrisData
game_pb_ExchangeCardDebrisData = {
    ---@type uint32 卡片ID
    CardId = nil,
    ---@type uint32 兑换数量
    num = nil,
}
---卡牌系统：兑换碎片
---@return game_pb_ExchangeCardDebrisData
function game_pb.ExchangeCardDebris()
end
---@class game_pb_ExchangeCardDebrisRspData
game_pb_ExchangeCardDebrisRspData = {
    ---@type uint32 卡片ID
    CardId = nil,
    ---@type uint32 卡片碎片核心数量
    num = nil,
    ---@type uint32 通用碎片核心
    CommonDebris = nil,
}
---卡牌系统：兑换碎片回调
---@return game_pb_ExchangeCardDebrisRspData
function game_pb.ExchangeCardDebrisRsp()
end
---@class game_pb_HandleCardData
game_pb_HandleCardData = {
    ---@type uint32 
    Page = nil,
    ---@type uint32 
    Index = nil,
    ---@type uint32 
    CardID = nil,
    ---@type CARD_HANDLE 
    Handle = nil,
}
---卡牌系统：卸载/安装上卡牌
---@return game_pb_HandleCardData
function game_pb.HandleCard()
end
---@class game_pb_HandleCardRspData
game_pb_HandleCardRspData = {
    ---@type uint32 当前页
    Page = nil,
    ---@type uint32 卡槽
    Index = nil,
    ---@type uint32 卡片id
    CardID = nil,
    ---@type CARD_HANDLE 卸载/安装
    Handle = nil,
}
---卡牌系统：卸载/安装上卡牌回调
---@return game_pb_HandleCardRspData
function game_pb.HandleCardRsp()
end
---@class game_pb_UnlockCardSlotData
game_pb_UnlockCardSlotData = {
    ---@type uint32 当前页
    Page = nil,
    ---@type uint32 卡槽
    Index = nil,
    ---@type CURRENCY 货币类型
    CurrencyType = nil,
}
---卡牌系统：解锁卡槽
---@return game_pb_UnlockCardSlotData
function game_pb.UnlockCardSlot()
end
---@class game_pb_UnlockCardSlotRspData
game_pb_UnlockCardSlotRspData = {
    ---@type uint32 当前页
    Page = nil,
    ---@type uint32 卡槽
    Index = nil,
    ---@type CURRENCY 货币类型
    CurrencyType = nil,
}
---卡牌系统：解锁卡槽回调
---@return game_pb_UnlockCardSlotRspData
function game_pb.UnlockCardSlotRsp()
end
---@class matchbattle_pb_DoMatchData
matchbattle_pb_DoMatchData = {
    ---@type uint32 匹配玩家人数(此字段暂时不用)
    MatchNum = nil,
    ---@type string 匹配码(需前后端约定生成规则)
    MatchCode = nil,
    ---@type bool 是否自动匹配
    MatchTeamAuto = nil,
}
---开始匹配
---@return matchbattle_pb_DoMatchData
function matchbattle_pb.DoMatch()
end
---@class matchbattle_pb_DoMatchRspData
matchbattle_pb_DoMatchRspData = {
    ---@type string 匹配请求的唯一ID
    MatchRequestId = nil,
}
---
---@return matchbattle_pb_DoMatchRspData
function matchbattle_pb.DoMatchRsp()
end
---@class matchbattle_pb_QueryMatchData
matchbattle_pb_QueryMatchData = {
    ---@type string 匹配请求的唯一ID
    MatchRequestId = nil,
    ---@type uint32 房间最大玩家数量
    RoomMaxPlayerNum = nil,
}
---查询匹配结果
---@return matchbattle_pb_QueryMatchData
function matchbattle_pb.QueryMatch()
end
---@class matchbattle_pb_QueryMatchRspData
matchbattle_pb_QueryMatchRspData = {
    ---@type string 匹配请求的唯一ID
    MatchRequestId = nil,
    ---@type string 已经匹配到的玩家ID
    MatchedPlayerIds = nil,
    ---@type MATCH_STATUS 状态
    Status = nil,
    ---@type string 游戏服务器会话ID
    GameServerSessionId = nil,
}
---
---@return matchbattle_pb_QueryMatchRspData
function matchbattle_pb.QueryMatchRsp()
end
---@class matchbattle_pb_CancelMatchData
matchbattle_pb_CancelMatchData = {
    ---@type string 匹配请求的唯一ID
    MatchRequestId = nil,
}
---取消匹配
---@return matchbattle_pb_CancelMatchData
function matchbattle_pb.CancelMatch()
end
---@class matchbattle_pb_CancelMatchRspData
matchbattle_pb_CancelMatchRspData = {
    ---@type string 
    MatchRequestId = nil,
}
---
---@return matchbattle_pb_CancelMatchRspData
function matchbattle_pb.CancelMatchRsp()
end
---@class matchbattle_pb_JoinBattleData
matchbattle_pb_JoinBattleData = {
    ---@type string 游戏服务器会话ID
    GameServerSessionId = nil,
}
---用于加入游戏服务器会话
---@return matchbattle_pb_JoinBattleData
function matchbattle_pb.JoinBattle()
end
---@class matchbattle_pb_JoinBattleRspData
matchbattle_pb_JoinBattleRspData = {
    ---@type string 游戏服务器会话ID
    GameServerSessionId = nil,
    ---@type string 玩家ID
    PlayerId = nil,
    ---@type string 玩家会话ID
    PlayerSessionId = nil,
    ---@type string 玩家会话的状态
    Status = nil,
    ---@type string 游戏服务器会话运行的DNS标识
    DnsName = nil,
    ---@type string 游戏服务器会话运行的CVM地址
    IpAddress = nil,
    ---@type uint64 端口号
    Port = nil,
    ---@type string 战斗局ID
    RoundId = nil,
    ---@type string 组队ID
    TeamId = nil,
}
---
---@return matchbattle_pb_JoinBattleRspData
function matchbattle_pb.JoinBattleRsp()
end
---@class reward_db_pb_RewardData
reward_db_pb_RewardData = {
    ---@type int32 对应道具表的ID
    PropsId = nil,
    ---@type int32 数量
    Quantity = nil,
    ---@type int32 有效时间
    Ttl = nil,
    ---@type int32 获得时间
    GetTime = nil,
    ---@type int32 过期时间
    Expired = nil,
    ---@type int32 展示优先级
    ShowPriority = nil,
}
---奖励
---@return reward_db_pb_RewardData
function reward_db_pb.Reward()
end
---@class reward_db_pb_SummaryRewardData
reward_db_pb_SummaryRewardData = {
    ---@type int32 对应道具表的ID
    PropsId = nil,
    ---@type int32 数量
    Quantity = nil,
}
---结算奖励(金币，经验等结算固定奖励)
---@return reward_db_pb_SummaryRewardData
function reward_db_pb.SummaryReward()
end
---@class subnotice_pb_NoticeData
subnotice_pb_NoticeData = {
    ---@type GAME_CMD CMD
    Cmd = nil,
    ---@type string 发送人
    FromUID = nil,
    ---@type string 接收人,如果空表示所有人
    ToUID = nil,
    ---@type bytes 内容
    Body = nil,
}
---通知
---@return subnotice_pb_NoticeData
function subnotice_pb.Notice()
end
---@class subnotice_pb_NoticeObjUpdateData
subnotice_pb_NoticeObjUpdateData = {
    ---@type NOTICE_UPDATE_OBJ 需要更新的对象
    UpdateObj = nil,
    ---@type string 用户ID
    UID = nil,
    ---@type string 字段
    Fileds = nil,
}
---通知更新
---@return subnotice_pb_NoticeObjUpdateData
function subnotice_pb.NoticeObjUpdate()
end
---@class subnotice_pb_NoticeOffLineData
subnotice_pb_NoticeOffLineData = {
    ---@type string 通知用户下线GAME_CMD_NOTICEOFFLINE下线消息内容
    Message = nil,
}
---
---@return subnotice_pb_NoticeOffLineData
function subnotice_pb.NoticeOffLine()
end
---@class subnotice_pb_NoticeUpLevelData
subnotice_pb_NoticeUpLevelData = {
    ---@type Reward 通知升级奖励奖励
    Rewards = nil,
}
---
---@return subnotice_pb_NoticeUpLevelData
function subnotice_pb.NoticeUpLevel()
end
---@class subnotice_pb_NoticeSummaryData
subnotice_pb_NoticeSummaryData = {
    ---@type string 通知结算局唯一ID
    RoundId = nil,
    ---@type int32 战局玩家数
    RoundPlayerCount = nil,
    ---@type BattleRoundTeam 战斗结算
    BattleSummary = nil,
    ---@type PlayerBaseInfo 玩家角色信息
    PlayerInfo = nil,
}
---
---@return subnotice_pb_NoticeSummaryData
function subnotice_pb.NoticeSummary()
end
---@class testcase_pb_GetTestData
testcase_pb_GetTestData = {
}
---
---@return testcase_pb_GetTestData
function testcase_pb.GetTest()
end
---@class testcase_pb_ResultData
testcase_pb_ResultData = {
    ---@type int64 int64数字类型
    id = nil,
    ---@type string 数组嵌套数组repeatedAvatarSettingDataSettingData2;列表
    Sons = nil,
    ---@type Gender Enum值
    Gender = nil,
    ---@type string 新的对象List
    Name = nil,
    ---@type uint64 Any对象
    Age = nil,
    ---@type map<string,AvatarSettingData> 定义Map对象
    StrMap = nil,
    ---@type string 嵌套
    url = nil,
    ---@type string 
    title = nil,
    ---@type string 
    snippets = nil,
}
---
---@return testcase_pb_ResultData
function testcase_pb.Result()
end
----MD5----
--nitySynDemo/batDemo/Tools/GameProto/main/battle_round_db.proto|3874dc5bc8c8fde21dcc7acd73f7812a
--nitySynDemo/batDemo/Tools/GameProto/main/cmd.proto|a39e059e78b034a6d10ae085a8e794f0
--nitySynDemo/batDemo/Tools/GameProto/main/cmdpkg.proto|de759a73a800a22b54243086fe5f52fd
--nitySynDemo/batDemo/Tools/GameProto/main/db.proto|5e0ae6cc5cf2cf7b6ace360d975b7c47
--nitySynDemo/batDemo/Tools/GameProto/main/enum.proto|0d0004cbd4440b4a95461edfeb5e92a1
--nitySynDemo/batDemo/Tools/GameProto/main/game.proto|3241bf4a8b11326259fe71e2eee7facb
--nitySynDemo/batDemo/Tools/GameProto/main/matchbattle.proto|cf50070d0921870819567a223e02d01b
--nitySynDemo/batDemo/Tools/GameProto/main/matchbattle_db.proto|3d030f3b271a45911e07ba2180ff8c6f
--nitySynDemo/batDemo/Tools/GameProto/main/reward_db.proto|4fb175eae117105a4645c45630e4ae27
--nitySynDemo/batDemo/Tools/GameProto/main/statuscode.proto|41f914cd7e3c7c9eeef2a7e3d6baa1ad
--nitySynDemo/batDemo/Tools/GameProto/main/subnotice.proto|d28266ab9ca2f3449861ec8a2e3bb493
--nitySynDemo/batDemo/Tools/GameProto/main/testcase.proto|5eb575327b5fd16b574c780f292f87a6
