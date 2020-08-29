
   

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
       /**移动完毕*/
       public static string MOVE_END = "MOVE_END";
        public static string Jump_To_Ground = "JumpToGround";
        public static string Jump_Fall = "JumpFall";
        public static string Jump_Rise = "JumpRise";
        /****改成正常状态*******/
        public static string Syn_NormalState="Syn_NormalState";

    }
