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
        this.onViewLoadFin();
    }

    //eg: initAvatar("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
    public void initAvatar(string aniUrl, string[] modelpaths){
        if(this.avatar=null)return; 
        this.avatar.Init(aniUrl,modelpaths);
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
        this.ChangeState(CharState.Char_Idle,null,false);
    }

    //回收.
     public override void onRecycle(){
        base.onRecycle();
     }
    public override void Release(){
        this.avatar=null;
        base.Release();
    }
}