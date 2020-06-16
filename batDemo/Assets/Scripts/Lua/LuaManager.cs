using LuaInterface;
using System.Collections;
using System.IO;
using UnityEngine;
namespace Lua
{
    class LuaManager : MonoSingleton<LuaManager>
    {
        static LuaLooper loop;

        private LuaFunction _ViewHelperShowMsgFunc;
        public LuaState lua
        {
            get;
            private set;
        }
        private static LuaTable mainTable = null;

        static bool ok = false;

        void Awake()
        {

        }
         public IEnumerator InitStart()
        {
            if (GameSettings.Instance.useAssetBundle)
            {
                LuaFileUtils.Instance.beZip = true;
                yield return GameAssetManager.Instance.LoadLuaBundle();
            }
            lua =new LuaState();
            this.OpenLibs();
            this.OpenCJson();
            lua.LuaSetTop(0);
            InitLuaPath();
            this.StartMain();
        }


        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        private void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
            lua.OpenLibs(LuaDLL.luaopen_struct);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            //===========
            lua.OpenLibs(LuaDLL.luaopen_pb_io);
            lua.OpenLibs(LuaDLL.luaopen_pb_conv);
            lua.OpenLibs(LuaDLL.luaopen_pb_buffer);
            lua.OpenLibs(LuaDLL.luaopen_pb_slice);
            lua.OpenLibs(LuaDLL.luaopen_pb);
            //===========
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            lua.OpenLibs(LuaDLL.luaopen_bit);
#endif
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        private void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath()
        {
            if (!GameSettings.Instance.useAssetBundle)
            {
                if(Application.isEditor)
                {
                    lua.AddSearchPath(Application.dataPath + "/Editor");
                }
                lua.AddSearchPath(LuaConst.luaDir);
                lua.AddSearchPath(LuaConst.toluaDir);
            }
        }
          void StartMain()
        {
            lua.Start();    //启动LUAVM
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
            DelegateFactory.Init();
            LuaBinder.Bind(lua);
            lua.Require("common/common");
            mainTable = lua.Require<LuaTable>("Main");
            mainTable.Call("Start");
        }
        public LuaTable NewTable()
        {
            return lua.NewTable();
        }
        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
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
                if(loop){
                    loop.Destroy();
                }
                loop=null;
                lua.Dispose();
                lua = null;
            }
            catch { }
        }
    }
}
