﻿


using System.Collections.Generic;

public class Char_Idle : State<Character>
{
    private Dictionary<string,bool> canActionDic=new Dictionary<string,bool>();
    private Character m_Owner = null;
    public Char_Idle(StateMachine<Character> machine)
        : base(machine)
    {
        m_nStateID = (int)GameEnum.CharState.Char_Idle;
    }

    // 进入状态
    public override void Enter(object param)
    {
        m_Owner = m_Statemachine.GetOwner();
        CharData charData=m_Owner.objData as CharData;
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
}

