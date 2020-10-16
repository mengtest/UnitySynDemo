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
    
    public Bullet()
    {
         charType=GameEnum.ObjType.Bullet;
    }
     public override void init(){
        base.init();
        GetMovePart().rotateSpeed=0;
    }
    public void setOwner(Gun gun){
         if(gun!=null){
            _owner=gun.getOwner();
            _gun=gun;
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
    //移动专用方法.
    public override void OnMove(Vector3 dic){
        //每帧打个射线.
        //check  然后再移动. //我和 队友跳过
        float moveDic =dic.magnitude;
        DebugLog.Log("bulletDic",moveDic);
        fireRay.origin = this.node.transform.position;
        fireRay.direction = dic.normalized;
        int hitCount = Physics.RaycastNonAlloc(fireRay, fireRayHits, moveDic, LayerHelper.GetHitLayerMask());

        if (hitCount > 1)
        {
           //Todo记录 owner 子弹命中.
            SortFireRayHits(fireRayHits, hitCount);
        }
        GameObject hitterObj;
        // Target was hit.
        for (int i = 0; i < hitCount; i++)
        {
            hitterObj=fireRayHits[i].collider.gameObject;
            ELayer layer =(ELayer)hitterObj.layer;
            switch(layer){
                case ELayer.Damageable:
                   if (TagHelper.CompareTag(hitterObj, ETag.Shield))
                   {   
                      //击中护盾;
                   }else{
                       Character hitter=  ObjManager.Instance.GetCharacterByBody(hitterObj);
                        if(hitter!=null){
                            if(hitter==_owner){
                                continue;
                            }
                            if(hitter.charData.camp==_owner.charData.camp){
                                continue;
                            }
                            //击中玩家.
                            hitter.CalculateGunDamage(_gun.getGunData(),hitterObj.tag,_owner);
                            //普通子弹
                            this.recycleSelf();
                            return;
                        }
                   }
                break;
                case ELayer.Bound:
                   if(onHitOther(hitterObj)){
                       return;
                   }
                break;
                case ELayer.Water:
                    if(onHitWater(hitterObj)){
                      return;
                    }
                break;
            }
        }
       
        
        this.node.transform.position =  this.node.transform.position + dic;
        _moveDic+=moveDic;
        if(_maxDic>0&&_moveDic>=_maxDic){
            this.recycleSelf();
            return;
        }
    }
    private bool onHitWater(GameObject gameObject){
        //普通子弹
        this.recycleSelf();
        return true;
    }
    private bool onHitOther(GameObject gameObject){
        //普通子弹
        this.recycleSelf();
          return true;
    }
    protected override void Update(){
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