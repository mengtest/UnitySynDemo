
/****
控制器基类
****/
public class Controller :PoolObj, IController
{
    protected Player _player= null;
    
    public Controller()
    {
        
    }
    public  virtual void  init(Player character){
        this._player=character;
    }

    public  void SendMessage(string cmd, object[] param=null)
    {
        if (this._player==null||this._player.isRecycled) {
            return;
        }
        this._player.OnEvent(cmd,param);
        //this._char.GetEvent().send(cmd,param);
    }
    public override void onRecycle()
    {
        this.OnRecycle_Fun();
        this._player=null;
    }
    public  override void onRelease(){
        this.OnRelease_Fun();
        this._player=null;
    }
    public override void onGet(){
        this.OnGet_Fun();
    }

    #region 继承 可重写..................
    //GameSettings.Instance.deltaTime FixedUpdate
    public virtual void Update()
    {
        
    }
    public virtual void LateUpdate(){
        
    }
    //角色动作改变需要重置数据.
    public virtual void OnActionChange(){

    }
    protected  virtual void OnRelease_Fun()
    {
      
    }
    //回收.
    protected  virtual void OnRecycle_Fun()
    {
      
    }
    //获取
    protected  virtual void OnGet_Fun()
    {
      
    }
    #endregion  
}