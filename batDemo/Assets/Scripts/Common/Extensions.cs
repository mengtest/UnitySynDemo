using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using UnityEngine;



public static class Extensions
{
    #region 扩展方法
    public static Vector2 ToVector2(this String value)
    {
        string v = TrimBracket(value);
        string[] parts = v.Split(',');
        float x = Convert.ToSingle(parts[0]);
        float y = Convert.ToSingle(parts[1]);
        return new Vector2(x, y);
    }

    public static Vector3 ToVector3(this String value)
    {
        string v = TrimBracket(value);
        string[] parts = v.Split(',');
        float x = Convert.ToSingle(parts[0]);
        float y = Convert.ToSingle(parts[1]);
        float z = Convert.ToSingle(parts[2]);
        return new Vector3(x, y, z);
    }

    public static Vector4 ToVector4(this String value)
    {
        string v = TrimBracket(value);
        string[] parts = v.Split(',');
        float x = Convert.ToSingle(parts[0]);
        float y = Convert.ToSingle(parts[1]);
        float z = Convert.ToSingle(parts[2]);
        float w = Convert.ToSingle(parts[3]);
        return new Vector4(x, y, z, w);
    }

    public static Quaternion ToQuaternion(this String value)
    {
        return Quaternion.Euler(value.ToVector3());
    }

    public static bool Contains(this XmlNode node, string attributeName)
    {
        return node.Attributes[attributeName] != null;
    }
    #endregion

    #region 帮助方法
    static string TrimBracket(string source)
    {
        if (source.StartsWith("{"))
            source = source.TrimStart('{');

        if (source.EndsWith("}"))
            source = source.TrimEnd('}');

        return source;
    }
    #endregion
}
