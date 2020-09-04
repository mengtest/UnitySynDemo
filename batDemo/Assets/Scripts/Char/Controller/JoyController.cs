
/****
遥感控制器
****/

using UnityEngine;

public class JoyController : Controller
{
    public bool onJoyTouch=false;
    public bool IsDraging(){
       return _IsDraging;
    }
    private bool _IsDraging=false;
    
    public float JoyAngle=0;
    private float _last_H=0;
    private Vector2 lastDirPos;
    private bool isDashing=false;

    public JoyController()
    {
       
    }

    public void  initCharDir(){
        //如果是自己才可以旋转人物.
     //   DebugLog.Log(this.onJoyTouch);
        if(this.onJoyTouch){
            if(_IsDraging&&JoyAngle>=-70&&JoyAngle<=70&&_last_H!=CameraManager.Instance.cameraCtrl.GetH()){
                _last_H=CameraManager.Instance.cameraCtrl.GetH();
                Vector3 worldDir =this._char.GetMovePart().getRotation(_last_H);
                this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,isDashing,false,this.JoyAngle});
            }
        }else if(isDashing&&_last_H!=CameraManager.Instance.cameraCtrl.GetH()){
            _last_H=CameraManager.Instance.cameraCtrl.GetH();
             Vector3 worldDir =this._char.GetMovePart().getRotation(_last_H);
             this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,isDashing,false,this.JoyAngle});
        }
    }
    public override void Update()
    {
        if(_char.charData.currentBaseAction == GameEnum.ActionLabel.Run ||_char.charData.currentBaseAction == GameEnum.ActionLabel.Dash){
             //奔跑中
             initCharDir();
        }
        
    }
    private void  OnJoyMove(object[] data){
        if(data==null)return;
        this.onJoyTouch = true;
        double db=(double) data[3];
        this.JoyAngle =  (float)db;
        this.lastDirPos= (Vector2) data[0];
        this.lastDirPos.Normalize();
        Vector3 forward =CameraManager.Instance.mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward.Normalize();
        Vector3 right =new  Vector3(forward.z, 0, -forward.x);
        Vector3 worldDir = forward *  this.lastDirPos.y + right *  this.lastDirPos.x;
        //重新赋值.
        data[0]=worldDir;
      //  DebugLog.Log("worldDir ",worldDir);
        this.SendMessage(CharEvent.OnJoy_Move,data);
    }
    private void  OnJoyUp(object[] data=null){
       //停止移动.
        this.onJoyTouch=false;
        if(!isDashing){
            this.SendMessage(CharEvent.OnJoy_Up);
        }
    }
    //冲刺状态改变.
    private void OnSprint(object[] data){
        if(data==null)return;
        isDashing= (bool) data[0];
        _char.charData.isDashing=isDashing;
       if(isDashing&&_char.GetCurStateID()==GameEnum.CharState.Char_Idle && _char.charData.currentBaseAction!=GameEnum.ActionLabel.Run&&_char.charData.currentBaseAction!=GameEnum.ActionLabel.Dash){
           this.SendMessage(CharEvent.OnJoy_Move,new object[]{this._char.gameObject.transform.forward,isDashing,false,this.JoyAngle});
       }else  if(!isDashing&&_char.GetCurStateID()==GameEnum.CharState.Char_Idle && (_char.charData.currentBaseAction==GameEnum.ActionLabel.Run ||_char.charData.currentBaseAction==GameEnum.ActionLabel.Dash)){
           this.SendMessage(CharEvent.OnJoy_Up);
       }
    }
    private void onTouchState(object[] data){
        if(data==null)return;
        _IsDraging = (bool) data[0];
        if(this.onJoyTouch&&!_IsDraging && (_char.charData.currentBaseAction==GameEnum.ActionLabel.Run || _char.charData.currentBaseAction==GameEnum.ActionLabel.Dash) ){
 //              DebugLog.Log("changeDir");
                 Vector3 forward = CameraManager.Instance.mainCamera.transform.TransformDirection(Vector3.forward);
                forward.y = 0;
                forward.Normalize();
                Vector3 right =new  Vector3(forward.z, 0, -forward.x);
                Vector3 worldDir = forward *  this.lastDirPos.y + right *  this.lastDirPos.x;
                //重新赋值.
                this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,isDashing,false,this.JoyAngle});
        }
    }
    private void  onTouchMove(object[] data){
        if(data==null)return;
        Vector2 delta= (Vector2) data[0];
        if(delta.magnitude<=1){
               return;
        }
      //  DebugLog.Log("onthuch<<<<<<<<<<<<<<<<<<",delta);
      //  delta.Normalize();
	    CameraManager.Instance.cameraCtrl.onTouchMove(delta);
      //  angleH += delta.x * horizontalAimingSpeed*0.01f;
     //	angleV += delta.y * verticalAimingSpeed*0.01f;
    }
    private void  onJump(object[] data){
        //  if(data==null)return;
        // bool isDown= (bool) data[0];
        this.SendMessage(CharEvent.On_KeyState,new  object[]{GameEnum.KeyInput.Jump});
    }
    
    protected override void OnGet_Fun(){
         //添加监听
        this.onJoyTouch=false;
        EventCenter.addListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.addListener(SystemEvent.UI_HUD_ON_JOYSTICK_UP,OnJoyUp);
        EventCenter.addListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
        EventCenter.addListener(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_MOVE,onTouchMove);
        EventCenter.addListener(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,onTouchState);
         EventCenter.addListener(SystemEvent.UI_BAT_ON_JUMP,onJump);
    }
    protected override void OnRecycle_Fun(){
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_UP,OnJoyUp);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_MOVE,onTouchMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,onTouchState);
         EventCenter.removeListener(SystemEvent.UI_BAT_ON_JUMP,onJump);
    }
    protected override void OnRelease_Fun(){
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_UP,OnJoyUp);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_MOVE,onTouchMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_ROTATE_TOUCH_STATE,onTouchState);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_JUMP,onJump);
    }

}