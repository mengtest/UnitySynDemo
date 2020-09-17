using GameEnum;
using UnityEngine;
/****
武器基类
****/
[AutoRegistLua]
public class Weapon : Item
{
    public Weapon()
    {
         charType=GameEnum.ObjType.Weapon;
    }

    //重写Data.
    public override void initData(){
      base.initData();

    }
   
    //安装手上
    public void EquipWeaponRightHand(Player player){
        itemData.OnPickUp();
        ownerPlayer = player;
    //    this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
        gameObject.transform.SetParent(player.weaponSystem.rightHand);
        Weapon_Gun gunD=this.itemData.getGunData();
        gameObject.transform.localPosition =gunD.rightHandPosition;
        gameObject.transform.localRotation = Quaternion.Euler(gunD.relativeRotation);
    }
     //安装背部
    public void EquipWeaponBackChest(Player player){
        itemData.OnPickUp();
        ownerPlayer = player;
   //     this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
        gameObject.transform.SetParent(player.weaponSystem.chest);
        Weapon_Gun gunD=this.itemData.getGunData();
        gameObject.transform.localPosition =gunD.backChestPosition;
        gameObject.transform.localRotation = Quaternion.Euler(gunD.backChestRotation);
    }

    public override void onViewLoadFin(){
        base.onViewLoadFin();
    }

    public override void onGet(){
        base.onGet();
     }
    //回收.
    public override void onRecycle(){
        ownerPlayer=null;
        base.onRecycle();
     }
    public override void onRelease(){
        ownerPlayer=null;
        base.onRelease();
    }
}