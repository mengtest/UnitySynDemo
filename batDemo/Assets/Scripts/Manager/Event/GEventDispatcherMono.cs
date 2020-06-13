using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GEventDispatcherMono : MonoBehaviour
{
    public delegate void callback(GEvent e);
    class EventCallback
    {
        public callback cb = null;
        public object param = null;
    }
    private Dictionary<string, List<EventCallback>> dict;
    private System.Object m_target=null;

    public GEventDispatcherMono()
    {
        dict = new Dictionary<string, List<EventCallback>>();
    }
    public void addEventListener(short type, callback fn, object param = null)
    {
        string _type= type.ToString();
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(_type))
        {
  //          StarEngine.Debuger.LogTrace("加入了侦听:" + _type);
            dict.Add(_type, new List<EventCallback>());
        }
        List<EventCallback> list = dict[_type] as List<EventCallback>;
        if (list.Find(x => x.cb == fn) != null)
        {
       //     DebugLog.Log("重复加入了{0}侦听",null, type);
            return;
        }
        EventCallback ecb = new EventCallback();
        ecb.cb = fn;
        ecb.param = param;
        dict[_type].Add(ecb);
        //List<callback> c = dict[type];

    }
    public void addEventListener(string type, callback fn, object param = null)
    {
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(type))
        {
//			StarEngine.Debuger.LogTrace("加入了侦听:" + type);
            dict.Add(type, new List<EventCallback>());
        }
        List<EventCallback> list = dict[type] as List<EventCallback>;
        if (list.Find(x => x.cb == fn) != null)
        {
           // DebugLog.Log("重复加入了{0}侦听",null, type);
            return;
        }

        EventCallback ecb = new EventCallback();
        ecb.cb = fn;
        ecb.param = param;
        dict[type].Add(ecb);

        //List<callback> c = dict[type];

    }

    //删除一个类型的，一个指定回调
    public void removeEventListener(string type, callback fn)
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

    //删除一个类型的，一个指定回调
    public void removeEventListener(short type, callback fn)
    {
        string _type = type.ToString();
        if (dict.ContainsKey(_type))
        {
//			StarEngine.Debuger.LogTrace("删除了侦听:" + _type);
            List<EventCallback> list = dict[_type] as List<EventCallback>;
            //			StarEngine.Debuger.LogTrace("删除了侦听:" + type);
            list.RemoveAll(x => x.cb == fn);
            if (list.Count <= 0)
            {
                dict.Remove(_type);
            }
        }

    }
    //将一个类型的事件都删除
    public void removeEventListenerByType(string type)
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
    //发出一个事件，简化操作
    public void dispatchEventWith(string type, object data = null)
    {
        GEvent e = new GEvent(type, data);
        dispatchEvent(e);
    }
    //发出一个事件
    public void dispatchEvent(GEvent e,object data=null)
    {
        //如果存在这个事件
        if (dict.ContainsKey(e.type))
        {
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
        }
    }
}
