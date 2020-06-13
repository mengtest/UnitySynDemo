using UnityEngine;
using System.Collections.Generic;

public static class SupportedLanguages
{
    public const string English = "English";

    public const string Chinese = "Chinese";

    public static string[] All;

    // 数组内第一个是默认语言
    public static string[] SupportedList;

    public static Dictionary<SystemLanguage, string> SystemMap = new Dictionary<SystemLanguage, string>();

    static SupportedLanguages()
    {
        All = new string[] { English, Chinese };
        SupportedList = new string[] { English, Chinese };
        //SupportedList = new string[] { English};

        //与UnityEngine.SystemLanguage的映射:
        SystemMap[SystemLanguage.English] = English;
        SystemMap[SystemLanguage.Chinese] = Chinese;
        SystemMap[SystemLanguage.ChineseSimplified] = Chinese;
    }

    public static string GetCurrentLanguage()
    {
        string currentLanguage;
        if(SystemMap.TryGetValue(Application.systemLanguage, out currentLanguage) && 
            System.Array.IndexOf(SupportedList, currentLanguage) >= 0)
        {
            return currentLanguage;
        }
        return SupportedList[0];
    }
}
