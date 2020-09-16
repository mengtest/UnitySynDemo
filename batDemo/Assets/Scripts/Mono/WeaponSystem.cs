using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//武器系统  我想弱化远程  武器栏只
public class WeaponSystem : MonoBehaviour
{
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~武器~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    //右手武器;  主武器
    private Weapon weaponMain_1;
    //右手武器;  主武器2
    private Weapon weaponMain_2;
    //正在使用的武器
    public int UseActiveSide=1;
    //~~~end~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~武器~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private bool inited = false;
    private Player _objBase;
    private Animator ani;
    // 盘骨 , 脊柱 , 胸部 , 右手 , 左臂
    public Transform hips, spine, chest, rightHand, leftArm;   
    public void Init(Player objBase=null)
    {
        this._objBase=objBase;
    }
    void Update()
    {
       if(!inited){   
          initAni();
       }

    }
    private void initAni(){
        if(!inited&&this._objBase.initViewFin){
            ani = _objBase.gameObject.GetComponent<Animator>();
            hips =ani.GetBoneTransform(HumanBodyBones.Hips);
            spine = ani.GetBoneTransform(HumanBodyBones.Spine);
            chest = ani.GetBoneTransform(HumanBodyBones.Chest);
            rightHand = ani.GetBoneTransform(HumanBodyBones.RightHand);
            leftArm = ani.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            inited = true;
        }
    }
    public void UseWeaponSide(int side = 1){
         if(UseActiveSide!=side){
             switch(UseActiveSide){
                 case 1:
                     if(weaponMain_1!=null){
                         weaponMain_1.EquipWeaponBackChest(_objBase);
                     }
                     if(weaponMain_2!=null){
                         weaponMain_2.EquipWeaponRightHand(_objBase);
                     }
                 break;
                 case 2:
                    if(weaponMain_2!=null){
                         weaponMain_2.EquipWeaponBackChest(_objBase);
                     }
                     if(weaponMain_1!=null){
                         weaponMain_1.EquipWeaponRightHand(_objBase);
                     }
                 break;
             }
            UseActiveSide=side;
         }
    }
    public void EquipWeapon(Weapon weapon)
    {
        EquipWeapon(weapon.poolname,weapon);
    }
    //装备武器 无序
    public void EquipWeapon(string weaponUrl,Weapon weapon=null)
    {
        //默认装备 武器1;
        if(weaponMain_1==null){
           ChangeWeapon(weaponUrl,1,weapon);
        }else if(weaponMain_2==null){
           ChangeWeapon(weaponUrl,2,weapon);
        }else{
           ChangeWeapon(weaponUrl,UseActiveSide,weapon);
        }
    }

    //换武器.  武器不合贴图 同步用方法.
    public void ChangeWeapon(string weaponUrl, int side = 1,Weapon weapon=null)
    {
        StartCoroutine(ChangeWeaponCoroutine(weaponUrl, side,weapon));
    }
    private void RemoveWeapon(int side = 1)
    {
        if (side==1)
        {
            if (weaponMain_1 != null)
            {
                weaponMain_1.DropWeapon();
            }
        }
        else
        {
            if (weaponMain_2 != null)
            {
                weaponMain_2.DropWeapon();
            }
        }
    }
    IEnumerator AddWeapon(int side = 1,Weapon weapon=null)
    {
        inited = false;
        if (side==1)
        {
            weaponMain_1 = weapon;
        }
        else
        {
            weaponMain_2 = weapon;
        }
        while (!weapon.initViewFin)
        {
            yield return 0;
        }

        weapon.EquipWeaponRightHand(this._objBase);

        // if (GameSettings.Instance.useAssetBundle)
        // {
        //     RenderHelper.RefreshShader(ref weapon);
        // }
        inited = true;
    }
    // public GameEnum.ItemType getWeaponItemType(string url){

    // }



    IEnumerator ChangeWeaponCoroutine(string weaponUrl, int side = 1,Weapon weapon=null)
    {
        while(!inited)
        {
            yield return 0;
        }
        //卸载武器.
        RemoveWeapon(side);
        inited=false;
        if(weapon==null){
          weapon =  ObjManager.Instance.CreatWeapon(weaponUrl);
        }
        //加载武器.
        yield return AddWeapon(side,weapon);
    }

 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void OnDestroy()
    {
        //释放部件 	
        this.ani=null;
        if (weaponMain_1 != null)
        {
            weaponMain_1 = null;
        }
        if (weaponMain_2 != null)
        {
            weaponMain_2 = null;
        }
        _objBase=null;
    }
}