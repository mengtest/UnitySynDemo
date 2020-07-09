namespace TcpSocket
{
    using LuaInterface;
    using UnityEngine;
    //using LuaInterface;
    public class LuaHelper
    {
        public const string LUA_NETWORK_MODULE = "NetworkManager:";
        public const string SOCKET_CONNECT = "onSocketConnect";                 // 连接服务器
        public const string SOCKET_EXCEPTION = "onSocketException";             // 异常掉线
        public const string SOCKET_DISCONNECT = "onSocketDisconnect";           // 正常断线
        public const string SOCKET_RECEIVE_MSG = "onSocketReceiveMsg";          // 收到消息
        public const string SOCKET_FINISH_SEND_MSG = "onSocketFinishSendMsg";   // 发送完消息
        //
        public static void PostMessage(string funcName, string str)
        {
            LuaFuncManager.dispatchCustomEvent(LUA_NETWORK_MODULE + funcName, str);
        }

        public static void PostByteMessage(string funcName, byte[] bytes )
        {
            LuaFuncManager.dispatchCustomEvent(LUA_NETWORK_MODULE + funcName, bytes);
        }

        public static void PostLuaByteMessage(string funcName, LuaByteBuffer str)
        {
            LuaFuncManager.dispatchCustomEvent(LUA_NETWORK_MODULE + funcName, str);
        }
    }
}