
/****
控制器基类
****/
public class Controller : IController
{
    public static int m_iNewID = 1000;
    public static int CreateID {
            get{
                return m_iNewID++;
            }
    }
    public int id { get; private set; }

    protected Character _char= null;

    public Controller()
    {
        this.id = CreateID;
    }
    public  void  init(Character character){
        this._char=character;
        this.OnInit();
    }

    public  void sendMessage(int code, object[] param)
    {
        if (this._char.isDead ||this._char.Target.isRecycled ) {
            return;
        }
        this.OnMessage(code,param);
    }
    public   void Release(){
        this.OnRelease();
        this._char=null;
    }

    #region 继承 可重写..................
    public virtual void Update(float dt)
    {
        
    }

    public  virtual void OnInit()
    {

    }
    public  virtual void OnMessage(int code, object[] param)
    {

    }
    public  virtual void OnRelease()
    {
      
    }
    #endregion  
}