﻿
   
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
       public static string UI_HUD_ON_JOYSTICK_UP = "UI_HUD_ON_JOYSTICK_UP";
       public static string UI_BAT_ON_SPRINT_STATE = "UI_BAT_ON_SPRINT_STATE";
       public static string KEY_INPUT_ONSPRINT_STATE ="KEY_INPUT_ONSPRINT_STATE";
       public static string KEY_INPUT_ONRELOAD_STATE ="KEY_INPUT_ONRELOAD_STATE";

      public static string UI_BAT_ON_Aiming="UI_BAT_ON_Aiming";
       public static string UI_BAT_ON_JUMP = "UI_BAT_ON_JUMP";

       public static string UI_BAT_ON_KEYSTATE="UI_BAT_ON_KEYSTATE";

       //角色 碰到道具.
       public static string ITEM_ON_PLAYER_TRIGGER ="ITEM_ON_PLAYER_TRIGGER";
    }
