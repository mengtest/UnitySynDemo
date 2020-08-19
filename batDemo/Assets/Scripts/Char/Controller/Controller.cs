
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
        this.OnRecycle_Fun();
        this._char=null;
    }
    public  override void onRelease(){
        this.OnRelease_Fun();
        this._char=null;
    }
    public override void onGet(){
        this.OnGet_Fun();
    }

    #region 继承 可重写..................
    //Time.deltaTime update
    public virtual void Update()
    {
        
    }
    public  virtual void OnMessage(int code, object[] param)
    {

    }
    public  virtual void OnRelease_Fun()
    {
      
    }
    //回收.
    public  virtual void OnRecycle_Fun()
    {
      
    }
    //获取
    public  virtual void OnGet_Fun()
    {
      
    }
    #endregion  
}