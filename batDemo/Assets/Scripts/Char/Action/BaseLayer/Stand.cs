//*************************************************************************
//动作
//*************************************************************************
 
    //动作
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
        }
         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
             this.currentFrame = frame;
             this.obj.GetMovePart().StopMove();
             //播放 站立动作.
             this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Idle_Wait_A,frame,2f,1.5f,0.25f,0,true);

        }

         //动作更新;
        public override void Update(){
            base.Update();
        }

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

