using GameEnum;
using UnityEngine;
/****
武器基类
****/
public class Weapon : ObjBase
{
    public WeaponGunData weaponData=null;
    public Weapon()
    {
         charType=GameEnum.ObjType.Weapon;
    }

    //重写Data.
    public override void initData(){
        WeaponGunData oldData=this.dataNode.GetComponent<WeaponGunData>();
        if(oldData!=null){
            GameObject.DestroyImmediate(oldData);
        }
        this.weaponData = this.dataNode.AddComponent<WeaponGunData>();
        this.objData=this.weaponData;
        this.weaponData.init(this,fixUpdate);
    }
    public override void onViewLoadFin(){
        this.weaponData.initGunData();
    }

    public override void onGet(){
        base.onGet();
     }
    //回收.
    public override void onRecycle(){
        base.onRecycle();
     }
    public override void onRelease(){
        this.weaponData=null;
        base.onRelease();
    }
}