//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Dash : ActionBase
{
    private  CharData charData;
    //单次创建.
    public override void init(){
        this.name=GameEnum.ActionLabel.Dash;
        this.actionLayer=GameEnum.ActionLayer.BaseLayer;
        this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
    }

        // 初始化动作.
    public override void InitAction(ObjBase obj)
    {
        base.InitAction(obj);
        charData=this.obj.objData as CharData;
        this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Move,onJoyMove);
        this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Up,onJoyUp);
    }
        //动作进入
    public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;
            this.obj.GetMovePart().StopMove();
             if(charData.aimState!=GameEnum.AimState.Null){
                     this.obj.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle,0,false);
             }
            //播放 站立动作.
            //跑步 改变 动画属性.
         //   DebugLog.Log("Dash.........");
            this.obj.moveSpeed=charData.DashSpeed;
            this.obj.GetAniBasePart().Play(GameEnum.AniLabel.Mvm_Dash,frame*GameSettings.Instance.deltaTime,0.533f,1f,0.25f,0,true);
            if(param!=null){
                Vector3 dir=(Vector3)param[0];
            //    DebugLog.Log("Run..dir ",dir,this.obj.gameObject.transform.forward);
                this.obj.GetMovePart().StartMove(dir);
           }else{
                this.obj.GetMovePart().StartMove( this.obj.gameObject.transform.forward);
           }
       //     this.obj.GetMovePart().StartMove((Vector3)param[0]);
            if(charData.isMyPlayer){
                 CameraManager.Instance.cameraCtrl.SetFOV(80);
            }
       //     CameraManager.Instance.postLayer.enabled=true;
    }

        //动作更新;
    public override void Update(){
        base.Update();
    }
    private  void onJoyMove(object[] data){
        Vector3 dirPos = (Vector3) data[0];
        bool isDash = (bool) data[1];
        if(isDash){
            this.obj.GetMovePart().SetTargetDir(dirPos);
        }else{
            this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Run,0,true,new object[]{dirPos});
        }
    }
    // 回到站立动作.
    private  void onJoyUp(object[] data=null){
        this.obj.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,true,new object[]{GameEnum.AniLabel.Mvm_Stop_Front,0.9f,1f});
    }
    


    /**
    * 切换动作 处理逻辑;
    */
    public override void executeSwichAction(){
         this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Move,onJoyMove);
         this.obj.GetEvent().removeEventListener(CharEvent.OnJoy_Up,onJoyUp);
         CameraManager.Instance.cameraCtrl.ResetFOV();
    //     CameraManager.Instance.postLayer.enabled=false;
    }

    //动作 需要改成3种分支 base up add
    public override void onGet()
    {
            base.onGet();
    }
    /**
    回收..
    **/
    public override void onRecycle()
    {
        charData=null;
        base.onRecycle();
    }
    /*********
    销毁
    *******/
    public override void onRelease()
    {        
        charData=null;
        base.onRelease();
    }
}

