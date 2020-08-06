
using System;
using System.Collections.Generic;
using System.Linq;

[AutoRegistLua]
public class GEventDispatcher 
{
    private Dictionary<string, List<Event_Callback>> dict;

    public GEventDispatcher()
    {
        dict = new Dictionary<string, List<Event_Callback>>();
    }

    public void addEventListener(string type,  Action<object[]> fn)
    {
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(type))
        {
//            StarEngine.Debuger.LogTrace("加入了侦听:" + type);
            dict.Add(type, new List<Event_Callback>());
        }

        List<Event_Callback> list = dict[type] as List<Event_Callback>;
        if (list.Find(x => x.cb == fn) != null)
        {
            DebugLog.Log("重复加入了侦听");
            return;
        }

        Event_Callback ecb = new Event_Callback();
        ecb.cb = fn;
        dict[type].Add(ecb);

        //List<callback> c = dict[type];
    }

    //删除一个类型的，一个指定回调
    public void removeEventListener(string type,  Action<object[]> fn)
    {
        if (dict.ContainsKey(type))
        {
            List<Event_Callback> list = dict[type] as List<Event_Callback>;
            //StarEngine.Debuger.LogTrace("````````````````````````````删除了侦听:" + type);
            list.RemoveAll(x => x.cb == fn);
            if (list.Count <= 0)
            {
                dict.Remove(type);
            }
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
            List<Event_Callback> list = (dict[type] as List<Event_Callback>).ToList();
             Action<object[]> fn = null;
            for (int i = 0; i < list.Count; i++)
            {
                fn = list[i].cb;
                if (fn != null)
                {
                    fn(data);
                }
            }
            list.Clear();
            list = null;
        }
    }
    public virtual void ClearAllEvent() {
        if (dict == null) return;
        dict.Clear();
    }
    public virtual void Dispose()
    {
        ClearAllEvent();
        dict = null;
    }
}
