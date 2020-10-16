using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// tag枚举，需要和tag设置保持一致
/// </summary>
public class ETag
{
    ////////////////////unity自带////////////////////////
    public const string Untagged = "Untagged";
    public const string Respawn = "Respawn";
    public const string Finish = "Finish";
    public const string EditorOnly = "EditorOnly";
    public const string MainCamera = "MainCamera";
    public const string Player = "Player";
    public const string GameController = "GameController";
    ////////////////////自定义(注释要说明用处)////////////////////////
    /// <summary>
    /// 身体碰撞体
    /// </summary>
    public const string Body = "Body";
    /// <summary>
    /// 头部碰撞体
    /// </summary>
    public const string Head = "Head";
    /// <summary>
    /// 四肢碰撞体
    /// </summary>
    public const string Limb = "Limb";
    /// <summary>
    /// 场景流加载用（似乎已经弃用）
    /// </summary>
    public const string SceneStreamer = "SceneStreamer";
    /// <summary>
    /// UI相机
    /// </summary>
    public const string UICamera = "UICamera";
    /// <summary>
    /// 天空岛
    /// </summary>
    public const string SceneBorn = "SceneBorn";
    /// <summary>
    /// 脖子碰撞体
    /// </summary>
    public const string Neck = "Neck";
    /// <summary>
    /// 盾牌技能
    /// </summary>
    public const string Shield = "Shield";
    /// <summary>
    /// 第一人称相机
    /// </summary>
    public const string FpCamera = "FpCamera";
    /// <summary>
    /// 第一人称控制器
    /// </summary>
    public const string FPSController = "FPSController";

}

/// <summary>
/// tag工具类
/// </summary>
public class TagHelper
{
    /// <summary>
    /// 比较目标tag
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static bool CompareTag(GameObject target,string tag)
    {
        return target.CompareTag(tag);
    }
}
