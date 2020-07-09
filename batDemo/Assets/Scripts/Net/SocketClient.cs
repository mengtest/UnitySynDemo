



////=======================此Socket连接已经废弃，使用新的TcpSocketClient连接




//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using UnityEngine;
//using LuaInterface;
//using System.Text;

//[AutoRegistLua]
//public class SocketClient
//{
//	private static SocketClient _instance = null;
//	public static SocketClient Instance { get { if (_instance == null) _instance = new SocketClient(); return _instance; } }
//	private TcpClient client = null;
//    private NetworkStream outStream = null;
//    private MemoryStream memStream;
//    private BinaryReader reader;

//    private bool useCrypt = false;

//    private byte[] receiveBytes = null;

//	private const int SEND_TIME_OUT = 10000;
//	private const int RECEIVE_TIME_OUT = 10000;

//    private const string LUA_NETWORK_MODULE = "NetworkManager:";
//    private const string SOCKET_CONNECT = "onSocketConnect";                 // 连接服务器
//    private const string SOCKET_EXCEPTION = "onSocketException";             // 异常掉线
//    private const string SOCKET_DISCONNECT = "onSocketDisconnect";           // 正常断线
//    private const string SOCKET_RECEIVE_MSG = "onSocketReceiveMsg";          // 收到消息
//    private const string SOCKET_FINISH_SEND_MSG = "onSocketFinishSendMsg";   // 发送完消息

//    // 默认加密key
//    private static string DefaultAesCryptKey = "0c286e90da5b581b26a3b7e970c2d535";
//    // 动态加密key
//    public string dynAesCryptKey = null;
//    // 上一个动态加密key
//    public string lastDynAesCryptKey = null;

//	public void Initialize()
//	{
//		if (memStream == null)
//		{
//            memStream = new MemoryStream();
//		}
//        if (reader == null)
//        {
//            reader = new BinaryReader(memStream);
//        }
//	}

//    public void SetDynSymCryptKey (string cryptKey)
//    {
//        if (cryptKey != null && cryptKey != "")
//        {
//            if (dynAesCryptKey != null)
//            {
//                lastDynAesCryptKey = dynAesCryptKey;
//            }
//            dynAesCryptKey = cryptKey;
//        }
//    }

//    public void CleanDynSymCryptKey ()
//    {
//        dynAesCryptKey = null;
//    }

//    public void SetUseCrypt(bool isUse)
//    {
//        useCrypt = isUse;
//    }

//    public bool IsUseCrypt()
//    {
//        return useCrypt;
//    }

//    private string GetAesCryptKey()
//    {
//        return dynAesCryptKey != null ?  dynAesCryptKey : DefaultAesCryptKey;
//    }

//    void ConnectServer(string host, int port) {
//		//Debug.LogError("host =" + host + " port =" + port);
//        client = null;
//        try {
//            IPAddress[] address = Dns.GetHostAddresses(host);
//            if (address.Length == 0) {
//                Debug.Log("host invalid");
//                return;
//            }
//            if (address[0].AddressFamily == AddressFamily.InterNetworkV6) {
//                client = new TcpClient(AddressFamily.InterNetworkV6);
//            }
//			else {
//                client = new TcpClient(AddressFamily.InterNetwork);
//            }
//            client.SendTimeout = SEND_TIME_OUT;
//            client.ReceiveTimeout = RECEIVE_TIME_OUT;
//            client.NoDelay = true;
//            client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
//        } catch (Exception e) {
//            OnDisconnected(SOCKET_EXCEPTION, "ConnectServer, "+e.Message);
//        }
//    }

//	protected void PostMessage(string funcName, string str)
//	{
//		LuaFuncManager.dispatchCustomEvent(LUA_NETWORK_MODULE + funcName, str);
//	}

//	protected void PostByteMessage(string funcName, byte[] bytes )
//	{
//        LuaFuncManager.dispatchCustomEvent(LUA_NETWORK_MODULE+funcName, bytes);
//	}

//    void OnConnect(IAsyncResult asr)
//    {
//		memStream.SetLength(0);
//		try {
//			outStream = client.GetStream();
//            receiveBytes = new byte[client.ReceiveBufferSize];
//			client.GetStream().BeginRead(receiveBytes, 0, System.Convert.ToInt32(client.ReceiveBufferSize), new AsyncCallback (OnRead), null);
//			PostMessage(SOCKET_CONNECT,  "");
//		} catch (Exception e) {
//			OnDisconnected(SOCKET_EXCEPTION, "OnConnect, "+e.Message);
//		}
//    }

//    void WriteMessage(byte[] message)
//    {
//		//Debug.Log("SocketClient WriteMessage length =" + message.Length);
//		try
//		{
//			outStream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
//		}catch (Exception e) {
//			OnDisconnected(SOCKET_EXCEPTION, "WriteMessage, "+e.Message);
//        }
//    }

//    void OnRead(IAsyncResult asr)
//    {
		
//		int bytesRead = 0;
//        try {
//            // if (client.GetStream().DataAvailable == false)
//            // {
//            //     OnDisconnected(SOCKET_EXCEPTION, "data is unavailable");
//            //     return;
//            // }
//            lock (client.GetStream()) {         //读取字节流到缓冲区
//                bytesRead = client.GetStream().EndRead(asr);
//            }
//            if (bytesRead < 1) {
//				//包尺寸有问题，断线处理
//				Debug.Log("SocketClient OnRead connected");
//				// OnDisconnected(SOCKET_EXCEPTION, "bytesRead < 1");
//				return;
//            }
//            OnReceive(receiveBytes, bytesRead);   //分析数据包内容，抛给逻辑层
//            lock (client.GetStream()) {         //分析完，再次监听服务器发过来的新消息
//				Array.Clear(receiveBytes, 0, receiveBytes.Length);   //清空数组
//                receiveBytes = new byte[client.ReceiveBufferSize];
//				client.GetStream().BeginRead(receiveBytes, 0, System.Convert.ToInt32(client.ReceiveBufferSize), new AsyncCallback(OnRead), null);
//            }
//        } catch (Exception e) {
//            OnDisconnected(SOCKET_EXCEPTION, "OnReadTry, "+e.Message);
//        }
//    }

//	void OnDisconnected(string protocal, string msg)
//    {
//        Close();
//		PostMessage(protocal, "");
//		Debug.Log("SocketClient OnDisconnected err : " + msg);
//    }

//    void OnWrite(IAsyncResult r)
//    {
//        try {
//            outStream.EndWrite(r);
//        } catch (Exception e) {
//			OnDisconnected(SOCKET_EXCEPTION, "OnWrite, "+e.Message);
//        }
//    }

//	void OnReceive(byte[] bytes, int length)
//	{
//		//Debug.LogError("OnReceive: " + length);
//		if (length > 16)
//		{
//			byte[] packetLenthBytes = new byte[4];
//			Array.Copy(bytes, 2, packetLenthBytes, 0, 4);

//			int packetLenth = System.BitConverter.ToInt32(packetLenthBytes, 0);

//			byte[] packetBytes = new byte[packetLenth];
//			Array.Copy(bytes, 16, packetBytes, 0, packetLenth);
//			if (packetBytes.Length <= 0)
//			{
//				Debug.LogError("SocketClient onReceive length <= 0 error ");
//				return;
//			}

//			var str111 = "[";
//			for (var i = 0; i < packetBytes.Length; ++i)
//			{
//				str111 += packetBytes[i] + ",";
//			}
//			str111 += "]";
//			//Debug.LogError(str111);
			
//			string packetStr;
//			if (IsUseCrypt())
//			{
//				byte[] aesBytes = null;
//				try
//				{
//					aesBytes = FileUtil.AesDecrypt(packetBytes, GetAesCryptKey());
//				}
//				catch (Exception e)
//				{
//					string lastKey = lastDynAesCryptKey != null ? lastDynAesCryptKey : DefaultAesCryptKey;
//					aesBytes = FileUtil.AesDecrypt(packetBytes, lastKey);
//				}
//				packetStr = System.Convert.ToBase64String(aesBytes);
//			}
//			else
//			{
//				packetStr = System.Convert.ToBase64String(packetBytes);
//			}
//			// Debug.Log("*** packetStr : " + packetStr);
//			PostMessage(SOCKET_RECEIVE_MSG, packetStr);
//        }else {
//            Debug.Log("SocketClient OnReceive Log msg");
//        }
//    }

//    public void Close() 
//    {
//        if (client != null) {
//			TcpClient tmp = client;
//			client = null;
//			if (tmp.Connected) {
//				tmp.Close();
//				outStream.Close();
//			}
//        }
//    }
//	public void SendByte(byte[] msgBytes)
//	{
//		byte[] magicBytes = Encoding.UTF8.GetBytes("BM");

//		byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);
//		// Array.Reverse(lengthBytes); // 反转数组转成大端

//		byte[] reservedBytes = new byte[10];

//		byte[] buffer = new byte[magicBytes.Length + lengthBytes.Length + reservedBytes.Length + msgBytes.Length];
//		magicBytes.CopyTo(buffer, 0);
//		lengthBytes.CopyTo(buffer, magicBytes.Length);
//		reservedBytes.CopyTo(buffer, magicBytes.Length + lengthBytes.Length);
//		msgBytes.CopyTo(buffer, magicBytes.Length + lengthBytes.Length + reservedBytes.Length);
//		WriteMessage(buffer);
//	}

//	public void Send(string msg)
//    {
//        //Debug.LogError("Send123 " + msg);
//        byte[] msgBytes = System.Convert.FromBase64String(msg);
//		byte[] magicBytes = Encoding.UTF8.GetBytes("SP");

//		byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);
//		// Array.Reverse(lengthBytes); // 反转数组转成大端

//		byte[] reservedBytes = new byte[10];

//		byte[] buffer = new byte[magicBytes.Length + lengthBytes.Length + reservedBytes.Length + msgBytes.Length];
//		magicBytes.CopyTo(buffer, 0);
//		lengthBytes.CopyTo(buffer, magicBytes.Length);
//		reservedBytes.CopyTo(buffer, magicBytes.Length + lengthBytes.Length);
//		msgBytes.CopyTo(buffer, magicBytes.Length + lengthBytes.Length + reservedBytes.Length);
//		WriteMessage(buffer);
//	}

//    // test functio
//	public void logByte(byte[] by, string name)
//	{
//		string byteName = name != null ? name : "byte";
//		Debug.Log("*** " + byteName + ".Length : " + by.Length);
//		string byteValue = "";
//		for (int i = 0; i < by.Length; i++)
//		{
//			byteValue = byteValue + by[i] + ",";
//			// Debug.Log("*** "+byteName+"["+i+"] : " + by[i]);
//		}
//		Debug.Log("*** " + byteName + " : " + byteValue);
//	}

//	public void SendConnect(string host, int port)
//    {
//        string socketHost = host;
//        int socketPort = port;
//        if (socketHost == null)
//        {
//            socketHost = GameConst.SocketAddress;
//        }

//        if (port == 0)
//        {
//            socketPort = GameConst.SocketPort;
//        }
//        ConnectServer(socketHost, socketPort);
//    }
//}
