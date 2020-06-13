using LuaInterface;
using UnityEngine;
namespace Lua
{
    class LuaManager : MonoBehaviour
    {
        public static LuaManager instance;
        static LuaLooper loop;
        static GameObject obj;

        private LuaFunction _ViewHelperShowMsgFunc;
        public LuaState lua
        {
            get;
            private set;
        }
        private static LuaTable mainTable = null;

        static bool ok = false;
        public static void Init(string notice)
        {
            ok = false;
            obj = new GameObject("lua");
            instance = obj.AddComponent<LuaManager>();
            DontDestroyOnLoad(obj);

            var lua = new LuaState();
            instance.lua = lua;
            OpenLibs();
            OpenCJson();
            lua.LuaSetTop(0);
            lua.Start();
            loop = obj.AddComponent<LuaLooper>();
            loop.luaState = lua;
            DelegateFactory.Init();
            LuaBinder.Bind(lua);
            
            lua.Require("common/common");
            mainTable = lua.Require<LuaTable>("Main");
            mainTable.Call("Start", notice);
        }

        static void OpenLibs()
        {
            instance.lua.OpenLibs(LuaDLL.luaopen_pb);
            instance.lua.OpenLibs(LuaDLL.luaopen_struct);
            instance.lua.OpenLibs(LuaDLL.luaopen_lpeg);
            //===========
            instance.lua.OpenLibs(LuaDLL.luaopen_pb_io);
            instance.lua.OpenLibs(LuaDLL.luaopen_pb_conv);
            instance.lua.OpenLibs(LuaDLL.luaopen_pb_buffer);
            instance.lua.OpenLibs(LuaDLL.luaopen_pb_slice);
            instance.lua.OpenLibs(LuaDLL.luaopen_pb);
            //===========
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            instance.lua.OpenLibs(LuaDLL.luaopen_bit);
#endif
        }

        static void OpenCJson()
        {
            instance.lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            instance.lua.OpenLibs(LuaDLL.luaopen_cjson);
            instance.lua.LuaSetField(-2, "cjson");

            instance.lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            instance.lua.LuaSetField(-2, "cjson.safe");
        }

        private void OnApplicationFocus(bool focus)
        {
            if (mainTable == null) return;
            var fun = mainTable.GetLuaFunction("OnApplicationFocus");
            if (fun != null)
            {
                fun.Call(focus);
            }
        }
        private void OnApplicationPause(bool pause)
        {
            if (mainTable == null) return;
            var fun = mainTable.GetLuaFunction("OnApplicationPause");
            if (fun != null)
            {
                fun.Call(pause);
            }
        }
        private void OnApplicationQuit()
        {
            if (mainTable == null) return;
            var fun = mainTable.GetLuaFunction("OnApplicationQuit");
            if (fun != null)
            {
                fun.Call();
            }
        }

        public void CallOpenView(string canStr)
        {
            lua.DoString("ViewHelper.showView(" + canStr + ")");
        }

        public void CallViewHelperFunc(string Msg)
        {
            if (_ViewHelperShowMsgFunc == null)
            {
                LuaTable tab = lua.GetTable("ViewHelper");
                if (tab != null)
                {
                    _ViewHelperShowMsgFunc = tab.GetLuaFunction("showFlowMsg");
                    if (_ViewHelperShowMsgFunc != null)
                    {
                        _ViewHelperShowMsgFunc.Call(Msg);
                        _ViewHelperShowMsgFunc.Dispose();
                    }
                }
            }
            else
            {
                _ViewHelperShowMsgFunc.Call(Msg);
                _ViewHelperShowMsgFunc.Dispose();
            }
        }
            

        private void OnDestroy()
        {
            try
            {
                obj = null;
                if(loop)loop.Destroy();
                lua.Dispose();
                lua = null;
                instance = null;
            }
            catch { }
        }
    }
}
