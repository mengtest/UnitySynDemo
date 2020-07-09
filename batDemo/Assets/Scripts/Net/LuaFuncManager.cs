using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

[AutoRegistLua]
public class LuaFuncManager
{
	private static LuaFuncManager _instance = null;
	public static LuaFuncManager Instance { get { if (_instance == null) _instance = new LuaFuncManager(); return _instance; } }
	private Dictionary<string, LuaFunction> luaFuncDic = new Dictionary<string, LuaFunction>();
    static readonly object m_lockObject = new object();
    static Queue<KeyValuePair<string, byte[]>> mByteEvents = new Queue<KeyValuePair<string, byte[]>>();
    static Queue<KeyValuePair<string, LuaByteBuffer>> mLuaByteEvents = new Queue<KeyValuePair<string, LuaByteBuffer>>();
    static Queue<KeyValuePair<string, bool>> mBoolEvents = new Queue<KeyValuePair<string, bool>>();
    static Queue<KeyValuePair<string, string>> mStringEvents = new Queue<KeyValuePair<string, string>>();

    public void addCustomListener(string funcName, LuaFunction func)
    {
        luaFuncDic[funcName] = func;
    }

    public static void dispatchCustomEvent(string _event, byte[] data)
    {
        lock (m_lockObject) {
            mByteEvents.Enqueue(new KeyValuePair<string, byte[]>(_event, data));
        }
    }

    public static void dispatchCustomEvent(string _event, LuaByteBuffer data)
    {
        lock (m_lockObject)
        {
            mLuaByteEvents.Enqueue(new KeyValuePair<string, LuaByteBuffer>(_event, data));
        }
    }

    public static void dispatchCustomEvent(string _event, bool data)
    {
        lock (m_lockObject) {
            mBoolEvents.Enqueue(new KeyValuePair<string, bool>(_event, data));
        }
    }

    public static void dispatchCustomEvent(string _event, string data)
    {
        lock (m_lockObject) {
            mStringEvents.Enqueue(new KeyValuePair<string, string>(_event, data));
        }
    }

    public void Update()
    {
        if (mByteEvents.Count > 0 ) {
            while (mByteEvents.Count > 0) {
                KeyValuePair<string, byte[]> _event = mByteEvents.Dequeue();
                if (luaFuncDic.ContainsKey(_event.Key))
                {
                    LuaFunction func = luaFuncDic[_event.Key];
					
					if(func != null)
						func.Call(_event.Value);
					else
						Debug.LogError("eventLua =======================" + _event.Key);
				}
            }
        }

        if (mLuaByteEvents.Count > 0)
        {
            while (mLuaByteEvents.Count > 0)
            {
                KeyValuePair<string, LuaByteBuffer> _event = mLuaByteEvents.Dequeue();
                if (luaFuncDic.ContainsKey(_event.Key))
                {
                    LuaFunction func = luaFuncDic[_event.Key];

                    if (func != null)
                        func.Call(_event.Value);
                    else
                        Debug.LogError("eventLua =======================" + _event.Key);
                }
            }
        }

        if (mBoolEvents.Count > 0 ) {
            while (mBoolEvents.Count > 0) {
                KeyValuePair<string, bool> _event = mBoolEvents.Dequeue();
                if (luaFuncDic.ContainsKey(_event.Key))
                {
                    LuaFunction func = luaFuncDic[_event.Key];
					if (func != null)
						func.Call(_event.Value);
					else
						Debug.LogError("eventLua =======================" + _event.Key);
				}
            }
        }

        if (mStringEvents.Count > 0 ) {
            while (mStringEvents.Count > 0) {
                KeyValuePair<string, string> _event = mStringEvents.Dequeue();
                if (luaFuncDic.ContainsKey(_event.Key))
                {
                    LuaFunction func = luaFuncDic[_event.Key];
                    //Debug.LogError(func);
                    if (func != null)
                    {
                        func.Call(_event.Value);
                    }
                    else
                    {
                        Debug.LogError("eventLua =======================" + _event.Key);
                    }
                }
            }
        }
    }
}
