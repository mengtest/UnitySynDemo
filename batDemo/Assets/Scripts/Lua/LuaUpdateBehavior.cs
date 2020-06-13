using Lua;
using LuaInterface;

namespace Lua
{
    class LuaUpdateBehavior : LuaBehaviourBase
    {
        public static bool Have(LuaTable cls)
        {
            return cls.GetLuaFunction("Update") != null || cls.GetLuaFunction("LateUpdate") != null;
        }
        private LuaFunction update;
        private LuaFunction lateUpdate;
        public override void SetLuaTable(LuaTable lua)
        {
            base.SetLuaTable(lua);
            update = lua.GetLuaFunction("Update");
            lateUpdate = lua.GetLuaFunction("lateUpdate");
        }
        private void Update()
        {
            if (update != null)
                update.Call(lua);
        }
        private void LateUpdate()
        {
            if (lateUpdate != null)
                lateUpdate.Call(lua);
        }
    }
}
