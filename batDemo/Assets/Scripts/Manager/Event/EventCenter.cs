
using System;
using System.Collections.Generic;
using System.Linq;
using Lua;
using LuaInterface;

// 
[AutoRegistLua]
public class Event_Callback
{
    public Action<object[]> cb = null;
    public object data = null;
}

[AutoRegistLua]
public class EventCenter
{
    /// <summary>
    /// lua层的事件管理层table
    /// </summary>
    private static LuaTable _LEventTable = null;
    
    private static Dictionary<string, List<Event_Callback>> dict = new Dictionary<string, List<Event_Callback>>();

    public static void init(){

        DebugLog.Log("C#层事件管理中心初始化");
        removeListener(SystemEvent.LUA_INIT_COMPLETE, initLuaTable);
        addListener(SystemEvent.LUA_INIT_COMPLETE, initLuaTable);
    }

    /// <summary>
    /// 在LuaManager层初始化完成后，获取到需要LuaEventCenter的Table
    /// </summary>
    public static void initLuaTable(object[] data)
    {
        DebugLog.Log("C#获取lua层的事件管理层");
        _LEventTable = LuaManager.Instance.lua.GetTable("EventManager");
        removeListener(SystemEvent.LUA_INIT_COMPLETE, initLuaTable);
    }

    //加入一个侦听
    public static void addListener(string type,  Action<object[]> fn)
    {
        //如果不存在就创建一个字典  
        if (!dict.ContainsKey(type))
        {
            //            StarEngine.Debuger.LogTrace("````````````````````````````加入了侦听:    " + type);
            dict.Add(type, new List<Event_Callback>());
        }

        List<Event_Callback> list = dict[type] as List<Event_Callback>;
        if (list.Find(x => x.cb == fn) != null)
        {
            //           StarEngine.Debuger.LogTrace("````````````````````````````重复加入了侦听    " + type);
            return;
        }

        Event_Callback ecb = new Event_Callback();
        ecb.cb = fn;
        dict[type].Add(ecb);
    }

    //删除一个类型的，一个指定回调
    public static void removeListener(string type,  Action<object[]> fn)
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
    public static void removeListenerByType(string type)
    {
        if (dict.ContainsKey(type))
        {
            //         StarEngine.Debuger.LogTrace("````````````````````````````删除了所有侦听:" + type);
            dict.Remove(type);
        }
    }

   
    //发出一个事件
    public static void send(string type, object[] data = null,bool sendLua=false)
    {
        //如果存在这个事件
        if (dict.ContainsKey(type))
        {
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
        }
        if(sendLua){
           fireForLua(type,data);       
        }
    }
    #region For lua层的触发事件调用
    /// <summary>
    /// 触发事件，不带参数
    /// </summary>
    /// <param name="type">事件类型</param>
    public static void fireForLua(string type,object[] data = null)
    {
        if (_LEventTable != null)
        {
            _LEventTable.Call("dispatchEvent", type, data);
        }
    }
    
    #endregion
    public static void ClearAll()
    {
        if (dict != null)
        {
            dict.Clear();
        }
        // if (_LEventTable != null)
        // {
        //     _LEventTable=null;
        // }
    }
}