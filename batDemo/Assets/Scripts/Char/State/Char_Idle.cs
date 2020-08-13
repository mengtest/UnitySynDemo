


using System.Collections.Generic;
using GameEnum;

public class Char_Idle : State<Character>
{
    private Dictionary<string,bool> canActionDic=new Dictionary<string,bool>();
    private Character m_Owner = null;
    private  CharData charData=null;
    public Char_Idle(StateMachine<Character> machine)
        : base(machine)
    {
        m_nStateID = GameEnum.CharState.Char_Idle;
    }

    // 进入状态
    public override void Enter(object param)
    {
        m_Owner = m_Statemachine.GetOwner();
        charData=m_Owner.objData as CharData;
        charData.isNumb = false;
        charData.isLie = false;
        charData.lowFLy = false;
        charData.isStandUp = false;
    }

    // 退出状态
    public override void Leave()
    {
        m_Owner = null;
    }

    public override void Update(float dt)
    {
        
    }

    public override void OnEvent(int nEventID, object param)
    {
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
           m_Owner.GetEvent().send(CharEvent.Syn_NormalState, charData.pvpId);
        }        
        return true;
    }
}

