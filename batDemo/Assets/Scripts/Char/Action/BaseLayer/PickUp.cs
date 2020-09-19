//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class PickUp : ActionBase
{
        private bool pickUpEnd=false;
        private float pickTime=0f;
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
            (this.obj as Player).EquipNearWeapon();
        }

         //动作更新;
        public override void Update(){
            base.Update();
            pickTime+=Time.fixedDeltaTime;
            if(pickTime>=0.297f){
                 pickUpEnd=true;
                 this.cancelPriorityLimit=this.defultPriority;
                 (this.obj as Player).checkPickUpNearItem();
            }
            if(pickTime>=0.667f){
              this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true);
            }
        }
           //new object[]{worldDir,isSprint,false,this.JoyAngle}
        private  void onJoyMove(object[] data){
            if(!pickUpEnd)return;
            Vector3 dirPos = (Vector3) data[0];
         //  DebugLog.Log("stand Move>>>",dirPos);
            bool isDash = (bool) data[1];
            bool canStop= (bool) data[2];
            if(canStop){
                return;
            }
            if(isDash){
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Dash,0,true,new object[]{dirPos});
            }else{
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Run,0,true,new object[]{dirPos});
            }
        }
        private  void onJoyUp(object[] data=null){
             this.obj.GetMovePart().StopMove(false,true,false);
        }

        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
             this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
             this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Up,onJoyUp);
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
            base.onRecycle();
        }
        /*********
        销毁
        *******/
        public override void onRelease()
        {
             base.onRelease();
        }
}

