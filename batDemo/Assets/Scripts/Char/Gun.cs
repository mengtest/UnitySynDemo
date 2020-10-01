using GameEnum;
using UnityEngine;
/****
枪
****/
[AutoRegistLua]
public class Gun : Weapon
{
    protected bool onFire=false;
    private  Weapon_Gun gunD;
    private bool isShooting;
    public Gun()
    {
         charType=GameEnum.ObjType.Gun;
    }

    //重写Data.
    public override void initData(){
      base.initData();
      this.gunD=this.itemData.getGunData();
    }
   
    //安装手上
    public override void EquipWeaponRightHand(Player player){
        itemData.OnPickUp();
        itemData.ItemOnHand=true;
        ownerPlayer = player;
    //    this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
        gameObject.transform.SetParent(player.weaponSystem.rightHand);
        gameObject.transform.localPosition =gunD.rightHandPosition;
        gameObject.transform.localRotation = Quaternion.Euler(gunD.relativeRotation);
    }
     //安装背部
    public override void EquipWeaponBackChest(Player player){
        itemData.OnPickUp();
        itemData.ItemOnHand=false;
        ownerPlayer = player;
   //     this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
        gameObject.transform.SetParent(player.weaponSystem.chest);
        gameObject.transform.localPosition =gunD.backChestPosition;
        gameObject.transform.localRotation = Quaternion.Euler(gunD.backChestRotation);
    }
   
    public override void Fire(){
        onFire=true;
    }
    public override void StopFire(){
        onFire=false;
    }

    protected override void Update(){
        base.Update();
        if(!FireCheck()) return;
        //开火咯.
        if(gunD.gunState==GunState.Reloading){
            //换弹中判断是否有子弹
           if(gunD.CurrentMagzine<=0){
               return ;
           }
           //有直接切攻击 取消换弹.
        }
        switch(gunD.FireType){
            case FireType.SEMI:
             
            break;
             case FireType.BURST:
             
            break;
             case FireType.AUTO:
             
            break;
        }
    }
    private void onSingleFire(){
       isShooting = true;
    }
    private bool FireCheck(){
        if(!itemData.ItemOnHand) return false;
        if(!onFire) return false;
        if(ownerPlayer.charData.aimState!=AimState.AimingFinish){
            StopFire();
            return false;
        }
        if(!ownerPlayer.charData.Btn_Fire){
            StopFire();
            return false;
        }
        return true;
    }

    public override void onGet(){
        base.onGet();
     }
    //回收.
    public override void onRecycle(){
     
        base.onRecycle();
     }
    public override void onRelease(){

        base.onRelease();
    }
}