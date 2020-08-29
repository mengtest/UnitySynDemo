


using System.Collections.Generic;
using GameEnum;
using UnityEngine;

public class Char_Idle : State<Character>
{
    private Dictionary<string,bool> canActionDic=new Dictionary<string,bool>();
    private Character _char = null;
    private  CharData charData=null;


    private int standFrame=0;

    private Vector3 _lastAngle=Vector3.zero;
   // private Vector3 worldDir=Vector3.zero;
    private float _lastSendTime=0;



    public Char_Idle(StateMachine<Character> machine)
        : base(machine)
    {
        m_nStateID = GameEnum.CharState.Char_Idle;
    }

    // 进入状态
    public override void Enter(object param)
    {
        _char = m_Statemachine.GetOwner();
        charData=_char.objData as CharData;
        charData.isNumb = false;
        charData.isLie = false;
        charData.lowFLy = false;
        charData.isStandUp = false;
    }

    // 退出状态
    public override void Leave()
    {
        _char = null;
    }

    public override void Update(float dt)
    {
        
    }

    public override void OnEvent(int cmd, object[] param=null)
    {
         switch(cmd){
            case GameEnum.ControllerCmd.OnJoy_Move:
               this.OnJoy_Move(param);
            break;
            case GameEnum.ControllerCmd.OnJoy_Up:
               this.OnJoyUp(param);
            break;
            case GameEnum.ControllerCmd.OnJump:
               this.OnJoyUp(param);
            break;
        }
    }
    private void OnJoy_Move(object[] data=null){
        Vector3 dirPos= (Vector3) data[0];
        bool isDash = (bool) data[1];
        bool canStop= (bool) data[2];
        if(canStop){
            if(charData.currentBaseAction==GameEnum.ActionLabel.Run){
                this.standFrame+=1;
                if(this.standFrame>=4){
                    //发送站立.
                    this.OnJoyUp();
                }
            }
            return;
        }
        if(charData.currentBaseAction==GameEnum.ActionLabel.Stand){
                this._lastAngle=Vector3.zero;
        }
        if(this._lastAngle!= Vector3.zero){
            //攻击中 重新定位Dir
            // if (charData.isAttacking || charData.currentActionType > 0 || charData.currentNotActionType > 0) {
            //     this._lastAngle=Vector2.zero;
            // }
        }
    //          DebugLog.Log(Time.time);
              //时间 先不要  && Time.time - this._lastSendTime >= 0.033f
        if(charData.currentBaseActionType == 0 && (this._lastAngle == Vector3.zero || dirPos!=this._lastAngle)){
            this._lastAngle=dirPos;
       //     DebugLog.Log("onJoystickMove: " + this._lastAngle);
            //派发事件 帧同步;
            
            // pos.rotate(CameraCtrl.Instance.cameraRotation/180*Math.PI,this.dirCamera);
            this._lastSendTime =Time.time;
            if (GameSettings.Instance.playMode == GameEnum.PlayMode.SingleMode ||GameSettings.Instance.playMode == GameEnum.PlayMode.ReplayMode ) {
                this._char.Do_JoyMove(dirPos,isDash);
            } else {
                CharData charD= this._char.objData as CharData;
                if (charD.currentBaseActionType > 0 || charD.currentBaseAction == GameEnum.ActionLabel.BackOff) {

                //     MGLog.l("move"); 发送移动事件.
                    ///  this.battleHandler.reportFrameCmdMoveMsg(
                }else{
                   this._char.Do_JoyMove(dirPos,isDash);
                }
                
            }
            if(this.standFrame>0){
                this.standFrame=0;
            }
        }
    }
     public void  OnJoyUp(object[] data=null){
        this._lastAngle=Vector2.zero;
        if (GameSettings.Instance.playMode == GameEnum.PlayMode.SingleMode ||GameSettings.Instance.playMode == GameEnum.PlayMode.ReplayMode ) {
             this._char.Do_JoyUp();
        } else {
            CharData charD= this._char.objData as CharData;
            if (charD.currentBaseActionType > 0   || charD.currentBaseAction == GameEnum.ActionLabel.BackOff) {

            }else{
               //移动对象。停止
              //  dd.x= (this.char.charData.position.x * 10000) | 0 ;
              //  dd.y= (this.char.charData.position.y * 10000) | 0 ;
                this._char.Do_JoyUp();
            }

            // let charLink = this.char.LinkObj();
            // if(charLink&&!charLink.isRecycled){
         //       console.log("send stop Move===============>",this.char.LinkObj().charData.pvpId );
             //   this.battleHandler.reportFrameCmdStopMoveMsg(charLink.charData.pvpId,dd.x,dd.y);
           // }

        }
        if(this.standFrame>0){
            this.standFrame=0;
        }
    }
     public void  OnJump(object[] data=null){
        if(charData.currentBaseAction==GameEnum.ActionLabel.Jump){
            //二段跳.
            return;
        }
        if (GameSettings.Instance.playMode == GameEnum.PlayMode.SingleMode ||GameSettings.Instance.playMode == GameEnum.PlayMode.ReplayMode ) {
             this._char.Do_Jump();
        } else {
            

        }
    }


    public override bool CanDoAction(string ActionLabel)
    {
        return true;
    }
    public override bool EnterStateChk(int nStateID)
    {
        //上一次的伤害表现数据.
       // m_Owner.lastAtkHitData=null;
        if(this.m_Statemachine.GetCurStateID()== m_nStateID ) return true;

        if (this.m_Statemachine.GetCurStateID() == GameEnum.CharState.Char_Dead) return false;

        if (charData.isSwoon)
        {
            if (this.m_Statemachine.GetCurStateID() != GameEnum.CharState.Char_Swoon){
                this.m_Statemachine.ChangeState(CharState.Char_Swoon,null,false);
            }
            return false;
        }
          //charData.Syn&&(
        if (this.m_Statemachine.GetCurStateID() != CharState.Char_Skill )
        {
           _char.GetEvent().send(CharEvent.Syn_NormalState, charData.pvpId);
        }        
        return true;
    }
}

