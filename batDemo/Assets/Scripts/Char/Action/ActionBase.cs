//*************************************************************************
//	技能部件
//*************************************************************************
 
    //技能部件
    public class ActionBase
    {
       
        private ObjBase _obj;

        // 初始化动画控制器
        public void Init(ObjBase obj)
        {
            this._obj=obj;
        }
     


        // 清理资源
        public void Release()
        {
            _obj = null;
        }

    }

