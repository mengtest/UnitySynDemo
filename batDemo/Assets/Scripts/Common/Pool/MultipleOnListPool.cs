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
         str.Append(name);
        str.Append(" onList count: " + this.onList.Count + " \n");
       foreach (var item in this.map)
        {
            List<IRecycleAble> Templist=item.Value;
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

