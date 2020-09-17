//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class ItemDrop : ActionBase
{
    private int standFrame=0;
    private MovePart movePart;
    //单次创建.
    public override void init(){
        this.name=GameEnum.ActionLabel.ItemDrop;
        this.actionLayer=GameEnum.ActionLayer.BaseLayer;
        this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
    }

        // 初始化动作.
    public override void InitAction(ObjBase obj)
    {
        base.InitAction(obj);
        //添加监听.
        this.obj.GetEvent().addEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
        movePart=this.obj.GetMovePart();
    }
        //动作进入
    public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;
            if(this.movePart.IsJumping()){
                switch(this.movePart.jumpState){
                        // case GameEnum.JumpState.JumpFall:
                        // this.onJumpFall();
                        // break;
                        case GameEnum.JumpState.JumpRise:
                        this.onJumpToGround();
                        break;
                        case GameEnum.JumpState.JumpOnGround:
                        this.onBeginJump(frame);
                        break;
                } 
            }else{
                //播放 站立动作.
                //跑步 改变 动画属性.
                this.onBeginJump(frame);
            }
    }
    private void onBeginJump(int frame=0){
      //  this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Jmp_Base_A_Rise,frame,0.5f/this.speed,1.3f*this.speed,0.25f,1,true,true);
        this.movePart.acceleratedupPow = this.movePart.GravityPower * this.speed * this.speed;
        this.movePart.upPow = 8 * this.speed;
        this.movePart.ZeroUpStop=false;
        this.movePart.useWeightPower = false;
        this.movePart.Jump();
    }

    // private void onJumpFall(object[] param=null) {
    //     //  DebugLog.Log("action >> onJumpFall.........");
    //      this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Jmp_Base_A_Fall,0,0.333f/this.speed, 1.3f*this.speed,0.25f,1,true,true);
    // }

    private void onJumpToGround(object[] param=null) {
        //  DebugLog.Log("action >> onJumpToGround.........");
        (this.obj as Item).OnGround();
        this.obj.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
          // 回到站立状态.
    }

    //动作更新;
    public override void Update(){
        this.obj.gameObject.transform.Rotate(Vector3.right, 1000 * Time.deltaTime);
        base.Update();
    }
    /**
    * 切换动作 处理逻辑;
    */
    public override void executeSwichAction(){
        //移除监听.
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
    }

    //动作 需要改成3种分支 base up add
    public override void onGet()
    {
       this.standFrame=0;
       base.onGet();
    }
    /**
    回收..
    **/
    public override void onRecycle()
    {
        this.movePart=null;
        base.onRecycle();
    }
    /*********
    销毁
    *******/
    public override void onRelease()
    {
        this.movePart=null;
        base.onRelease();
    }
}

