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
    //0  layer层 动画控件 整体全身动作层
    protected AniPart aniBasePart=null;
    //1  Uplayer层 动画控件 上半身动作层
    protected AniPart aniUpPart=null;
    //2  Uplayer层 动画控件 叠加动作层
    protected AniPart aniAddPart=null;

    //baseLayer  整体动作.
    protected StateMachine<Character> m_FSM = null;
    public Character()
    {
       // this.charType=ObjType.Character;
       /// this.needUpdate=false;
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
    //用的是 objData mono fixUpdate ;
    public virtual void onFixUpdate() {
       if(this._move!=null){
            this._move.fixUpdate();
        }
        if(this.aniBasePart!=null){
             this.aniBasePart.fixUpdate();
        }
        if(this.aniUpPart!=null){
             this.aniUpPart.fixUpdate();
        }
        if(this.aniAddPart!=null){
             this.aniAddPart.fixUpdate();
        }
    }
    //获取控制器.
    public Controller GetCtrl(){
        return this.ctrl;
    }

    public AniPart GetAniBasePart(){
        if(this.aniBasePart==null){
            this.aniBasePart=new AniPart();
            this.aniBasePart.Init(this,0);
        }
        return this.aniBasePart;
    }
      public AniPart GetAniUpPart(){
        if(this.aniUpPart==null){
            this.aniUpPart=new AniPart();
            this.aniUpPart.Init(this,1);
        }
        return this.aniUpPart;
    }
      public AniPart GetAniAddPart(){
        if(this.aniAddPart==null){
            this.aniAddPart=new AniPart();
            this.aniAddPart.Init(this,2);
        }
        return this.aniAddPart;
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
        if(this.aniBasePart!=null){
             this.aniBasePart.stop();
        }
        if(this.aniUpPart!=null){
             this.aniUpPart.stop();
        }
        if(this.aniAddPart!=null){
             this.aniAddPart.stop();
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
        if(this.aniBasePart!=null){
             this.aniBasePart.Release();
             this.aniBasePart=null;
        }
        if(this.aniUpPart!=null){
              this.aniUpPart.Release();
             this.aniUpPart=null;
        }
        if(this.aniAddPart!=null){
             this.aniAddPart.Release();
             this.aniAddPart=null;
        }
        if (m_FSM != null)
        {
            m_FSM.UnRegisterAllState();
            m_FSM = null;
        }
        base.Release();
    }
}