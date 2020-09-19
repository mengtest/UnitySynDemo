//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Run : ActionBase
{
     private int standFrame=0;
    //单次创建.
    public override void init(){
        this.name=GameEnum.ActionLabel.Run;
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
            this.obj.GetMovePart().StopMove();
            //播放 站立动作.
            //跑步 改变 动画属性.
           // DebugLog.Log("Run.........",GameEnum.ActionLabel.Mvm_Jog);
            CharData charData=this.obj.objData as CharData;
            this.obj.moveSpeed=charData.RunSpeed;
            //*this.speed;  //0.75f
            this.obj.GetAniBasePart().Play(GameEnum.AniLabel.Mvm_Jog,frame,0.933f/this.speed,1.12f * this.speed,0.15f,0,true);
            Vector3 dir=(Vector3)param[0];
        //    DebugLog.Log("Run..dir ",dir,this.obj.gameObject.transform.forward);
            this.obj.GetMovePart().StartMove(dir);
    }

        //动作更新;
    public override void Update(){
        base.Update();
    }
    /**
    * 切换动作 处理逻辑;
    */
    public override void executeSwichAction(){
        this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
        this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Up,onJoyUp);
    }

    private  void onJoyMove(object[] data){
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
        if(isDash){
            this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Dash,0,true,new object[]{dirPos});
        }else{
            this.obj.GetMovePart().SetTargetDir(dirPos);
        }
        if(this.standFrame>0){
           this.standFrame=0;
        }
      //  DebugLog.Log("onmove");
    }
    private  void onJoyUp(object[] data=null){
        this.standFrame=0;
        this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true,new object[]{GameEnum.AniLabel.Mvm_Stop_Front,0.9f,1.7f});
    }


    public override void onGet()
    {
       base.onGet();
    }
    /**
    回收..
    **/
    public override void onRecycle()
    {
        standFrame=0;
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

