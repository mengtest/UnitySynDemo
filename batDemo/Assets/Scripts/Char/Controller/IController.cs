


//池接口
public interface IController {
    int id { get;}
    
    void SendMessage(int cmd,object[] param=null);
    //回收;
    void Release();
    //更新;
    void Update();
}
