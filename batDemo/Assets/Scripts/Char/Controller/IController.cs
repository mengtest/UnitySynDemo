


//池接口
public interface IController {
    int id { get;}
    
    void sendMessage(int code,object[] param);
    //回收;
    void Release();
    //更新;
    void Update(float dt);
}
