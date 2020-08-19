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
    public override void initData(){
        //每次初始化 应该重置data.防止 数据 没清空.
         CharData oldData=this.dataNode.GetComponent<CharData>();
         if(oldData!=null){
             GameObject.Destroy(oldData);
         }
        this.objData = this.dataNode.AddComponent<CharData>();
        this.objData.init(this,fixUpdate);
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

    //用的是 charData mono fixUpdate ;
    protected override void fixUpdate() {
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
    public override bool hasAni(int layer=0){
       switch(layer){
            case 0:
              return this.aniBasePart ==null ? false:true;
            case 1:
              return this.aniUpPart ==null ? false:true;
            case 2:
              return this.aniAddPart ==null ? false:true;
            default:
              return false;
        }
    }
    public override void pauseAni(int layer=0){
        switch(layer){
            case 0:
              this.GetAniBasePart().pause();
            break;
            case 1:
              this.GetAniUpPart().pause();
            break;
            case 2:
              this.GetAniAddPart().pause();
            break;
        }
    }
    public override void resumeAni(int layer=0){
        switch(layer){
            case 0:
              this.GetAniBasePart().resume();
            break;
            case 1:
              this.GetAniUpPart().resume();
            break;
            case 2:
              this.GetAniAddPart().resume();
            break;
        }
    }

    public bool doActionSkillByLabel(string actionLabel ,int frame=0,bool chkCancelLv=true,object[] param=null,int skillID=0){
           if(m_FSM==null) return false;
           if(!m_FSM.GetCurState().CanDoAction(actionLabel))return false;
           //技能部件 判断动作 skillPart.doActionSkillByLabel();
          // ActionManager.instance.GetAction(actionLabel);
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
    public override void onRelease(){
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
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