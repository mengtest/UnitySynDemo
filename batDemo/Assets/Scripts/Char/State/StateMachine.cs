//*************************************************************************
//	创建日期:	2015-7-4   12:11
//	文件名称:	statemachine.cs
//  创 建 人:   Even	
//	版权所有:	星辰时代
//	说    明:	状态机
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Text;


    // 状态接口
    public abstract class State<T>
    {
        protected StateMachine<T> m_Statemachine  = null;
        protected int m_nStateID = 0;

         public State(StateMachine<T> machine)
        {
            m_Statemachine = machine;
        }
        //改变状态检测.是否能改变该状态.
        public abstract bool EnterStateChk(int nStateID);
        // 进入状态
        public abstract void Enter(object param);
        
        // 退出状态
        public abstract void Leave();

        public abstract void Update(float dt) ;

        public abstract void OnEvent(int nEventID, object param);

        public abstract  bool CanDoAction(string ActionLabel);

        public virtual int GetStateID() { return m_nStateID; }
    }

    // 状态机
    public class StateMachine<T>
    {
        // 状态列表 不允许重复状态
        private Dictionary<int, State<T>> m_dicState = new Dictionary<int,State<T>>();

        private T m_Owner = default(T);  // 状态所有者

        // 当前状态
        private State<T> m_curState = null;
        public int BeforeStateId = 0;
        public StateMachine(T owner)
        {
            m_Owner = owner;
        }
        
        public void RegisterState(State<T> s,object[] nextStateIds=null)
        {
            if (!m_dicState.ContainsKey(s.GetStateID()))
            {
                m_dicState.Add(s.GetStateID(), s);
            }
        }

        public void UnRegisterState(int nStateID)
        {
            if (m_dicState.ContainsKey(nStateID))
            {
                m_dicState.Remove(nStateID);
            }
        }

        public void UnRegisterAllState()
        {
            m_dicState.Clear();
        }

        public T GetOwner() { return m_Owner; }

        public State<T> GetCurState()
        {
            return m_curState;
        }

        public int GetCurStateID()
        {
            if( m_curState != null )
            {
                return m_curState.GetStateID();
            }

            return -1;
        }
        //外部接口
         public void ChangeState(int nStateID, object param=null,bool EnterCheck=true){
             if(EnterCheck){
                 if(EnterStateChk(nStateID)){
                      this.onChangeState(nStateID,param);
                 }
             }else{
                 this.onChangeState(nStateID,param);
             }
         }
         
        //状态机内部用.
        private void onChangeState(int nStateID, object param=null)
        {
            State<T> tarState = null;
            if (m_dicState.TryGetValue(nStateID, out tarState))
            {

                if(tarState != null){
                    if( m_curState != null )
                    {
                        if(m_curState.GetStateID()==nStateID){
                            //相同状态.
                            m_curState.Leave();
                            m_curState.Enter(param);
                            return ;
                        }
                        m_curState.Leave();
                        BeforeStateId = m_curState.GetStateID();   
                    }
                    //State s;
                    //m_dicState.TryGetValue(nStateID, out s);
                    m_curState = tarState;
                    m_curState.Enter(param);
                }
            }
            else
            {
                // 状态不支持
            }
        }
        public void Update(float dt)
        {
            if( m_curState != null )
            {
                m_curState.Update(dt);
            }
        }

        public void OnEvent(int nEventID, object param)
        {
            if (m_curState != null)
            {
                m_curState.OnEvent(nEventID, param);
            }
        }
        public bool EnterStateChk(int nStateID)
        {
            State<T> tarState = null;
            if (m_dicState.TryGetValue(nStateID, out tarState))
            {
                  return tarState.EnterStateChk(nStateID);
            }
            return false;
        }
    }

