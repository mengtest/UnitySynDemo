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
        //可切换状态数组;
        private Dictionary<int,Dictionary<int,bool>> m_dicCanCgState =new Dictionary<int,Dictionary<int,bool>> ();

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
            if (!m_dicCanCgState.ContainsKey(s.GetStateID()))
            {
                if(nextStateIds!=null){
                    Dictionary<int, bool> dic=new Dictionary<int, bool>();
                    m_dicCanCgState.Add(s.GetStateID(), dic);
                    for (int i = 0; i < nextStateIds.Length; i++)
                    {
                        dic[(int)nextStateIds[i]]=true;
                    }
                }
            }
        }

        public void UnRegisterState(int nStateID)
        {
            if (m_dicState.ContainsKey(nStateID))
            {
                m_dicState.Remove(nStateID);
            }
            if (m_dicCanCgState.ContainsKey(nStateID))
            {
                m_dicCanCgState.Remove(nStateID);
            }
        }

        public void UnRegisterAllState()
        {
            m_dicState.Clear();
            m_dicCanCgState.Clear();
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

        // 添加状态
        public bool ChangeState(int nStateID, object param=null,bool checkDic=true)
        {
            State<T> tarState = null;
            if (m_dicState.TryGetValue(nStateID, out tarState))
            {
                bool canChange=false;
                if( m_curState != null )
                {
                    if(m_curState.GetStateID()==nStateID){
                        //相同状态.
                        m_curState.Leave();
                        m_curState.Enter(param);
                       return true;
                    }
                    Dictionary<int,bool> changeStateDic=null;   
                    //当前状态 获取 可切换状态列表
                    if (m_dicCanCgState.TryGetValue(m_curState.GetStateID(), out changeStateDic))
                    {
                        if(changeStateDic!=null&&checkDic){
                          //条件切换
                            changeStateDic.TryGetValue(nStateID, out canChange);
                        }else{
                           //可任意切换.
                           canChange=true;
                        }
                    }else{
                        canChange=true;
                    }
                    if(canChange){
                        m_curState.Leave();
                        BeforeStateId = m_curState.GetStateID();
                    }else{
                        //不能转换.
                        return canChange;
                    }    
                }else{
                    canChange=true;
                }

                //State s;
                //m_dicState.TryGetValue(nStateID, out s);
                if(canChange&&tarState != null){
                    m_curState = tarState;
                    m_curState.Enter(param);
                }
                return canChange;
            }
            else
            {
                // 状态不支持
                return false;
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

    }

