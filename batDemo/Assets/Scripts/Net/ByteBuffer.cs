namespace TcpSocket
{
    using System;
    using TcpSocket.Proto;
    using UnityEngine;

    public class ByteBuffer
    {
        //字节缓存区
        private byte[] buffer;
        //有效长度
        private uint bufferLen = 0;
        //
        private int bufferSize = 0;

        /**
         * 构造方法
         */
        public ByteBuffer(int capacity)
        {
            this.buffer = new byte[capacity];
            this.bufferSize = capacity;
        }

        //
        public uint Length()
        {
            return this.bufferLen;
        }

        //
        public uint Write(byte[] data)
        {
            lock (this)
            {
                Array.Copy(data, 0, this.buffer, this.bufferLen, data.Length);
                this.bufferLen += (uint) data.Length;
            }

            return this.bufferLen;
        }

        //
        public byte[] Read(int len)
        {
            if (len <= 0)
            {
                throw new Exception("read len can not less then 0");
            }

            //
            byte[] buf = new byte[len];
            lock (this)
            {
                if (this.bufferLen < len)
                {
                    throw new Exception("buf is too less");
                }

                Array.Copy(this.buffer, 0, buf, 0, len);
                //
                this.bufferLen = this.bufferLen - (uint) len;
                Array.Copy(this.buffer, len, this.buffer, 0, this.bufferSize - len);
            }

            return buf;
        }

        //读取一个独立的数据包
        //此处已经做了拼包处理。
        public BasePackage ReadBasePackage()
        {
            BasePackage pkg = null;
            if (this.bufferLen < BasePackage.minPackageSize)
            {
                return null;
            }

            byte[] headBuf = new byte[BasePackage.minPackageSize];
            lock (this)
            {
                try
                {
                    uint pos = 0;
                    Array.Copy(this.buffer, pos, headBuf, 0, BasePackage.minPackageSize);
                    pos += BasePackage.minPackageSize;
                    //
                    pkg = BasePackage.Unmarshal(headBuf);
                    if (pkg.Length > 0 && this.bufferLen < pkg.Length + BasePackage.minPackageSize)
                    {
                        return null;
                    }
                    //
                    pkg.Body = new byte[pkg.Length];
                    Array.Copy(this.buffer, pos, pkg.Body, 0, pkg.Length);
                    pos += pkg.Length;
                    //
                    Array.Copy(this.buffer, pos, this.buffer, 0, this.bufferSize - pos);
                    this.bufferLen -= pos;
                    //
                    //Debug.Log(this.bufferLen);
                }catch(Exception e){
                    Array.Clear(this.buffer, 0, this.bufferSize); 
                    Debug.Log("ReadBasePackage Err: "+e.Message);
                }
            }
            return pkg;
        }
    }
}