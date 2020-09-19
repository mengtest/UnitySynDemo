//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Dash : ActionBase
{
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
        this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Move,onJoyMove);
        this.obj.GetEvent().addEventListener(CharEvent.OnJoy_Up,onJoyUp);
    }
        //动作进入
    public override void GotoFrame(int frame=0,object[] param=null){
            this.currentFrame = frame;
            this.obj.GetMovePart().StopMove();
            //播放 站立动作.
            //跑步 改变 动画属性.
         //   DebugLog.Log("Dash.........");
            CharData charData=this.obj.objData as CharData;
            this.obj.moveSpeed=charData.DashSpeed;
            this.obj.GetAniBasePart().Play(GameEnum.AniLabel.Mvm_Dash,frame,0.533f,1f,0.15f,0,true);
            this.obj.GetMovePart().StartMove((Vector3)param[0]);
            CameraManager.Instance.cameraCtrl.SetFOV(80);
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
        base.onRecycle();
    }
    /*********
    销毁
    *******/
    public override void onRelease()
    {
            base.onRelease();
    }
}

