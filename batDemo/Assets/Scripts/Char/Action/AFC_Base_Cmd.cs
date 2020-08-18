//*************************************************************************
//	技能部件
//*************************************************************************
 
    //技能部件
    public class AFC_Base_Cmd
    {

        public bool executed = false;
        //只运行一次
        public bool onceEvent;

        protected AFC_Base_Data afcData;
        
       // protected CmdAction action;
        private int _frameId = 0;
        private int _maxFrame = 1;
        //如果有 需要 被意外终止，自然终止  需要执行的方法 为true;
        public bool hasSwitchFun = false;
        //需要更新;
        public bool needUpdate = false;
        public bool updateFinished = false;

        public  AFC_Base_Cmd()
        {
            
        }
       
      

    }

