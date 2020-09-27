using System;
using System.Collections;
using System.Collections.Generic;
using GameEnum;
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
    public Transform ActiveWeapon;
    //~~~end~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~武器~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private bool inited = false;
    private Player _objBase;
    private Animator ani;
    // 盘骨 , 脊柱 , 胸部 , 右手 , 左臂
    public Transform hips, spine, chest, rightHand, leftArm;   
	public Vector3 initialRootRotation;                                  // Initial root bone local rotation.
	public Vector3 initialHipsRotation;                                  // Initial hips rotation related to the root bone.
	public Vector3 initialSpineRotation;                                 // Initial spine rotation related to the root bone.
    private Vector3 initialChestRotation;                          // Initial chest rotation related to the spine bone.
	private float distToHand;                                      // Distance from neck to hand.
	private Vector3 castRelativeOrigin;                            // Position of neck to cast for blocked aim test.
    private bool isAimBlocked;                                       
    private int blockedAimBool;
    public void Init(Player objBase=null)
    {
        this._objBase=objBase;
        this.UseActiveSide=1;
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
            Transform neck = ani.GetBoneTransform(HumanBodyBones.Neck);
            if (!neck)
            {
                neck = ani.GetBoneTransform(HumanBodyBones.Head).parent;
            }
            hips =ani.GetBoneTransform(HumanBodyBones.Hips);
            spine = ani.GetBoneTransform(HumanBodyBones.Spine);
            chest = ani.GetBoneTransform(HumanBodyBones.Chest);
            rightHand = ani.GetBoneTransform(HumanBodyBones.RightHand);
            leftArm = ani.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            initialRootRotation = (hips.parent == _objBase.gameObject.transform) ? Vector3.zero : hips.parent.localEulerAngles;
            initialHipsRotation = hips.localEulerAngles;
            initialSpineRotation = spine.localEulerAngles;
            initialChestRotation = chest.localEulerAngles;
            castRelativeOrigin = neck.position - _objBase.gameObject.transform.position;
            distToHand = (rightHand.position - neck.position).magnitude * 1.5f;
       //     blockedAimBool = Animator.StringToHash("BlockedAim");
            inited = true;
        }
    }
    // Check if aim is blocked by obstacles. 检查瞄准是否被障碍物阻挡。
	public bool CheckforBlockedAim()
	{
		isAimBlocked = Physics.SphereCast(_objBase.gameObject.transform.position + castRelativeOrigin, 0.1f,CameraManager.Instance.mainCamera.transform.forward, out RaycastHit hit, distToHand - 0.1f);
		isAimBlocked = isAimBlocked && hit.collider.transform != this.transform;
       // ani.SetBool(blockedAimBool, isAimBlocked);
	//	Debug.DrawRay(this.transform.position + castRelativeOrigin, CameraManager.Instance.mainCamera.transform.forward * distToHand, isAimBlocked ? Color.red : Color.cyan);
		return isAimBlocked;
	}
    public void OnAniGunIK(Weapon_Gun weapon){
        //  上半身 朝人物旋转

        //  Orientate upper body where camera  is targeting.
        Quaternion targetRot = Quaternion.Euler(0, _objBase.gameObject.transform.eulerAngles.y, 0);
        targetRot *= Quaternion.Euler(initialRootRotation);
        targetRot *= Quaternion.Euler(initialHipsRotation);
        targetRot *= Quaternion.Euler(initialSpineRotation);
        // Set upper body horizontal orientation.
       ani.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Inverse(hips.rotation) * targetRot);

        
        //上下 随摄像机旋转
        // // Keep upper body orientation regardless strafe direction.
        float xCamRot = Quaternion.LookRotation(CameraManager.Instance.mainCamera.transform.forward).eulerAngles.x;
        targetRot = Quaternion.AngleAxis(xCamRot + weapon.armsRotationY, _objBase.gameObject.transform.right);

        targetRot *= Quaternion.AngleAxis(weapon.armsRotationX, _objBase.gameObject.transform.up);
       //  targetRot *= Quaternion.AngleAxis(20f, _objBase.gameObject.transform.up);

        targetRot *= spine.rotation;
        targetRot *= Quaternion.Euler(initialChestRotation);
        // Set upper body vertical orientation.
        ani.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Inverse(spine.rotation) * targetRot);
    }

    //切换武器;
    public void UseWeaponSide(int side = 1){
         if(UseActiveSide!=side){
             switch(UseActiveSide){
                 case 1:
                     if(weaponMain_1!=null){
                         weaponMain_1.EquipWeaponBackChest(_objBase);
                     }
                     if(weaponMain_2!=null){
                         weaponMain_2.EquipWeaponRightHand(_objBase);
                          this.ActiveWeapon=weaponMain_2.gameObject.transform;
                     }
                 break;
                 case 2:
                    if(weaponMain_2!=null){
                         weaponMain_2.EquipWeaponBackChest(_objBase);
                     }
                     if(weaponMain_1!=null){
                         weaponMain_1.EquipWeaponRightHand(_objBase);
                          this.ActiveWeapon=weaponMain_1.gameObject.transform;
                     }
                 break;
             }
            UseActiveSide=side;
         }
    }
    //是否可以自动拾取武器
    public bool checkCanAutoPickUpWeapon(){
        if(weaponMain_1==null||weaponMain_2==null){
            return true;
        }
        return false;
    }

    //是否有手持武器;
    public bool hasActiveWeapon(){
        switch(UseActiveSide){
            case 1:
                return weaponMain_1!=null?true:false;
            case 2:
                return weaponMain_2!=null?true:false;
        }
        return false;
    }
    public Weapon getActiveWeapon()
    {
        if (UseActiveSide==1)
        {
           return    weaponMain_1 ;
        }
        else
        {
           return    weaponMain_2 ;
        }
    }
    //丢弃当前手持武器
    public void DropWeapon(int side=0){
        if(side==0){
            ChangeWeapon("", UseActiveSide,null);
        }else{
            ChangeWeapon("", side,null);
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
        switch(UseActiveSide){
            case 1:
                if(weaponMain_1==null){
                    ChangeWeapon(weaponUrl,1,weapon);
                }else if(weaponMain_2==null){
                    ChangeWeapon(weaponUrl,2,weapon);
                }else{
                   ChangeWeapon(weaponUrl,UseActiveSide,weapon);
                }
            break;
            case 2:
              if(weaponMain_2==null){
                    ChangeWeapon(weaponUrl,2,weapon);
              }else if(weaponMain_1==null){
                    ChangeWeapon(weaponUrl,1,weapon);
              }else{
                 ChangeWeapon(weaponUrl,UseActiveSide,weapon);
              }
            break;
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
                weaponMain_1.DropItem();
                weaponMain_1=null;
                 this._objBase.charData.mainWeapon_1="";
            }
        }
        else
        {
            if (weaponMain_2 != null)
            {
                weaponMain_2.DropItem();
                weaponMain_2=null;
                  this._objBase.charData.mainWeapon_2="";
            }
        }
    }
    IEnumerator AddWeapon(int side = 1,Weapon weapon=null)
    {
        inited = false;
        if (side==1)
        {
            weaponMain_1 = weapon;
            this._objBase.charData.mainWeapon_1=weapon.poolname;
        }
        else
        {
            weaponMain_2 = weapon;
            this._objBase.charData.mainWeapon_2=weapon.poolname;
        }
        while (!weapon.initViewFin)
        {
            yield return 0;
        }

        if(UseActiveSide!=side){
           weapon.EquipWeaponBackChest(this._objBase);
        }else{
          weapon.EquipWeaponRightHand(this._objBase);
            this.ActiveWeapon=weapon.gameObject.transform;
        }

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
        if(weapon==null&&weaponUrl!=""){
          weapon =  ObjManager.Instance.CreatWeapon(weaponUrl);
        }
        if(weapon!=null){
           //加载武器.
            yield return AddWeapon(side,weapon);   
        }else{
            inited=true;
        }
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
        this.ActiveWeapon=null;
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