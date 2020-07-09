using System;
using Google.Protobuf.WellKnownTypes;
using UnityEditor;
using UnityEngine;
using pb = global::Google.Protobuf;
namespace TcpSocket
{
    using System.Collections.Generic;
    //CMD 注册器
    public class CmdRegistrar
    {
        
        private Dictionary<uint, Func<Proto.CmdPacket,bool>> CmdMap;
        
        //
        public CmdRegistrar()
        {
            this.CmdMap = new Dictionary<uint, Func<Proto.CmdPacket,bool>>();
        }

        //ex: Proto.CmdPacket.Parser
        public void RegisteHandle(uint cmd,Func<Proto.CmdPacket,bool> f)
        {
            this.CmdMap.Add(cmd,f);
        }
        
        
        //
        public bool UnRegisteHandle(uint cmd)
        {
            return this.CmdMap.Remove(cmd);
        }
        
        //
        public bool DoHandle(Proto.CmdPacket cmdPkg)
        {
            if (!this.CmdMap.ContainsKey(cmdPkg.Head.Cmd))
            {
                return false;
            }
            //
            Func<Proto.CmdPacket,bool> f = this.CmdMap[cmdPkg.Head.Cmd];
            f(cmdPkg);
            return true;
        }
    }
}