using System;
using System.Collections;
using System.Collections.Generic;
using GameEnum;
using UnityEngine;
//目前逻辑是 通过这里配置 常用影响手感的配置 导出可以粘贴到表中
//枪_武器数据..同步数据也写在这
public class Weapon_Gun : MonoBehaviour
{
    public WeaponGun_Serializable data ;     

    [Tooltip("枪名称")]
     public string Name ;

    [Tooltip("枪型号")]
    public GunNameLink GunLinkType = GunNameLink.M4;

     [Tooltip("枪类型")]
    public Weapon_GunType Weapon_GunType = Weapon_GunType.AR;

    [Tooltip("开火类型")]
    public FireType FireType = FireType.AUTO;

    [Tooltip("切换武器时间")]
    public float SwitchWeaponTime=0.5f;
    
    [Tooltip("子弹伤害")]
    public int Damage = 40; 

     [Tooltip("前置弹夹容量")]
    public int Magzine=30;

    [Tooltip("全弹换弹时间")]
    public float ReloadTime=2.1f;

    [Tooltip("战术换弹时间")]
    public float BattleReloadTime=1.9f;

    [Tooltip("特殊单发换弹时间")]
    public float SpecialReloadTime=0f;
    
    [Tooltip("单次发射弹头数量")]
    public int BulletNum=1;

    [Tooltip("开火间隔(秒)")]
    public float FireRate=0.0857f;

    [Tooltip("连发数量")]
    public int FireCount=0;

    [Tooltip("三连发单发间隔")]
    public float FireInterval=0;

    [Tooltip("腰射开镜时间")]
    public float AimTime=0.5f;


    // Position offsets relative to the player's right hand.
    // 右手偏移;
     [Tooltip("右手偏移")]
    public Vector3 rightHandPosition=Vector3.zero;                         
    // Rotation Offsets relative to the player's right hand.
    // 旋转;
    [Tooltip("握手旋转")]
	public Vector3 relativeRotation=Vector3.zero;        

     [Tooltip("后坐力")]
    public float recoilAngle=3; 


    [Tooltip("子弹偏移数组")]
    public List<Vector2> recoilList= new List<Vector2>();
    [Tooltip("子弹偏移数组")]
    public string Yaw_params = "";
    [Tooltip("子弹偏移数组")]
    public string Pitch_params = "";


  
    [Tooltip("开火位置")]
	public Vector3 muzzlePos=Vector3.zero;   

     [Tooltip("开火位置")]
    public Transform muzzleTrans;    
    // Start is called before the first frame update

     public void LoadData(){
        WeaponGun_Serializable source=data;
        Weapon_Gun tar=this;
         
        tar.Name=source.Name;
        tar.GunLinkType=source.GunLinkType;
        tar.Weapon_GunType=source.Weapon_GunType;
        tar.FireType=source.FireType;
        tar.SwitchWeaponTime=source.SwitchWeaponTime;
        tar.Damage=source.Damage;
        tar.Magzine=source.Magzine;
        tar.ReloadTime=source.ReloadTime;
        tar.BattleReloadTime=source.BattleReloadTime;
        tar.SpecialReloadTime=source.SpecialReloadTime;
        tar.BulletNum=source.BulletNum;
        tar.FireRate=source.FireRate;
        tar.FireCount=source.FireCount;
        tar.FireInterval=source.FireInterval;
        tar.AimTime=source.AimTime;
        tar.rightHandPosition=source.rightHandPosition;
        tar.relativeRotation=source.relativeRotation;
        tar.recoilAngle=source.recoilAngle;
        tar.recoilList= new List<Vector2>(source.recoilList.ToArray());
        tar.Yaw_params=source.Yaw_params;
        tar.Pitch_params=source.Pitch_params;
        tar.muzzlePos=source.muzzlePos;
    }
    public void SaveData(){
#if UNITY_EDITOR   
        Weapon_Gun source=this;
        WeaponGun_Serializable tar = data;

        tar.Name=source.Name;
        tar.GunLinkType=source.GunLinkType;
        tar.Weapon_GunType=source.Weapon_GunType;
        tar.FireType=source.FireType;
        tar.SwitchWeaponTime=source.SwitchWeaponTime;
        tar.Damage=source.Damage;
        tar.Magzine=source.Magzine;
        tar.ReloadTime=source.ReloadTime;
        tar.BattleReloadTime=source.BattleReloadTime;
        tar.SpecialReloadTime=source.SpecialReloadTime;
        tar.BulletNum=source.BulletNum;
        tar.FireRate=source.FireRate;
        tar.FireCount=source.FireCount;
        tar.FireInterval=source.FireInterval;
        tar.AimTime=source.AimTime;
        tar.rightHandPosition=source.rightHandPosition;
        tar.relativeRotation=source.relativeRotation;
        tar.recoilAngle=source.recoilAngle;
        tar.recoilList= new List<Vector2>(source.recoilList.ToArray());
        tar.Yaw_params=source.Yaw_params;
        tar.Pitch_params=source.Pitch_params;
        tar.muzzlePos=source.muzzlePos;

#endif
    }


    public void OnDestroy() {
       
    }
}
