


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
        _char.GetEvent().addEventListener(CharEvent.Begin_Fall,OnBeginFall);
        // _char.GetEvent().addEventListener(CharEvent.On_Select_Weapon,OnSelectWeapon);
        // _char.GetEvent().addEventListener(CharEvent.On_Drop_Weapon,OnDropWeapon);
        // _char.GetEvent().addEventListener(CharEvent.On_PickUp_Item,OnPickUpItem);
        // _char.GetEvent().addEventListener(CharEvent.On_Aim_Button,OnAimButton);
        // _char.GetEvent().addEventListener(CharEvent.On_Fire_Button,OnFireButton);
    }

    // 退出状态
    public override void Leave()
    {
        _char.GetEvent().removeEventListener(CharEvent.Begin_Fall,OnBeginFall);
        // _char.GetEvent().removeEventListener(CharEvent.On_Select_Weapon,OnSelectWeapon);
        // _char.GetEvent().removeEventListener(CharEvent.On_Drop_Weapon,OnDropWeapon);
        // _char.GetEvent().removeEventListener(CharEvent.On_PickUp_Item,OnPickUpItem);
        // _char.GetEvent().removeEventListener(CharEvent.On_Aim_Button,OnAimButton);
        // _char.GetEvent().removeEventListener(CharEvent.On_Fire_Button,OnFireButton);
        _char = null;
    }

    public override void Update(float dt)
    {

    }

    public override void OnEvent(string cmd, object[] param=null)
    {
        
         switch(cmd){
            // case CharEvent.OnJoy_Move:
               
            // break;
            case CharEvent.On_KeyState:
               if(param==null){
                   return;
               }
               string keyType =(string)param[0];
               switch(keyType){
                   case GameEnum.KeyInput.Jump:
                       this.OnJump();  
                   break;
                   case GameEnum.KeyInput.SelectWeapon_1:
                       this.OnSelectWeapon(1);  
                   break;
                   case GameEnum.KeyInput.SelectWeapon_2:
                       this.OnSelectWeapon(2);  
                   break;
                   case GameEnum.KeyInput.DropItem:
                       //0 使用中武器, 1 武器_1 2 武器_2
                       int select=0;
                       if(param.Length>1){
                          select =(int)param[1];
                       }
                       this.OnDropWeapon(select); 
                   break;
                   case GameEnum.KeyInput.PickUp:
                       this.OnPickUpItem();  
                   break;
                   case GameEnum.KeyInput.Aim:
                       bool isDown=true;
                       if(param.Length>1){
                          isDown =(bool)param[1];
                       }
                       this.OnAimButton(isDown);  
                   break;
                   case GameEnum.KeyInput.Attack:
                       isDown=true;
                       if(param.Length>1){
                          isDown =(bool)param[1];
                       }
                       this.OnFireButton(isDown);  
                   break;
               }
            break;
        }
    }
    // private void OnJoy_Move(object[] data=null){
    //     Vector3 dirPos= (Vector3) data[0];
    //     bool isDash = (bool) data[1];
    //     bool canStop= (bool) data[2];
    //     if(canStop){
    //         if(charData.currentBaseAction==GameEnum.ActionLabel.Run){
    //             this.standFrame+=1;
    //             if(this.standFrame>=4){
    //                 //发送站立.
    //                 this.OnJoyUp();
    //             }
    //         }
    //         return;
    //     }
    //     if(charData.currentBaseAction==GameEnum.ActionLabel.Stand){
    //             this._lastAngle=Vector3.zero;
    //     }
    //     if(this._lastAngle!= Vector3.zero){
    //         //攻击中 重新定位Dir
    //         // if (charData.isAttacking || charData.currentActionType > 0 || charData.currentNotActionType > 0) {
    //         //     this._lastAngle=Vector2.zero;
    //         // }
    //     }
    // //          DebugLog.Log(Time.time);
    //           //时间 先不要  && Time.time - this._lastSendTime >= 0.033f
    //     if(charData.currentBaseActionType == 0 && (this._lastAngle == Vector3.zero || dirPos!=this._lastAngle)){
    //         this._lastAngle=dirPos;
    //    //     DebugLog.Log("onJoystickMove: " + this._lastAngle);
    //         //派发事件 帧同步;
            
    //         // pos.rotate(CameraCtrl.Instance.cameraRotation/180*Math.PI,this.dirCamera);
    //         this._lastSendTime =Time.time;
    //         if (GameSettings.Instance.playMode == GameEnum.PlayMode.SingleMode ||GameSettings.Instance.playMode == GameEnum.PlayMode.ReplayMode ) {
    //             this._char.On_JoyMove(dirPos,isDash);
    //         } else {
    //             CharData charD= this._char.objData as CharData;
    //             if (charD.currentBaseActionType > 0 || charD.currentBaseAction == GameEnum.ActionLabel.BackOff) {

    //             //     MGLog.l("move"); 发送移动事件.
    //                 ///  this.battleHandler.reportFrameCmdMoveMsg(
    //             }else{
    //                this._char.On_JoyMove(dirPos,isDash);
    //             }
                
    //         }
    //         if(this.standFrame>0){
    //             this.standFrame=0;
    //         }
    //     }
    // }
    //  public void  OnJoyUp(object[] data=null){
    //     this._lastAngle=Vector2.zero;
    //     if (GameSettings.Instance.playMode == GameEnum.PlayMode.SingleMode ||GameSettings.Instance.playMode == GameEnum.PlayMode.ReplayMode ) {
    //          this._char.On_JoyUp();
    //     } else {
    //         CharData charD= this._char.objData as CharData;
    //         if (charD.currentBaseActionType > 0   || charD.currentBaseAction == GameEnum.ActionLabel.BackOff) {

    //         }else{
    //            //移动对象。停止
    //           //  dd.x= (this.char.charData.position.x * 10000) | 0 ;
    //           //  dd.y= (this.char.charData.position.y * 10000) | 0 ;
    //             this._char.On_JoyUp();
    //         }

    //         // let charLink = this.char.LinkObj();
    //         // if(charLink&&!charLink.isRecycled){
    //      //       console.log("send stop Move===============>",this.char.LinkObj().charData.pvpId );
    //          //   this.battleHandler.reportFrameCmdStopMoveMsg(charLink.charData.pvpId,dd.x,dd.y);
    //        // }

    //     }
    //     if(this.standFrame>0){
    //         this.standFrame=0;
    //     }
    // }
    private void OnPickUpItem(){
       (this._char as Player).PickUpNearItem();
    }
    private void OnAimButton(bool isDown){
      // bool bol =(bool)data[0];
       Player player=this._char as Player;
       player.charData.Btn_Aim=isDown;
       if(isDown){
            if(player.weaponSystem.hasActiveWeapon()&&charData.currentUpLayerAction!=GameEnum.ActionLabel.Aiming){
                    player.doActionSkillByLabel(GameEnum.ActionLabel.Aiming);
            }
       }
    }
    private void OnFireButton(bool isDown){
     //    bool bol =(bool)data[0];
       this._char.charData.Btn_Fire=isDown;
    }
    private void OnDropWeapon(int select){
        if(charData.currentUpLayerAction==GameEnum.ActionLabel.Aiming){
           this._char.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle);
        }
       (this._char as Player).weaponSystem.DropWeapon(select);
    }
    private void OnSelectWeapon(int select){
        WeaponSystem weaponSystem=  (this._char as Player).weaponSystem;
        if(select!=weaponSystem.UseActiveSide){
            // if(charData.currentUpLayerAction==GameEnum.ActionLabel.Aiming){
            //     this._char.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle);
            // }
            //播放 换武器动作.
            this._char.doActionSkillByLabel(GameEnum.ActionLabel.ChangeWeapon);
        }
        weaponSystem.UseWeaponSide(select);
    }
    private void OnBeginFall(object[] data){
         if(charData.currentBaseAction!=GameEnum.ActionLabel.Jump){
             this._char.doActionSkillByLabel(GameEnum.ActionLabel.Jump,0,true);
         }
    }
    public void  OnJump(){
        if(charData.currentBaseAction==GameEnum.ActionLabel.Jump){
            //二段跳.
            return;
        }
        this._char.doActionSkillByLabel(GameEnum.ActionLabel.Jump,0,true);
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

