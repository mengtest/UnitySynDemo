
/****
控制器基类
****/
public class Controller :PoolObj, IController
{
    protected Character _char= null;

    public Controller()
    {
        
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
      public override void onRecycle()
    {
        base.onRecycle();
    }
    public  override void Release(){
        this.OnRelease();
        this._char=null;
        base.Release();
    }
    public override void onGet(){
        base.onGet();
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