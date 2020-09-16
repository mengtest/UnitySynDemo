using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using LuaInterface;

public class GameAssetManager : MonoSingleton<GameAssetManager>
{
    enum BundleLoadType
    {
        Default,
        Singleton,
        Manifeset,
    }

    AssetBundleManifest _manifest;
    // 正在加载和已加载的资源列表
    Dictionary<string, LoadAssetRequest> _loadReqeustDict = new Dictionary<string, LoadAssetRequest>();
    // 正在加载中的资源集
    List<GameAssetRequest> _gameAssetReqeustList = new List<GameAssetRequest>();
    // 资源加载队列，同一时间只加载一个资源
    Queue<LoadAssetRequest> _loadReqeustQueue = new Queue<LoadAssetRequest>();
    // 缓存依赖列表
    Dictionary<string, string[]> _dependencyDict = new Dictionary<string, string[]>();
    // 已加载AssetBundle列表
    Dictionary<string, AssetBundleInfo> _loadedAssetBundleDict = new Dictionary<string, AssetBundleInfo>();
    // 卸载检查间隔
    WaitForSeconds _unloadInterval = new WaitForSeconds(1);
    // 在内存中保持的已标记为卸载的Asset的最大数量
    int _maxCacheCount = 10;
    // 在内存中保持的已标记为卸载的Asset的最大缓存时间(秒)
    float _maxCacheTime = 10;
    // 只在GameAssetManager重启游戏时才设为false
    bool _unloadAllLoadedObjects = true;
    // AssetBundleConst AssetBundleFolder
    public static string ABManifest="res_android";
    public static string EditorResPath="Assets/Res/";
    public bool Log=true;
    public delegate string GetFileLocalURL(string fileName, bool isOrigin = false);

    private GetFileLocalURL getFileLocalURLFun=null;

    private void Start()
    {
        StartCoroutine(CheckLoadAssetRequests(_unloadInterval));
    }

    public void OnRestartGame()
    {
        UnloadUnusedAssets();
        _unloadAllLoadedObjects = false;
        foreach (GameAssetRequest gameAssetRequest in _gameAssetReqeustList)
        {
            gameAssetRequest.Unload();
        }
        _gameAssetReqeustList.Clear();
        _loadReqeustQueue.Clear();
        foreach(var keyValue in _loadReqeustDict)
        {
            keyValue.Value.Destroy();
        }
        _loadReqeustDict.Clear();
        List<string> bundleList = new List<string>(_loadedAssetBundleDict.Keys);
        foreach(string key in bundleList)
        {
            UnloadBundleInternal(key, true);
        }
        bundleList.Clear();
        _loadedAssetBundleDict.Clear();
        _dependencyDict.Clear();
        _manifest = null;
        _unloadAllLoadedObjects = true;
    }
    public void setLocalUrlFun(GetFileLocalURL fn){
        if(fn!=null){
              this.getFileLocalURLFun=fn;
        }
    }


    public IEnumerator LoadManifest()
    {
        if (_manifest != null)
        {
            UnloadBundleInternal(ABManifest, true);
            _manifest = null;
        }
        yield return LoadBundleInCoroutine(ABManifest, BundleLoadType.Manifeset);
        AssetBundleInfo bundleInfo = GetLoadedAssetBundle(ABManifest);
        if (bundleInfo != null)
        {
            _manifest = bundleInfo.assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        else
        {
            DebugLog.LogError("Load bundle failed:" + ABManifest);
        }
    }

    public IEnumerator LoadLuaBundle()
    {
        string slpa = "32";
        if (SystemIs64())
        {
            slpa = "64";
        }
        yield return LoadBundleInCoroutine(AssetBundleConst.ToLuaBundle + slpa, BundleLoadType.Singleton);
        AssetBundleInfo bundleInfo = GetLoadedAssetBundle(AssetBundleConst.ToLuaBundle + slpa);
        if (bundleInfo != null)
        {
            yield return LuaFileUtils.Instance.AddSearchBundle(bundleInfo.assetBundle);
        }
        else
        {
            DebugLog.LogError("Load bundle failed:" + AssetBundleConst.ToLuaBundle + slpa);
        }

        yield return LoadBundleInCoroutine(AssetBundleConst.LuaBundle + slpa, BundleLoadType.Singleton);

        bundleInfo = GetLoadedAssetBundle(AssetBundleConst.LuaBundle + slpa);
        if (bundleInfo != null)
        {
            yield return LuaFileUtils.Instance.AddSearchBundle(bundleInfo.assetBundle);
        }
        else
        {
            DebugLog.LogError("Load bundle failed:" + AssetBundleConst.LuaBundle + slpa);
        }
    }
    public static bool SystemIs64()
    {
        return System.IntPtr.Size > 4;
    }

    public void SetMaxCacheCount(int count)
    {
        _maxCacheCount = count;
    }

    public void SetMaxCacheTime(float time)
    {
        _maxCacheTime = time;
    }
    //Lua单个加载
    public GameAssetRequest LoadAssetLua(string path, Type assetType, LuaFunction callback)
    {
        return LoadAsset(new string[] { path }, assetType,null, callback);
    }
    //Lua多个加载
    public GameAssetRequest LoadAssetLua(string[] paths, Type assetType, LuaFunction callback)
    {
        return LoadAsset(paths, assetType,null, callback);
    }
    //指定类型单个加载.
    public GameAssetRequest LoadAsset<T>(string path, Action<UnityEngine.Object[]> callback) where T : UnityEngine.Object
    {
        return LoadAsset(new string[] { path }, typeof(T), callback);
    }
    //指定类型多个加载.
    public GameAssetRequest LoadAsset<T>(string[] paths, Action<UnityEngine.Object[]> callback) where T : UnityEngine.Object
    {
        return LoadAsset(paths, typeof(T), callback);
    }

    private GameAssetRequest LoadAsset(string[] paths, Type assetType, Action<UnityEngine.Object[]> callback=null,LuaFunction luaCallback=null)
    {
        List<LoadAssetRequest> loadReqeustList = new List<LoadAssetRequest>();
        for (int i = 0; i < paths.Length; ++i)
        {
            string path = paths[i];
            string bundleName = GetBundleNameByPath(path);
        //    string bundleName = RemapVariantName(GetBundleNameByPath(path));
            // if(GameSettings.Instance.useAssetBundle){
            //     //把res path 变成 打包后的 ab名字/prefab名字
            //    GetUseABloadPath(ref path);
            // }
            LoadAssetRequest loadReqeust;
            if (!_loadReqeustDict.TryGetValue(path, out loadReqeust))
            {
                //获得 prefab名字 资源名字
                string assetName = GetAssetNameByPath(path);
                loadReqeust = new LoadAssetRequest(path, bundleName, assetName, assetType);
                _loadReqeustDict[path] = loadReqeust;
                _loadReqeustQueue.Enqueue(loadReqeust);
                if (_loadReqeustQueue.Count == 1)
                {
                    StartCoroutine(LoadAssetInCoroutine(loadReqeust));
                }
            }
            else
            {
                loadReqeust.Reload();
            }
            loadReqeustList.Add(loadReqeust);
        }
        GameAssetRequest request = new GameAssetRequest(loadReqeustList, callback,luaCallback);
        if (request.IsLoadComplete())
        {
            request.OnLoadComplete();
        }
        else
        {
            _gameAssetReqeustList.Add(request);
        }
        return request;
    }

    IEnumerator LoadAssetInCoroutine(LoadAssetRequest loadReqeust)
    {
        if (!GameSettings.Instance.useAssetBundle)
        {
            //编辑器加载prefab.ogg 等其他文件.
            yield return new WaitForSeconds(0.1f);
            loadReqeust.asset = LoadGameAssetInEditor(loadReqeust.path, loadReqeust.assetType);
            if(Log){
                DebugLog.Log("LoadGameAssetInEditor :"+loadReqeust.path);
            }
        }
        else
        {
            string bundleName = loadReqeust.bundleName;
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(bundleName);
            if (bundleInfo == null)
            {
                BundleLoadType loadType = BundleLoadType.Default;
                if (loadReqeust.assetType == typeof(AssetBundleManifest)&&bundleName.StartsWith(ABManifest))
                {
                    loadType = BundleLoadType.Manifeset;
                }
                DebugLog.Log("star Load  bundleName: "+bundleName,"loadReqeust.path: ",loadReqeust.path,"loadReqeust.assetType: ",loadReqeust.assetType);
                yield return LoadBundleInCoroutine(bundleName, loadType);
                bundleInfo = GetLoadedAssetBundle(bundleName);
                if (bundleInfo == null)
                {
                     DebugLog.LogError("Load bundle failed:" + bundleName);
                }
            }
            else
            {
                ++bundleInfo.referencedCount;
            }
            if (bundleInfo != null)
            {
                AssetBundle bundle = bundleInfo.assetBundle;
                AssetBundleRequest request = bundle.LoadAssetAsync(loadReqeust.assetName, loadReqeust.assetType);
                yield return request;
                loadReqeust.asset = request.asset;
                if(request.asset == null)
                {
                    DebugLog.LogError("Load asset failed:" + loadReqeust.path);
                }
            }
            else
            {
                DebugLog.LogError("Load bundle failed:" + bundleName);
            }
        }

        _loadReqeustQueue.Dequeue();
        if (_loadReqeustQueue.Count > 0)
        {
            StartCoroutine(LoadAssetInCoroutine(_loadReqeustQueue.Peek()));
        }
        CheckGameAssetReqeust();
    }
    private  string GetStreamingAssetsURL()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Application.streamingAssetsPath;
            case RuntimePlatform.IPhonePlayer:
                return "file://" + Application.streamingAssetsPath;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
                return "file://" + Application.streamingAssetsPath;
            default:
                return Application.streamingAssetsPath;
        }
    }
    IEnumerator LoadBundleInCoroutine(string bundleName, BundleLoadType loadType)
    {
        string url="";
        if(this.getFileLocalURLFun!=null){
           url= this.getFileLocalURLFun(bundleName,false);
        }else{
            DebugLog.Log(">>>>>> no set getFileLocalURLFun");
           url= Path.Combine(GetStreamingAssetsURL(), bundleName);            
        }
        UnityWebRequest webRequest = null;
        if (loadType == BundleLoadType.Manifeset)
        {
            webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);
            if(Log){
                DebugLog.Log("LoadBundleInCoroutine Manifeset :"+url);
            }
        }
        else
        {
            if (loadType == BundleLoadType.Default)
            {
                string[] dependencies = _manifest.GetAllDependencies(bundleName);
                if (dependencies.Length > 0)
                {
                    _dependencyDict.Add(bundleName, dependencies);
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        string dependency = dependencies[i];
                        AssetBundleInfo bundleInfo = null;
                        if (_loadedAssetBundleDict.TryGetValue(dependency, out bundleInfo))
                        {
                            ++bundleInfo.referencedCount;
                        }
                        else
                        {
                            yield return LoadBundleInCoroutine(dependency, BundleLoadType.Singleton);
                        }
                    }
                }
            }
            webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url, _manifest.GetAssetBundleHash(bundleName), 0);
             if(Log){
                DebugLog.Log("LoadBundleInCoroutine GetAssetBundle :"+url);
            }
        }
        yield return webRequest.SendWebRequest();
        if (webRequest.isHttpError||webRequest.isNetworkError)
        {
            DebugLog.LogError(string.Format("Load AssetBundle failed. BundleName: {0} Error: {1} URL: {2}", bundleName, webRequest.error, url));
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
            _loadedAssetBundleDict.Add(bundleName, new AssetBundleInfo(bundle));
        }
        webRequest.Dispose();
    }

    public AssetBundleInfo GetLoadedAssetBundle(string bundleName)
    {
        AssetBundleInfo bundle = null;
        if (!_loadedAssetBundleDict.TryGetValue(bundleName, out bundle))
        {
            return null;
        }
        string[] dependencies = null;
        if (!_dependencyDict.TryGetValue(bundleName, out dependencies))
        {
            return bundle;
        }

        foreach (string dependency in dependencies)
        {
            AssetBundleInfo dependentBundle;
            _loadedAssetBundleDict.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null)
            {
                return null;
            }
        }
        return bundle;
    }

    private void UnloadBundle(string bundleName)
    {
        if(UnloadBundleInternal(bundleName))
        {
            this.UnloadDependencies(bundleName);
        }
    }

    private void UnloadDependencies(string bundleName)
    {
        string[] dependencies = null;
        if (!_dependencyDict.TryGetValue(bundleName, out dependencies))
        {
            return;
        }
        foreach (var dependency in dependencies)
        {
            UnloadBundleInternal(dependency);
        }
        _dependencyDict.Remove(bundleName);
    }

    private bool UnloadBundleInternal(string bundleName, bool force = false)
    {
        AssetBundleInfo bundleInfo = null;
        if (_loadedAssetBundleDict.TryGetValue(bundleName, out bundleInfo))
        {
            if (force || --bundleInfo.referencedCount <= 0)
            {
                bundleInfo.assetBundle.Unload(_unloadAllLoadedObjects);
                _loadedAssetBundleDict.Remove(bundleName);
                return true;
            }
        }
        return false;
    }

    private bool IsLoadingReqeust(LoadAssetRequest request)
    {
        return _loadReqeustQueue.Count > 0 && _loadReqeustQueue.Peek() == request;
    }

    private void CheckGameAssetReqeust()
    {
        for (int i = _gameAssetReqeustList.Count - 1; i >= 0; --i)
        {
            GameAssetRequest reqeust = _gameAssetReqeustList[i];
            if (reqeust.IsLoadComplete())
            {
                reqeust.OnLoadComplete();
                _gameAssetReqeustList.RemoveAt(i);
            }
        }
    }

    IEnumerator CheckLoadAssetRequests(WaitForSeconds interval)
    {
        while (true)
        {
            yield return interval;

            int unloadedCount = 0;
            float oldestTime = Time.time;
            string oldestAsset = null;
            Dictionary<string, LoadAssetRequest>.Enumerator itor = _loadReqeustDict.GetEnumerator();
            while (itor.MoveNext())
            {
                if (!itor.Current.Value.IsUnloaded())
                {
                    continue;
                }
                ++unloadedCount;
                // 正在加载中的资源不会被卸载
                if (itor.Current.Value.unloadTime < oldestTime && !IsLoadingReqeust(itor.Current.Value))
                {
                    oldestTime = itor.Current.Value.unloadTime;
                    oldestAsset = itor.Current.Key;
                }
            }
            if (oldestAsset != null && unloadedCount > _maxCacheCount || (Time.time - oldestTime) > _maxCacheTime)
            {
                string name=_loadReqeustDict[oldestAsset].bundleName;
                UnloadBundle(name);
                _loadReqeustDict[oldestAsset].Destroy();
                _loadReqeustDict.Remove(oldestAsset);
                DebugLog.Log("bundle Unload:" + name+" count: "+_loadReqeustDict.Count);
            }
        }
    }

    public void UnloadUnusedAssets()
    {
        foreach(var keyValue in _loadReqeustDict)
        {
            if(keyValue.Value.IsUnloaded())
            {
                UnloadBundle(keyValue.Value.bundleName);
                keyValue.Value.Destroy();
                _loadReqeustDict.Remove(keyValue.Key);
            }
        }
    }
//     //ab包 的 ab名字/资源名字  abName/assetName ..................
//     public static void GetUseABloadPath(ref string path)
//     {
//       //  string bundleNameAssetName;
//         //"Monster", "Avatar","Building","Character"
//          if (path.StartsWith("Monster")||path.StartsWith("Building")||path.StartsWith("Character"))
//          {
//                 // "Monster/mst_xg01/mst_xg01_01"
//              string[] split = path.Split('/');
//          //    bundleNameAssetName = split[1];
//              path = split[0]+"_"+split[1]+"/"+split[2];
//                //path  monster_mst_xg01/mst_xg01_01
//          }else if(path.StartsWith("Effect")){
//              //"Effect/Monster/mst_xg20/mst_xg20@Skill_01"
//               string[] split = path.Split('/');
//              path = split[0]+"_"+split[2]+"/"+split[3];
//          }else if(path.StartsWith("Avatar")||path.StartsWith("NAvatar")){
//             // Avatar/infility/Model/infility_body_01      
//             //  获得 Avatar_infility/infility_body_01       
//               string[] split = path.Split('/');
//               path = split[0]+"_"+split[1]+"/"+split[3];
//          }else if (path.StartsWith("View")){
//              string[] split = path.Split('/');
//             path = split[0]+"_"+split[1]+"/"+split[2];;
//             //"View/Login/LoginPanel"  获得   view_login/LoginPanel
//         }else{
//             //Item
//         }
//    }
   //获取 资源AB包名称  有特殊 需求的ab名称拼接需要走这里.
    public static string GetBundleNameByPath(string path)
    {
        string bundleName;
        //"Monster", "Avatar","Building","Character"
        //  if (path.StartsWith("Monster")||path.StartsWith("Building")||path.StartsWith("Character"))
        //  {
        //         // "Monster/mst_xg01/mst_xg01_01"
        //      string[] split = path.Split('/');
        //      bundleName = split[0]+"_"+split[1];
        //  }else
         if(path.StartsWith("Effect")){
              string[] split = path.Split('/');
             bundleName = split[0]+"_"+split[2];
             //"Effect/Monster/mst_xg20/mst_xg20@Skill_01"
         }else if(path.StartsWith("Avatar")||path.StartsWith("NAvatar")){
              string[] split = path.Split('/');
             bundleName = split[0]+"_"+split[3];
            // Avatar/infility/Model/infility_body_01   avatar_infility_body_01.model
         }
        // else if (path.StartsWith("View")){
        //      string[] split = path.Split('/');
        //     bundleName = split[0]+"_"+split[1];
        //     //"View/Login/LoginPanel"     view_login
        // }
        else
        {
            bundleName = Path.GetDirectoryName(path).Replace("\\", "_");
        }
        bundleName = bundleName.ToLower();
        return bundleName;
    }

    public static string GetBundleNameByFloderPath(string path)
    {
        path += (path.EndsWith("/") ? "" : "/") + "Any";
        return GetBundleNameByPath(path);
    }

    public static string GetAssetNameByPath(string path)
    {
        return Path.GetFileName(path);
    }

#if UNITY_EDITOR
    static string[] s_supportedExtensions = { ".prefab",".txt", ".png", ".jpg", ".tga", ".mat", ".asset", ".spriteatlas", ".mp3", ".ogg", ".wav"};
    public static UnityEngine.Object LoadGameAssetInEditor(string path, Type assetType)
    {
        UnityEngine.Object asset = null;
        foreach (string extension in s_supportedExtensions)
        {
            asset = UnityEditor.AssetDatabase.LoadAssetAtPath(EditorResPath + path + extension, assetType);
            if (asset != null)
            {
                break;
            }
        }
        return asset;
    }
#else
    public static UnityEngine.Object LoadGameAssetInEditor(string path, Type assetType)
    {
        return null;
    }
#endif
}
public class AssetBundleInfo
{
    public AssetBundle assetBundle { get; private set; }
    public int referencedCount { get; set; }

    public AssetBundleInfo(AssetBundle assetBundle)
    {
        this.assetBundle = assetBundle;
        referencedCount = 1;
    }
}
[AutoRegistLua]
public class GameAssetRequest
{
   private List<LoadAssetRequest> _loadReqeusts;
   private Action<UnityEngine.Object[]> _callback;
   private LuaFunction _luaCallback;
    public GameAssetRequest(List<LoadAssetRequest> loadReqeusts, Action<UnityEngine.Object[]> callback=null,LuaFunction luaCallback=null)
    {
        _loadReqeusts = loadReqeusts;
        _callback = callback;
        _luaCallback = luaCallback;
    }

    public void OnLoadComplete()
    {
        if(_loadReqeusts == null)
        {
            return;
        }
        UnityEngine.Object[] assets = new UnityEngine.Object[_loadReqeusts.Count];
        for (int i = 0; i < _loadReqeusts.Count; ++i)
        {
            LoadAssetRequest loadReqeust = _loadReqeusts[i];
            assets[i] = loadReqeust.asset;
        }
        if (_callback != null)
        {
            _callback(assets);
        }
        if (_luaCallback != null)
        {
            _luaCallback.Call(assets);
            if(_luaCallback!=null){
              _luaCallback.Dispose();
            }
        }
        DisposeCallback();
    }

    void DisposeCallback()
    {
        if (_callback != null)
        {
            _callback = null;
        }
        if (_luaCallback != null)
        {
            _luaCallback = null;
        }
    }

    public bool IsLoadComplete()
    {
         if(_loadReqeusts == null)
        {
            return true;
        }
        foreach (LoadAssetRequest loadReqeust in _loadReqeusts)
        {
            if(loadReqeust.asset == null)
            {
                return false;
            }
        }
        return true;
    }

    public void Unload()
    {
        DisposeCallback();
        if (_loadReqeusts != null)
        {
            foreach (LoadAssetRequest loadReqeust in _loadReqeusts)
            {
                loadReqeust.Unload();
            }
            _loadReqeusts = null;
        }
    }

    public UnityEngine.Object GetAsset(int index = 0)
    {
        return (index < _loadReqeusts.Count && index >= 0) ? _loadReqeusts[index].asset : null;
    }
    public AssetBundleInfo GetAssetBundleInfo(int index = 0)
    {
        if((index < _loadReqeusts.Count && index >= 0)){
            return GameAssetManager.Instance.GetLoadedAssetBundle(_loadReqeusts[index].bundleName);
        }else{
            return null;
        }
    }
}

public class LoadAssetRequest
{
    public string bundleName { get; private set; }
    public string assetName { get; private set; }
    public Type assetType { get; private set; }
    public UnityEngine.Object asset { get; set; }
    public float unloadTime { get; private set; }
    public string path { get; private set; }

    int _referencedCount = 0;

    public LoadAssetRequest(string path, string bundleName, string assetName, Type type)
    {
        this.path = path;
        _referencedCount = 1;
        unloadTime = -1;
        assetType = type;
        this.bundleName = bundleName;
        this.assetName = assetName;
    }

    public void Reload()
    {
        ++_referencedCount;
        unloadTime = -1;
    }

    public bool IsUnloaded()
    {
        return _referencedCount <= 0;
    }

    public void Unload()
    {
        if (_referencedCount == 1)
        {
            _referencedCount = 0;
            unloadTime = Time.time;
        }
        else if (_referencedCount > 1)
        {
            --_referencedCount;
        }
    }

    public void Destroy()
    {
        asset = null;
        unloadTime = -1;
        _referencedCount = 0;
    }
}
