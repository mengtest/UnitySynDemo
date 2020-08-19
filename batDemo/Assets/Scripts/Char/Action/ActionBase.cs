//*************************************************************************
//	技能部件
//*************************************************************************
 
    //技能部件
    public class ActionBase : PoolObj
    {
        //动作在SkillActionPool 中 存储的名称.. nan1@Skill_01 之类.
       // public string name;

        //当前使用技能对象;
        public  ObjBase obj;
        //技能类型; 1 攻击 2技能 0其他.. 3 roll 滚
        public int actionType=0;
        // BaseLayer 0 UpLayer  1  AddLayer 2
        public int actionLayer=0;

        public float lengh =0;
        public float totalFrame =0;

        public float currentFrame=0;

        //上次执行帧方法的帧索引
        protected  int _previousFrameIndex = -1;

        //暂停;
        protected bool _pause=false;
        //技能ID
        public int skillActionId=0;
        /*
        * 播放速度 整个动作的播放速度.
        */
        public float speed = 1;
        /**
        * 是否循环动作;
        */
        public bool isLoop = false;
        // public aniCmdSpeed:number=1;

        /*
        * 取消优先级限制 可在动作 不同时间段调整取消优先级; 技能是否能被其他技能取消; -1为不能被打断;
        */
        public  int cancelPriorityLimit=GameEnum.CancelPriority.Stand_Move;
        /**
        * 不变  默认 取消优先级;
        */
        public int defultPriority=GameEnum.CancelPriority.Stand_Move;

        // 初始化动画控制器
        public virtual void Init(ObjBase obj)
        {
            this.obj=obj;
            this.speed=this.obj.objData.PlaySpeed;
        }
         //更新;
        public virtual void Update(){
            if (!this._pause)
            {
                this.currentFrame += 1;
                if (this.currentFrame > this.totalFrame) {
                    this.currentFrame = this.totalFrame;
                }
                if(this.isLoop){
                    if (this.currentFrame >= this.totalFrame) {
                        this.currentFrame=0;
                    }
                }
            }
        }
        public virtual void Begin(int frame=0,object param=null){

        //   this. cancelPriorityLimit = -1;
            //帧计数归0
            frame = frame == -1 ? 0 : frame;
            this.GotoFrame(frame,param);

        }

        public virtual void GotoFrame(int frame=0,object param=null){
             this.currentFrame = frame;

        }
        /**
        * 切换动作 处理逻辑;
        */
        public virtual void executeSwichAction(){

        }
        //动作 需要改成3种分支 base up add
        public  void Paused()
        {
            this._pause = true;
            if(this.obj.hasAni(this.actionLayer)){
                this.obj.pauseAni(this.actionLayer);
            }
        }
        public bool isPaused() {
            return this._pause;   
        }
        public void  Resume() {
            this._pause = false;
            if(this.obj.hasAni(this.actionLayer)){
               this.obj.resumeAni(this.actionLayer);
            }
        }
        public bool isFinish(){
        return this.currentFrame >= this.totalFrame ;
        }
        public override void onGet()
        {
            this.cancelPriorityLimit=this.defultPriority;
        }
        /**
        回收..
        **/
        public override void onRecycle()
        {
            this.obj=null;
            this.skillActionId=0;
        }
        /*********
        销毁
        *******/
        public override void onRelease()
        {
             this.obj=null;
        }
    }

