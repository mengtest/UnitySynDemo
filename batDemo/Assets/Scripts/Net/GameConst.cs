using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    //设计分辨率
    public static readonly Vector2 ReferenceResolution = new Vector2(640, 1140);
    // Socket服务器端口
    // 9800         外网内网预发布环境
	public const int SocketPort = 9800;
    // Socket服务器地址
    // planettest.boomegg.cn                    外网环境
    // "192.168.0.53"                           内网环境
    // "192.168.0.126"                          内网环境
    // "piggytravel-beta.aladinfun.com"         预发布环境
	public const string SocketAddress = "planettest.boomegg.cn";
    // http url
    // "http://planettest.boomegg.cn/testenv/";     外网环境
    // "http://pt-dev.boomegg.net/testenv/";        内网环境
    // "http://piggytravel-beta.aladinfun.com/";    预发布环境
    public const string HttpUri = "http://planettest.boomegg.cn/testenv/";
    // 数据上报url
    public const string ReportHttpUrl = "https://PiggyTravel-dev.boomegg.cn";
}
