using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
//using LuaInterface;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GameUtils
{

    public static Vector2 GetResolution()
    {
        Vector2 resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
#if UNITY_EDITOR
        resolution = UnityEditor.Handles.GetMainGameViewSize();
#endif
        return resolution;
    }

    public static void SaveHeadImage(byte[] imageData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "TargetIamge.png");
        FileEx.WriteAllBytes(filePath, imageData);
    }

    // // byte 读取文件
	// public static LuaByteBuffer ReadFileLuaByte(string filePath)
	// {
    //     byte[] bytes = File.ReadAllBytes(filePath);
    //     return new LuaByteBuffer(bytes);
	// }

    // base64加密
	public static string EncodeBase64(byte[] bytes)
	{
        return System.Convert.ToBase64String(bytes);
	}

    // // base64解密
	// public static LuaByteBuffer DecodeBase64(string str64)
	// {
    //     byte[] bytes = System.Convert.FromBase64String(str64);
    //     return new LuaByteBuffer(bytes);
	// }

    public static void CheckEditorDebugFile()
    {
#if UNITY_EDITOR
         string filePath = "Assets/Scripts/Lua/EditorDebug.lua";
        if(!File.Exists(filePath))
        {
            string templateFilePath = "Assets/Editor/Template/EditorDebugTemplate.lua";
            string template = File.ReadAllText(templateFilePath);
            FileEx.WriteAllText(filePath, template);
            AssetDatabase.Refresh();
        }
#endif
    }

    // aes加密
    public static byte[] AesEncrypt(byte[] value, string password)
    {
        byte[] VIKey = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(password), 0, VIKey, 0, 16);

        var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };
        var encryptor = symmetricKey.CreateEncryptor(Encoding.UTF8.GetBytes(password), VIKey);

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(value, 0, value.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();
                memoryStream.Close();

                return memoryStream.ToArray();
            }
        }
    }

    // aes解密
    public static byte[] AesDecrypt(byte[] value, string password)
    {
        byte[] VIKey = new byte[16];
        Array.Copy(Encoding.UTF8.GetBytes(password), 0, VIKey, 0, 16);

        var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };
        var decryptor = symmetricKey.CreateDecryptor(Encoding.UTF8.GetBytes(password), VIKey);

        using (var memoryStream = new MemoryStream(value))
        {
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                byte[] plainTextBytes = new byte[value.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                memoryStream.Close();
                cryptoStream.Close();

                byte[] resultBytes = new byte[decryptedByteCount];
                Array.Copy(plainTextBytes, 0, resultBytes, 0, decryptedByteCount);

                return resultBytes;
            }
        }
    }

    public static int GetVersionCode(string version)
    {
        string[] codes = version.Split('.');
        int versionCode = 0;
        if (codes.Length >= 3)
        {
            versionCode += Convert.ToInt32(codes[0]) * 1000000;
            versionCode += Convert.ToInt32(codes[1]) * 10000;
            versionCode += Convert.ToInt32(codes[2]) * 100;
            if (codes.Length >= 4)
            {
                versionCode += Convert.ToInt32(codes[3]);
            }
        }
        return versionCode;
    }

    public static string TrimFullVersionToShortVersion(string fullVersion)
    {
        string[] codes = fullVersion.Split('.');
        if (codes.Length >= 4)
        {
            return string.Join(".", codes, 0, 3);
        }
        return fullVersion;
    }

   
    static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static long ConvertUTCDateTimeToMillisecond(DateTime utcTime)
    {
        return (long)(utcTime - Jan1St1970).TotalMilliseconds;
    }

    public static int ConvertUTCDateTimeToSecond(DateTime utcTime)
    {
        return (int)(utcTime - Jan1St1970).TotalSeconds;
    }

    public static string GetTransformPath(Transform transform, bool editorOnly = true)
    {
        if (!editorOnly || Application.isEditor)
        {
            List<string> paths = new List<string>();
            while (transform != null)
            {
                paths.Insert(0, transform.name);
                transform = transform.parent;
            }
            return string.Join("/", paths.ToArray());
        }
        else
        {
            return transform.name;
        }
    }
    public static Transform FindChildRecursively(Transform parent, string name)
    {
        Transform t = null;
        t = parent.Find(name);
        if (t == null)
        {
            foreach (Transform tran in parent)
            {
                t =  FindChildRecursively(tran, name);
                if (t != null)
                {
                    return t;
                }
            }
        }

        return t;
    }
    
}
