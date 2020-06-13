//*************************************************************************
//	创建日期:	2015-7-4   12:10
//	文件名称:	centerstarint.cs
//  创 建 人:   Even	
//	版权所有:	星辰时代
//	说    明:	防内存修改整数实现
//*************************************************************************
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public struct CenterStarInt
{
    public static int magic = 0;
    private int m_nData;

    public CenterStarInt(int v) { 
        m_nData = v ^ magic; 
    }

    static public implicit operator CenterStarInt(int v) { return new CenterStarInt(v); }
    //////////////////////////////////////////////////////////////////////////////
    static public explicit operator int(CenterStarInt n) { return n.Number; }

    public static void Init() {
        if (magic == 0)
        {
            magic = Random.Range(1000, 10000);
        } 
    }
    public override bool Equals(object obj)
    {
        return obj is int && this.Number == (int)obj;
    }

    public override int GetHashCode()
    {
        return this.Number.GetHashCode();
    }

    public int Number
    {
//         set
//         {
//             m_nData = value ^ magic;
//         }
        get
        {
            return m_nData ^ magic;
        }
    }
}