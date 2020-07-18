using System.Collections.Generic;
using System.Text;
/***
* 多对象通用池;
*/
public class MultipleOnListPool<T> : MultiplePool  where T: PoolObj,new ()
{
    private List<T> onList;

    public MultipleOnListPool(string name="MultipleOnListPool")
    {
        this.name = name;
        this.map = new Dictionary<string, List<IRecycleAble>>();
        this.onList=new List<T>();
    }

     /**
     * 获得在使用中的数组;
     */
    public List<T> getOnList(){
        return this.onList;
    }
    public override void onGet(PoolObj item){ 
         T obj = (T)item;
        if(!this.onList.Contains(obj)){
            this.onList.Add(obj);
        }
    }
    public override void onRecycle(IRecycleAble item){ 
        T obj=(T)item;
        if(this.onList.Contains(obj)){
            this.onList.Remove(obj);
        }
    }
    public override void onClear(IRecycleAble item){ 
        T obj=(T)item;
        if(this.onList.Contains(obj)){
            this.onList.Remove(obj);
        }
    }
      /**清除一个池 */
    public void recycleAll() {
         List<PoolObj> Templist=new List<PoolObj>(this.onList.ToArray());
        this.onList.Clear();
        for (int i = Templist.Count - 1; i >= 0 ; i--)
        {
            Templist[i].recycleSelf();
        }
    }
    /**
     * 清空所有对象;
     */
    public override void clearAll() {
        this.onList.Clear();
        base.clearAll();
    }

    public override string toString() {
        StringBuilder str=new StringBuilder();
        str.Append(name+ " \n");
        str.Append("mapCount: "+ this.map.Count+ " \n");
        str.Append(" onList count: " + this.onList.Count + " \n");
        foreach (var item in this.map)
        {
            if (item.Value != null) {
                str.Append(item.Key + " onPool count: " + item.Value.Count + " \n");
            }
        };
        return str.ToString();
    }
}

