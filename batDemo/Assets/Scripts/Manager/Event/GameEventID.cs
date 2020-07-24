
   
namespace GameEventID
{
    public class GameEventID{
        public static int m_iNewID = 100000;
        public static int CreateID {
                get{
                    return ++m_iNewID;
                }
            }
    }
    //// 实体消息
    public class CharEvent
    {
       /**移动完毕*/
       public static int MOVE_END = GameEventID.CreateID;

    }
}