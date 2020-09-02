


using System.Collections.Generic;

public class Char_Squat : State<Character>
{
    private Dictionary<string,bool> canActionDic=new Dictionary<string,bool>();
    private Character m_Owner = null;
    public Char_Squat(StateMachine<Character> machine)
        : base(machine)
    {
        m_nStateID = GameEnum.CharState.Char_Squat;
    }

    // 进入状态
    public override void Enter(object param)
    {
        m_Owner = m_Statemachine.GetOwner();
        CharData charData=m_Owner.objData as CharData;

    }

    // 退出状态
    public override void Leave()
    {
        m_Owner = null;
    }

    public override void Update(float dt)
    {
        
    }

    public override void OnEvent(string nEventID, object[] param=null)
    {
    }

    public override bool CanDoAction(string ActionLabel)
    {
        return true;
    }
    public override bool EnterStateChk(int nStateID)
    {
        if(this.m_Statemachine.GetCurStateID()== m_nStateID ) return true;

        if (this.m_Statemachine.GetCurStateID() == GameEnum.CharState.Char_Dead) return false;
        
        return true;
    }
}

