using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace Lua
{
    abstract class LuaBehaviourBase:MonoBehaviour
    {
        public LuaTable lua;
        protected  void CallFun(string name)
        {
            if (lua == null) return;
            var fun = lua.GetLuaFunction(name);
            if (fun == null) return;
            try
            {
                fun.Call(lua);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        public virtual void SetLuaTable(LuaTable lua)
        {
            this.lua = lua;
        }
        private  void OnDestroy()
        {
            this.lua.Dispose();
            this.lua = null;
        }
    }
    class LuaBehaviour : LuaBehaviourBase
    {
        public override void SetLuaTable(LuaTable lua)
        {
            base.SetLuaTable(lua);
            lua.RawSet("transform",transform);
            lua.RawSet("gameObject", gameObject);transform.GetComponents<Text>().GetEnumerator();
            lua.RawSet("rectTransform", GetComponent<RectTransform>());
            CallFun("_Init");
        }
        private void Awake()
        {
            CallFun("_Awake");
        }
        private void Start()
        {
            CallFun("_Start");
        }
        private void OnEnable()
        {
            CallFun("_OnEnable");
            CallFun("_AddListener");
        }
        private void OnDisable()
        {
            CallFun("_RemoveListener");
            CallFun("_OnDisable");
        }
        private void OnDestroy()
        {
            CallFun("_OnDestroy");
            this.lua.Dispose();
            this.lua = null;
        }
    }
}
