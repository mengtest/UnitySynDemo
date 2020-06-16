
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.IO;
using UnityEngine.EventSystems;
using LuaInterface;

[AutoRegistLua]
public static class Utils
{
    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <returns></returns>
    public static string getTimestampStr()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <returns></returns>
    public static int getTimestamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return int.Parse(Convert.ToInt64(ts.TotalSeconds).ToString());
    }

    /// <summary>
    /// 用于立刻重新绘制UI部件，结合ContentSizeFitter使用
    /// </summary>
    public static void RebuildLayout(RectTransform transform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
    }

    public static void SetObjLayer(GameObject go, int layer)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].gameObject.layer = layer;
        }
    }

    public static void SetLayer(Transform target, int layer)
    {
        foreach (Transform tran in target.GetComponentsInChildren<Transform>())
        {
            tran.gameObject.layer = 10; //更改物体的Layer层
        }
    }

    public static void ResetTransform(Transform target)
    {
        if (target != null)
        {
            target.localPosition = new Vector3(0, 0, 0);
            target.localRotation = new Quaternion(0, 0, 0, 0);
            target.localScale = new Vector3(1, 1, 1);
        }
    }

    /// <summary>
    /// 帧数限制执行,true为可执行
    /// </summary>
    /// <param name="frame"></param>
    /// <returns></returns>
    public static bool LimitRum(int frame = 5)
    {
        if (Time.frameCount % frame != 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取文件后缀
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetSuffix(string path)
    {
        if (path.Contains("."))
        {
            string[] strs = path.Split('.');
            return strs[1];
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 通过文件路径获取文件名称,截取‘/’和‘.’之间
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileNameByPath(string path)
    {
        path = path.Split('.')[0];
        int index = path.LastIndexOf("/");
        if (index >= 0)
        {
            path = path.Substring(index+1);
        }
        return path;
    }

    public static string Name2ABName(string name)
    {
        string abname = name.ToLower().Replace("/", "_");
        return abname;
    }

    public static Vector2 GetResolution()
    {
        Vector2 resolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
#if UNITY_EDITOR
        resolution = UnityEditor.Handles.GetMainGameViewSize();
#endif
        return resolution;
    }
	
	public static void SetLogSwitcher(bool isOpenLog, bool isOpenError, bool isOpenWarning)
	{
	 	 DebugLog.setLogSwitcher(isOpenLog, isOpenError, isOpenWarning);
	}

    //把长度信息转换，比如10000B转成10KB
    public static string SizeToMes(int size)
    {
        var mes = "";
        if (size < 1024)
        {
            mes = "1KB";
        }
        else if (size > 1024 * 1024)
        {
            mes = ((float)size / 1024 / 1024).ToString("0.0") + "MB";
        }
        else
        {
            mes = ((float)size / 1024).ToString("0.0") + "KB";
        }
        return mes;
    }

    public static string GetStreamingAssetsURL()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Application.streamingAssetsPath;
            case RuntimePlatform.IPhonePlayer:
                return "file://" + Application.streamingAssetsPath;
            //case RuntimePlatform.WindowsPlayer:
            //case RuntimePlatform.OSXPlayer:
            //    return "file://" + Application.streamingAssetsPath;
            default:
                return Application.streamingAssetsPath;
        }
    }

    public static string GetPersistentDataURL()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Application.persistentDataPath;
            case RuntimePlatform.IPhonePlayer:
                return "file://" + Application.persistentDataPath;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
                return "file://" + Application.persistentDataPath;
            default:
                return Application.persistentDataPath;
        }
    }

    public static string GetPlatformFlag()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "android";
            case RuntimePlatform.IPhonePlayer:
                return "ios";
            default:
                return "window";
        }
    }

    public static bool IsWin()
    {
        return Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer;
    }

    public static bool IsLittleEndian()
    {
        return BitConverter.IsLittleEndian;
    }
    public static bool SystemIs64()
    {
        return System.IntPtr.Size > 4;
    }

    public static int GetRandomSeed()
    {
        return (int)System.DateTime.Now.ToFileTimeUtc();
    }

    public static int GetRandom(int min = 0, int max = int.MaxValue)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float GetRandomFloat(float min = 0, float max = float.MaxValue)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static void BtnAddlistener(Button btn, Action f, bool removeOther = true)
    {
        if (removeOther)
        {
            btn.onClick.RemoveAllListeners();
        }
        btn.onClick.AddListener(delegate ()
        {
            f();
        });
    }

    public static void ToggleAddlistener(Toggle btn, Action<bool> f, bool removeOther = true)
    {
        if (removeOther)
        {
            btn.onValueChanged.RemoveAllListeners();
        }
        btn.onValueChanged.AddListener((x) =>
        {
            f(x);
        });
    }

    public static void SliderAddlistener(Slider sliider, Action<float> f)
    {

        sliider.onValueChanged.AddListener(x =>
        {
            f(x);
        });
    }

    public static void ScrollRectAddlistener(ScrollRect scroll, Action<Vector2> fun)
    {
        scroll.onValueChanged.AddListener(x =>
        {
            fun(x);
        });
    }

    public static void DropDownAddListener(Dropdown drop, Action<int> fun)
    {
        drop.onValueChanged.AddListener(x =>
        {
            fun(x);
        });
    }

	//static List<string> lstDrop = new List<string>();
	public static void AddOptions(Dropdown drop, string ip)
	{
		//if (drop.captionText.text == null)
		//	drop.captionText.text = ip;
		drop.options.Add(new Dropdown.OptionData(ip));
	}

	public static void ClearOptions(Dropdown drop)
	{
		drop.options.Clear();		
	}

    public static byte[] Utf8ToByte(string utf8)
    {
        return System.Text.Encoding.UTF8.GetBytes(utf8);
    }

    public static string ByteToUtf8(byte[] bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    public static void ArrayCopy(byte[] srcdatas, long srcLen, byte[] dstdatas, long dstLen, long len)
    {
        Array.Copy(srcdatas, srcLen, dstdatas, dstLen, len);
    }

    public static Color GetColor(string colorString)
    {
        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        return color;
    }

    public static Transform FindChildRecursion(Transform t, string name)
    {
        try
        {
            foreach (Transform item in t)
            {
                if (item.name == name) return item;
                var child = FindChildRecursion(item, name);
                if (child) return child;
            }
        }
        catch { }
        return null;
    }

    public static Transform[] FindChildsRecursion(Transform t, string name)
    {
        var list = new List<Transform>();
        foreach (Transform item in t)
        {
            if (item.name == name)
            {
                list.Add(item);
            }
            list.AddRange(FindChildsRecursion(item, name));
        }
        return list.ToArray();
    }

    static DateTime oriTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
    public static DateTime UtcToDateTime(uint second, bool local = true)
    {
        if (local)
            return oriTime.AddSeconds(second + (DateTime.Now - DateTime.UtcNow).TotalSeconds);
        else
            return oriTime.AddSeconds(second);
    }

    public static void CopyToClipboard(string mes)
    {
        //Platform.Manager.Instance.CopyToClipboard(mes);
    }

    public static string GetDeviceUID()
    {
        return "";
        //return Platform.Manager.Instance.GetUID();
    }

    public static string FormatToDate(int stamp)
    {
        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long time = ((long)stamp * 10000000);
        TimeSpan tsp = new TimeSpan(time);
        DateTime targetDt = dt.Add(tsp);
        return targetDt.ToString("yyyy-MM-dd");
    }

    // base64加密
    public static string EncodeBase64(byte[] bytes)
    {
        return System.Convert.ToBase64String(bytes);
    }

    // base64解密
    public static LuaByteBuffer DecodeBase64(string str64)
    {
        byte[] bytes = System.Convert.FromBase64String(str64);
        return new LuaByteBuffer(bytes);
    }

    public static void OpenUrl(string www)
    {
        Application.OpenURL(www);
    }

    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#endif
        return false;
    }

    public static bool IsIos()
    {
#if (UNITY_IOS || UNITY_IPHONE) && !UNITY_EDITOR
        return true;
#endif
        return false;
    }

    #region 提供lua 层使用

    /// <summary>
    /// 添加Component
    /// </summary>
    static public Component AddComponent(GameObject parent, string type)
    {
        return parent.AddComponent(GetTypeFromAssembly(type));
    }

    /// <summary>
    /// 根据根节点，路径，找到路径的对象并返回
    /// </summary>
    public static GameObject FindChild(GameObject parent, string path)
    {
        if (parent != null)
        {
            Transform trans = parent.transform.Find(path);
            if (trans)
            {
                return trans.gameObject;
            }
            else
            {
                return null;
            }
        }
        else
        {
            Debug.LogError("UGUITool.FindChild parent is null  "+path);
            return null;
        }
    }

    /// <summary>
    /// 根据组件类型名字，根节点，路径，找到路径对象上挂载的组件并返回
    /// </summary>
    public static Component GetChildComponent(GameObject parent, string path, string componentTypeName)
    {
        Transform trans = parent.transform.Find(path);
        if (trans == null)
        {
            return null;
        }
        else
        {
            return trans.gameObject.GetComponent(componentTypeName);
        }
    }

    /// <summary>
    /// 在某个对象下添加子对象
    /// </summary>
    static public GameObject AddChild(GameObject parent)
    {
        return AddChild(parent, true);
    }

    /// <summary>
    /// 在某个对象下添加子对象
    /// </summary>
    static public GameObject AddChild(GameObject parent, bool undo)
    {
        GameObject go = new GameObject();
#if !GAME_PUBLISH && UNITY_EDITOR
        if (undo) UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    /// <summary>
    /// 根据某个对象，实例化一个新的对象，并且放在指定对象之下
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    static public GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if !GAME_PUBLISH && UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    static public Vector2 GetGridLayoutGroupCellSize(GridLayoutGroup canGrid)
    {
        return new Vector2(canGrid.cellSize.x, canGrid.cellSize.y);
    }

    static public Vector4 GetGridLayoutGroupPadding(GridLayoutGroup canGrid)
    {
        return new Vector4(canGrid.padding.left, canGrid.padding.right, canGrid.padding.top, canGrid.padding.bottom);
    }

    static public Component AddMissingComponent(GameObject go, string typeName)
    {
#if UNITY_5 || UNITY_2017 || UNITY_2018 || UNITY_2019
        Component comp = go.GetComponent(typeName);
        if (comp == null)
        {
            Type t = GetTypeFromAssembly(typeName);
            if (t != null)
            {
                comp = go.AddComponent(t);
            }
            else
            {
                Debug.LogErrorFormat("AddMissingComponent  {0}  can't find type", typeName);
                return null;
            }
        }
        return comp;
#else
            Component comp = go.GetComponent(typeName);
            if (comp == null)
            {
                comp = go.AddComponent(typeName);
            }
            return comp;
#endif
    }

    //依次获取 Assembly-CSharp, UnityEngine.UI, UnityEngine Assembly 以便后面进行获取 type
    static public Type GetTypeFromAssembly(string typeName)
    {
        Type t = GetTypeFromAssemblyCSharp(typeName);
        if (t != null)
        {

        }
        else
        {
            t = GetTypeFromAssemblyUnityEngineUI(typeName);
            if (t != null)
            {

            }
            else
            {
                t = GetTypeFromAssemblyUnityEngine(typeName);
            }
        }
        return t;
    }

    static private Assembly _assemblyCSharp = null;
    static public Type GetTypeFromAssemblyCSharp(string typeName)
    {
        if (_assemblyCSharp == null)
        {
            _assemblyCSharp = Assembly.Load("Assembly-CSharp");
        }
        Type t = _assemblyCSharp.GetType(typeName);
        return t;
    }

    static private Assembly _assembltUnityEngineUI = null;
    static public Type GetTypeFromAssemblyUnityEngineUI(string typeName)
    {
        if (_assembltUnityEngineUI == null)
        {
            _assembltUnityEngineUI = Assembly.Load("UnityEngine.UI");
        }
        if (!typeName.StartsWith("UnityEngine.UI"))
        {
            typeName = string.Concat("UnityEngine.UI.", typeName);
        }
        Type t = _assembltUnityEngineUI.GetType(typeName);
        return t;
    }

    static private Assembly _assembltUnityEngine = null;
    static public Type GetTypeFromAssemblyUnityEngine(string typeName)
    {
        if (_assembltUnityEngine == null)
        {
            _assembltUnityEngine = Assembly.Load("UnityEngine");
        }
        if (!typeName.StartsWith("UnityEngine"))
        {
            typeName = string.Concat("UnityEngine.", typeName);
        }
        Type t = _assembltUnityEngine.GetType(typeName);
        return t;
    }

	//添加登陆缓存文件
	public static string GetCacheName(string openId)
	{
		string path = Application.streamingAssetsPath + "login.txt";
		if (!File.Exists(path))
		{
			using (StreamWriter sw = File.CreateText(path))
			{
				sw.Write(openId);
			}
			return null;
		}
		using (StreamReader sw = File.OpenText(path))
		{
			return sw.ReadLine();
		}
	}

    /// <summary>
    /// 临时使用，请不要在正式版本使用
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadSceneTemp(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// 临时使用，请不要在正式版本使用
    /// </summary>
    /// <param name="scene"></param>
    public static void UnLoadSceneTemp(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene, 
            UnityEngine.SceneManagement.UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    //批量设置一个root下面所有自物体的layer
    public static void SetTransformChildLayer(Transform root, string layerName)
    {
        foreach (Transform child in root)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            SetTransformChildLayer(child.gameObject.transform, layerName);
        }
    }

    //删除对象下的所有子物体
    public static void DestroyObjAllChild(Transform root)
    {
        for(int i= root.childCount;i>0;i--)
        {
            GameObject.Destroy(root.GetChild(i).gameObject);
        }
    }

    public static Texture2D CreateTextureForRenderTex(RenderTexture canRenderTex)
    {
        Texture2D tempTex = new Texture2D(canRenderTex.width, canRenderTex.height, TextureFormat.ARGB32, false);
        tempTex.ReadPixels(new Rect(0, 0, canRenderTex.width, canRenderTex.height), 0, 0);
        tempTex.Apply();
        return tempTex;
    }

    public static Texture2D RenderTextureForRenderTex(Texture2D canTex, RenderTexture canRenderTex)
    {
        canTex = new Texture2D(canRenderTex.width, canRenderTex.height, TextureFormat.ARGB32, false);
        canTex.ReadPixels(new Rect(0, 0, canRenderTex.width, canRenderTex.height), 0, 0);
        canTex.Apply();
        return canTex;
    }

    // public static RenderTexture GetNewRenderTexture()
    // {
    //     RenderTexture tempN = RenderTexture.GetTemporary((int)AppConst.width, (int)AppConst.height);
    //     return tempN;
    // }

    public static Camera GetNewCamera(int canClearFlags)
    {
        GameObject tempObj = new GameObject();
        Camera tempCamera = tempObj.AddComponent<Camera>();
        tempCamera.clearFlags = (CameraClearFlags)canClearFlags;
        return tempCamera;
    }

    public static void ChangeRenderTextureActive(RenderTexture canRenderTex)
    {
        RenderTexture.active = canRenderTex;
    }

    public static void ChangeRawImageTexture(RawImage canRawImg, Texture2D canTex)
    {
        canRawImg.texture = canTex;
    }

    public static int GetTimeIntHHMM()
    {
        return int.Parse(DateTime.Now.ToString("hhmm"));
    }

    public static void AddDrapEevent(GameObject canObj,LuaFunction canFunc)
    {
        // UIEventListener.Get(canObj).onDrag = (PointerEventData eventData)=>
        // {
        //     canFunc.Call(eventData.delta);
        // };
    }
    #endregion

    #region 提供lua ScrollView使用
    /// <summary>
    /// 添加拖拽监听
    /// </summary>
    public static void AddScrollViewOnValueChangeListener(ScrollRect canRect, LuaFunction canFunc)
    {
        canRect.onValueChanged.AddListener((canVector2) =>
        {
            canFunc.Call(canVector2);
        });
    }
    #endregion

    #region 提供proto使用
    // public static string GetProtoBytesPath()
    // {
    //     string persistentDataPath;
    //     bool tempUseBundleBoo = true;
    //     bool tempIsEditorBoo = false;
    //     switch (Application.platform)
    //     {
    //         case RuntimePlatform.Android:
    //             persistentDataPath = Application.persistentDataPath;
    //             break;
    //         case RuntimePlatform.IPhonePlayer:
    //             persistentDataPath = "file://" + Application.persistentDataPath;
    //             break;
    //         case RuntimePlatform.WindowsPlayer:
    //             persistentDataPath = Application.persistentDataPath;
    //             break;
    //         case RuntimePlatform.WindowsEditor:
    //             tempIsEditorBoo = true;
    //             if (AppConst.LoadType == ResLoadType.WebRoot)
    //             {
    //                 persistentDataPath = Application.dataPath.Replace("Assets", "Bundle") + "/editorStorage";
    //                 break;
    //             }
    //             else
    //             {
    //                 persistentDataPath = Application.dataPath + "/ScriptsLua/PB";
    //                 tempUseBundleBoo = false;
    //                 break;
    //             }
    //         case RuntimePlatform.OSXEditor:
    //             persistentDataPath = "file://" + Application.persistentDataPath;
    //             break;
    //         default:
    //             persistentDataPath = Application.persistentDataPath;
    //             break;
    //     }
    //     string tempBytesPath = tempIsEditorBoo ? "/LuaBundle/" : "/data/LuaBundle/";
    //     string tempPath = tempUseBundleBoo ? persistentDataPath + tempBytesPath : persistentDataPath+"/";
    //     return tempPath;
    // }
    #endregion
}