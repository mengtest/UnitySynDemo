using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GEventDispatcherMono : MonoBehaviour ,IDispatcher
{
    public delegate void callback(GEvent e);
    class EventCallback
    {
        public callback cb = null;
        public object param = null;
    }
    private Dictionary<int, List<EventCallback>> dict;

    private void Awake() 
    {
        dict = new Dictionary<int, List<EventCallback>>();
    }
    public void addEventListener(int type, callback fn, object param = null)
    {
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(type))
        {
  //          StarEngine.Debuger.LogTrace("加入了侦听:" + _type);
            dict.Add(type, new List<EventCallback>());
        }
        List<EventCallback> list = dict[type] as List<EventCallback>;
        if (list.Find(x => x.cb == fn) != null)
        {
       //     DebugLog.Log("重复加入了{0}侦听",null, type);
            return;
        }
        EventCallback ecb = new EventCallback();
        ecb.cb = fn;
        ecb.param = param;
        dict[type].Add(ecb);
        //List<callback> c = dict[type];

    }

    //删除一个类型的，一个指定回调
    public void removeEventListener(int type, callback fn)
    {
        if (dict.ContainsKey(type))
        {
            List<EventCallback> list = dict[type] as List<EventCallback>;
//			StarEngine.Debuger.LogTrace("删除了侦听:" + type);
            list.RemoveAll(x => x.cb == fn);
            if (list.Count <= 0)
            {
                dict.Remove(type);
            }
        }

    }

  //将一个类型的事件都删除
    public virtual void removeEventListenerByType(int type)
    {
        if (dict.ContainsKey(type))
        {
//			StarEngine.Debuger.LogTrace("删除了所有侦听:" + type);
            dict.Remove(type);
        }
    }
    public bool hasEventListener(int type)
    {
        if(dict.ContainsKey(type)){
            return true;
        }
        return false;
    }
    //发出一个事件，简化操作
    public void dispatchEventWith(int type, object data = null)
    {
        GEvent e = new GEvent(type, data);
        dispatchEvent(e);
    }
    //发出一个事件
    public virtual void dispatchEvent(GEvent e)
    {
        //如果存在这个事件
        if (dict != null && dict.ContainsKey(e.type))
        {
            e.eventTarget = this;
            List<EventCallback> list = (dict[e.type] as List<EventCallback>).ToList();
            callback fn = null;
            for (int i = 0; i < list.Count; i++)
            {
                fn = list[i].cb;
                e.param = list[i].param;
                if (fn != null)
                {
                    fn(e);
                }
            }
            list.Clear();
            list = null;
        }
        e.release();
        e = null;
    }
    public virtual void ClearAllEvent() {
        if (dict == null) return;
        dict.Clear();
    }
    private void OnDestroy() 
    {
        ClearAllEvent();
        dict = null;
    }
}
