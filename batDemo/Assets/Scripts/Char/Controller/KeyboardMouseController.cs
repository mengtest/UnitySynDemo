
/****
键盘鼠标控制器
****/
using GameEnum;
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
    private bool dashCg=false;
     private Vector2 KeyDir=Vector2.zero;
    // Horizontal Axis.
    private float h;                                      
    // Vertical Axis.
	private float v;
    public string sprintButton = "Sprint";        
    public string jumpButton = "Jump";  
    public string aimButton = "Aim"; 
    public string shootButton = "Fire1";                                
    private bool jumpEvt=false;
    public bool stopMouse=false;

    public KeyboardMouseController()
    {
         if(Cursor.visible){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
          }
    }
    public  override void  init(Player character){
        base.init(character);
        _player.cameraCtrl.isMouseMove=!stopMouse;
    }
     //FixedUpdate
    public override void Update()
    {
        if(_player.cameraCtrl.isMouseMove){
           _player.cameraCtrl.onMouseMove();
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
           this.isDashing=false;
           this.KeyDir.y=-1;
        }else{
            this.KeyDir.y=0;
        }
 
        if (Input.GetKey(KeyCode.A))
        {
        //按键盘A向左移动
            this.KeyDir.x=-1;
          if(this.KeyDir.y<=0){
             this.isDashing=false;         
          }
        }
        else if (Input.GetKey(KeyCode.D))
        {
        //按键盘D向右移动
           this.KeyDir.x=1; 
           if(this.KeyDir.y<=0){
             this.isDashing=false;
           }
        }else{
            this.KeyDir.x=0;
        }
    
        //按下冲锋
        if(Input.GetKeyDown (KeyCode.LeftShift)){
            this.dashCg=true;
            // if(!this.onJoyTouch){
            //     this.OnSprint();
            // }else{
                this.isDashing=!this.isDashing;
           // }
          //   this.OnSprint();
        }
        if(this._player.charData.isDashing!=this.isDashing){
             EventCenter.send(SystemEvent.KEY_INPUT_ONSPRINT_STATE,new object[]{this.isDashing},true);
        }

    //    //按住冲锋
    //     if(this.isDashing != Input.GetButton (sprintButton)){
    //        this.isDashing = Input.GetButton (sprintButton);
    //        this.OnSprint();
    //     }
        if(this.onJoyTouch){
            if(this.KeyDir==Vector2.zero){
                 this._player.charData.joyTouch=false;
                this.OnJoyUp();
            }else{
                this._player.charData.joyTouch=true;
                this.OnJoyMove(this.KeyDir);
            }  
        }else{
            if(this.isDashing&&_last_H!=_player.cameraCtrl.GetH()){
                //根据屏幕的旋转向前走.
                  _last_H=_player.cameraCtrl.GetH();
                   Vector3 worldDir =this._player.GetMovePart().getRotation(_last_H);
              //   DebugLog.Log("isDashing rotat",worldDir);
                  this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,isDashing,false,0});
            }else if(this.KeyDir!=Vector2.zero){
           //       DebugLog.Log("move ");
                this.OnJoyMove(this.KeyDir);
            }
        }
     //   DebugLog.Log("this.KeyDir",this.KeyDir,this.onJoyTouch);
       	if (!jumpEvt && Input.GetKeyDown(KeyCode.Space)){
               jumpEvt=true;
               this.onJump();
        }
        if(Input.GetKeyUp(KeyCode.Space)){
               jumpEvt=false;
        }
         if(Input.GetKeyDown(KeyCode.Escape)){
     //         DebugLog.Log("Down");
             stopMouse=!stopMouse;
             _player.cameraCtrl.isMouseMove=!stopMouse;
             if(!stopMouse){
             if(Cursor.visible){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
             }
            }else if(!Cursor.visible){
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.SelectWeapon_1});
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
             this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.SelectWeapon_2});
        }
        if(Input.GetKeyDown(KeyCode.G)){
         //丢使用中的武器;
           this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.DropItem,0});
        }
        if(Input.GetKeyDown(KeyCode.E)){
         //捡武器;
           this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.PickUp});
        }
        if(Input.GetKeyDown(KeyCode.R)){
         //换弹;
           this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.Reload});
        }
        if (!this._player.charData.Btn_Aim && Input.GetAxisRaw(aimButton) != 0)
		{
            this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.Aim,true});
		}
		else if (this._player.charData.Btn_Aim &&Input.GetAxisRaw(aimButton) == 0)
		{
			this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.Aim,false});
		}
         if (!this._player.charData.Btn_Fire &&Input.GetAxisRaw(shootButton) != 0)
		{
		  this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.Attack,true});
		}
		else if (this._player.charData.Btn_Fire&&Input.GetAxisRaw(shootButton) == 0)
		{
			this.SendMessage(CharEvent.On_KeyState,new object[]{KeyInput.Attack,false});
		}
        
    }
    private void  OnJoyMove(Vector2 dir){
        this.onJoyTouch = true;
      //  this.lastDirPos.Normalize();
        Vector3 forward = CameraManager.Instance.mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward.Normalize();
        Vector3 right =new  Vector3(forward.z, 0, -forward.x);
        Vector3 worldDir = forward *  dir.y + right *  dir.x;
        if( this.lastDirPos!=worldDir||this.dashCg){
       //     DebugLog.Log("move Dir",worldDir);
           this.lastDirPos=worldDir;
           this.dashCg=false;
            this.SendMessage(CharEvent.OnJoy_Move,new object[]{worldDir,this.isDashing,false,0 });
     //  DebugLog.Log("move ");
        }
      //dir,self.isSprinting,canStop,angle
    }
    private void  OnJoyUp(){
       //停止移动.
        this.onJoyTouch=false;
         this.lastDirPos=Vector3.zero;
      //        DebugLog.Log("Up ");
        if(!isDashing){
            this.SendMessage(CharEvent.OnJoy_Up);
        }
    }
    //冲刺状态改变.
    private void OnSprint(object[] data=null){
        if(data!=null){
           this.isDashing= (bool) data[0];
        }
        _player.charData.isDashing=this.isDashing;
        this.dashCg=false;
       if(isDashing){
           if(_player.GetCurStateID()==GameEnum.CharState.Char_Idle ){
                if( _player.charData.currentBaseAction!=GameEnum.ActionLabel.Run&&_player.charData.currentBaseAction!=GameEnum.ActionLabel.Dash){
                    this.SendMessage(CharEvent.OnJoy_Move,new object[]{this._player.gameObject.transform.forward,isDashing,false,0});
                }else if( _player.charData.currentBaseAction==GameEnum.ActionLabel.Run){
                    this.SendMessage(CharEvent.OnJoy_Move,new object[]{this._player.gameObject.transform.forward,isDashing,false,0});
                }
           }
       }else {
            if(_player.GetCurStateID()==GameEnum.CharState.Char_Idle ){
                if(!this.onJoyTouch &&(_player.charData.currentBaseAction==GameEnum.ActionLabel.Run ||_player.charData.currentBaseAction==GameEnum.ActionLabel.Dash)){
                    this.SendMessage(CharEvent.OnJoy_Up);
                }else if( _player.charData.currentBaseAction==GameEnum.ActionLabel.Dash){
                    this.SendMessage(CharEvent.OnJoy_Move,new object[]{this._player.gameObject.transform.forward,isDashing,false,0});
                }
            }
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
         this.dashCg=false;
        EventCenter.addListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }
    protected override void OnRecycle_Fun(){
         this.onJoyTouch=false;
         stopMouse=true;
         _player.cameraCtrl.isMouseMove=!stopMouse;
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }
    protected override void OnRelease_Fun(){
         this.onJoyTouch=false;
         stopMouse=true;
         _player.cameraCtrl.isMouseMove=!stopMouse;
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }

}