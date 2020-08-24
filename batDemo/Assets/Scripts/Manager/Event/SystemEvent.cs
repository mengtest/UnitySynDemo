
   
    // public class GameEventID{
    //     public static int m_iNewID = 100000;
    //     public static int CreateID {
    //             get{
    //                 return ++m_iNewID;
    //             }
    //         }
    // }

    //// 系统消息.
    [AutoRegistLua]
    public class SystemEvent
    {
       /**lua 初始化完成*/
       public static string LUA_INIT_COMPLETE = "LUA_INIT_COMPLETE";

       public static string UI_HUD_ON_ROTATE_TOUCH_MOVE = "UI_HUD_ON_ROTATE_TOUCH_MOVE";
       public static string UI_HUD_ON_ROTATE_TOUCH_STATE = "UI_HUD_ON_ROTATE_TOUCH_STATE";
       public static string UI_HUD_ON_JOYSTICK_MOVE = "UI_HUD_ON_JOYSTICK_MOVE";
       public static string UI_HUD_ON_JOYSTICK_STOP_MOVE = "UI_HUD_ON_JOYSTICK_STOP_MOVE";
       public static string UI_BAT_ON_SPRINT_STATE = "UI_BAT_ON_SPRINT_STATE";
    }
