


using System.Collections.Generic;

public class Char_Dead : State<Character>
{
    private Dictionary<string,bool> canActionDic=new Dictionary<string,bool>();
    private Character m_Owner = null;
    public Char_Dead(StateMachine<Character> machine)
        : base(machine)
    {
        m_nStateID = GameEnum.CharState.Char_Dead;
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
        //死亡不能进入多次.
        if(this.m_Statemachine.GetCurStateID() == m_nStateID ) return false;
        //其他所有状态都可以切死亡.
        return true;
    }
}

