using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleBuildInfo
{
    const char PropertSeparator = '|';

    public string timestamp { get; private set; }
    public string configMD5 { get; private set; }
  //  public string remoteConfigMD5 { get; private set; }
    
    public AssetBundleBuildInfo(string timestamp, string configMD5)
    {
        Initialize(timestamp, configMD5);
    }

    public AssetBundleBuildInfo(string info)
    {
        string[] properties = info.Split(PropertSeparator);
        if (properties.Length >= 2)
        {
            Initialize(properties[0], properties[1]);
        }
    }

    void Initialize(string timestamp, string configMD5)
    {
        this.timestamp = timestamp;
        this.configMD5 = configMD5;
    }

    public override string ToString()
    {
        string[] properties = { timestamp, configMD5 };
        return string.Join(PropertSeparator.ToString(), properties);
    }
}
