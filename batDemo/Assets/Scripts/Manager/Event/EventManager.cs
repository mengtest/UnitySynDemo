
using System.Collections.Generic;
using System.Linq;

// 
public class EventManager
{
    public delegate void callback(GEvent e);
    public delegate bool votecallback(GEvent e);

    class EventCallback
    {
        public callback cb = null;
        public object param = null;
    }

    class VoteCallback
    {
        public votecallback cb = null;
        public object param = null;
    }

    private static Dictionary<int, List<EventCallback>> dict = new Dictionary<int, List<EventCallback>>();
    private static Dictionary<int, List<VoteCallback>> dictvote = new Dictionary<int, List<VoteCallback>>();

    //加入一个侦听
    public static void addListener(int type, callback fn, object param = null)
    {
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(type))
        {
            //            StarEngine.Debuger.LogTrace("````````````````````````````加入了侦听:    " + type);
            dict.Add(type, new List<EventCallback>());
        }

        List<EventCallback> list = dict[type] as List<EventCallback>;
        if (list.Find(x => x.cb == fn) != null)
        {
            //           StarEngine.Debuger.LogTrace("````````````````````````````重复加入了侦听    " + type);
            return;
        }

        EventCallback ecb = new EventCallback();
        ecb.cb = fn;
        ecb.param = param;
        dict[type].Add(ecb);
    }

    //删除一个类型的，一个指定回调
    public static void removeListener(int type, callback fn)
    {
        if (dict.ContainsKey(type))
        {
            List<EventCallback> list = dict[type] as List<EventCallback>;
            //StarEngine.Debuger.LogTrace("````````````````````````````删除了侦听:" + type);
            list.RemoveAll(x => x.cb == fn);
            if (list.Count <= 0)
            {
                dict.Remove(type);
            }
        }
    }
    //将一个类型的事件都删除
    public static void removeListenerByType(int type)
    {
        if (dict.ContainsKey(type))
        {
            //         StarEngine.Debuger.LogTrace("````````````````````````````删除了所有侦听:" + type);
            dict.Remove(type);
        }
    }

    public static void addVote(int type, votecallback fn, object param = null)
    {
        //如果不存在就创建一个字典  
        if (!dictvote.ContainsKey(type))
        {
            //           StarEngine.Debuger.LogTrace("````````````````````````````加入了侦听:    " + type);
            dictvote.Add(type, new List<VoteCallback>());
        }

        List<VoteCallback> list = dictvote[type] as List<VoteCallback>;
        if (list.Find(x => x.cb == fn) != null)
        {
            //          StarEngine.Debuger.LogTrace("````````````````````````````重复加入了投票侦听    " + type);
            return;
        }

        VoteCallback ecb = new VoteCallback();
        ecb.cb = fn;
        ecb.param = param;
        dictvote[type].Add(ecb);
    }

    //删除一个类型的，一个指定回调
    public static void removeVote(int type, votecallback fn)
    {
        if (dictvote.ContainsKey(type))
        {
            List<VoteCallback> list = dictvote[type] as List<VoteCallback>;
            //StarEngine.Debuger.LogTrace("````````````````````````````删除了侦听:" + type);
            list.RemoveAll(x => x.cb == fn);
            if (list.Count <= 0)
            {
                dict.Remove(type);
            }
        }
    }
    //将一个类型的事件都删除
    public static void removeVoteByType(int type)
    {
        if (dictvote.ContainsKey(type))
        {
            //         StarEngine.Debuger.LogTrace("````````````````````````````删除了所有侦听:" + type);
            dictvote.Remove(type);
        }
    }

    //发出一个事件，简化操作
    public static void send(int type, object data = null)
    {
        GEvent e = new GEvent(type, data);
        sendEvent(e);
    }
    //发出一个事件
    public static void sendEvent(GEvent e)
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
        e.release();
        e = null;
    }


    //发出一个事件，简化操作
    public static bool sendVote(int type, object data = null)
    {
        GEvent e = new GEvent(type, data);
        return sendVoteEvent(e);
    }
    //发出一个事件
    static bool sendVoteEvent(GEvent e)
    {
        //如果存在这个事件
        if (dictvote.ContainsKey(e.type))
        {
            List<VoteCallback> list = (dictvote[e.type] as List<VoteCallback>).ToList();
            votecallback fn = null;
            for (int i = 0; i < list.Count; i++)
            {
                fn = list[i].cb;
                e.param = list[i].param;
                if (fn != null)
                {
                    if (!fn(e))
                    {
                        e.release();
                        e = null;
                        return false;
                    }
                }
            }
        }
        e.release();
        e = null;
        return true;
    }

    public static void ClearAll()
    {
        if (dict != null)
        {
            dict.Clear();
        }
        if (dictvote != null)
        {
            dictvote.Clear();
        }
    }
}