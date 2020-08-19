//*************************************************************************
//	技能部件
//*************************************************************************

//技能部件
using System.Collections.Generic;
using UnityEngine;

public class CmdAction: ActionBase
{
    //动作在SkillActionPool 中 存储的名称.. nan1@Skill_01 之类.
    public string name;
    private ActionData actData;
    private string lastActionName;

    /**脚本 动画  播放速度 */
    public  float aniCmdSpeed = 1;
    //动画融合
    public float fadeLength = 0.15f;

    // /* 目前游戏用不到连击combo 先不实现; 
    // * 当切换到此action时执行的功能,每个action只会在刚切入此action时执行一次
    // */
    // public List<AFC_Base_Cmd> startActionFunctions;
    // /*
    // * 此action自然结束时执行的功能
    // */
    // public  List<AFC_Base_Cmd> endActionFunctions ;

    /*
    * 切出此action时执行的功能，自然结束和强制跳转都会触发
    * 这个列表一般不需要手工维护，如果需要添加项目，注意其中不可以有ActFunctionType.TO_ACTION 跳转功能，否则会引起堆栈上溢
    */
    private List<AFC_Base_Cmd> switchActionFunctions;
    /*
    * 每一帧对应一个功能列表，当前帧索引的列表可能为空，如果不为空则遍历这个子列表实施每一项设置的功能
    */
    //public Dictionary<number, List<AFC_Base_Cmd>> frameFunctionsMap;

    private List<AFC_Base_Cmd> frameFunctionList;

    /* 
    *  目前游戏用不到连击combo 先不实现; 
    关注的comboRespond
    */
    // public attentionComboResponds:Array<ComboRespond>  = new Array<ComboRespond>();

    //需要更新的cmd
    // public updataCmdList: Array<AFC_Base_Cmd> = new Array<AFC_Base_Cmd>();

    //是否是刚跳转到Action
    protected bool _justJumpAction = false;

    public override void init(){
        this.name=this.poolname;
        this.actData = ActionManager.instance.GetActionData(this.poolname);
        this.totalFrame = Mathf.RoundToInt (this.actData.length /  GameSettings.Instance.deltaTime);
        this.lastActionName = this.actData.actionName;
        this.isLoop = this.actData.loop;
        //解析完ActionData 后 this.actionType赋值。

        //  this.startActionFunctions =new List<AFC_Base_Cmd>();
        //  this.endActionFunctions = new List<AFC_Base_Cmd>();
        this.switchActionFunctions = new List<AFC_Base_Cmd>();
       // this.frameFunctionsMap = new Map<number, Array<AFC_Base_Cmd>>();
        this.frameFunctionList=new List<AFC_Base_Cmd>();

        for (int i = 0; i < this.actData.cmdDataList.Count; i++) {

            AFC_Base_Data data=this.actData.cmdDataList[i];
            AFC_Base_Cmd cmd  = AFC_Manager.getAFC(data.eventName);
            if (cmd!=null) {
                cmd.init(data,this);
                this.AddActionFunction(cmd);
                if (cmd.hasSwitchFun) {
                    this.switchActionFunctions.Add(cmd);
                }
                // if (cmd.needUpdate) {
                //     this.addCmdUpdate(cmd);
                // }
            }
        }
    }
    //添加帧功能添加到到指定帧
    public void AddActionFunction(AFC_Base_Cmd afc) {
        this.frameFunctionList.Add(afc);
    }
    public override void Init(ObjBase obj) {
            base.Init(obj);
            this.Reset();
    }
     public void Reset() {
         int len = this.frameFunctionList.Count;
        for (int index = 0; index < len; index++) {
            this.frameFunctionList[index].Reset();
        }
        this.Resume();
    }
      //更新;
    public override void Update() {
         //累算经过帧数
         if (!this._pause) {
            this.currentFrame += this.speed;
            //*Core.deltaTime*Core.FrameRate;
        }
        if(this._justJumpAction){
            this._justJumpAction=false;
        }else{
            this.checkAndRunFrameFunction();
        }
        //     this.checkCmdUpdate();
        this.checkLoop();
    }
     private void checkLoop() {
        if (this.currentFrame >= this.totalFrame) {
            if (this.isLoop) {
                this.currentFrame = -this.speed;
                this._previousFrameIndex = -1;
                //目前只有更新列表有fin判斷 只重置更新列表。
                int len = this.frameFunctionList.Count;
                for (int i = 0; i < len; i++) {
   //                 if(this.frameFunctionList[i].needUpdate){
                        this.frameFunctionList[i].updateFinished = false;
  //                  }
                }
            }
            // if (this.endActionFunctions.length > 0)
            // {
            //     this.executeActionFunctionCommand(this.endActionFunctions);
            // }
        }
    }
    /************************************************************************/
    /*检测执行帧方法                                                        */
    /************************************************************************/
    private void checkAndRunFrameFunction() {
        if (this.currentFrame > this.totalFrame) {
            this.currentFrame = this.totalFrame;
        }
        int runCount = this.isLoop ? Mathf.FloorToInt((this.currentFrame % this.totalFrame) - this._previousFrameIndex) : Mathf.FloorToInt(this.currentFrame - this._previousFrameIndex);
        //遍历经过的可运行的帧数
        if(runCount>0){
            int recordPreviousFrameIndex = this._previousFrameIndex;
            for (int i = 0; i < runCount; i++) {
                this._previousFrameIndex = recordPreviousFrameIndex + i + 1;
                //如果包含对应帧索引的方法
                this.executeCmd(this._previousFrameIndex);
            }
        }
    }
      /**
    * 执行ActFunction列表
    * @param idx 第几帧
    *
    */
    private void executeCmd(int idx) {
        int len = this.frameFunctionList.Count;
        for (int i = 0; i < len; i++) {
            AFC_Base_Cmd cmd = this.frameFunctionList[i];
            if(this.isRecycled){
                return;
            }
            if(cmd.FrameId()==idx){
                //如果是运行一次的
                if (cmd.onceEvent) {
                    if (!cmd.executed) {
                        cmd.execute();
                        cmd.executed = true;
                    }
                }
                else {
                    cmd.execute();
                }
            }
            if(cmd.needUpdate){
                if (cmd.updateFinished) {
                    continue;
                }
                if (this._previousFrameIndex >= cmd.FrameId() && this._previousFrameIndex <= cmd.MaxFrame()) {
                    cmd.update(this._previousFrameIndex);
                }
            }
        }
    }
    /**
 * 
 * @param frame 
 * @param param param:number previousFrame 从第几帧开始计算帧 
 */
    public override void Begin(int frame=0,object param=null) {
        //   this. cancelPriorityLimit = -1;
        //帧计数归0
        frame = frame == -1 ? 0 : frame;
       this.GotoFrame(frame,param);
    } 
  /**
    * 正常从第0帧播放， 代表 跳转到制定帧 连同开始帧一起计算，计算到当前帧逻辑。
    * @param frame 从第几帧开始执行
    * @param param 参数 number previousFrame 从第几帧开始计算帧 
    */
    public override void GotoFrame(int frame=0,object param=null) {
        this.currentFrame = frame;
        //执行action初始方法
        //  this.executeActionFunctionCommand(this.startActionFunctions);
        if(param!=null){
            this._previousFrameIndex = (int)param-1;
        }else{
            this._previousFrameIndex = frame-1;
        }

        this.checkAndRunFrameFunction();
        // //执行跳转到的当前帧的功能
        // if (this.frameFunctionsMap.has(frame)) {
        //     this.executeActionFunctionCommand(this.frameFunctionsMap.get(frame));
        // }
     //   this._previousFrameIndex = frame;
        this._justJumpAction = true;
    }

    /**
     * 切换动作 处理逻辑;
     */
    public override void executeSwichAction() {
        // this.updataCmdList.length=0;
        if (this.switchActionFunctions.Count <= 0) return;
        int len=this.switchActionFunctions.Count;
        for (int j = 0; j < len; j++) {
            this.switchActionFunctions[j].switchCmd();
        }
    }
    public override void onRelease()
    {
        if (this.actData != null) {
          //  this.actData.Release();
            this.actData = null;
        }
        // if(this.startActionFunctions!=null){
        //     this.startActionFunctions.length=0;
        //     this.startActionFunctions = null;
        // }
        // if(this.endActionFunctions!=null){
        //     this.endActionFunctions.length=0;
        //     this.endActionFunctions = null;
        // }
        if (this.switchActionFunctions != null) {
            this.switchActionFunctions = null;
        }
        if (this.frameFunctionList != null) {
              int len=this.frameFunctionList.Count;
            for (int i = 0; i < len; i++) {
                this.frameFunctionList[i].Release();
            }
            this.frameFunctionList = null;
        }
        base.onRelease();
    }
    
}  

