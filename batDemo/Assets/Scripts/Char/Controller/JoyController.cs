
/****
遥感控制器
****/
using UnityEngine;

public class JoyController : Controller
{
    public JoyController()
    {
       
    }
    public void  OnJoyMove(object[] data){
        if(data==null)return;
        Vector2 isSprint= (Vector2) data[0];
        bool isRun= (bool) data[1];

    }
    public void  OnJoyUp(object[] data){
       //停止移动.
    }
    //冲刺状态改变.
    public void OnSprint(object[] data){
          if(data==null)return;
        bool isSprint= (bool) data[0];
    }

    public override void OnMessage(int code, object[] param)
    {

    }
    public override void OnGet_Fun(){
         //添加监听
        EventCenter.addListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.addListener(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE,OnJoyUp);
        EventCenter.addListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }
    public override void OnRecycle_Fun(){
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE,OnJoyUp);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }
    public override void OnRelease_Fun(){
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_MOVE,OnJoyMove);
        EventCenter.removeListener(SystemEvent.UI_HUD_ON_JOYSTICK_STOP_MOVE,OnJoyUp);
        EventCenter.removeListener(SystemEvent.UI_BAT_ON_SPRINT_STATE,OnSprint);
    }

}