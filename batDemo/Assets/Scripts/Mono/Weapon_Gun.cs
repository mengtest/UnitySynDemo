using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameEnum;
using UnityEngine;

//目前逻辑是 通过这里配置 常用影响手感的配置 导出可以粘贴到表中
//枪_武器数据..同步数据也写在这
public class Weapon_Gun : MonoBehaviour,IItemData
{
    public WeaponGun_Serializable data ;     
    
   // public GunState gunState=GunState.Idle;

    public int CurrentMagzine=0;

    public string UrlPath;
    
    public bool onGround=false;

    public bool  dontClearDecals=false;

    //需要储存的数据...........................................................
    [Tooltip("枪名称")]
     public string Name ;
     public ItemType itemType = ItemType.Gun;

    [Tooltip("枪型号")]
    public GunNameLink GunLinkType = GunNameLink.M4;

     [Tooltip("枪类型")]
    public Weapon_GunType Weapon_GunType = Weapon_GunType.AR;

    [Tooltip("开火类型")]
    public FireType FireType = FireType.AUTO;

     [Tooltip("射程")]
    public float DamageRange=500f;

    [Tooltip("射速每秒MuzzleVelocity")]
    public float BulletSpeed=880f;

    [Tooltip("切换武器时间")]
    public float SwitchWeaponTime=0.5f;
    
    [Tooltip("子弹伤害")]
    public int Damage = 40; 

     [Tooltip("前置弹夹容量")]
    public int FullMagzine=30;

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
    public Vector3 rightHandPosition=new Vector3(-0.2f,0.029f,0.01f);                   
    // Rotation Offsets relative to the player's right hand.
    // 旋转;
    [Tooltip("握手旋转")]
	public Vector3 relativeRotation=new Vector3(0,90,90);        

     [Tooltip("背饰偏移")]
    public Vector3  backChestPosition=new Vector3(-0.041f,-0.205f,-0.055f); 
     [Tooltip("背饰旋转")]
     public Vector3  backChestRotation=new Vector3(356.53f,317.77f,86.14f);          
    
     public bool ArmTesting=false;

    [Tooltip("肩射摄像机偏移")]
     public  Vector3 aimPivotOffset = new Vector3(0.5f, 1.13f,  0f); 
     [Tooltip("肩射摄像机相对位置")]
	 public  Vector3 aimCamOffset   = new Vector3(0f, 0.55f, -0.71f); 
    public  float armsRotationY   = 16f; 
    public  float armsRotationX   = 40f; 

     [Tooltip("后坐力")]
    public float recoilAngle=3; 


    [Tooltip("子弹偏移数组")]
    public List<Vector2> recoilList= new List<Vector2>();

    [Tooltip("后坐力缩放比例X轴")]
    public float RecoilRateYaw = 1;
    [Tooltip("后坐力缩放比例Y轴")]
    public float RecoilRatePitch = 1;
    

    [Tooltip("子弹偏移数组")]
    public string Yaw_params = "";
    [Tooltip("子弹偏移数组")]
    public string Pitch_params = "";


  
    [Tooltip("开火位置")]
	public Vector3 muzzlePos=Vector3.zero;   

     [Tooltip("开火位置")]
    public Transform muzzleTrans;    

      
    [Tooltip("弹壳飞出位置")]
	public Vector3 shellPos=Vector3.zero;   

     [Tooltip("弹壳飞出位置")]
    public Transform shellTrans;    

	public SphereCollider interactiveRadius;      
    public  Vector3 SphereColCenter =Vector3.zero;
    public float SphereColRadius=1; 
	public BoxCollider col;                                  
    public Vector3 BoxCenter =Vector3.zero;
    public Vector3 BoxSize =new Vector3(0.2f,0.2f,0.2f);
    //以上需要存储数据...........................

    // Weapon collider.

       //通用_显示对象加载reqs.
    private  GameAssetRequest _Reqs=null;
    private string dataUrl;
    private bool isDestory=false;
    private bool pickable = false;
    private Weapon weapon;

    private void Start() {
    }
    public void init(Item items){
       weapon=items as Weapon;
  //     DebugLog.Log("item>>>>>>>>>>",item);
    }
     //U3D加载数据.
     public void LoadData_u3d(string wpUrl){
         this.Name= Path.GetFileName(wpUrl);
         dataUrl="Data/GunData/"+Name;
     //    DebugLog.Log(dataUrl);
         //生成dataUrl;
          _Reqs = GameAssetManager.Instance.LoadAsset<WeaponGun_Serializable>(dataUrl,onLoaded);
     }
    private void onLoaded(UnityEngine.Object[] objs){
       if(this.isDestory){
           return; 
       }
        if (objs.Length>0){
           //替换node.
            data= objs[0] as WeaponGun_Serializable;
            LoadData();
            checkPos();
            if(weapon.itemData.defaultFull){
                //子弹满上.
               FillMagzine();
            }
        }
    }
    private void checkPos(){
        if(muzzleTrans==null){
            muzzleTrans= gameObject.transform.Find("muzzle");
            if(muzzleTrans==null){
                GameObject muzzle= new GameObject("muzzle"); 
                muzzleTrans=muzzle.transform;
                muzzleTrans.parent=transform;
                muzzleTrans.localPosition=Vector3.zero;
                muzzleTrans.localRotation=Quaternion.Euler(0,180,0);
            }
        }else{
            muzzlePos=muzzleTrans.localPosition;
        }
        if(shellTrans==null){
            shellTrans= gameObject.transform.Find("shellPos");
            if(shellTrans==null){
                GameObject shellPos= new GameObject("shellPos"); 
                shellTrans=shellPos.transform;
                shellTrans.parent=transform;
                shellTrans.localPosition=Vector3.zero;
                shellTrans.localRotation=Quaternion.Euler(0,180,0);
            }
        }else{
            shellPos=shellTrans.localPosition;
        }
    }
    public void FillMagzine(){
         CurrentMagzine=FullMagzine;
    }
    IEnumerator CreateActiveRadius()
	{
    //    DebugLog.Log("CreateActiveRadius");
         yield return new WaitForSeconds(1f);
        // if(col==null){
        //       col=gameObject.GetComponent<BoxCollider>();
        //   if(col==null){
        //        col = gameObject.AddComponent<BoxCollider>();
        //    }
        // }
        if(interactiveRadius==null){
           interactiveRadius=gameObject.GetComponent<SphereCollider>();
           if(interactiveRadius==null){
		      interactiveRadius = gameObject.AddComponent<SphereCollider>();
           }
        }
        // col.enabled=false;
        // col.center=this.BoxCenter;
        // col.size=this.BoxSize;
        interactiveRadius.center = this.SphereColCenter;
        interactiveRadius.radius = this.SphereColRadius;
        interactiveRadius.isTrigger = true;
        this.onGround=true;
	}
    public string getGunPath(){
         string subPath="";
         switch(Weapon_GunType){
             case Weapon_GunType.AR:
                subPath="AR";
             break;
             case Weapon_GunType.DMR:
                subPath="DMR";
             break;
             case Weapon_GunType.SR:
                subPath="SR";
             break;
             case Weapon_GunType.LMG:
                subPath="LMG";
             break;
             case Weapon_GunType.SMG:
                subPath="SMG";
             break;
             case Weapon_GunType.ShotGun:
                subPath="ShotGun";
             break;
             case Weapon_GunType.Special:
                subPath="Special";
             break;
             case Weapon_GunType.Launchers:
                subPath="Launchers";
             break;
             case Weapon_GunType.Pistol:
                subPath="Pistol";
             break;
         }
         UrlPath="Gun/"+subPath+"/"+this.Name;
         return UrlPath;
    }
     public void LoadData(){
        WeaponGun_Serializable source=data;
        Weapon_Gun tar=this;
         
        tar.Name=source.Name;
        tar.GunLinkType=source.GunLinkType;
        tar.Weapon_GunType=source.Weapon_GunType;
        tar.FireType=source.FireType;
        tar.DamageRange=source.DamageRange;
        tar.BulletSpeed=source.BulletSpeed;
        tar.SwitchWeaponTime=source.SwitchWeaponTime;
        tar.Damage=source.Damage;
        tar.FullMagzine=source.FullMagzine;
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
        tar.backChestPosition=source.backChestPosition;
        tar.backChestRotation=source.backChestRotation;      
        tar.aimPivotOffset=source.aimPivotOffset;
        tar.aimCamOffset=source.aimCamOffset;
        tar.armsRotationY=source.armsRotationY;
        tar.armsRotationX=source.armsRotationX;
        tar.recoilAngle=source.recoilAngle;
        tar.recoilList= new List<Vector2>(source.recoilList.ToArray());
        tar.RecoilRateYaw=source.RecoilRateYaw;
        tar.RecoilRatePitch=source.RecoilRatePitch;
        tar.Yaw_params=source.Yaw_params;
        tar.Pitch_params=source.Pitch_params;
        tar.muzzlePos=source.muzzlePos;
        if(muzzleTrans!=null){
            muzzleTrans.localPosition= tar.muzzlePos;
        }
        tar.shellPos=source.shellPos;
        if(shellTrans!=null){
            shellTrans.localPosition= tar.shellPos;
        }
        tar.SphereColCenter=source.SphereColCenter;
        tar.SphereColRadius=source.SphereColRadius;
        tar.BoxCenter=source.BoxCenter;
        tar.BoxSize=source.BoxSize;
        if(this.gameObject.transform.parent==null){
           OnGround();
        }
    }
    public void SaveData(){
#if UNITY_EDITOR   
        Weapon_Gun source=this;
        WeaponGun_Serializable tar = data;

        tar.Name=source.Name;
        tar.GunLinkType=source.GunLinkType;
        tar.Weapon_GunType=source.Weapon_GunType;
        tar.FireType=source.FireType;
        tar.DamageRange=source.DamageRange;
        tar.BulletSpeed=source.BulletSpeed;
        tar.SwitchWeaponTime=source.SwitchWeaponTime;
        tar.Damage=source.Damage;
        tar.FullMagzine=source.FullMagzine;
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
        tar.backChestPosition=source.backChestPosition;
        tar.backChestRotation=source.backChestRotation;  
        tar.aimPivotOffset=source.aimPivotOffset;
        tar.aimCamOffset=source.aimCamOffset;
        tar.armsRotationY=source.armsRotationY;
        tar.armsRotationX=source.armsRotationX;
        tar.recoilAngle=source.recoilAngle;
        tar.recoilList= new List<Vector2>(source.recoilList.ToArray());
        tar.RecoilRateYaw=source.RecoilRateYaw;
        tar.RecoilRatePitch=source.RecoilRatePitch;
        tar.Yaw_params=source.Yaw_params;
        tar.Pitch_params=source.Pitch_params;
        tar.muzzlePos=source.muzzlePos;
        tar.shellPos=source.shellPos;
        tar.SphereColCenter=source.SphereColCenter;
        tar.SphereColRadius=source.SphereColRadius;
        tar.BoxCenter=source.BoxCenter;
        tar.BoxSize=source.BoxSize;

#endif
    }
    public void OnPickUp(){
        if(this.interactiveRadius!=null){
          GameObject.Destroy(this.interactiveRadius);
        }
        this.interactiveRadius=null;
        this.onGround=false;
      //  this.col.enabled=false;
    }
    public void OnDrop(){
     //   this.col.enabled=false;
    }
    public void OnGround(){
        StartCoroutine(CreateActiveRadius());
     //   this.col.enabled=false;
    }
    public float getHeight()
    {
        return BoxSize.y;
    }
    // Handle player exiting radius of interaction.
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
      //     DebugLog.Log("Exit");
           CharData charData= other.gameObject.GetComponentInChildren<CharData>();
           charData.getChar().OnItemTrigger(weapon,false);
		}
	}
    private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player")
		{
      //     DebugLog.Log("Stay");
	       CharData charData= other.gameObject.GetComponentInChildren<CharData>();
           charData.getChar().OnItemTrigger(weapon);
		}
    }

    public void OnDestroy() {
        isDestory=true;
        weapon=null;
        if(this._Reqs!=null){
            this._Reqs.Unload();
            this._Reqs = null;
       }
    }

    public ItemType getItemType()
    {
       return itemType;
    }
}
