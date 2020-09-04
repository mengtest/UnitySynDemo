
/****
键盘鼠标控制器
****/

using UnityEngine;

public class KeyboardMouseController : Controller
{
    public bool onJoyTouch=false;
    public bool IsDraging(){
       return _IsDraging;
    }
    private bool _IsDraging=false;
    
  //  public float JoyAngle=0;
    private float _last_H=0;
    private Vector3 lastDirPos;
    private bool isDashing=false;
     private Vector2 KeyDir=Vector2.zero;
    // Horizontal Axis.
    private float h;                                      
    // Vertical Axis.
	private float v;
    public string sprintButton = "Sprint";        
    public string jumpButton = "Jump";                                  
    private bool jumpEvt=false;

    public bool stopMouse=false;
    public KeyboardMouseController()
    {
       
    }

    public override void Update()
    {
         if(!stopMouse){
              CameraManager.Instance.cameraCtrl.onMouseMove();
         }
        // this.KeyDir.x = Input.GetAxis("Horizontal");
		// this.KeyDir.y = Input.GetAxis("Vertical");
    //按键盘W向上移动
        if (Input.GetKey(KeyCode.W))
        {
           this.KeyDir.y=1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
        //按键盘S向下移动
          this.KeyDir.y=-1;
        }else{
            this.KeyDir.y=0;
        }
 
        if (Input.GetKey(KeyCode.A))
        {
        //按键盘A向左移动
            this.KeyDir.x=-1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
        //按键盘D向右移动
           this.KeyDir.x=1;
        }else{
            this.KeyDir.x=0;
        }
        //按下冲锋
        // if(Input.GetButtonDown (sprintButton)){
        //     this.isDashing=!this.isDashing;
        //      this.OnSprint();
        // }
       //按住冲锋
        if(this.isDashing != Input.GetButton (sprintButton)){
           this.isDashing = Input.GetButton (sprintButton);
           this.OnSprint();
        }

        if(this.onJoyTouch){
            if(this.KeyDir==Vector2.zero){
                this.OnJoyUp();
            }else{
                this.OnJoyMove(this.KeyDir);
            }  
        }else{
            if(this.KeyDir!=Vector2.zero){
                this.OnJoyMove(this.KeyDir);
            }
            if(this.isDashing&&_last_H!=CameraManager.Instance.cameraCtrl.GetH()){
                //根据屏幕的旋转向前走.
                  _last_H=CameraManager.Instance.cameraCtrl.GetH();
                   Vector3 worldDir =this._char.GetMovePart().getRotation(_last_H);
            //       DebugLog.Log("this.worldDir",worldDir);
                  this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,isDashing,false,0});
            }
        }
     //   DebugLog.Log("this.KeyDir",this.KeyDir,this.onJoyTouch);
       	if (!jumpEvt && Input.GetButtonDown(jumpButton)){
               jumpEvt=true;
               this.onJump();
        }
        if(Input.GetButtonUp(jumpButton)){
               jumpEvt=false;
        }
         if(Input.GetKeyDown(KeyCode.Escape)){
     //         DebugLog.Log("Down");
             stopMouse=!stopMouse;
             CameraManager.Instance.cameraCtrl.isMouseMove=!stopMouse;
         }

        // if(_char.charData.currentBaseAction == GameEnum.ActionLabel.Run ||_char.charData.currentBaseAction == GameEnum.ActionLabel.Dash){
        //      //奔跑中
        //      initCharDir();
        // }
        
    }
    private void  OnJoyMove(Vector2 dir){
        this.onJoyTouch = true;
      //  this.lastDirPos.Normalize();
        Vector3 forward =CameraManager.Instance.mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward.Normalize();
        Vector3 right =new  Vector3(forward.z, 0, -forward.x);
        Vector3 worldDir = forward *  dir.y + right *  dir.x;
        if( this.lastDirPos!=worldDir){
            DebugLog.Log("move Dir",worldDir);
           this.lastDirPos=worldDir;
            this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,this.isDashing,false,0 });
        }
       // DebugLog.Log("worldDir ",worldDir);
      //dir,self.isSprinting,canStop,angle
    }
    private void  OnJoyUp(){
       //停止移动.
        this.onJoyTouch=false;
        if(!isDashing){
            this.SendMessage(CharEvent.OnJoy_Up);
        }
    }
    //冲刺状态改变.
    private void OnSprint(){
        _char.charData.isDashing=this.isDashing;
       if(isDashing&&_char.GetCurStateID()==GameEnum.CharState.Char_Idle && _char.charData.currentBaseAction!=GameEnum.ActionLabel.Run&&_char.charData.currentBaseAction!=GameEnum.ActionLabel.Dash){
           this.SendMessage(CharEvent.OnJoy_Move,new object[]{this._char.gameObject.transform.forward,isDashing,false,0});
       }else  if(!isDashing&&_char.GetCurStateID()==GameEnum.CharState.Char_Idle && (_char.charData.currentBaseAction==GameEnum.ActionLabel.Run ||_char.charData.currentBaseAction==GameEnum.ActionLabel.Dash)){
           this.SendMessage(CharEvent.OnJoy_Up);
       }
    }

    private void  onJump(){
        //  if(data==null)return;
        // bool isDown= (bool) data[0];
        this.SendMessage(CharEvent.On_KeyState,new  object[]{GameEnum.KeyInput.Jump});
    }
    
    protected override void OnGet_Fun(){
         //添加监听
        this.onJoyTouch=false;
         stopMouse=false;
         CameraManager.Instance.cameraCtrl.isMouseMove=!stopMouse;
    }
    protected override void OnRecycle_Fun(){
         this.onJoyTouch=false;
         stopMouse=true;
         CameraManager.Instance.cameraCtrl.isMouseMove=!stopMouse;
    }
    protected override void OnRelease_Fun(){
         this.onJoyTouch=false;
         stopMouse=true;
         CameraManager.Instance.cameraCtrl.isMouseMove=!stopMouse;
    }

}