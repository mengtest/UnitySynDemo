using Lua;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Lua
{
    class LuaPointerBehavior : LuaBehaviourBase,IPointerClickHandler,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler
    {
        public static bool Have(LuaTable cls)
        {
            return cls.GetLuaFunction("OnPointerClick") != null || cls.GetLuaFunction("OnPointerDown") != null|| cls.GetLuaFunction("OnPointerEnter") != null || cls.GetLuaFunction("OnPointerExit") != null|| cls.GetLuaFunction("OnPointerUp") != null ;
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

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            CallFun("OnPointerClick",eventData);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            CallFun("OnPointerDown", eventData);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            CallFun("OnPointerEnter", eventData);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            CallFun("OnPointerExit", eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            CallFun("OnPointerUp", eventData);
        }
    }
}
