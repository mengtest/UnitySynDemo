using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorConst
{
    public static string PlatformAndroid = "Android";
    public static string PlatformIos = "iPhone";
    public static string PlatformWindows = "Windows";

    /// <summary>
    /// 图片后缀字符数组
    /// </summary>
    public static string[] TextureExts = { ".tga", ".png", ".jpg", ".tif", ".psd", ".exr" };

    /// <summary>
    /// 比特格式
    /// </summary>
    public readonly static string BytesFormatter = "<fmt_bytes>";

    /// <summary>
    /// 列表头正常颜色
    /// </summary>
    public static Color TitleColor = (Color)new Color32(135, 206, 250, 255);
    /// <summary>
    /// 列表头选中颜色
    /// </summary>
    public static Color TitleSelectedColor = (Color)new Color32(0, 191, 255, 255);

    /// <summary>
    /// 列表行正常颜色
    /// </summary>
    public static Color SelectionColor = (Color)new Color32(238, 232, 170, 255);
    /// <summary>
    /// 列表行选中颜色
    /// </summary>
    public static Color SelectionSelectedColor = (Color)new Color32(255, 255, 0, 255);
}

/// <summary>
/// 列表unity内置格式
/// </summary>
public class EditorNormalStyle
{
    public static GUIStyle Toolbar = "Toolbar";
    public static GUIStyle ToolbarButton = "ToolbarButton";
    public static GUIStyle TextField = "TextField";
}
