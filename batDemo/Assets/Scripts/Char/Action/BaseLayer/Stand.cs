//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Stand : ActionBase
{
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.Stand;
            this.actionLayer=GameEnum.ActionLayer.BaseLayer;
            this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
            
        }

         // 初始化动作.
        public override void InitAction(ObjBase obj)
        {
            base.InitAction(obj);
            this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Move,onJoyMove);
        }
         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
             this.currentFrame = frame;
             this.obj.GetMovePart().StopMove();
             if(param!=null){
                string actionLabel=(string)param[0];
                float lenth =(float)param[1];
                float speed =(float)param[2];
                this.obj.GetAniBasePart().Play(actionLabel,0,lenth,speed,0.25f,1,true,true);
                this.obj.GetAniBasePart().endAniAction=doStandAction;
             }else{
                //播放 站立动作.
                this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Idle_Wait_A,frame,2f,1.5f,0.25f,0,true);
             }
        }
    
         // 回到站立动作.
        private void doStandAction(){
    //       DebugLog.Log("this.obj",this.obj);
            if(this.isRecycled)return;
            this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Idle_Wait_A,0,2f,1.5f,0.25f,0,true);
        }

         //动作更新;
        public override void Update(){
            base.Update();
        }
           //new object[]{worldDir,isSprint,false,this.JoyAngle}
        private  void onJoyMove(object[] data){
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


        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
             this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
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

