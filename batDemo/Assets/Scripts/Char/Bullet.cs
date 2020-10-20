using GameEnum;
using UnityEngine;
/****
子弹
****/
[AutoRegistLua]
public class Bullet : ObjBase
{
    private Player _owner;
    private Gun _gun;
    //最大射程.
    private float _maxDic;
    //已经飞行距离.
    private float _moveDic;

    protected Ray fireRay;
    protected RaycastHit[] fireRayHits = new RaycastHit[10];

    private Vector3 targetPoint;
    
    public Bullet()
    {
         charType=GameEnum.ObjType.Bullet;
    }
     public override void init(){
        base.init();
        GetMovePart().rotateSpeed=0;
         GetMovePart().faceToRotation=false;
    }
    public void setOwner(Gun gun){
         if(gun!=null){
            _owner=gun.getOwner();
            _gun=gun;
            this.setMaxDic(_gun.getGunData().DamageRange);
            this.gameObject.transform.position=this._gun.getGunData().muzzleTrans.position;
            this.moveSpeed=_gun.getGunData().BulletSpeed;
         }
    }
    public void setMaxDic(float maxDic){
         if(maxDic>0){
             _maxDic=maxDic;
         }
    }
    void SortFireRayHits(RaycastHit[] array, int count)
    {
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                if (array[i].distance > array[j].distance)
                {
                    //交换
                    RaycastHit temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }
    public void moveToFirePoint(){
        //通过firePoint 和 射程 打射线 计算出最终目标点.
         // _owner.cameraCtrl.FirePoint;
         Vector3 dir= _owner.cameraCtrl.FirePoint.transform.forward;
         fireRay.origin = _owner.cameraCtrl.FirePoint.transform.position;
         fireRay.direction = dir;
        int hitCount = Physics.RaycastNonAlloc(fireRay, fireRayHits, _maxDic, LayerHelper.GetHitLayerMask());
        if (hitCount > 0)
        {
            if(hitCount>1){
                SortFireRayHits(fireRayHits, hitCount);
            }
           //Todo记录 owner 子弹命中.
            GameObject hitterObj;
            RaycastHit ray;
            Vector3 hitPoint=Vector3.zero;
            // Target was hit.
            for (int i = 0; i < hitCount; i++)
            {
                ray=fireRayHits[i];
                hitterObj=ray.collider.gameObject;
                ELayer layer =(ELayer)hitterObj.layer;
                switch(layer){
                    case ELayer.Damageable:
                        if (TagHelper.CompareTag(hitterObj, ETag.Shield))
                        {   
                            //击中护盾;
                            hitPoint = ray.point;
                        }else{
                            Character hitter=  ObjManager.Instance.GetCharacterByBody(hitterObj);
                            if(hitter!=null){
                                if(hitter==_owner){
                                    continue;
                                }
                                if(hitter.charData.camp==_owner.charData.camp){
                                    continue;
                                }
                                hitPoint = ray.point;
                            }
                        }
                    break;
                    case ELayer.Bound:
                       hitPoint = ray.point;
                    break;
                    case ELayer.Water:
                       hitPoint = ray.point;
                    break;
                }
                if(hitPoint!=Vector3.zero){
                    break;
                }
            }
            if(hitPoint!=Vector3.zero){
                targetPoint=hitPoint;
            }else{
                 targetPoint=_owner.cameraCtrl.FirePoint.transform.position+dir*_maxDic;  
            }
        }else{
             targetPoint=_owner.cameraCtrl.FirePoint.transform.position+dir*_maxDic;  
        }
        dir=(targetPoint-this.gameObject.transform.position).normalized;
    //    DebugLog.Log("targetPoint",targetPoint,dir,this.gameObject.transform.position);
        this.gameObject.transform.forward=dir;
        this.GetMovePart().StartMove(dir);
       // this.GetMovePart().
    }
    //移动专用方法.
    public override void OnMove(Vector3 dic){
        //每帧打个射线.
        //check  然后再移动. //我和 队友跳过
        float moveDic =dic.magnitude;
    //    DebugLog.Log("bulletDic",moveDic);
        fireRay.origin = this.node.transform.position;
        fireRay.direction =  this.node.transform.forward;
        int hitCount = Physics.RaycastNonAlloc(fireRay, fireRayHits, moveDic, LayerHelper.GetHitLayerMask());

        if (hitCount > 0)
        {
            if(hitCount>1){
                SortFireRayHits(fireRayHits, hitCount);
            }
           //Todo记录 owner 子弹命中.
            GameObject hitterObj;
            RaycastHit ray;
            // Target was hit.
            for (int i = 0; i < hitCount; i++)
            {
                ray=fireRayHits[i];
                hitterObj=ray.collider.gameObject;
                ELayer layer =(ELayer)hitterObj.layer;
                switch(layer){
                    case ELayer.Damageable:
                        if (TagHelper.CompareTag(hitterObj, ETag.Shield))
                        {   
                            //击中护盾;
                            onHitDecalEffect(hitterObj,ray);
                            if(onHitShield(hitterObj)){
                                   return;
                            }
                        }else{
                            Character hitter=  ObjManager.Instance.GetCharacterByBody(hitterObj);
                            if(hitter!=null){
                                if(hitter==_owner){
                                    continue;
                                }
                                if(hitter.charData.camp==_owner.charData.camp){
                                    continue;
                                }
                                onHitDecalEffect(hitterObj,ray);
                                //击中玩家.
                                hitter.CalculateGunDamage(_gun.getGunData(),hitterObj.tag,_owner);
                                if(onHitChar(hitterObj)){
                                   return;
                                }
                            }
                        }
                    break;
                    case ELayer.Bound:
                        onHitDecalEffect(hitterObj,ray);
                        if(onHitBound(hitterObj)){
                            return;
                        }
                    break;
                    case ELayer.Water:
                        onHitDecalEffect(hitterObj,ray);
                        if(onHitWater(hitterObj)){
                           return;
                        }
                    break;
                }
            }
        }
       
        this.node.transform.position =  this.node.transform.position + dic;
        _moveDic+=moveDic;
        if(_maxDic>0&&_moveDic>=_maxDic){
            //最大移动距离 销毁.
            this.recycleSelf();
            return;
        }
    }
    private bool onHitWater(GameObject gameObject){
        //普通子弹
        this.recycleSelf();
        return true;
    }
    private bool onHitBound(GameObject gameObject){
        //普通子弹
    //    DebugLog.Log("回收");
        this.recycleSelf();
        return true;
    }
     private bool onHitChar(GameObject gameObject){
        //普通子弹
        this.recycleSelf();
        return true;
    }
     private bool onHitShield(GameObject gameObject){
        //普通子弹
        this.recycleSelf();
        return true;
    }
    
    //贴花 特效
    private void onHitDecalEffect(GameObject gameObject,RaycastHit hitInfo){
        PhysicMaterial physicMaterial = hitInfo.collider.sharedMaterial;
        EffectBase effect=null;
        if (physicMaterial != null){
            DebugLog.Log(physicMaterial.name);
            switch(physicMaterial.name){
                case HitMatType.Meat:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_flesh",null,0,7f);
                    //如果需要音效.
                break; 
                case HitMatType.Metal:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_metal",null,0,7f);
                    //如果需要音效.
                break; 
                case HitMatType.Sand:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_sand",null,0,1f);
                    //如果需要音效.
                break; 
                case HitMatType.Snow:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_snow",null,0,1f);
                    //如果需要音效.
                break; 
                case HitMatType.Stone:
                case HitMatType.Prototype:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_stone",null,0,6f);
                    //如果需要音效.
                break; 
                case HitMatType.Wood:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_wood",null,0,7f);
                    //如果需要音效.
                break; 
                case HitMatType.Grass:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_grass",null,0,1.2f);
                    //如果需要音效.
                break; 
                default:
                    effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_stone",null,0,6f);
                break;
            }
        }else{
            effect= ObjManager.Instance.CreatEffect("Effect/shoot/decal/fx_decal_stone",null,0,6f);
        }
        if(_gun.getGunData().dontClearDecals){
             effect.autoDestroy(600f);
        }
        effect.gameObject.transform.parent=hitInfo.transform;
        effect.gameObject.transform.position=hitInfo.point;
        effect.gameObject.transform.rotation=Quaternion.LookRotation(hitInfo.normal);
    }

    protected override void Update(){
            // if(_owner!=null){
            //     Debug.DrawRay(_owner.cameraCtrl.FirePoint.transform.position, _owner.cameraCtrl.FirePoint.transform.forward*_maxDic,  Color.red );
            //     Debug.DrawRay(this.gameObject.transform.position, this.gameObject.transform.forward*_maxDic,  Color.green );
            //     DebugLog.Log(this.isDead,this.isRecycled,this.GetMovePart().IsMove());
            // }
        base.Update();
    }
     public override void onGet(){
        _moveDic=0;
        _maxDic=500;
        base.onGet();
    }
    public override void onRecycle(){
         _owner=null;
         _gun=null;
        base.onRecycle();
    }
    public override void onRelease(){
        _owner=null;
        _gun=null;
        base.onRelease();
    }
}