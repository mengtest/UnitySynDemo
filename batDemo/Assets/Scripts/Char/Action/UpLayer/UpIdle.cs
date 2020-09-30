//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class UpIdle : ActionBase
{
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.UpIdle;
            this.actionLayer=GameEnum.ActionLayer.UpLayer;
            this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
        }

         // 初始化动作.
        public override void InitAction(ObjBase obj)
        {
            base.InitAction(obj);
        }
         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
             this.currentFrame = frame;
            (this.obj as Character).GetAniUpPart().Play(GameEnum.AniLabel.UpIdle,0,0.1f,1f,0.25f,0);
             
        }
         //动作更新;
        public override void Update(){
            base.Update();
        }
           //new object[]{worldDir,isSprint,false,this.JoyAngle}
  

        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
            
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

