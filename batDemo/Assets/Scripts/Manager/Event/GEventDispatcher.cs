
using System;
using System.Collections.Generic;
using System.Linq;

[AutoRegistLua]
public class GEventDispatcher 
{
    private Dictionary<string, List<Action<object[]>>> dict;
    private string DispatchingType="";
    private List<Action<object[]>> DelList;

    public GEventDispatcher()
    {
        dict = new Dictionary<string, List<Action<object[]>>>();
        DelList=new List<Action<object[]>>();
        DispatchingType="";
    }

    public void addEventListener(string type,  Action<object[]> fn)
    {
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(type))
        {
//            StarEngine.Debuger.LogTrace("加入了侦听:" + type);
            dict.Add(type, new List<Action<object[]>>());
        }
        if(type==DispatchingType){
            if(DelList.Contains(fn)){
                DelList.Remove(fn);
                return;
            }
        }
        List<Action<object[]>> list = dict[type];
        if (list.Contains(fn))
        {
            DebugLog.Log("重复加入了侦听");
            return;
        }

     //   Event_Callback ecb = new Event_Callback();
       // ecb.cb = fn;
        dict[type].Add(fn);

        //List<callback> c = dict[type];
    }

    //删除一个类型的，一个指定回调
    public void removeEventListener(string type,  Action<object[]> fn)
    {
        if (dict.ContainsKey(type))
        {
            if(DispatchingType==type){
                DelList.Add(fn);
                return;
            }
            List<Action<object[]>> list = dict[type];
            //StarEngine.Debuger.LogTrace("````````````````````````````删除了侦听:" + type);
            if(list.Contains(fn)){
               list.Remove(fn);
            }
            // if (list.Count <= 0)
            // {
            //     dict.Remove(type);
            // }
        }

    }
    //将一个类型的事件都删除
    public virtual void removeEventListenerByType(string type)
    {
        if (dict.ContainsKey(type))
        {
//			StarEngine.Debuger.LogTrace("删除了所有侦听:" + type);
            dict.Remove(type);
        }
    }
    public bool hasEventListener(string type)
    {
        if(dict.ContainsKey(type)){
            return true;
        }
        return false;
    }
    public virtual void send(string type, object data=null){
        this.dispatchEvent(type,new object[]{data} );
    }
    //发出一个事件
    public virtual void dispatchEvent(string type, object[] data=null)
    {
        //如果存在这个事件
        if (dict != null && dict.ContainsKey(type))
        {
           // e.eventTarget = this;
           this.DispatchingType=type;
            List<Action<object[]>> list = dict[type];
            //).ToList();
             Action<object[]> fn = null;
            for (int i = 0; i < list.Count; i++)
            {
                fn = list[i];
                if (fn != null)
                {
                    fn(data);
                }
            }
         //   list.Clear();
            if(DelList.Count>0){
                for (int i = 0; i < DelList.Count; i++)
                {
                    fn=DelList[i];
                   if(list.Contains(fn)){
                        list.Remove(fn);
                   }
                }
                DelList.Clear();
            }
            // fn=null;
            // list = null;
            this.DispatchingType="";
        }
    }
    public virtual void ClearAllEvent() {
        if (dict == null) return;
        dict.Clear();
        DelList.Clear();
        DispatchingType="";
    }
    public virtual void Dispose()
    {
        ClearAllEvent();
        DispatchingType="";
        dict = null;
        DelList =null;
    }
}
