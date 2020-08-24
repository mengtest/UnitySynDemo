using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
/***
* 多对象通用池;
*/
public class MultiplePool : IPool
{
    public string name;
    protected Dictionary<string,List<IPoolObj>> map;
    private int m_iNewID = 1;

    public MultiplePool(string name="MultiplePool")
    {
        this.name = name;
        this.map = new Dictionary<string, List<IPoolObj>>();
    }
     public int CreateID() {
        return this.m_iNewID++;
    }
    public virtual void recycle(IPoolObj item)
    {
         if (!item.isRecycled) {
            item.isRecycled = true;
    //        DebugLog.Log("recycle: "+item.poolname);
            this.map[item.poolname].Add(item);
            this.onRecycle(item);
        }
    }
    public int getCount(string classname) {
        if(this.map==null){
            return  0;
        }
        if(!this.map.ContainsKey(classname)){
            return 0;
        }
        return this.map[classname].Count;
    }
    //通过url 存成 poolname 默认会初始化对象池.
    public  T get<T>(string poolname) where T:PoolObj,new() {
        if(!this.map.ContainsKey(poolname)){
         //    DebugLog.Log("creat: "+poolname);
           this.map[poolname]=new List<IPoolObj>();
        }
        List<IPoolObj> tempList = this.map[poolname];
        T item;
        if (tempList.Count > 0) {
            item  =(T)tempList[tempList.Count-1];
            tempList.RemoveAt(tempList.Count-1);
		}else{
            item = new T();
            item.pool = this;
            item.poolname = poolname;
            item.isRecycled = false;
            item.id = this.CreateID();
            item.init();
        }
        this.onGet(item);
        item.pool = this;
        item.isRecycled = false;
        item.onGet();
        return item;
    }
    public T get<T>(string poolname,Type type) where T:PoolObj,new() {
        if(!this.map.ContainsKey(poolname)){
    //         DebugLog.Log("creat: "+poolname);
           this.map[poolname]=new List<IPoolObj>();
        }
        List<IPoolObj> tempList = this.map[poolname];
        T item;
        if (tempList.Count > 0) {
            item  =(T)tempList[tempList.Count-1];
            tempList.RemoveAt(tempList.Count-1);
		}else{
            item = (T) Activator.CreateInstance(type);
            item.pool = this;
            item.poolname = poolname;
            item.isRecycled = false;
            item.id = this.CreateID();
            item.init();
        }
        this.onGet(item);
        item.pool = this;
        item.isRecycled = false;
        item.onGet();
        return item;
    }

    public virtual void onGet(PoolObj item){ 

    }
    public virtual void onRecycle(IPoolObj item){ 

    }
    public virtual void onClear(IPoolObj item){ 

    }
      /**清除一个池 */
    public void clear(string poolname) {
         List<IPoolObj> lists;
          if(this.map.TryGetValue(poolname,out lists)){
            List<IPoolObj> Templist=new List<IPoolObj>(lists.ToArray());
             this.map[poolname]=new List<IPoolObj>();
            for (int i = Templist.Count - 1; i >= 0 ; i--)
            {
                IPoolObj obj=Templist[i];
                this.onClear(obj);
                Templist[i].Release();
            }
          }
    }
    /**
     * 清空所有对象;
     */
    public virtual void clearAll() {
        foreach (var item in this.map)
        {
            List<IPoolObj> Templist=new List<IPoolObj>(item.Value.ToArray());
            item.Value.Clear();
            for (int i = Templist.Count - 1; i >= 0 ; i--)
            {
                Templist[i].Release();
            }
            Templist.Clear();
        }
    }
    public string getName() {
        return this.name;
    }
    public virtual string toString() {
        StringBuilder str=new StringBuilder();
       foreach (var item in this.map)
        {
            List<IPoolObj> Templist=item.Value;
            for (int i = Templist.Count - 1; i >= 0 ; i--)
            {
                if (Templist[i] != null) {
                    str.Append(item.Key + " onPool count: " + Templist.Count + " \n");
                }
            }
        };
        return str.ToString();
    }
}

