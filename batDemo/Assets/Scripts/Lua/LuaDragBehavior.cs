using Lua;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Lua
{
    class LuaDragBehavior : LuaBehaviourBase,IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        public static bool Have(LuaTable cls)
        {
            return cls.GetLuaFunction("OnBeginDrag") != null || cls.GetLuaFunction("OnDrag") != null|| cls.GetLuaFunction("OnEndDrag") != null ;
        }
        protected void CallFun(string name,PointerEventData eventData)
        {
            if (lua == null) return;
            var fun = lua.GetLuaFunction(name);
            if (fun == null) return;
            try
            {
                fun.Call(lua,eventData);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        private LuaFunction dragFun;
        public override void SetLuaTable(LuaTable lua)
        {
            base.SetLuaTable(lua);
            dragFun= lua.GetLuaFunction("OnDrag");
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            CallFun("OnBeginDrag", eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            CallFun("OnDrag", eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            CallFun("OnEndDrag", eventData);
        }
    }
}
