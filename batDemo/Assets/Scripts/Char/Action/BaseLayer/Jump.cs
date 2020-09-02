﻿//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Jump : ActionBase
{
    private int standFrame=0;
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
        this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Move,onJoyMove);
        this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Up,onJoyUp);
        movePart=this.obj.GetMovePart();
    }
        //动作进入
    public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;

            if(this.movePart.IsJumping()){
                switch(this.movePart.jumpState){
                        case GameEnum.JumpState.JumpFall:
                        this.onJumpFall();
                        break;
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
        CharData charData=this.obj.objData as CharData;
            CameraManager.Instance.cameraCtrl.followTargetSmooth=10f;
        if(charData.isDashing){
             CameraManager.Instance.cameraCtrl.SetFOV(80);
            CameraManager.Instance.postLayer.enabled=true;
            this.obj.moveSpeed=charData.DashSpeed;
            this.movePart.speed=charData.RunSpeed;
        }else{
            CameraManager.Instance.cameraCtrl.ResetFOV();
           CameraManager.Instance.postLayer.enabled=false;
            this.obj.moveSpeed=charData.RunSpeed;
            this.movePart.speed=charData.RunSpeed;
        }
        this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Jmp_Base_A_Rise,frame,0.5f/this.speed,1.3f*this.speed,0.25f,1,true,true);
        this.movePart.acceleratedupPow = this.movePart.GravityPower * this.speed * this.speed;
        this.movePart.upPow = 15 * this.speed;
        this.movePart.ZeroUpStop=false;
        this.movePart.useWeightPower = false;
        this.movePart.Jump();
    }
    private void onJumpFall(object[] param=null) {
        //  DebugLog.Log("action >> onJumpFall.........");
         this.obj.GetAniBasePart().Play(GameEnum.ActionLabel.Jmp_Base_A_Fall,0,0.333f/this.speed, 1.3f*this.speed,0.25f,1,true,true);
    }
    private void onJumpToGround(object[] param=null) {
        //  DebugLog.Log("action >> onJumpToGround.........");
        Character character=this.obj as Character;
        if(character.dirPos!=Vector3.zero){
            //还在移动需要继续移动.
            if(character.charData.isDashing){
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Dash,0,true,new object[]{character.dirPos});
            }else{
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Run,0,true,new object[]{character.dirPos});
            }
        }else{
           this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true,new object[]{GameEnum.ActionLabel.Jmp_Base_A_OnGround,0.367f/this.speed, 0.7f*this.speed});
        }
          // 回到站立状态.
    }

    //动作更新;
    public override void Update(){
        base.Update();
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
        if(this.movePart.IsMove()){
            this.movePart.SetTargetDir(dirPos);
        }else{
            this.movePart.StartMove(dirPos);
        }
        if(this.standFrame>0){
           this.standFrame=0;
        }
    }
    private  void onJoyUp(object[] data=null){
        this.standFrame=0;
        this.movePart.StopMove();
    }

    /**
    * 切换动作 处理逻辑;
    */
    public override void executeSwichAction(){
        //移除监听.
        CameraManager.Instance.cameraCtrl.followTargetSmooth=50f;
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_Fall,onJumpFall);
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
        this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
        this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Up,onJoyUp);
        if(CameraManager.Instance.postLayer.enabled){
            CameraManager.Instance.cameraCtrl.ResetFOV();
            CameraManager.Instance.postLayer.enabled=false;
        }
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

