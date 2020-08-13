using UnityEngine;
using GameEnum;
/****
角色基类
****/
[AutoRegistLua]
public class Character : ObjBase
{
    private CtrlType _ctrlType=CtrlType.Null;
    protected Controller ctrl=null;
    //baseLayer  整体动作.
    protected StateMachine<Character> m_FSM = null;
    public Character()
    {
        this.charType=ObjType.Character;
        this.needUpdate=false;
        initStateMachine();
    }
    //状态机初始化  不同状态怪物 可以初始化 不同的状态机.
    protected virtual void initStateMachine(){
        m_FSM = new StateMachine<Character>(this);
        m_FSM.RegisterState(new Char_Idle(m_FSM));
        this.ChangeState(CharState.Char_Idle,null,false);
    }
    protected void ChangeState(int charState, object param=null,bool checkDic=true)
    {
        m_FSM.ChangeState(charState, param,checkDic);
    }
    public int GetCurStateID()
    {
        if (m_FSM != null)
        {
            return  m_FSM.GetCurStateID();
        }
        return CharState.Char_Dead;
    }
    public CtrlType ctrlType{ 
        get{ 
            return _ctrlType;
        }
        set {
           if(_ctrlType!=value){
               _ctrlType=value;
               if(ctrl!=null){
                   ctrl.recycleSelf();
               }
               ctrl=CtrlManager.Instance.getController(this._ctrlType);
               if(ctrl!=null){
                 ctrl.init(this);
               }
           }       
        }
    }
    public override void onViewLoadFin(){
        objData = this.node.GetComponent<CharData>();
        if(objData==null){
            DebugLog.LogError("no charData mono ->",this.poolname);
        }else{
            objData.init(this,onFixUpdate);
        }
    }
    //mono 各自的updtate;
    public virtual void onFixUpdate() {
       if(this._move!=null){
            this._move.fixUpdate();
        }
    }
    //获取控制器.
    public Controller GetCtrl(){
        return this.ctrl;
    }

    public bool doActionSkillByLabel(string actionLabel ,int frame=0,bool chkCancelLv=true,object[] param=null,int skillID=0){
           if(m_FSM==null) return false;
           if(!m_FSM.GetCurState().CanDoAction(actionLabel))return false;
           //技能部件 判断动作 skillPart.doActionSkillByLabel();
           return true;
    }

    //回收.
     public override void onRecycle(){
         _ctrlType=CtrlType.Null;
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
         }
        if (m_FSM != null)
        {
            this.ChangeState(CharState.Char_Idle,null,false);
        }
        base.onRecycle();
     }
    public override void Release(){
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
         }
        if (m_FSM != null)
        {
            m_FSM.UnRegisterAllState();
            m_FSM = null;
        }
        base.Release();
    }
}