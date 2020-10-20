//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Walk : ActionBase
{
    private CharData charData;
     private int standFrame=0;
    //单次创建.
    public override void init(){
        this.name=GameEnum.ActionLabel.Walk;
        this.actionLayer=GameEnum.ActionLayer.BaseLayer;
        this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
    }

        // 初始化动作.
    public override void InitAction(ObjBase obj)
    {
        base.InitAction(obj);
        charData=this.obj.objData as CharData;
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
            //*this.speed;  //0.75f
            this.obj.moveSpeed=charData.WalkSpeed;
            this.obj.GetAniBasePart().Play(GameEnum.AniLabel.walk_fwd,frame*GameSettings.Instance.deltaTime,0.99f,1f * this.speed,0.25f,0,true);
            if(param!=null){
                Vector3 dir=(Vector3)param[0];
            //    DebugLog.Log("Run..dir ",dir,this.obj.gameObject.transform.forward);
                this.obj.GetMovePart().StartMove(dir);
           }else{
                this.obj.GetMovePart().StartMove( this.obj.gameObject.transform.forward);
           }
    }

        //动作更新;
    public override void Update(){
        base.Update();
        if(charData.currentUpLayerAction!=GameEnum.ActionLabel.Aiming){
            doRun();
        }else{
            if(charData.aimState==GameEnum.AimState.Null){
                doRun();
            }
        }
    }
    private void doRun(){
        if(charData.isDashing){
            this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Dash);
        }else{
            this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Run);
        }
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
        this.obj.GetMovePart().SetTargetDir(dirPos);
        
        if(this.standFrame>0){
           this.standFrame=0;
        }
      //  DebugLog.Log("onmove");
    }
    private  void onJoyUp(object[] data=null){
        this.standFrame=0;
        this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true);
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

