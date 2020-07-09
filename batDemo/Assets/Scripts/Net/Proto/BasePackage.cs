namespace TcpSocket.Proto
{
    using System;
    using System.Text;
    using UnityEngine;

    public class BasePackage
    {

        public byte[] Tag; //2
        public uint Length; //4
        public byte[] Expand;//10
        public byte[] Body = null;// n
        //
        private const string TAG = "SP";

        public const int minPackageSize = 16;
        //
        private const int expandSize = 10;

        //构造函数1
        public BasePackage() : this(new byte[0])
        {

        }

        //构造函数2
        public BasePackage(string body) : this(System.Convert.FromBase64String(body))
        {
        }

        //构造函数3
        public BasePackage(byte[] body)
        {
            this.Tag = Encoding.UTF8.GetBytes(TAG);
            this.Expand = new byte[expandSize];
            if (body.Length > 0)
            {
                this.SetBody(body);
            }
        }

        //
        public void SetBody(byte[] body)
        {
            this.Length = (UInt32)body.Length;
            this.Body = body;
        }

        //序列化基础数据包
        public byte[] Marshal()
        {
            int l = minPackageSize + (int)this.Length;
            int pos = 0;
            //
            byte[] buffer = new byte[l];
            Array.Copy(this.Tag, 0, buffer, pos, 2);
            pos += 2;
            //
            byte[] lengthBytes = BitConverter.GetBytes(this.Length);
            Array.Copy(lengthBytes, 0, buffer, pos, 4);
            pos += 4;
            //
            Array.Copy(this.Expand, 0, buffer, pos, expandSize);
            pos += expandSize;
            //
            if (this.Length > 0)
            {
                Array.Copy(this.Body, 0, buffer, pos, this.Length);
            }
            //
            return buffer;
        }

        //反序列化基础数据包
        public static BasePackage Unmarshal(byte[] data)
        {
            if (data.Length < minPackageSize)
            {
                throw new Exception("Invalid base package");
            }
            //
            BasePackage pkg = new BasePackage();
            //
            int pos = 0;
            Array.Copy(data, 0, pkg.Tag, 0, 2);
            pos += 2;
            //
            if (TAG != Encoding.UTF8.GetString(pkg.Tag))
            {
                throw new Exception("Invalid base package with tag");
            }
            pkg.Length = BitConverter.ToUInt32(data, pos);
            pos += 4;
            //
            Array.Copy(data, pos, pkg.Expand, 0, expandSize);
            pos += expandSize;
            //
            if (pkg.Length <= 0)
            {
                return pkg;
            }
            //不足包的长度
            if (data.Length < minPackageSize + pkg.Length)
            {
                return pkg;
            }
            //
            pkg.Body = new byte[pkg.Length];
            Array.Copy(data, pos, pkg.Body, 0, pkg.Length);
            return pkg;
        }

        //反序列化基础数据包
        public static BasePackage UnmarshalHead(byte[] data)
        {
            if (data.Length < minPackageSize)
            {
                throw new Exception("Invalid base package");
            }
            //
            BasePackage pkg = new BasePackage();
            //
            int pos = 0;
            Array.Copy(data, 0, pkg.Tag, 0, 2);
            pos += 2;
            //
            if (TAG != Encoding.UTF8.GetString(pkg.Tag))
            {
                throw new Exception("Invalid base package with tag");
            }
            pkg.Length = BitConverter.ToUInt32(data, pos);
            pos += 4;
            //
            Array.Copy(data, pos, pkg.Expand, 0, expandSize);
            pos += expandSize;
            //
            return pkg;
        }
        //
        public string Dump()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
