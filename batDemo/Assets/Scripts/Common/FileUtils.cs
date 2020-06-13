//*************************************************************************
//	创建日期:	2015-7-4   12:11
//	文件名称:	fileutil.cs
//  创 建 人:   Even	
//	版权所有:	星辰时代
//	说    明:	文件操作
//*************************************************************************
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;


    /// <summary>
    /// 文件工具类 支持读取文本文件
    /// </summary>
    public class FileUtils
    {
        // 复制文件回调
        public delegate void CopyFileCallback(string strFileName);


        public static string FormatDirString(string strDir)
        {
            DirectoryInfo dir = new DirectoryInfo(strDir);
            string strRet = dir.FullName;
            strRet = strRet.Replace("\\", "/");
            return strRet;
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


        /// <summary>
        /// 写文件 写可读可写空间里的文本文件
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="info"></param>
        public static void WriteStreamFile(ref string strName, string info)
        {
            WriteStreamFile(ref strName, info, true);
        }

        /**   用于写可读写空间的文本文件
           * name：文件的名称    
           *  info：写入的内容    
           */
        public static void WriteStreamFile(ref string strName, string info, bool overwrite)
        {
            //文件流信息  
            int pos = strName.LastIndexOf('/');
            string strDir = "";
            if (pos > 0)
            {
                strDir = strName.Substring(0, pos);
                CreateDir(strDir);
            }

            StreamWriter sw = null;
            FileInfo t = null;
            try
            {
                t = new FileInfo(strName);
                if (!t.Exists)
                {
                    //如果此文件不存在则创建

                    sw = t.CreateText();
                }
                else
                {
                    if (overwrite)
                    {
                        sw = t.CreateText();
                    }
                    else
                    {
                        sw = t.AppendText();
                    }
                }
                sw.Write(info);

            }
            finally
            {
                //关闭流  
                sw.Close();
                //销毁流  
                sw.Dispose();
                t = null;
            }
        }

        /**   用于写可读写空间的二进制文件
        * name：文件的名称    
        *  info：写入的内容    
        */
        public static void WriteBinaryFile(ref string strName, byte[] buff, int nLen, bool overwrite)
        {
            int pos = strName.LastIndexOf('/');
            string strDir = "";
            if (pos > 0)
            {
                strDir = strName.Substring(0, pos);
                CreateDir(strDir);
            }

            //文件流信息  
            FileStream fs = new FileStream(strName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            if (fs == null)
            {
                return;
            }
            if (bw == null)
            {
                return;
            }
            try
            {
                bw.Write(buff);
            }
            finally
            {
                //关闭流 
                bw.Close();
                fs.Close();
            }
       
        }

        /// <summary>
        /// 写入一行 只适用来文本文件
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strInfo"></param>
        public static void WriteLine(ref string strName, string strInfo, bool overwrite)
        {
            int pos = strName.LastIndexOf('/');
            string strDir = "";
            if (pos > 0)
            {
                strDir = strName.Substring(0, pos);
                CreateDir(strDir);
            }

            StreamWriter sw = null;
            FileInfo t = null;
            try
            {
                //文件流信息  
                t = new FileInfo(strName);
                if (!t.Exists)
                {
                    //如果此文件不存在则创建  
                    sw = t.CreateText();
                }
                else
                {
                    if (overwrite)
                    {
                        sw = t.CreateText();
                    }
                    else
                    {
                        sw = t.AppendText();
                    }
                }
                sw.WriteLine(strInfo);  // 写入一行
            }
            finally
            {
                if (sw != null)
                {
                    //关闭流  
                    sw.Close();
                    //销毁流  
                    sw.Dispose();
                }
            }
        }

        /**   用于读可读写空间的文件 
       * name：读取文件的名称  lstInfo 返回一行行的字符串  
       */
        public static bool ReadStreamFile(string strName, out ArrayList lstInfo)
        {
            lstInfo = new ArrayList();

            FileInfo fi = new FileInfo(strName);
            if (!fi.Exists)
            {
                return false;
            }
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(strName);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空 
                Debug.LogError("ReadStreamFile Err:" + strName + e.ToString());
                //关闭流  
                sr.Close();
                //销毁流  
                sr.Dispose();
                return false;
            }

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                lstInfo.Add(line);
            }

            //关闭流  
            sr.Close();
            //销毁流  
            sr.Dispose();
            //将数组链表容器返回     
            return true;
        }

        /**   用于读可读写空间的文件 
        * name：读取文件的名称   返回整个文件内容
        */
        public static string ReadStreamFile(string strName)
        {
            FileInfo fi = new FileInfo(strName);
            if (!fi.Exists)
            {
                return "";
            }
            //使用流的形式读取
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(strName);
            }
            catch (Exception e)
            {
                //路径与名称未找到文件则直接返回空 
                Debug.LogError("ReadStreamFile Err:" + strName + e.ToString());
                //关闭流  
                sr.Close();
                //销毁流  
                sr.Dispose();
                return "";
            }

            string strContent = sr.ReadToEnd();

            //关闭流  
            sr.Close();
            //销毁流  
            sr.Dispose();
            //将数组链表容器返回     
            return strContent;
        }

        /**   用于删除可读写空间的文件 
       * name：删除文件的名称    
       */
        public static void DeleteFile(string strName)
        {
            FileInfo fi = new FileInfo(strName);
            if( fi.Exists )
            {
                fi.Delete();
            }
        }

        // 复制StreamAsset目录下文本文件到可读可写空间
        public static bool CopyStreamFileToPersistent(string strSrcFile, string strDestFile)
        {
            WWW w = new WWW(strSrcFile);
            {
                if (string.IsNullOrEmpty(w.error))
                {
                    while (w.isDone == false)
                    {
                    }

                    WriteStreamFile(ref strDestFile, w.text, true);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        public static bool CopyBinaryFileToPersistent(string strSrcFile, string strDestFile)
        {
            WWW w = new WWW(strSrcFile);
            {
                if (string.IsNullOrEmpty(w.error))
                {
                    while (w.isDone == false)
                    {
                    }

                    if (File.Exists(strDestFile))
                    {
                        File.Delete(strDestFile);
                    }

                    WriteBinaryFile(ref strDestFile, w.bytes, w.bytes.Length, true);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        // 统计指定目录下文件的数量
        public static int GetFileCount(string strDir)
        {
            strDir.Replace("\\", "/");
            DirectoryInfo dir = new DirectoryInfo(strDir);
            if (dir == null)
            {
                return 0;
            }
            if (!dir.Exists)
            {
                return 0;
            }

            int nFileNum = 0;
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                {
                    nFileNum++;
                }
                else
                {
                    string strFullDirName = files[i].FullName;
                    nFileNum += GetFileCount(strFullDirName);
                }
            }

            return nFileNum;
        }

        // 获取目录下文件列表
        public static void GetFileList(string strDir, ref List<string> lstFile)
        {
            strDir.Replace("\\", "/");
            DirectoryInfo dir = new DirectoryInfo(strDir);
            if (dir == null)
            {
                return;
            }
            if (!dir.Exists)
            {
                return;
            }

            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                {
                    lstFile.Add(file.FullName);
                }
                else
                {
                    string strFullDirName = files[i].FullName;
                    GetFileList(strFullDirName, ref lstFile);
                }
            }
        }

        // 拷贝目录下所有文件和文件夹
        public static void CopyDirectory(string strSrcDir, string strDestDir, ref List<string> lstFile, CopyFileCallback callback = null)
        {
            strSrcDir.Replace("\\", "/");
            strDestDir.Replace("\\", "/");
            FileSystemInfo[] files = null;
            DirectoryInfo src = new DirectoryInfo(strSrcDir);
            if (src.Exists)
            {
                DirectoryInfo dir = src as DirectoryInfo;
                //不是目录 
                if (dir == null) return;
                files = dir.GetFileSystemInfos();
            }
            else
            {
                if (!File.Exists(strSrcDir))
                {
                    return;
                }
                FileSystemInfo info = new FileInfo(strSrcDir);
                files = new FileSystemInfo[] { info };
            }
            DirectoryInfo dest = new DirectoryInfo(strDestDir);
            if (!dest.Exists)
            {
                CreateDir(strDestDir);
            }

            if (strSrcDir.LastIndexOf('/') != strSrcDir.Length - 1)
            {
                strSrcDir += "/";
            }
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                {
                    string strDestFileName = strDestDir + "/";
                    strDestFileName += file.Name;
                    file.CopyTo(strDestFileName, true);
                    lstFile.Add(file.FullName);

                    if (callback != null)
                    {
                        callback(file.FullName);
                    }
                }
                else
                {
                    string strFullDirName = files[i].FullName;
                    strFullDirName = strFullDirName.Replace("\\", "/");
                    DirectoryInfo srcDir = new DirectoryInfo(strSrcDir);
                    strSrcDir = srcDir.FullName.Replace("\\", "/");
                    string strDir = strFullDirName.Replace(strSrcDir, "");
                    string strSubDir = strDestDir + "/";
                    strSubDir += strDir;

                    CopyDirectory(strFullDirName, strSubDir, ref lstFile, callback);
                }
            }
        }

        /// <summary>
        /// 写入文件，和Copy的功能一样
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        public static void WriteFile(string fromPath, string toPath)
        {
            DirectoryInfo dest = new DirectoryInfo(toPath);
            if (!dest.Exists)
            {
                CreateDir(toPath);
            }
            if (toPath.LastIndexOf('/') != toPath.Length - 1)
            {
                toPath += "/";
            }
            string fileName = fromPath;
            if(fromPath.Contains("/"))
            {
                int pos = fromPath.LastIndexOf('/');
                fileName = fromPath.Substring(pos + 1, fromPath.Length - pos - 1);
            }
            System.IO.FileStream outFileStream = new System.IO.FileStream(toPath + fileName, System.IO.FileMode.Create);
            System.IO.FileStream inFileStream = new System.IO.FileStream(fromPath, System.IO.FileMode.Open);
            try
            {
                CopyStream(inFileStream, outFileStream);
            }
            finally
            {
                outFileStream.Close();
                inFileStream.Close();
            }
        }



        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static int GetFileLength(string strFileName)
        {
            FileInfo fileInfo = new FileInfo(strFileName);
            if (!fileInfo.Exists)
            {
                return 0;
            }
            return (int)fileInfo.Length;
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
        

    }