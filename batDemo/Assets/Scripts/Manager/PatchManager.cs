using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class PatchManager : MonoSingleton<PatchManager>
{
    const char FileNameSplit = '-';
    const int MaxLoadingFileCount = 6;

    public bool needUpDownloadWeb=false;

    List<DownloadFileRequest> _reuqestList = new List<DownloadFileRequest>();
    //更新下载后写入的文件路径.
    string _persistentDataPath; 
    //更新后 下载的 加载路径
    string  _persistentDataURL; 
    string   _streamingAssetsPath;
    string  _streamingAssetsURL;
    //下载到手机本地的临时路径.
    string _downloadPath;
    string _rootURL;
    //Initialize时赋值，若不需要更新则置为空
    AssetBundleFileInfo _configInfo;
    //包里版本
    AssetBundleBuildInfo _buildInfo;
    AssetBundleBuildInfo _remotebuildInfo;
    AssetBundleFileManifest _originManifest = new AssetBundleFileManifest();
    AssetBundleFileManifest _manifest = new AssetBundleFileManifest();
    int _loadingFileCount = 0;

    public static string GetStreamingAssetsURL()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return Application.streamingAssetsPath;
            case RuntimePlatform.IPhonePlayer:
                return "file://" + Application.streamingAssetsPath;
            case RuntimePlatform.WindowsEditor:
                 return "file://" + Application.streamingAssetsPath;
            case RuntimePlatform.OSXEditor:
                return "file://" + Application.streamingAssetsPath;
            default:
                return Application.streamingAssetsPath;
        }
    }

    public static string GetPersistentDataURL()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "file://" + Application.persistentDataPath;
            case RuntimePlatform.IPhonePlayer:
                return "file://" + Application.persistentDataPath;
            case RuntimePlatform.WindowsEditor:
                return  GetEdtiorAssetPath();
            case RuntimePlatform.OSXEditor:
                 return "file://" + Application.persistentDataPath;
            default:
                return Application.persistentDataPath;
        }
    }
    public static string GetEdtiorAssetPath(){
        string projectPath= Application.dataPath.Replace("/Assets","");
        string outPath = projectPath ;
        return outPath;
    }

    private void Awake()
    {
        _persistentDataPath = Path.Combine(Application.persistentDataPath, AssetBundleConst.AssetBundleFolderSigned);
        _persistentDataURL = Path.Combine(GetPersistentDataURL(), AssetBundleConst.AssetBundleFolderSigned);
        _streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, AssetBundleConst.AssetBundleFolderSigned);
        _streamingAssetsURL = Path.Combine(GetStreamingAssetsURL(), AssetBundleConst.AssetBundleFolderSigned);
        _downloadPath = Path.Combine(Application.persistentDataPath, AssetBundleConst.AssetBundleTempFolder);
    //     DebugLog.Log(GetPersistentDataURL());
       //   DebugLog.Log(Application.persistentDataPath);
#if UNITY_EDITOR
       _persistentDataPath=GetEdtiorAssetPath()+"/"+AssetBundleConst.AssetBundleFolderSigned;

#else
        if (!Directory.Exists(_persistentDataPath))
        {
            Directory.CreateDirectory(_persistentDataPath);
        }

        if (!Directory.Exists(_downloadPath))
        {
            Directory.CreateDirectory(_downloadPath);
        }
#endif
    }

    private void OnDestroy()
    {
        foreach (DownloadFileRequest request in _reuqestList)
        {
            request.OnDestroy();
        }
        _reuqestList.Clear();
    }
    //初始化  step1 加载 LoadBuildInfo
    public IEnumerator Initialize()
    {
        yield return LoadBuildInfo();
        yield return LoadManifest(_manifest, AssetBundleConst.AssetBundleFiles, false);
        if(GameSettings.Instance.isLoadRemoteAsset){
             yield return LoadManifest(_originManifest, AssetBundleConst.AssetBundleFiles, true);
        }
    }
    //远程拿到buildinfo后  如果需要更新 则到远程更新. step2
    public IEnumerator InitializePatch(string rootURL)
    {
        needUpDownloadWeb=false;
        //rootURL = "http://192.168.0.3";
        _rootURL = string.Format("{0}/{1}/", rootURL, AssetBundleConst.AssetBundleFolderSigned);
        yield return LoadRemotoBuildInfo(_rootURL);

        string configMD5=_buildInfo.configMD5;
        if(_remotebuildInfo!=null){
            DebugLog.Log("load_remote configMD5: "+_remotebuildInfo.configMD5);
            configMD5=_remotebuildInfo.configMD5;
        }
        _configInfo = new AssetBundleFileInfo(AssetBundleConst.AssetBundleFiles, configMD5, 1024);
        bool needUpdate = ValidateConfig(_configInfo);
        if(!needUpdate)
        {
            _configInfo = null;
            FileUtils.ClearDirectory(_downloadPath);
        }
        needUpDownloadWeb = needUpdate;
        DebugLog.Log("needUpDownloadWeb : "+needUpDownloadWeb);
    }
    //判断本地存储目录是否需要更新. 网络获取的md5 与 本地(下载)assetbundlefiles.txt比较
    public bool ValidateConfig(AssetBundleFileInfo configInfo)
    {
        bool needUpdate = true;
        string persistentDataPath = Path.Combine(_persistentDataPath, AssetBundleFileInfo.GetConfigLocalName(configInfo.fileName));
        string downloadPath = Path.Combine(_downloadPath, configInfo.signedFileName);
        if (File.Exists(persistentDataPath) && FileUtils.MD5File(persistentDataPath) == configInfo.md5)
        {
            DebugLog.Log("ValidateConfig persistentDataPath sameFile: "+configInfo.signedFileName+"needUpdate: "+needUpdate);
            needUpdate = false;
        }
        else if (string.IsNullOrEmpty(configInfo.md5) || _buildInfo.configMD5 == configInfo.md5)
        {
            //远程与包里版本打包一样. 清除远程保存的数据.
            FileUtils.DeleteFileSafely(persistentDataPath);
            needUpdate = false;
        }
        if (needUpdate)
        {
            if (File.Exists(downloadPath) && FileUtils.MD5File(downloadPath) != configInfo.md5)
            {
                //如果缓存与下载不一致,清空当前文件.
                FileUtils.DeleteFileSafely(downloadPath);
            }
        }
        return needUpdate;
    }
    //获取已重命名md5的 本地文件.
    public string GetSignedFileLocalURL(string fileName, bool isOrigin = false)
    {
        string signedFileName = fileName;
        if (!AssetBundleFileInfo.IsConfigFile(fileName))
        {
            AssetBundleFileInfo fileInfo = null;
            AssetBundleFileManifest manifest = _manifest;
            if (manifest != null)
            {
                manifest.fileInfoDict.TryGetValue(fileName, out fileInfo);
            }
            if (fileInfo != null)
            {
                signedFileName = fileInfo.signedFileName;
            }else{
               DebugLog.LogError("GetSignedFileLocalURL LogError --> "+fileName+" manifest count: "+manifest.fileInfoDict.Count);
               return "";
            }
        }
        else
        {
            if(isOrigin){
               signedFileName = AssetBundleFileInfo.GetConfigLocalSignName(fileName);
            }else{
                signedFileName = AssetBundleFileInfo.GetConfigLocalName(fileName);
            }
            //  signedFileName = AssetBundleFileInfo.GetConfigLocalName(fileName);
        }
        string persistentDataPath = Path.Combine(_persistentDataPath, signedFileName);
        string persistentDataURL = Path.Combine(_persistentDataURL, signedFileName);
        string streamingAssetsURL = Path.Combine(_streamingAssetsURL, signedFileName);
        if(AssetBundleFileInfo.IsConfigFile(fileName)){
            if(isOrigin){
                return streamingAssetsURL;
            }else{
                if(File.Exists(persistentDataPath)){
                    return persistentDataURL;
                }else{
                    //改成包内版本 加载.
                    signedFileName = AssetBundleFileInfo.GetConfigLocalSignName(fileName);
                    streamingAssetsURL = Path.Combine(_streamingAssetsURL, signedFileName);
                    return  streamingAssetsURL;
                }
            }
        }

        return !isOrigin && File.Exists(persistentDataPath) ? persistentDataURL : streamingAssetsURL;
    }
    IEnumerator LoadRemotoBuildInfo(string urlPath)
    {
        if (_remotebuildInfo == null)
        {
            //本地获取 BuildInfo 更新文件的md5 打包的时间戳
            string url = Path.Combine(urlPath, AssetBundleConst.AssetBundleBuildInfo);
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
              DebugLog.Log("load_remotebuildInfo url: "+url);
            yield return webRequest.SendWebRequest();
            if(webRequest.isNetworkError||webRequest.isHttpError){
                DebugLog.LogError("load_remotebuildInfo webRequest: "+webRequest.error);
            }else{
                 DebugLog.Log("load_remotebuildInfo text: "+webRequest.downloadHandler.text);
                _remotebuildInfo = new AssetBundleBuildInfo(webRequest.downloadHandler.text);
            }
            webRequest.Dispose();
        }
    }
    IEnumerator LoadBuildInfo()
    {
        if (_buildInfo == null)
        {
            //本地获取 BuildInfo 更新文件的md5 打包的时间戳
            string url = Path.Combine(_streamingAssetsURL, AssetBundleConst.AssetBundleBuildInfo);
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            _buildInfo = new AssetBundleBuildInfo(webRequest.downloadHandler.text);
            AssetBundleFileInfo.BuildTimestamp = _buildInfo.timestamp;
             AssetBundleFileInfo.AssetBundleFilesMd5 = _buildInfo.configMD5;
            webRequest.Dispose();
        }
    }
   
    IEnumerator LoadManifest(AssetBundleFileManifest manifest, string fileName, bool isOrigin)
    {
        //热更新完成后，当前manifest需要重新加载，包内manifest不会改变所以不用重新加载
        if (!isOrigin || manifest.fileInfoDict.Count == 0)
        {
            string localFileURL = GetSignedFileLocalURL(fileName, isOrigin);
            DebugLog.Log("isOrigin: "+isOrigin+" LoadManifest: "+localFileURL);
            UnityWebRequest webRequest = UnityWebRequest.Get(localFileURL);
            yield return webRequest.SendWebRequest();
             if (webRequest.isHttpError||webRequest.isNetworkError)
            {
                DebugLog.LogError("isOrigin: "+isOrigin+" LoadManifest: "+localFileURL+webRequest.error);
            }else{     
                 manifest.Initialize(webRequest.downloadHandler.text,isOrigin);
            }
            webRequest.Dispose();
        }
    }

    AssetBundleFileManifest LoadDownloadedManifest(string localFileName)
    {
        string filePath = Path.Combine(_downloadPath, localFileName);
        AssetBundleFileManifest manifest = null;
        if (File.Exists(filePath))
        {
            string configText = File.ReadAllText(filePath);
            manifest = new AssetBundleFileManifest(configText);
        }
        return manifest;
    }
     public delegate void onCompleteFun(bool isFinish);
     public delegate void onUpdateProgressFun(float progress,ulong totalLoadedBytes,ulong totalBytes);
    // 对比远程文件MD5 如果需要更新 执行更新.step3
    public void UpdatePatch(onCompleteFun onComplete, onUpdateProgressFun updateProgress)
    {
        if(_configInfo != null)
        {
            StartCoroutine(UpdatePatchInCoroutine(onComplete, updateProgress));
        }
    }

    IEnumerator UpdatePatchInCoroutine(onCompleteFun onComplete, onUpdateProgressFun updateProgress)
    {
        //下载主文件. assetbundlefiles
        yield return DownloadFile(_configInfo);
        AssetBundleFileManifest manifest = LoadDownloadedManifest(_configInfo.signedFileName);
        if(manifest == null)
        {
            if(onComplete != null)
            {
                onComplete(false);
            }
            yield break;
        }
         //下载所有需要更新的文件. 
        DownloadFileRequest request = new DownloadFileRequest(onComplete, updateProgress);
        request.isPatchRequest = true;
        _reuqestList.Add(request);
        foreach (AssetBundleFileInfo fileInfo in manifest.fileInfoDict.Values)
        {
            if(CheckNeedDownload(fileInfo))
            {
                request.AddFileInfo(fileInfo);
            }
        }
        if(request.fileInfoDict.Count > 0)
        {
            request.downloadCoroutine = StartCoroutine(DownloadInCoroutine(request));
        }
        else
        {
            OnCompleteRequest(request, true);
        }
    }


    IEnumerator DownloadInCoroutine(DownloadFileRequest request)
    {
        DebugLog.Log("DownloadInCoroutine");
        foreach (AssetBundleFileInfo fileInfo in request.fileInfoDict.Values)
        {
            while (_loadingFileCount > MaxLoadingFileCount)
            {
                yield return 0;
            }
            StartCoroutine(DownloadFile(fileInfo, request));
        }
        do
        {
            yield return 0;
        } while (_loadingFileCount > 0);
        foreach (AssetBundleFileInfo fileInfo in request.fileInfoDict.Values)
        {
            MoveDownloadedFile(fileInfo);
        }
        OnCompleteRequest(request, true);
    }
    //下载每个单独文件并 保存.
    IEnumerator DownloadFile(AssetBundleFileInfo fileInfo, DownloadFileRequest request = null)
    {
        //文件已存在,可能是上次更新失败时下载的文件
        if (IsFileDownloaded(fileInfo))
        {
            UpdateProgress(request, fileInfo, fileInfo.size);
            DebugLog.Log("File has downloaded:" + fileInfo.fileName);
            yield break;
        }
        ++_loadingFileCount;
        string url = _rootURL + fileInfo.signedFileName;
        DebugLog.Log("Start load file:" + url);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
            UpdateProgress(request, fileInfo, webRequest.downloadedBytes);
            yield return 0;
        }
        if (string.IsNullOrEmpty(webRequest.error))
        {
            UpdateProgress(request, fileInfo, fileInfo.size);
            SaveFile(fileInfo, webRequest.downloadHandler.data);
            DebugLog.Log("Load file success:" + fileInfo.fileName);
        }
        else
        {
            OnCompleteRequest(request, false);
            DebugLog.LogError(string.Format("Download file[{0}] failed. Error:{1}", fileInfo.fileName, webRequest.error));
        }
        --_loadingFileCount;
    }

    void UpdateProgress(DownloadFileRequest request, AssetBundleFileInfo fileInfo, ulong downloadedBytes)
    {
        if(request != null)
        {
            request.UpdateProgress(fileInfo, downloadedBytes);
        }
    }

    void OnCompleteRequest(DownloadFileRequest request, bool success)
    {
        if (request != null)
        {
            if(request.isPatchRequest)
            {
                OnUpdatePatchComplete(success);
            }
            request.OnComplete(success);
            _reuqestList.Remove(request);
            if (!success && request.downloadCoroutine != null)
            {
                StopCoroutine(request.downloadCoroutine);
            }
        }
    }
    void OnUpdatePatchComplete(bool success)
    {
        if(_configInfo != null && success)
        {
            //成功才移动最后配置文件.
            MoveDownloadedFile(_configInfo);
            _configInfo = null;
        }
    }

    void SaveFile(AssetBundleFileInfo fileInfo, byte[] bytes)
    {
        string filePath = Path.Combine(_downloadPath, fileInfo.signedFileName);
        DebugLog.Log("save File: "+filePath);
        FileEx.WriteAllBytes(filePath, bytes);

    }
    //检测是否需要下载.
    bool CheckNeedDownload(AssetBundleFileInfo fileInfo)
    {
        string persistentDataPath = Path.Combine(_persistentDataPath, fileInfo.signedFileName);
        return !File.Exists(persistentDataPath) && !IsFileExitsInStreamingAssetsPath(fileInfo);
    }

    bool IsFileDownloaded(AssetBundleFileInfo fileInfo)
    {
        string downloadPath = Path.Combine(_downloadPath, fileInfo.signedFileName);
        return File.Exists(downloadPath);
    }
   //是否在StreamingAssets中存在
    bool IsFileExitsInStreamingAssetsPath(AssetBundleFileInfo fileInfo)
    {
        if (fileInfo.fileName == AssetBundleConst.AssetBundleFiles)
        {
            return _buildInfo != null && _buildInfo.configMD5 == fileInfo.md5;
        }
        else
        {
            AssetBundleFileManifest manifest =  _originManifest;
            AssetBundleFileInfo originFileInfo = null;
            if (manifest != null  && manifest.fileInfoDict.TryGetValue(fileInfo.fileName, out originFileInfo))
            {
                return originFileInfo.md5 == fileInfo.md5;
            }
        }
        return false;
    }

    void MoveDownloadedFile(AssetBundleFileInfo fileInfo)
    {
        string downloadPath ="";
          string persistentDataPath="";
        if(fileInfo == _configInfo){
             downloadPath = Path.Combine(_downloadPath, fileInfo.signedFileName);
             persistentDataPath = Path.Combine(_persistentDataPath, AssetBundleFileInfo.GetConfigLocalName(fileInfo.fileName));
        }
        else if(AssetBundleFileInfo.IsConfigFile(fileInfo.fileName)){
             downloadPath = Path.Combine(_downloadPath, fileInfo.signedFileName);
             persistentDataPath = Path.Combine(_persistentDataPath, AssetBundleFileInfo.GetConfigLocalName(fileInfo.fileName));
        }else{
             downloadPath = Path.Combine(_downloadPath, fileInfo.signedFileName);
             persistentDataPath = Path.Combine(_persistentDataPath, fileInfo.signedFileName);
        }
        DebugLog.Log("moveFile downloadPath: "+downloadPath);
         DebugLog.Log("moveFile persistentDataPath: "+persistentDataPath);
        FileUtils.MoveFileSafely(downloadPath, persistentDataPath);
        //顺便删除 旧版本的
        if(!AssetBundleFileInfo.IsConfigFile(fileInfo.fileName)){
             string  url= GetSignedFileLocalURL(fileInfo.fileName);
             if(url!=""&&!url.Contains(_streamingAssetsURL)){
                 DebugLog.Log("DeleteFileSafely :"+url);
                FileUtils.DeleteFileSafely(url);
             }
        }
    }

}

public class DownloadFileRequest
{
    public float progress { get; private set; }
    public Coroutine downloadCoroutine { get; set; }
    public bool isPatchRequest { get; set; }

    Dictionary<string, AssetBundleFileInfo> _fileInfoDict = new Dictionary<string, AssetBundleFileInfo>();
    Dictionary<string, ulong> _loadedBytesDict = new Dictionary<string, ulong>();
    PatchManager.onCompleteFun _onComplete;
    PatchManager.onUpdateProgressFun _updateProgress;

    public DownloadFileRequest(PatchManager.onCompleteFun onComplete,  PatchManager.onUpdateProgressFun updateProgress)
    {
        _onComplete = onComplete;
        _updateProgress = updateProgress;
    }

    public Dictionary<string, AssetBundleFileInfo> fileInfoDict
    {
        get { return _fileInfoDict; }
    }

    public void AddFileInfo(AssetBundleFileInfo fileInfo)
    {
        _fileInfoDict[fileInfo.fileName] = fileInfo;
        _loadedBytesDict[fileInfo.fileName] = 0;
    }

    public void OnComplete(bool success)
    {
        if (_onComplete != null)
        {
            try
            {
                _onComplete(success);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        OnDestroy();
    }

    public void UpdateProgress(AssetBundleFileInfo fileInfo, ulong loadedBytes)
    {
        ulong fileLoadedBytes = 0;
        if (!_loadedBytesDict.TryGetValue(fileInfo.fileName, out fileLoadedBytes) || fileLoadedBytes == loadedBytes)
        {
            return;
        }
        _loadedBytesDict[fileInfo.fileName] = loadedBytes;
        ulong totalLoadedBytes = 0;
        ulong totalBytes = 0;
        foreach (AssetBundleFileInfo value in _fileInfoDict.Values)
        {
            totalLoadedBytes += _loadedBytesDict[fileInfo.fileName];
            totalBytes += fileInfo.size;
        }
        progress = totalBytes > 0 ? (float)totalLoadedBytes / totalBytes : 1;
        if (_updateProgress != null)
        {
            try
            {
                _updateProgress(progress, totalLoadedBytes, totalBytes);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public void OnDestroy()
    {
        if (_onComplete != null)
        {
        //    _onComplete.Dispose();
            _onComplete = null;
        }
        if (_updateProgress != null)
        {
     //      _updateProgress.Dispose();
            _updateProgress = null;
        }
    }
}
