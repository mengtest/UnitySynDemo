using GameEnum;
using UnityEngine;
/****
武器基类
****/
[AutoRegistLua]
public class Weapon : Item
{
    private Player ownerPlayer;
    public Weapon()
    {
         charType=GameEnum.ObjType.Weapon;
    }

    //重写Data.
    public override void initData(){
      base.initData();

    }
   
    public void DropWeapon(){
       gameObject.transform.parent=null;
       //给它向上力 让它掉出来;
    }
    //安装手上
    public void EquipWeaponRightHand(Player player){

        ownerPlayer = player;
        gameObject.transform.SetParent(player.weaponSystem.rightHand);
        Weapon_Gun gunD=this.itemData.getGunData();
        gameObject.transform.position =gunD.rightHandPosition;
        gameObject.transform.rotation = Quaternion.Euler(gunD.relativeRotation);
    }
     //安装背部
    public void EquipWeaponBackChest(Player player){
        ownerPlayer = player;
        gameObject.transform.SetParent(player.weaponSystem.chest);
        Weapon_Gun gunD=this.itemData.getGunData();
        gameObject.transform.position =gunD.backChestPosition;
        gameObject.transform.rotation = Quaternion.Euler(gunD.backChestRotation);
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