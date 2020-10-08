//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Jump : ActionBase
{
    private Vector3 pivotOffset = new Vector3(0.0f, 0.5f,  0.0f);
    private  Vector3 camOffset   = new Vector3(0.45f, 0.6f, -3.2f);    //-2.8f   -6.8f
    private Vector3 pivotFallOffset=new Vector3(0.0f, 1f,  0.0f);
    private Vector3 camFallOffset= new Vector3(0.45f, 0.6f, -2.4f);
    private int standFrame=0;
    private MovePart movePart;
    private Character character;
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
        character=this.obj as Character;
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
        bool isCamTarget=CameraManager.Instance.cameraCtrl.isCamTarget(character);
        if(isCamTarget){
            if(character.charData.aimState==GameEnum.AimState.Null){
                CameraManager.Instance.cameraCtrl.SetTargetOffsets(pivotOffset,camOffset);
                CameraManager.Instance.cameraCtrl.smooth=5;
            }
        }
        if(character.charData.isDashing){
             if(isCamTarget){
                if(character.charData.aimState==GameEnum.AimState.Null){
                    CameraManager.Instance.cameraCtrl.SetFOV(80);
                }
             }
       //     CameraManager.Instance.postLayer.enabled=true;
            this.obj.moveSpeed=character.charData.DashSpeed;
            this.movePart.speed=character.charData.DashSpeed; 
        }else{
            if(isCamTarget){
                if(character.charData.aimState==GameEnum.AimState.Null){
                    CameraManager.Instance.cameraCtrl.ResetFOV();
               }
            }
    ///       CameraManager.Instance.postLayer.enabled=false;
            this.obj.moveSpeed=character.charData.RunSpeed;
            this.movePart.speed=character.charData.RunSpeed;
        }
        if(character.charData.aimState!=GameEnum.AimState.Null){
            //DebugLog.Log("player");
            character.AniPlay(GameEnum.AniLabel.Down_Jmp_Base_A_Fall,0,0.333f,1f*this.speed,0.25f,3);
        }else{
           this.obj.GetAniBasePart().Play(GameEnum.AniLabel.Jmp_Base_A_Rise,frame*GameSettings.Instance.deltaTime,0.5f/this.speed,1.3f*this.speed,0.25f,1,true,true);
        }
        this.movePart.acceleratedupPow = this.movePart.GravityPower * this.speed * this.speed;
        // 8f 
        this.movePart.upPow = 10f * this.speed;
        this.movePart.ZeroUpStop=false;
        this.movePart.useWeightPower = false;
        this.movePart.Jump();
         character.SetCharacterCtrlHeight(1);
    }
    private void onJumpFall(object[] param=null) {
        //  DebugLog.Log("action >> onJumpFall.........");
        if(CameraManager.Instance.cameraCtrl.isCamTarget(character)){
            if(character.charData.aimState==GameEnum.AimState.Null){
                //平滑 跳跃缩放.
                 CameraManager.Instance.cameraCtrl.SetTargetOffsets(pivotFallOffset,camFallOffset);
            }
        }
         if(character.charData.aimState!=GameEnum.AimState.Null){
            character.AniPlay(GameEnum.AniLabel.Down_Jmp_Base_A_Fall,0.333f,0.333f, 1f*this.speed,0f,3);
         }else{
            this.obj.GetAniBasePart().Play(GameEnum.AniLabel.Jmp_Base_A_Fall,0,0.333f/this.speed, 1.3f*this.speed,0.25f,1,true,true);
         }
    }
    private void onJumpToGround(object[] param=null) {
        //  DebugLog.Log("action >> onJumpToGround.........");
        //Character character=this.obj as Character;
         character.SetCharacterCtrlHeight(1.8f);
         if(character.charData.aimState!=GameEnum.AimState.Null){
              character.AniPlay(GameEnum.AniLabel.DownIdle,0,0.1f,1f,0.8f,3);
         }
        Vector3 dirp=character.dirPos;
        if(dirp!=Vector3.zero){
            if(character.charData.aimState!=GameEnum.AimState.Null){
                this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Walk,0,true,new object[]{dirp});
            }else{
                //还在移动需要继续移动.
                if(character.charData.isDashing){
                    this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Dash,0,true,new object[]{dirp});
                }else{
                    this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Run,0,true,new object[]{dirp});
                }
            }
        }else{
            this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true,new object[]{GameEnum.AniLabel.Jmp_Base_A_OnGround,0.367f/this.speed, 0.7f*this.speed});
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
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_Fall,onJumpFall);
        this.obj.GetEvent().removeEventListener(CharEvent.Jump_To_Ground,onJumpToGround);
        this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
        this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Up,onJoyUp);
        character.SetCharacterCtrlHeight(1.8f);
         character.AniPlay(GameEnum.AniLabel.DownIdle,0,0.1f,1f,0.8f,3);
        if(CameraManager.Instance.cameraCtrl.isCamTarget(character)){
            if(character.charData.aimState==GameEnum.AimState.Null){
                CameraManager.Instance.cameraCtrl.ResetFOV();
                CameraManager.Instance.cameraCtrl.ResetTargetOffsets();
                CameraManager.Instance.cameraCtrl.smooth=10;
            }
        //     CameraManager.Instance.cameraCtrl.smoothVerAngle=10;
        }
   //    CameraManager.Instance.postLayer.enabled=false;

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
        this.character=null;
        base.onRecycle();
    }
    /*********
    销毁
    *******/
    public override void onRelease()
    {
        this.movePart=null;
        this.character=null;
        base.onRelease();
    }
}

