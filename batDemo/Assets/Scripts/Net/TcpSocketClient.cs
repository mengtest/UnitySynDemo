using System.IO;
using Google.Protobuf;
using UnityEditor;
using pb = global::Google.Protobuf;

namespace TcpSocket
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using UnityEngine;
    using Proto;
    using LuaInterface;

    // todo 集成到lua 的时候去掉下面注释
    //using LuaInterface;

    // todo 集成到lua 的时候去掉下面注释
    [AutoRegistLua]
    public class TcpSocketClient
    {
        
        private static TcpSocketClient _instance = null;
        //10 秒
        private const int SEND_TIME_OUT = 30000;
        private const int RECEIVE_TIME_OUT = 30000;
        //
        private TcpClient client = null;
        private readonly byte[] receiveBytes = new byte[0];
        private NetworkStream outStream = null;
        //
        private AsyncCallback receiveAsyncCallback;
        private AsyncCallback writeAsyncCallback;
        private AsyncCallback connectAsyncCallback;
        //CMD注册器
        private CmdRegistrar mCmdRegistrar;
        private uint cmdPkgSerial = 0;
        //
        private bool isConnected = false;
        //
        private const string SOCKET_EXCEPTION = "onSocketException"; // 异常掉线
        //
        private static byte[] header;
        private int MaxMessageSize = 16 * 1024;
        static BasePackage mBasePackage = null;

        //
        public static TcpSocketClient GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            //
            _instance = new TcpSocketClient();
            return _instance;
        }

        //构造函数
        private TcpSocketClient()
        {
            this.writeAsyncCallback = new AsyncCallback(OnWrite);
            this.connectAsyncCallback = new AsyncCallback(OnConnect);
            this.mCmdRegistrar = new CmdRegistrar();
        }

        //注册处理方法
        public void RegisteHandle(uint cmd, Func<CmdPacket, bool> f, pb::MessageParser p)
        {
            this.mCmdRegistrar.RegisteHandle(cmd, f);
        }
        //
        public bool ConnectServer(string host, int port)
        {
            this.client = null;
            try
            {
                IPAddress[] address = Dns.GetHostAddresses(host);
                if (address.Length == 0)
                {
                    Debug.Log("host invalid");
                    return false;
                }

                if (address[0].AddressFamily == AddressFamily.InterNetworkV6)
                {
                    this.client = new TcpClient(AddressFamily.InterNetworkV6);
                }
                else
                {
                    this.client = new TcpClient(AddressFamily.InterNetwork);
                }
                //
                this.client.SendTimeout = SEND_TIME_OUT;
                this.client.ReceiveTimeout = RECEIVE_TIME_OUT;
                this.client.NoDelay = true;
                this.client.BeginConnect(host, port, this.connectAsyncCallback, null);
            }
            catch (Exception e)
            {
                OnDisconnected(SOCKET_EXCEPTION, "ConnectServer, " + e.Message);
            }

            return true;
        }

        //
        public void OnConnect(IAsyncResult asr)
        {
            try
            {
                this.outStream = this.client.GetStream();
                this.receiveAsyncCallback = new AsyncCallback(DoReceive);
                //this.client.ReceiveBufferSize = 4096;
                //this.receiveBytes = new byte[0];
                LuaHelper.PostMessage(LuaHelper.SOCKET_CONNECT, "");
                this.isConnected = true;
                this._doReceive();
            }
            catch (Exception e)
            {
                OnDisconnected(SOCKET_EXCEPTION, "OnConnect, " + e.Message);
            }
        }

        //
        public void DoReceive(IAsyncResult asr)
        {
            NetworkStream stream = client.GetStream();
            try
            {
                while (true)
                {
                    // read the next message (blocking) or stop if stream closed
                  //  byte[] content;
                    if (!ReadMessageBlocking(stream, MaxMessageSize, out mBasePackage))
                    {
                        // break instead of return so stream close still happens!
                        break;
                    }

                    //
                    //Debug.Log(mBasePackage.Dump());
                    byte[] tempByte = mBasePackage.Body;
                    //
                    CmdPacket cmdPkg = CmdPacket.Parser.ParseFrom(mBasePackage.Body);
                    //
                    //Debug.Log("c#层解析出来的协议内容：" + cmdPkg.ToString());
                    //todo 检测c#是否有注册，如果有则，调用c#
                    //Debug.LogError(cmdPkg.Head.TransType);

                    if (!this.mCmdRegistrar.DoHandle(cmdPkg))
                    {
                        //转到lua
                        string packetStr = System.Convert.ToBase64String(tempByte);
                        LuaByteBuffer tempBuf = Utils.DecodeBase64(packetStr);
                        LuaHelper.PostLuaByteMessage(LuaHelper.SOCKET_RECEIVE_MSG, tempBuf);
                    }

                }
            }
            catch (Exception exception)
            {
                // something went wrong. the thread was interrupted or the
                // connection closed or we closed our own connection or ...
                // -> either way we should stop gracefully
                Debug.Log("ReceiveLoop: finished receive function  reason: " + exception);
            }
            finally
            {
                // clean up no matter what
                _doReceive();
            }
        }

        //
        public void _doReceive()
        {

            //清空数组
            //Array.Clear(this.receiveBytes, 0, this.client.ReceiveBufferSize); 
            //try
            //{
            if (client != null)
            {
                this.client.GetStream().BeginRead(receiveBytes, 0, 0, this.receiveAsyncCallback, null);
            }
            //}
            //catch (Exception e)
            //{
            //    Debug.LogWarning("tcpsocket 接受数据，进程抛出异常：" + e.ToString());
            //}
        }

        // read message (via stream) with the <size,content> message structure
        static bool ReadMessageBlocking(NetworkStream stream, int MaxMessageSize, out BasePackage pkg)
        {
            pkg = null;
            // create header buffer if not created yet
            if (header == null)
            {
                header = new byte[BasePackage.minPackageSize];
            }
            // read exactly minPackageSize bytes for header (blocking)
            if (!stream.ReadExactly(header, BasePackage.minPackageSize))
            {
                return false;
            }
            try
            {
                pkg = BasePackage.UnmarshalHead(header);
                if (pkg.Length > 0)
                {
                    if (pkg.Length <= MaxMessageSize)
                    {
                        pkg.Body = new byte[pkg.Length];
                        return stream.ReadExactly(pkg.Body, (int)pkg.Length);
                    }
                    Debug.Log("ReadMessageBlocking: possible allocation attack with a header of: " + pkg.Length + " bytes.");
                }
            }
            catch (Exception e)
            {
                //Array.Clear(this.buffer, 0, this.bufferSize); 
                Debug.Log("ReadBasePackage Err: " + e.Message);
            }
            return false;
        }
        //---------------------------------------------
        //
        public void WriteMessage(byte[] message)
        {
            //Debug.Log("SocketClient WriteMessage length =" + message.Length);
            try
            {
                this.outStream.BeginWrite(message, 0, message.Length, this.writeAsyncCallback, null);
            }
            catch (Exception e)
            {
                OnDisconnected(SOCKET_EXCEPTION, "WriteMessage, " + e.Message);
            }
        }

        //
        public void OnWrite(IAsyncResult r)
        {
            try
            {
                this.outStream.EndWrite(r);
            }
            catch (Exception e)
            {
                OnDisconnected(SOCKET_EXCEPTION, "OnWrite, " + e.Message);
            }
        }

        //
        public void OnDisconnected(string protocal, string msg)
        {
            this.Close();
            LuaHelper.PostMessage(protocal, "");
            Debug.Log("SocketClient OnDisconnected err : " + msg);
        }

        //
        public void Close()
        {
            if (this.client != null)
            {
                if (this.isConnected)
                {
                    this.client.Close();
                    this.outStream.Close();
                    this.client = null;
                }

                this.isConnected = false;
            }
        }

        //发送节字节数组
        public void SendBytes(byte[] msgBytes)
        {
            try
            {
                BasePackage pkg = new BasePackage(msgBytes);
                //
                //Debug.Log(pkg.Dump());
                //
                WriteMessage(pkg.Marshal());
            }
            catch (Exception e)
            {
                Debug.Log("SendByte Err:" + e.Message);
            }
        }

        //
        public void Send(byte[] msg)
        {
            //byte[] msgBytes = System.Convert.FromBase64String(msg);
            this.SendBytes(msg);
        }

        //发送BP消息
        public void SendPbMsg(uint cmd, pb::IMessage msg)
        {
            cmdPkgSerial += 1;
            CmdPacket cmdPkg = new CmdPacket();
            cmdPkg.Head = new PkgHead();
            cmdPkg.Head.Cmd = cmd;
            cmdPkg.Head.TransType = TRANS_TYPE.Request;
            cmdPkg.Head.Serial = cmdPkgSerial;
            if (msg != null)
            {
                cmdPkg.Body = msg.ToByteString();
            }
            byte[] data = cmdPkg.ToByteArray();
            this.SendBytes(data);
        }

        //
        //todo 集成到lua 打开下面注释
        public void SendConnect(string host, int port)
        {
            string socketHost = host;
            int socketPort = port;
            if (socketHost == null)
            {
                socketHost = GameConst.SocketAddress;
            }

            if (port == 0)
            {
                socketPort = GameConst.SocketPort;
            }
            ConnectServer(socketHost, socketPort);
        }
        //是否已经连接
        public bool IsConnected()
        {
            return isConnected;
        }
    }
}
