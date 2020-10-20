/*
 * @Descripttion: 
 * @version: 1.0.0
 * @Author: xsddxr909
 * @Date: 2020-02-24 16:31:04
 * @LastEditors: xsddxr909
 * @LastEditTime: 2020-10-20 12:15:15
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Lua;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMain : MonoSingleton<GameMain> {

    public string LuaMemory="Lua内存使用量: 0 KB";
   //刷新时间.
    private float memTimeRefsh=0;
    private bool luainitCom=false;
    


    List<MonoBehaviour> _behaviourList = new List<MonoBehaviour>();
    private void Awake()
    {

        Application.targetFrameRate = 30;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        DontDestroyOnLoad(gameObject);

        GameObject obj= GameObject.Find("Reporter");
        if(obj!=null){
              DontDestroyOnLoad(obj);
        }
    }

    private void Update() {
        LuaMemoryShow();
    }
    private void LuaMemoryShow(){
        if(!luainitCom){
            return;
        }
        if(memTimeRefsh<=0){
           LuaMemory=string.Format("Lua内存使用量: {0}KB",LuaManager.Instance.getGCCount());
           memTimeRefsh=1;
        }else{
          memTimeRefsh-=Time.deltaTime;
        }
    }
	// Use this for initialization
	void Start () {
		StartCoroutine(StartInCoroutine());
	}
    private IEnumerator StartInCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Input.multiTouchEnabled=true;
        luainitCom=false;


        DebugLog.addErrorLogCall(onGameError);
        InitManagers();
        yield return InitGameAsset();

    //    LuaManager.Instance.InitStart();
    
    }	
	private void onGameError()
    {
#if UNITY_EDITOR
        return;
#endif

    }
	public void Restart()
    {
		//重置游戏 OnRestartGame()
		 StartCoroutine(RestartInCoroutine());
	}
	 IEnumerator RestartInCoroutine()
    {
		this.ResetManager();
		 yield return InitGameAsset();
	}


	void AddBehaviour<T>() where T: MonoBehaviour
    {
        _behaviourList.Add(gameObject.AddComponent<T>());
    }
   /**
     * @name: xsddxr909
     * @test: 初始化管理者
     * @return: 
     */
    void InitManagers()
    {
       DebugLog.setLogSwitcher(true,true,true);
      //  this.AddBehaviour<LogManager>();
      this.AddBehaviour<PatchManager>();
      this.AddBehaviour<GameAssetManager>();
      this.AddBehaviour<CtrlManager>();
      this.AddBehaviour<ObjManager>();
      this.AddBehaviour<CameraManager>();
      EventCenter.init();
      CtrlManager.Instance.Init();
      AFC_Manager.init();
      ObjManager.Instance.Init();
      CameraManager.Instance.Init();
      GameAssetManager.Instance.setLocalUrlFun(PatchManager.Instance.GetSignedFileLocalURL);
      GameAssetManager.ABManifest=AssetBundleConst.AssetBundleFolder;

	}
	void ResetManager(){
		//重置游戏管理者 OnRestartGame()
        //顺序 必须由大到小.char ctrl ai 等
         ObjManager.Instance.ClearAll();
         CtrlManager.Instance.ClearAll();
	     GameAssetManager.Instance.OnRestartGame();
         //  FontManager.Instance.OnRestartGame();
     //    LuaManager.Instance.ClearSearchBundle();
   //     LuaManager.Instance.CallFunction("DestroyGame");
	}
    private GameAssetRequest reqs;
	IEnumerator InitGameAsset()
    {
         if (GameSettings.Instance.useAssetBundle)
         {
             yield return PatchManager.Instance.Initialize();
             yield return GameAssetManager.Instance.LoadManifest();
    //       yield return GameAssetManager.Instance.LoadLuaBundle();
    //       GameAssetManager.Instance.SetActiveVariants(new string[] { GameSettings.CurrentLanguage.ToLower() });
         }

        if(GameSettings.Instance.isLoadRemoteAsset)
        {
            DebugLog.Log("isLoadRemoteAsset PatchManager.InitializePatch");
            //通过请求获得 这两个参数.或者 找serverList中解析
           yield return PatchManager.Instance.InitializePatch(GameSettings.Instance.GetInitWebPath());
           if(PatchManager.Instance.needUpDownloadWeb){
               //需要更新.
                PatchManager.Instance.UpdatePatch(onCompleteFun,null);
           }else{
               //不需要更新 进入游戏.
               startLogic();
           }
        }else{
            startLogic();
        }
	}
    public  void onCompleteFun(bool isFinish){
        if(isFinish){
            DebugLog.Log(">>>Update onCompleten Restart");
            this.Restart();
        }else{
            //下载未完成 检测网络.
        }
    }
     private void startLogic(){
          DebugLog.Log("startLogic","startLogic");
        StartCoroutine(startLogicCoroutine());
     }
    IEnumerator startLogicCoroutine(){
       //启动lua   
        this.AddBehaviour<LuaManager>();
          //完全  热更方案
        //1. 通过lua  做UI  驱动 服务器CS执行命令 反馈 客户端 动作表现  单向发送命令  UDP     服务器 单向 推送同步 UDP
         //2. 客户端 只存在 动作 事件与执行  还有渲染  最大减少性能开销 ps:优化 可以客户端先做移动 然后服务器数据去lerp 也行. 
         //3. 服务器 做逻辑  移动逻辑 开枪逻辑  同步结果   (因为移动逻辑开销其实不大, 50人同时做移动其实还好, 射击逻辑开销也很小 同时开枪频率不高)
         //4.  AI逻辑 behaviorDesginer 去做. 可以通过服务器实现 最多 同时 AI最大数 25人 消耗应该不大.
         //5. 共开25个房间  50人异常   1250 人 场 人均同时在线 800  4核8G  5000链接 
         
        yield return LuaManager.Instance.InitStart();
        luainitCom=true;
        //TODO: 关闭加载界面.
       //  CanvasGroup canvas= gameObject.GetComponent<CanvasGroup>();
    //     gameObject.SetActive
   //    yield return  GameExample.Instance.testPool();
        
    }
    #if !UNITY_EDITOR
        private void OnGUI()
        {
            GUILayout.Space(100);

            if (GUILayout.Button("REPOORT", GUILayout.Width(60), GUILayout.Height(60)))
            {
                Reporter.Instance.doShow();
            }
            GUILayout.Label(LuaMemory);

        }
    #else
        private void OnGUI()
        {
            GUILayout.Space(100);

            // if (GUILayout.Button("REPOORT", GUILayout.Width(60), GUILayout.Height(60)))
            // {
            //     Reporter.Instance.doShow();
            // }
            GUILayout.Label(LuaMemory);
            // if(CharManager.Instance.CharPool!=null){
            //  GUILayout.Label(CharManager.Instance.CharPool.toString());
            // }
        }
    #endif
}
