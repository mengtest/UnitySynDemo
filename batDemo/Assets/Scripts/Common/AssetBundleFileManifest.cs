using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleFileManifest
{
    public Dictionary<string, AssetBundleFileInfo> fileInfoDict { get; private set; }

    public AssetBundleFileManifest()
    {
        fileInfoDict = new Dictionary<string, AssetBundleFileInfo>();
    }

    public AssetBundleFileManifest(string text) : this()
    {
        Initialize(text);
    }

    public void Initialize(string text,bool isOrigin=false)
    {
        string[] infos = text.Split('\n');
        List<AssetBundleFileInfo> downloadFiles = new List<AssetBundleFileInfo>();
        for (int i = 0; i < infos.Length; ++i)
        {
            string info = infos[i].Trim();
            if (!string.IsNullOrEmpty(info))
            {
                AssetBundleFileInfo fileInfo = new AssetBundleFileInfo(info);
                fileInfoDict[fileInfo.fileName] = fileInfo;
            }
        }
        DebugLog.Log("AssetBundleFileManifest initFin isOrigin:"+isOrigin+"fileInfoDict.count "+fileInfoDict.Count);
    }
}
