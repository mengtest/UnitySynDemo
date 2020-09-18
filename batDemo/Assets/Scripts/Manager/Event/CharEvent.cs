
   

    // public class GameEventID{
    //     public static int m_iNewID = 100000;
    //     public static int CreateID {
    //             get{
    //                 return ++m_iNewID;
    //             }
    //         }
    // }
    
    //// 实体消息
    [AutoRegistLua]
    public class CharEvent
    {
       /**移动完毕 MovePart */
       public static string MOVE_END = "MOVE_END";
        public static string Jump_To_Ground = "JumpToGround";
        public static string Jump_Fall = "JumpFall";
        public static string Jump_Rise = "JumpRise";
        //移动 或者 待机中突然滑落
        public static string Begin_Fall ="Begin_Fall";


        /****改成正常状态*******/
        public static string Syn_NormalState="Syn_NormalState";


         //遥感移动;
         public const string OnJoy_Move="OnJoy_Move";
         //遥感抬起;
         public const string OnJoy_Up="OnJoy_Up";
         //各种按键 点击弹起;
         public const string On_KeyState="On_KeyState";
         //各种按键  选择武器
         public const string On_Select_Weapon="On_Key_Select_Weapon";
         // 丢出 武器 0当前使用 1武器栏 2武器栏
         public const string On_Drop_Weapon="On_Drop_Weapon";
         // pickup 
         public const string On_PickUp_Item="On_PickUp_Item";


         public const string Char_Move="Char_Move";
         public const string  Char_StopMove="Char_StopMove";
         public const string Char_FollowTarget="Char_FollowTarget";
         public const string Char_MoveToPos="Char_MoveToPos";
         public const string Char_UseSkill="Char_UseSkill";
         public const string Char_MoveToPosList="Char_MoveToPosList";
         public const string Char_MoveToPosWithoutPathFinder="Char_MoveToPosWithoutPathFinder";
         public const string Start_AI ="Start_AI";
         public const string Stop_AI="Stop_AI";
         public const string Paused_AI="Paused_AI";
         public const string Continue_AI="Continue_AI";

    }
