using System;
using System.Collections.Generic;

public class Pool<T>:IPool where T: IPoolObj,new ()
{
    public delegate T CreatPoolObj();
    protected List<T> _list;
    public string poolName;
    public Pool(string poolName)
    {
        this.poolName = poolName;
        this._list = new List<T>();
    }
    // 获取
    public  T get() {
         T item;
        if (_list.Count > 0) {
            item  = _list[_list.Count-1];
            _list.RemoveAt(_list.Count-1);
		}else{
             item = this._createItem();
        }
        item.pool = this;
        item.poolname = this.poolName;
        item.isRecycled = false;
        item.onGet();
        return item;
    }
    //预加载.
     public void prepare(int count) {
         for (int i = 0; i < count; i++)
         {
               T item = this._createItem();
                this.recycle(item);
         }
    }
    //新对象
     private T _createItem() {
        T item = new T();
        item.pool = this;
        item.isRecycled = false;
        item.init();
        return item;
    }
    //回收
    public void recycle(IPoolObj item)
    {
         if (!item.isRecycled) {
            item.isRecycled = true;
            this._list.Add((T)item);
        }
    }
    public void  clearAll() {
        List<T> Templist=this._list;
        this._list=new List<T>(); 
        for (int i = Templist.Count - 1; i >= 0 ; i--)
        {
            Templist[i].Release();
        }
    }
     public string getName() {
        return this.poolName;
    }
}

