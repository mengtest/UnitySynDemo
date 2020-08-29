//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Jump : ActionBase
{
    private MovePart movePart;
    //单次创建.
    public override void init(){
        this.name=GameEnum.ActionLabel.Jump;
        this.actionLayer=GameEnum.ActionLayer.BaseLayer;
        this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
    }

        // 初始化动作.
    public override void InitAction(ObjBase obj)
    {
        base.InitAction(obj);
        //添加监听.
        this.obj.GetEvent().addEventListener(CharEvent.Jump_Fall,onJumpFall);
        this.obj.GetEvent().addEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
        movePart=this.obj.GetMovePart();
    }
        //动作进入
    public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;
            //播放 站立动作.
            //跑步 改变 动画属性.
        //   DebugLog.Log("Run.........");
            this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Mvm_Jog,frame,0.933f/this.speed,1.12f * this.speed,0.25f,0,true);
            this.movePart.acceleratedupPow = this.movePart.GravityPower * this.speed * this.speed;
            this.movePart.upPow = 50 * this.speed;
            this.movePart.ZeroUpStop=false;
            this.movePart.useWeightPower = false;
            this.movePart.Jump();

            //switch
    }
    private void onJumpFall(object[] param) {
          this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Mvm_Jog,0,0.933f,1.12f,0.25f,0,true);
    }
    private void onJumpToGround(object[] param) {
          this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Mvm_Jog,0,0.933f,1.12f,0.25f,0,true);
           this.obj.GetAniBasePart().endAniAction=doStandAction;
          // 回到站立状态.
    }
    private void doStandAction(){
        Character chars=this.obj as Character;
        if(chars!=null){
            chars.Do_StopMove();
        }
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
        this.movePart=null;
        //移除监听.
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_Fall,onJumpFall);
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
        base.onRecycle();
    }
    /*********
    销毁
    *******/
    public override void onRelease()
    {
        this.movePart=null;
        //移除监听.
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_Fall,onJumpFall);
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
        base.onRelease();
    }
}

