//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class PickUp : ActionBase
{
      private Vector3 pivotOffset = new Vector3(0.0f, 1f,  0.0f);
      private  Vector3 camOffset   = new Vector3(0.24f, 0.8f, -2.8f);    //-2.8f   -6.8f
        private bool pickUpEnd=false;
        private float pickTime=0f;
        private int standFrame=0;
        private Player player;
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.PickUp;
            this.actionLayer=GameEnum.ActionLayer.BaseLayer;
            this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
            
        }

         // 初始化动作.
        public override void InitAction(ObjBase obj)
        {
            base.InitAction(obj);
             player= this.obj as Player;
            this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Move,onJoyMove);
            this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Up,onJoyUp);
        }
         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;
            pickUpEnd=false;
            pickTime=0;
            this.cancelPriorityLimit=GameEnum.CancelPriority.BaseAction;
        //    this.obj.GetMovePart().StopMove(false,true,true);
            //播放 站立动作.
            this.obj.GetAniBasePart().Play(GameEnum.AniLabel.PickUp,0,0.667f,1,0);
            player.EquipNearWeapon();
            player.GetMovePart().movePoint=4000;
           if(player.charData.isMyPlayer){
                if(player.charData.aimState==GameEnum.AimState.Null){
                    CameraManager.Instance.cameraCtrl.SetTargetOffsets(pivotOffset,camOffset);
         //           CameraManager.Instance.cameraCtrl.smooth=5;
                }
           }
        }

         //动作更新;
        public override void Update(){
            base.Update();
            pickTime+=Time.deltaTime;
            if(pickTime>=0.297f){
                 pickUpEnd=true;
                 this.cancelPriorityLimit=this.defultPriority;
                 player.checkPickUpNearItem();
            }
            if(pickTime>=0.667f){
                 this.onPickUpEnd();
            }
        }
        private void onPickUpEnd(){
            player.GetMovePart().movePoint=10000;
             Vector3 dirp=player.charData.getChar().dirPos;
            if(dirp!=Vector3.zero){
                if(player.charData.aimState!=GameEnum.AimState.Null){
                    this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Walk,0,true,new object[]{dirp});
                }else{
                //还在移动需要继续移动.
                // if(player.charData.isDashing){
                //     this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Dash,0,true,new object[]{dirp});
                // }else{
                    this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Run,0,true,new object[]{dirp});
                }
            }else{
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true);
            }
        }
           //new object[]{worldDir,isSprint,false,this.JoyAngle}
        private  void onJoyMove(object[] data){
            if(!pickUpEnd)return;
            Vector3 dirPos = (Vector3) data[0];
            bool isDash = (bool) data[1];
            bool canStop= (bool) data[2];
            if(canStop){
                this.standFrame+=1;
                if(this.standFrame>=4){
                    //发送站立.
                    this.onJoyUp();
                }
                return;
            }
            if( player.GetMovePart().IsMove()){
                player.GetMovePart().SetTargetDir(dirPos);
            }else{
                player.GetMovePart().StartMove(dirPos);
            }
            if(this.standFrame>0){
            this.standFrame=0;
            }
        }
        private  void onJoyUp(object[] data=null){
             this.obj.GetMovePart().StopMove(false,true,false);
        }

        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
              player.GetMovePart().movePoint=10000;
             this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
             this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Up,onJoyUp);
           if(player.charData.isMyPlayer){
                if(player.charData.currentUpLayerAction!=GameEnum.ActionLabel.Aiming){
          //          CameraManager.Instance.cameraCtrl.ResetFOV();
                    CameraManager.Instance.cameraCtrl.ResetTargetOffsets();
        //            CameraManager.Instance.cameraCtrl.smooth=10;
                }
            //     CameraManager.Instance.cameraCtrl.smoothVerAngle=10;
            }
        }

        //动作 需要改成3种分支 base up add
        public override void onGet()
        {
              this.standFrame=0;
             base.onGet();
        }
        /**
        回收..
        **/
        public override void onRecycle()
        {
            player=null;
            base.onRecycle();
        }
        /*********
        销毁
        *******/
        public override void onRelease()
        {
             player=null;
             base.onRelease();
        }
}

