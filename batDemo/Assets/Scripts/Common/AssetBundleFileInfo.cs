using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleFileInfo
{
    //生成build 时候的时间戳. 用于加载 本地缓存版本.
    public static string BuildTimestamp = "";
    public static string AssetBundleFilesMd5 = "";
    const char FileNameSplit = '-';
    const char PropertSeparator = '|';

    public string fileName { get; private set; }
    public string md5 { get; private set; }
    public ulong size { get; private set; }
    //标记MD5后名字.
    public string signedFileName { get; private set; }
    //本地名字
    //public string localFileName { get; private set; }

    public AssetBundleFileInfo(string fileName, string md5, ulong size)
    {
        Initialize(fileName, md5, size);
    }

    public AssetBundleFileInfo(string info)
    {
        string[] properties = info.Split(PropertSeparator);
        if (properties.Length >= 3)
        {
            Initialize(properties[0], properties[1], ulong.Parse(properties[2]));
        }
    }

    void Initialize(string fileName, string md5, ulong size)
    {
        this.fileName = fileName;
        this.md5 = md5;
        this.size = size;
        signedFileName = GetSignedFileName(fileName, md5);
    //     localFileName = IsConfigFile(fileName) ? GetConfigLocalName(fileName) : signedFileName;
  //      localFileName =  signedFileName;
    }

    public override string ToString()
    {
        string[] properties = { fileName, md5, size.ToString()};
        return string.Join(PropertSeparator.ToString(), properties);
    }

    public static bool IsConfigFile(string fileName)
    {
        return fileName == AssetBundleConst.AssetBundleFiles ;
    }

     public static string GetConfigLocalSignName(string fileName)
    {
        return GetSignedFileName(fileName, AssetBundleFilesMd5);
    }

    public static string GetConfigLocalName(string fileName)
    {
        return GetSignedFileName(fileName, BuildTimestamp);
    }

    public static string GetSignedFileName(string fileName, string sign)
    {
        //后缀
        string extension = Path.GetExtension(fileName);
        //文件名
        string tempFileName = Path.GetFileNameWithoutExtension(fileName);
        return tempFileName + FileNameSplit + sign + extension;
    }

    public static string GetUnsignedFileName(string fileName)
    {
        string extension = Path.GetExtension(fileName);
        string tempFileName = Path.GetFileNameWithoutExtension(fileName);
        string[] splits = tempFileName.Split(FileNameSplit);
        return splits[0] + extension;
    }
}
