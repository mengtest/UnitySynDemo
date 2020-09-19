
using System;
using System.Collections.Generic;
using System.Linq;
using Lua;
using LuaInterface;

// 
// [AutoRegistLua]
// public class Event_Callback
// {
//     public Action<object[]> cb = null;
//     public object data = null;
// }

[AutoRegistLua]
public class EventCenter
{
    /// <summary>
    /// lua层的事件管理层table
    /// </summary>
    private static LuaTable _LEventTable = null;
    
    private static Dictionary<string, List<Action<object[]>>> dict = new Dictionary<string, List<Action<object[]>>>();

    private static string DispatchingType="";
    private static List<Action<object[]>> DelList=new List<Action<object[]>>();

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

        // Event_Callback ecb = new Event_Callback();
        // ecb.cb = fn;
        dict[type].Add(fn);
    }

    //删除一个类型的，一个指定回调
    public static void removeListener(string type,  Action<object[]> fn)
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
             DispatchingType=type;
             List<Action<object[]>> list = dict[type];
             Action<object[]> fn = null;
            for (int i = 0; i < list.Count; i++)
            {
                fn = list[i];
                if (fn != null)
                {
                    fn(data);
                }
            }
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
            DispatchingType="";

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