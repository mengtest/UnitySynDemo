using GameEnum;
using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Player : Character
{
    public AvatarChar avatar;
    public Player()
    {
         charType=GameEnum.ObjType.Player;
    }

    //重写显示.
    public override void init(){
         string[] split = poolname.Split('/');
        if(split.Length>0){
            this._name=split[split.Length-1]+this.id;
        }else{
            this._name=poolname+this.id;
        }
        this.node.name=this._name;
        this.avatar = this.node.AddComponent<AvatarChar>();
    }

    //eg: initAvatar("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
    public void initAvatar(string aniUrl, string[] modelpaths){
        if(this.avatar=null)return; 
        this.avatar.Init(aniUrl,modelpaths,onBodyFin);
    }
    public override void ChangeNodeObj(GameObject obj,bool resetPos=true){
        base.ChangeNodeObj(obj,resetPos);
        this.avatar = this.node.AddComponent<AvatarChar>();
    }
    //换装.
    public void ChangePart(string partPath){
        if(this.avatar=null)return; 
        this.avatar.ChangePart(partPath);
    }
    protected virtual void onBodyFin(){
        this.initViewFin=true;
        this.characterController=this.node.GetComponent<CharacterController>();
    } 
    protected override void initStateMachine(){
        m_FSM = new StateMachine<Character>(this);
        m_FSM.RegisterState(new Char_Idle(m_FSM));
        m_FSM.RegisterState(new Char_Squat(m_FSM));
        m_FSM.RegisterState(new Char_Prone(m_FSM));
        m_FSM.RegisterState(new Char_Skill(m_FSM));
        m_FSM.RegisterState(new Char_Hurt(m_FSM));
        m_FSM.RegisterState(new Char_HurtLie(m_FSM));
        m_FSM.RegisterState(new Char_HurtLinkBone(m_FSM)); //挂点中不可变羊.
        m_FSM.RegisterState(new Char_Polymorph(m_FSM));
        m_FSM.RegisterState(new Char_Dead(m_FSM));
    }
     public override void onGet(){
        base.onGet();
        this.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,false);
     }
    //回收.
     public override void onRecycle(){
        base.onRecycle();
     }
    public override void onRelease(){
        this.avatar=null;
        base.onRelease();
    }
}