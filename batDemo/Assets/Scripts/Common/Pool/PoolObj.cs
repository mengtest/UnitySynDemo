public class PoolObj : IRecycleAble
{
    public int id { get ; set ; }
    public string poolname { get; set ; }
    public IPool pool  { get; set ; }
    public bool isRecycled { get; set ; }
    public PoolObj(){
        this.id = 0;
        this.poolname="PoolObj";
        this.isRecycled = false;
    }
    public virtual void init(){
           
    }

    public void recycleSelf()
    {
       if (this.isRecycled)
            return;
        this.onRecycle();
        this.pool.recycle(this);
    }
    /**
     * 释放
     **/
    public virtual void Release()
    {
       this.pool=null;
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
}

