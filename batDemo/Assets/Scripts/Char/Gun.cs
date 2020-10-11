using GameEnum;
using UnityEngine;
/****
枪
****/
[AutoRegistLua]
public class Gun : Weapon
{
    private  Weapon_Gun gunD;
    protected bool onFire=false;
    private bool isShooting;
    //开火间隔(秒)
    protected float _curFireRate=0;
    //连发数量
    protected int  _curFireCount=0;
    //连发间隔
    protected float _curFireInterval=0;
    protected bool _burstFire=false;
     //连续开火数
    protected float shotFire=0;
    protected Vector2 _CurRecoil=new Vector2(0,0);

    public Gun()
    {
         charType=GameEnum.ObjType.Gun;
    }

    //重写Data.
    public override void initData(){
      base.initData();
      this.gunD=this.itemData.getGunData();
    }
    public Weapon_Gun getGunData(){
        return this.gunD;
    }
    //安装手上
    public override void EquipWeaponRightHand(Player player){
        itemData.OnPickUp();
        itemData.ItemOnHand=true;
        resetFireGun();
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
    //    resetFireGun();
        ownerPlayer = player;
   //     this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
        gameObject.transform.SetParent(player.weaponSystem.chest);
        gameObject.transform.localPosition =gunD.backChestPosition;
        gameObject.transform.localRotation = Quaternion.Euler(gunD.backChestRotation);
    }
    public override void DropItem(){
    //    resetFireGun();
        base.DropItem();
    }
   
    public override void Fire(){
        onFire=true;
    }
    public override void StopFire(){
        onFire=false;
        shotFire=0;
        //TODO : 
    }
    public void FillMagzin(){
        gunD.FillMagzine();
    }

    protected override void Update(){
        base.Update();
        if(gunD==null)return;
        if(!itemData.ItemOnHand){
            return;
        } 
        if(_curFireRate<this.gunD.FireRate){
          _curFireRate+=GameSettings.Instance.deltaTime;
        }
        if(_curFireInterval<this.gunD.FireInterval){
          _curFireInterval+=GameSettings.Instance.deltaTime;
        }
        if(!FireCheck()) return;
        //开火咯.有子弹
        switch(gunD.FireType){
            case FireType.SEMI:
               onSingleFire();
            break;
            case FireType.BURST:
               onBurstFire();
            break;
            case FireType.AUTO:
                onAutoFire();
            break;
        }
    }

    private  void resetFireGun(){
        _curFireRate=this.gunD.FireRate;
        _curFireCount=this.gunD.FireCount;
        _curFireInterval=this.gunD.FireInterval;
        _burstFire=false;
    }
    //每次子弹发射.
    protected void FireProcessing(){
        //AddRecoil 后坐力.
        AddRecoil();
         for (int i = 0; i < gunD.BulletNum; i++)
         {
             //改变下FirePoint Acc改变.
            // CameraManager.Instance.cameraCtrl.FirePoint;
            gunD.CurrentMagzine-=1;
         //   DebugLog.Log("oneBullet","mag: ",gunD.CurrentMagzine);
           // CameraManager.Instance.cameraCtrl.resetFirePoint();
         }
        playShoot();
         shotFire+=1;
        // rifle_shot
    }
    private void playShoot(){
        if(ownerPlayer==null)return;
         switch(gunD.getItemType()){
            case GameEnum.ItemType.Gun:
                ownerPlayer.GetAniUpPart().Play(GameEnum.AniLabel.rifle_shot,0,0.5f,1,0,1,false,true);
            break;
            case GameEnum.ItemType.PistolGun:
                ownerPlayer.GetAniUpPart().Play(GameEnum.AniLabel.pistol_shot,0,0.967f,1,0,1,false,true);
            break;
        }
       ownerPlayer.GetAniUpPart().endAniAction=backAiming;
    }
    private void backAiming(){
        if(ownerPlayer==null)return;
        switch(gunD.getItemType()){
            case GameEnum.ItemType.Gun:
                ownerPlayer.GetAniUpPart().Play(GameEnum.AniLabel.rifle_aim,0,0.003f,1,0,0);
            break;
            case GameEnum.ItemType.PistolGun:
                ownerPlayer.GetAniUpPart().Play(GameEnum.AniLabel.pistol_aim,0,0.2f,1,0,0);
            break;
        }
    }
    //添加后坐力.
    private void AddRecoil(){
        int recoilIndex =Mathf.Clamp(Mathf.CeilToInt(shotFire),0,gunD.recoilList.Count-1);
        Vector2 recoil= gunD.recoilList[recoilIndex];
        if(recoilIndex>0){
             _CurRecoil.x=recoil.x-gunD.recoilList[recoilIndex-1].x;
             _CurRecoil.y=recoil.y-gunD.recoilList[recoilIndex-1].y;
        }
       _CurRecoil.x= _CurRecoil.x*gunD.RecoilRateYaw;
       _CurRecoil.y=_CurRecoil.y*gunD.RecoilRatePitch;
       CameraManager.Instance.cameraCtrl.SetRecoilAngle(_CurRecoil);
    }
    //单发.
    private void onSingleFire(){
        if(_curFireRate>=gunD.FireRate){
           _curFireRate=0;
           FireProcessing();
           onFire=false;
        }
    }
    //连发. 半自动
    private void onBurstFire(){
      if(!_burstFire && _curFireRate>=gunD.FireRate){
          _curFireRate=0;
          _burstFire=true;
          _curFireCount=gunD.FireCount;
      }
    //  _curFireInterval+=GameSettings.Instance.deltaTime;
      if(_burstFire){
           if(_curFireCount>0){
                if(_curFireInterval>=gunD.FireInterval){
                //    DebugLog.Log("_curFireCount",_curFireCount);
                    _curFireInterval=0;
                    _curFireCount-=1;
                    FireProcessing(); 
                }
           }else{
                _burstFire=false;
           }
      }
    }
    //自动
    private void onAutoFire(){
      if(_curFireRate>=gunD.FireRate){
          _curFireRate=0;
         FireProcessing(); 
      }
    }
    private bool FireCheck(){
        if(!onFire) return false;
        if(gunD.CurrentMagzine<=0){
           StopFire();
           return false;
        }
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
        gunD=null;
        base.onRecycle();
     }
    public override void onRelease(){
          gunD=null;
        base.onRelease();
    }
}