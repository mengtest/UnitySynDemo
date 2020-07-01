using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// 加解密，文件相关的工具方法
/// </summary>
public class FileUtil
{
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

    public static void CreateDirectory(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("创建目录失败:" + path);
        }
    }
    public static void DeleteDirectory(string target_dir)
    {
        if (!Directory.Exists(target_dir)) return;
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);
        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }
        Directory.Delete(target_dir, true);
    }
    public static string sha256(string str)
    {
        byte[] SHA256Data = Encoding.UTF8.GetBytes(str);

        SHA256Managed Sha256 = new SHA256Managed();
        byte[] by = Sha256.ComputeHash(SHA256Data);

        return BitConverter.ToString(by).Replace("-", "").ToLower();
    }
    public static string sha256File(string path)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] by = Sha256.ComputeHash(fs);
            return BitConverter.ToString(by).Replace("-", "").ToLower();
        }
        catch (System.Exception ex)
        {
            Debug.Log("sha256File() fail, error:" + ex.Message);
        }
        finally
        {
            if (fs != null)
                fs.Close();
        }
        return "";
    }
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    public static string md5file(string file)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            md5.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("md5file() fail, error:" + ex.Message);
        }
        finally
        {
            if (fs != null)
                fs.Close();
        }
        return "";
    }

    public static string md5buff(byte[] buffs)
    {
        try
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffs);
            md5.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (System.Exception ex)
        {
            Debug.Log("md5file() fail, error:" + ex.Message);
        }
        return "";
    }
    public static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
    {
        bool ret = false;
        try
        {
            SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
            DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

            if (Directory.Exists(SourcePath))
            {
                if (Directory.Exists(DestinationPath) == false)
                    Directory.CreateDirectory(DestinationPath);

                foreach (string fls in Directory.GetFiles(SourcePath))
                {
                    FileInfo flinfo = new FileInfo(fls);
                    flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                }
                foreach (string drs in Directory.GetDirectories(SourcePath))
                {
                    DirectoryInfo drinfo = new DirectoryInfo(drs);
                    if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                        ret = false;
                }
            }
            ret = true;
        }
        catch (System.Exception ex)
        {
            ret = false;
        }
        return ret;
    }
    public static bool MoveDirectory(string SourcePath, string DestinationPath, bool delSource = true)
    {
        bool ret = true;
        try
        {
            SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
            DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

            if (Directory.Exists(SourcePath))
            {
                if (File.Exists(SourcePath + ".meta"))
                {
                    File.Move(SourcePath + ".meta", DestinationPath + ".meta");
                }
                if (!Directory.Exists(DestinationPath))
                    Directory.CreateDirectory(DestinationPath);

                foreach (string fls in Directory.GetFiles(SourcePath))
                {
                    FileInfo flinfo = new FileInfo(fls);
                    flinfo.MoveTo(DestinationPath + flinfo.Name);
                }
                foreach (string drs in Directory.GetDirectories(SourcePath))
                {
                    DirectoryInfo drinfo = new DirectoryInfo(drs);
                    if (MoveDirectory(drs, DestinationPath + drinfo.Name, delSource))
                        ret = false;
                }
            }
            else ret = false;
            if (ret && delSource)
            {
                Directory.Delete(SourcePath, true);
            }
        }
        catch (System.Exception ex)
        {
            ret = false;
        }
        return ret;
    }
        public static void DelEmptyDir(string path)
        {
            if (Directory.Exists(path) && Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length < 1)
                Directory.Delete(path, true);
        }



        public static void CreateDirectorySafely(string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

        public static void MoveFileSafely(string sourceFileName, string destFileName)
        {
            if (!File.Exists(sourceFileName))
            {
                DebugLog.Log("MoveFileSafely noFile: "+sourceFileName);
                return;
            }
            if (File.Exists(destFileName))
            {
                File.Delete(destFileName);
            }
            File.Move(sourceFileName, destFileName);
        }

        public static void CopyDirectorySafely(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(sourceDirName))
            {
                return;
            }
            if (Directory.Exists(destDirName))
            {
                Directory.Delete(destDirName);
            }
            Directory.Move(sourceDirName, destDirName);
        }

        public static void MoveDirectorySafely(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(sourceDirName))
            {
                return;
            }
            if (Directory.Exists(destDirName))
            {
                Directory.Delete(destDirName);
            }
            Directory.Move(sourceDirName, destDirName);
        }
        public static void ClearDirectory(string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
            {
                return;
            }
            string[] files = Directory.GetFiles(directoryPath);
            foreach(string file in files)
            {
                File.Delete(file);
            }
            string[] directories = Directory.GetDirectories(directoryPath);
            foreach(string directory in directories)
            {
                Directory.Delete(directory, true);
            }
        }

        public static void CreateNewDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
            Directory.CreateDirectory(directoryPath);
        }

        public static void DeleteFileSafely(string path)
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void CreateDir(string strDir)
        {
            string strTemp = strDir.Replace("\\", "/");
            if (!Directory.Exists(strTemp))
            {
                int pos = strDir.LastIndexOf('/');
                if (pos == -1)
                {
                    Directory.CreateDirectory(strTemp);
                }
                else
                {
                    CreateDir(strTemp.Substring(0, pos));
                    Directory.CreateDirectory(strTemp);
                }
            }
        }
        public static string GetPathDir(string path){
              string strTemp = path.Replace("\\", "/");
               int pos = path.LastIndexOf('/');
                if (pos == -1)
                {
                    return path;
                }else
                {
                     return strTemp.Substring(0, pos);
                }
        }

          /************************************************************************/
        /* 计算文件md5                                                          */
        /************************************************************************/
        public static string GetFileMD5(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(file);
            file.Close();
            string md5str = System.BitConverter.ToString(bytes);
            md5str = md5str.Replace("-", "");
            md5str = md5str.ToLower();
            return md5str;
        }
        /// <summary>
        /// 计算文件的MD5值(只取中间16位)
        /// </summary>
        public static string MD5File(string file, bool shorten = true)
        {
            string md5Hash = string.Empty;
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();
                md5Hash = System.BitConverter.ToString(retVal).Replace("-", "").ToLower();
                return shorten ? md5Hash.Substring(8, 16) : md5Hash;
            }
            catch (Exception ex)
            {
                throw new Exception("md5 file fail, error:" + ex.Message);
            }
        }
}
