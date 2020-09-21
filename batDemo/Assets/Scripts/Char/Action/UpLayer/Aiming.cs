//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Aiming : ActionBase
{
        private float aimTurnSmoothing = 1f;                                // Speed of turn response when aiming to match camera facing.
	    private Vector3 aimPivotOffset = new Vector3(0.5f, 1.2f,  0f);         // Offset to repoint the camera when aiming.
	    private Vector3 aimCamOffset   = new Vector3(0f, 0.4f, -0.7f);         // Offset to relocate the camera when aiming.

        private Weapon_Gun weapon_gun;   
        private Player player;
        private Animator Anim;
        private float aimSmoothing;   
        private float aimTime=0f;
      
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.Aiming;
            this.actionLayer=GameEnum.ActionLayer.UpLayer;
            this.defultPriority=GameEnum.CancelPriority.BaseAction;
            this.activeIK=true;
            
        }

         // 初始化动作.
        public override void InitAction(ObjBase obj)
        {
            base.InitAction(obj);
             player=this.obj as Player;
             Anim=player.gameObject.GetComponent<Animator>();
        }
         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;
            //播放 站立动作.
            //判断武器类型 aiming

           if(player.charData.currentBaseAction==GameEnum.ActionLabel.PickUp){
               //自动捡东西在 瞄准需立刻切站立
                 player.doActionSkillByLabel(GameEnum.ActionLabel.Stand);
            }
          player.GetMovePart().faceToRotation=false;
             Weapon weapon =   player.weaponSystem.getActiveWeapon();
             weapon_gun= weapon.itemData.getGunData();
            aimSmoothing=weapon_gun.AimTime/Time.fixedDeltaTime ;
         //   DebugLog.Log(aimSmoothing,">>>>>>>>>>>>>>");
            aimTime=0;
            // smooth * Time.deltaTime
             switch(weapon_gun.getItemType()){
                 case GameEnum.ItemType.Gun:
                    player.GetAniUpPart().Play(GameEnum.AniLabel.rifle_aim,0,0.003f,1,weapon_gun.AimTime/2f,0);
                 break;
                 case GameEnum.ItemType.PistolGun:
                     player.GetAniUpPart().Play(GameEnum.AniLabel.pistol_aim,0,0.2f,1,weapon_gun.AimTime/2f,0);
                 break;
             }
             AimOn();
        }

         //动作更新;
        public override void Update(){
           base.Update();
           if(!player.charData.Btn_Aim){
                this.cancelPriorityLimit=GameEnum.CancelPriority.Stand_Move_Null;
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle,0,false);
               return;
           }
           if(!player.charData.isAimming){
                aimTime+=Time.fixedDeltaTime;
                if(aimTime>=weapon_gun.AimTime){
                    this.cancelPriorityLimit=GameEnum.CancelPriority.Stand_Move_Null;
                     CameraManager.Instance.cameraCtrl.smooth=10;
                    player.charData.isAimming=true;
                    if(player.charData.Btn_Fire){
                        Shoot();
                    }
                }
           }else{
               if(player.charData.Btn_Fire){
                     Shoot();
               }
           }
           if(player.charData.isMyPlayer){
               Rotating();
           }
        }
        //face To Camera
       private 	void Rotating()
	   {
            // Vector3 forward = CameraManager.Instance.mainCamera.transform.TransformDirection(Vector3.forward);
            // // Player is moving on ground, Y component of camera facing is not relevant.
            // forward.y = 0.0f;
            // forward = forward.normalized;

         //   player.GetMovePart().SetTargetRotation(CameraManager.Instance.cameraCtrl.GetH());

            // // Always rotates the player according to the camera horizontal rotation in aim mode.
             Quaternion targetRotation =  Quaternion.Euler(0, CameraManager.Instance.cameraCtrl.GetH(), 0); //-20

             float minSpeed = Quaternion.Angle(this.obj.gameObject.transform.rotation, targetRotation) * aimTurnSmoothing;


            // // Rotate entire player to face camera.
            // behaviourManager.SetLastDirection(forward);
            player.gameObject.transform.rotation = Quaternion.Slerp( player.gameObject.transform.rotation, targetRotation, minSpeed * Time.deltaTime);
            
	    }
        private void Shoot(){
            DebugLog.Log("shoot");
        }
        private void AimOn(){
            if(player.charData.isMyPlayer){
                 CameraManager.Instance.cameraCtrl.smooth=aimSmoothing;
                  CameraManager.Instance.cameraCtrl.SetTargetOffsets (aimPivotOffset, aimCamOffset);
            }
        }
        private void AimOff(){
            
        }
    public override void onAnimatorIK(int layerIndex)
    {
    //   DebugLog.Log("Ik");   
       if (player.weaponSystem.CheckforBlockedAim())
            return;

       player.weaponSystem.OnAniGunIK(weapon_gun.getItemType());
	
    }

    /**
    * 切换动作 处理逻辑;
    */

        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
             player.charData.isAimming=false;
             
            player.GetMovePart().faceToRotation=true;
             if(player.charData.isMyPlayer){
                CameraManager.Instance.cameraCtrl.smooth=10;
                CameraManager.Instance.cameraCtrl.ResetTargetOffsets();
                CameraManager.Instance.cameraCtrl.ResetMaxVerticalAngle();
             }
        }

        //动作 需要改成3种分支 base up add
        public override void onGet()
        {
             base.onGet();
        }
        /**
        回收..
        **/
        public override void onRecycle()
        {
            weapon_gun=null;
            player=null;
            base.onRecycle();
        }
        /*********
        销毁
        *******/
        public override void onRelease()
        {
             weapon_gun=null;
             player=null;
             base.onRelease();
        }
}

