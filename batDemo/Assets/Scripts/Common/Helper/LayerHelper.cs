using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 层级缓存，需要和setting中保持一致
/// </summary>
public enum ELayer
{
    ////////////////////unity自带////////////////////////
    Default = 0,
    TransparentFX = 1,
    IgonreRaycast = 2,
    Water = 4,
    UI = 5,
    ////////////////////自定义(注释要说明用处)////////////////////////

    Cover =8,
    //射击无视层 钢丝网.
    IgnoreShot=9,
    CoverInvisible=10,
    Player=11,
    Enemy=12,
    Bound=13,
    Trigger=14,
    /// <summary>
    /// 第三人称
    /// </summary>
    FPS = 15,
    /// <summary>
    /// 可被射线检测攻击层级
    /// </summary>
    Damageable = 16,
    /// <summary>
    /// 倍镜层级
    /// </summary>
    Scope = 17,
    /// <summary>
    /// 毒圈
    /// </summary>
    PoisonCircle = 18,
    /// <summary>
    /// 道具
    /// </summary>
    Item = 19,
     /// <summary>
    /// 草 云
    /// </summary>
    Grass=20,
      /// <summary>
    /// 摄像机后处理
    /// </summary>
    PostProcessing=21
}

/// <summary>
/// layer层级工具类
/// </summary>
[AutoRegistLua]
public class LayerHelper
{
    /// <summary>
    /// 比较目标层级
    /// </summary>
    /// <returns></returns>
    public static bool CompareLayer(GameObject target, ELayer layer)
    {
        return target.layer == (int)layer;
    }

    /// <summary>
    /// 将节点的子物体设置成UI层级
    /// </summary>
    /// <param name="go"></param>
    public static void ChangeObjLayerToUI(GameObject go)
    {
        SetObjRenderLayer(go, ELayer.UI);
    }

    /// <summary>
    /// 设置所有渲染节点的层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void SetObjRenderLayer(GameObject go, ELayer layer)
    {
        if (go == null)
        {
            return;
        }
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].gameObject.layer = (int)layer;
        }
    }

    /// <summary>
    /// 设置所有子节点的层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="layer"></param>
    public static void SetObjLayer(GameObject go, ELayer layer)
    {
        if (go == null)
        {
            return;
        }
        Transform[] trs = go.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trs.Length; i++)
        {
            trs[i].gameObject.layer = (int)layer;
        }
    }

    /// <summary>
    /// 获取射线层级Mask，配合射线用
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static int GetLayerMask(params ELayer[] layer)
    {
        if (layer == null) return 0;

        int mask = 0;
        for (int i = 0; i < layer.Length; ++i)
        {
            mask |= LayerMask.GetMask(LayerMask.LayerToName((int)layer[i]));
        }
        return mask;
    }

    public static int GetEverythingMask()
    {
        return -1;
    }

    public static int GetNothingMask()
    {
        return 0;
    }

    /// <summary>
    /// 跳伞地面检测层级
    /// </summary>
    /// <returns></returns>
    public static int GetParachutingLandMask()
    {
        return GetLayerMask(ELayer.Default, ELayer.Water);
    }

      private static int _HitLayerMask=-1;
    /// <summary>
    /// 获取可被击中的层级Mask
    /// </summary>
    /// <returns></returns>
    public static int GetHitLayerMask()
    {
         if(_HitLayerMask!=-1){
           return _HitLayerMask;
       } 
        _HitLayerMask=GetLayerMask(ELayer.Damageable, ELayer.Bound, ELayer.Water);
        return _HitLayerMask;
    }

    /// <summary>
    /// 获取环境层级（地面、建筑等不受伤害，但能遮挡攻击的物体 --- 用于判断是否被遮挡）
    /// </summary>
    /// <returns></returns>
    public static int GetEnviromentLayerMask()
    {
        return GetLayerMask(ELayer.Default);
    }
    private static int _GroundLayerMask=-1;
    /// <summary>
    /// 获取地面层
    /// </summary>
    /// <returns></returns>
    public static int GetGroundLayerMask()
    {
       if(_GroundLayerMask!=-1){
           return _GroundLayerMask;
       }
       _GroundLayerMask=GetLayerMask(ELayer.Bound,ELayer.IgnoreShot,ELayer.Water);
       return _GroundLayerMask;
    }
    private static int _CameraHitLayerMask=-1;
   /// <summary>
    /// 获取地面层
    /// </summary>
    /// <returns></returns>
    public static int GetCameraHitLayerMask()
    {
       if(_CameraHitLayerMask!=-1){
           return _CameraHitLayerMask;
       }
       _CameraHitLayerMask=GetLayerMask(ELayer.Bound,ELayer.IgnoreShot,ELayer.Player,ELayer.Enemy,ELayer.Water,ELayer.Grass);
       return _CameraHitLayerMask;
    }
}
