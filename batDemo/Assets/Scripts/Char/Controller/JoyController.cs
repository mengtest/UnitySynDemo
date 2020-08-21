
/****
遥感控制器
****/

public class JoyController : Controller
{
    public bool onJoyTouch=false;

    public JoyController()
    {
       
    }

    public void  OnJoyMove(object[] data){
        if(data==null)return;
        this.onJoyTouch=true;
        this.SendMessage(GameEnum.ControllerCmd.OnJoy_Move,data);
    }
    public void  OnJoyUp(object[] data=null){
       //停止移动.
        this.onJoyTouch=false;
        this.SendMessage(GameEnum.ControllerCmd.OnJoy_Up,data);
    }
    //冲刺状态改变.
    public void OnSprint(object[] data){
        if(data==null)return;
        bool isSprint= (bool) data[0];


    }
    protected override void OnGet_Fun(){
         //添加监听
        this.onJoyTouch=false;
        EventCenter.addListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.addListener(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE,OnJoyUp);
        EventCenter.addListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }
    protected override void OnRecycle_Fun(){
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE,OnJoyUp);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }
    protected override void OnRelease_Fun(){
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE,OnJoyUp);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }

}