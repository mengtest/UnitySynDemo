using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;

public static class FileEx
{
    public static string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public static byte[] ReadAllBytes(string path)
    {
        return File.ReadAllBytes(path);
    }

    public static void WriteAllText(string path, string contents, Encoding encoding, bool setNoBackupFlagOnIOS = true)
    {
        Write(path, setNoBackupFlagOnIOS, () => File.WriteAllText(path, contents, encoding));
    }

    public static void WriteAllText(string path, string contents, bool setNoBackupFlagOnIOS = true)
    {
        Write(path, setNoBackupFlagOnIOS, () => File.WriteAllText(path, contents));
    }

    public static void WriteAllBytes(string path, byte[] bytes, bool setNoBackupFlagOnIOS = true)
    {
        Write(path, setNoBackupFlagOnIOS, () => File.WriteAllBytes(path, bytes));
    }

    static void Write(string path, bool setNoBackupFlagOnIOS, Action action)
    {
        string directory = Path.GetDirectoryName(path);

        try
        {
            EnsureDirectoryExist(directory);
            action(); //UnauthorizedAccessException
#if UNITY_IOS
            if (setNoBackupFlagOnIOS)
            {
                UnityEngine.iOS.Device.SetNoBackupFlag(path);
            }
#endif
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
    }

    public static void EnsureDirectoryExist(string directory)
    {
        if (!Directory.Exists(directory))
        {
            try
            {
                Directory.CreateDirectory(directory);
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }
    }

    //����ļ�����/���д���ʱ��
    public static DateTime GetLastWriteTime(string path)
    {
        DateTime creationTime;
        if (!File.Exists(path))
        {
            return DateTime.Now;
        }
        FileInfo fileInfo = new FileInfo(path);
        creationTime = fileInfo.LastWriteTime;
        return creationTime;
    }
}
