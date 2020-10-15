public class PoolObj : IPoolObj
{
    public int id { get ; set ; }
    public string poolname { get; set ; }
    public IPool pool  { get; set ; }
    public bool isRecycled { get; set ; }
    public float delayRecycleTime { get; set; }
    public PoolObj(){
        this.id = 0;
        this.poolname="PoolObj";
        this.isRecycled = false;
    }
    public virtual void init(){
           
    }

    public void recycleSelf()
    {
       if (this.isRecycled) return;
        this.onRecycle();
        if(delayRecycleTime>0){
            this.isRecycled=true;
            TimerManager.Instance.Once(delayRecycleTime,onDelayRecycle,false);
        }else{
           this.pool.recycle(this);
        }
    }
    private void onDelayRecycle(){
        if(this.pool==null)return;
        this.isRecycled=false;
        this.pool.recycle(this);
    }
    public void Release()
    {
       onRelease();
       this.pool=null;
    }

    #region 继承 可重写..................
    /**
     * 释放
     **/
    public virtual void onRelease()
    {
       
    }
        /**
     * 获取
     **/
    public virtual void onGet()
    {
       
    }
    /**
     回收.
    **/
    public virtual void onRecycle()
    {

    }
    #endregion  
}

