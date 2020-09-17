//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class ItemDefault : ActionBase
{
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.ItemDefault;
            this.actionLayer=GameEnum.ActionLayer.BaseLayer;
            this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
        }


         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
             this.currentFrame = frame;
             this.obj.GetMovePart().StopMove();
              this.obj.gameObject.transform.localRotation= Quaternion.Euler(Vector3.zero);
             //旋转到正面.
        }
    
        //动作更新;
        public override void Update(){
            base.Update();
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

