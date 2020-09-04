using UnityEngine;
using GameEnum;
/****
角色基类
****/
[AutoRegistLua]
public class Character : ObjBase
{
    protected Controller ctrl=null;
    //1  Uplayer层 动画控件 上半身动作层
    protected AniPart aniUpPart=null;
    //2  Uplayer层 动画控件 叠加动作层
    protected AniPart aniAddPart=null;
    //baseLayer  整体动作.
    protected StateMachine<Character> m_FSM = null;
    protected SkillPart skillPart=null;
    public CharData charData=null;

 // Collider extents for ground test. 
    protected Vector3 colExtents=Vector3.zero; 
    protected CharacterController characterController=null;

    public   Vector3 dirPos{  get ; private set ; }

    // public float faceRotateSpeed=7;
    // public bool faceToRotation=false;
    // private Vector3 _moveRoate;
    // private Vector3 _targetDir;

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
             GameObject.DestroyImmediate(oldData);
         }
        this.objData = this.dataNode.AddComponent<CharData>();
        this.charData=this.objData as CharData;
        this.objData.init(this,fixUpdate);
        this.ChangeState(CharState.Char_Idle,null,false);
        this.GetMovePart().useGravityPower=true;
        //自己转向.
         this._move.isRotateLessSpeed=true;
        // this._move.rotateSpeed=18;
        // this._move.faceToRotation=false;
        // this._move.rotateSpeed=0;
        // this._move.Init();

    }
    //状态机初始化  不同状态怪物 可以初始化 不同的状态机.
    protected virtual void initStateMachine(){
        m_FSM = new StateMachine<Character>(this);
        m_FSM.RegisterState(new Char_Idle(m_FSM));
    }
    public void ChangeState(int charState, object param=null,bool checkDic=true)
    {
        m_FSM.ChangeState(charState, param,checkDic);
    }
    public void OnEvent(string cmd, object[] param=null){
        switch(cmd){
            case CharEvent.OnJoy_Move:
                dirPos= (Vector3) param[0];
            break;
            case CharEvent.OnJoy_Up:
                dirPos= Vector3.zero; // this.gameObject.transform.forward;
            break;
        }
        this.GetEvent().dispatchEvent(cmd,param);
        m_FSM.OnEvent(cmd,param);
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
            return charData.ctrlType;
        }
        set {
           if(charData.ctrlType!=value){
               charData.ctrlType=value;
               if(ctrl!=null){
                   ctrl.recycleSelf();
               }
               ctrl=CtrlManager.Instance.getController(charData.ctrlType);
               if(ctrl!=null){
                 ctrl.init(this);
               }
           }       
        }
    }
    public override void onViewLoadFin(){
        this.characterController=this.node.GetComponent<CharacterController>();
        this.colExtents=this.characterController.bounds.extents;
     //   DebugLog.Log(this.colExtents);
      //移动专用方法.
    }
    public override void OnMove(Vector3 dic){
        if(this.characterController!=null){
           this.characterController.Move(dic);
        }else{
            this.node.transform.position =  this.node.transform.position + dic;
        }
        // if(this.faceToRotation){
        //     if(this._move.forwardDirection!=this.gameObject.transform.forward){
        //         this._moveRoate=this._move.forwardDirection-this.gameObject.transform.forward;
        //         if (this._moveRoate.magnitude< 0.01f) {
        //             //最新方向;
        //             this.gameObject.transform.forward=this._move.forwardDirection;
        //         } else {
        //             this._targetDir = this._move.forwardDirection;
        //             if(this._moveRoate.magnitude>=2){
        //                 //180°不能直接相减
        //             this._targetDir = Quaternion.AngleAxis(1, Vector3.up) * this._move.forwardDirection;
        //             }
        //             this._moveRoate=this._targetDir*(faceRotateSpeed * Time.deltaTime);
        //             //最新方向;
        //             this._targetDir =  this.gameObject.transform.forward+this._moveRoate;
        //             this._targetDir.Normalize();
        //             this.gameObject.transform.forward=this._targetDir;
        //         }
        //     }
        // }
    }
    // public virtual void FaceToDir(Vector3 dir){
    //      this.gameObject.transform.forward=dir;
    // }
    public override bool IsGrounded()
	{
        if(this.colExtents!=Vector3.zero){
            //两倍半径高度 向下打射线;
            Ray ray = new Ray(this.gameObject.transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);
            return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.1f);
        }else{
             return base.IsGrounded();
        }
	}

    //用的是 charData mono fixUpdate ;
    protected override void fixUpdate() {
        if(this.skillPart!=null){
            this.skillPart.Update();
        }
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
            this.aniUpPart.Init(this,GameEnum.ActionLayer.UpLayer);
        }
        return this.aniUpPart;
    }
      public AniPart GetAniAddPart(){
        if(this.aniAddPart==null){
            this.aniAddPart=new AniPart();
            this.aniAddPart.Init(this,GameEnum.ActionLayer.AddLayer);
        }
        return this.aniAddPart;
    }
    public override bool hasAni(int layer=0){
       switch(layer){
            case GameEnum.ActionLayer.BaseLayer:
              return this.aniBasePart ==null ? false:true;
            case GameEnum.ActionLayer.UpLayer:
              return this.aniUpPart ==null ? false:true;
            case GameEnum.ActionLayer.AddLayer:
              return this.aniAddPart ==null ? false:true;
            default:
              return false;
        }
    }
    public override void pauseAni(int layer=0){
        switch(layer){
            case GameEnum.ActionLayer.BaseLayer:
              this.GetAniBasePart().pause();
            break;
            case GameEnum.ActionLayer.UpLayer:
              this.GetAniUpPart().pause();
            break;
            case GameEnum.ActionLayer.AddLayer:
              this.GetAniAddPart().pause();
            break;
        }
    }
    public override void resumeAni(int layer=0){
        switch(layer){
            case GameEnum.ActionLayer.BaseLayer:
              this.GetAniBasePart().resume();
            break;
            case  GameEnum.ActionLayer.UpLayer:
              this.GetAniUpPart().resume();
            break;
            case GameEnum.ActionLayer.AddLayer:
              this.GetAniAddPart().resume();
            break;
        }
    }
    public SkillPart GetSkillPart(){
        if(this.skillPart==null){
            this.skillPart=new SkillPart();
            this.skillPart.Init(this);
        }
        return this.skillPart;
    }

    //................................................................................  
     #region 角色 所有动作命令..................
    public void On_Jump(){
        // if(charData.currentBaseAction!=GameEnum.ActionLabel.Jump){
        //        this.doActionSkillByLabel(GameEnum.ActionLabel.Jump);
        // }
    }
     
    #endregion 
    //...............................................................................  
   

    public override bool doActionSkillByLabel(string actionLabel ,int frame=0,bool chkCancelLv=true,object[] param=null,int skillID=0){
           if(m_FSM==null) return false;
           if(!m_FSM.GetCurState().CanDoAction(actionLabel))return false;
          // ActionManager.instance.GetAction(actionLabel);
           return this.GetSkillPart().doActionSkillByLabel(actionLabel,frame,chkCancelLv,param,skillID);
    }

    //回收.
     public override void onRecycle(){
         charData.ctrlType=CtrlType.Null;
         this.dirPos=Vector3.zero;
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
        if(this.skillPart!=null){
            this.skillPart.Reset();
        }
        if (m_FSM != null)
        {
            this.ChangeState(CharState.Char_Idle,null,false);
        }
        base.onRecycle();
     }
    public override void onRelease(){
        this.charData=null;
        this.characterController=null;
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
         }
         if(this.skillPart!=null){
            this.skillPart.Release();
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