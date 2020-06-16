/*
 * @Descripttion: 
 * @version: 1.0.0
 * @Author: xsddxr909
 * @Date: 2020-02-24 16:31:04
 * @LastEditors: xsddxr909
 * @LastEditTime: 2020-06-16 20:09:48
 */
using System;
using System.Collections;
using System.Collections.Generic;
using Lua;
using LuaInterface;
using UnityEngine;

public class GameMain : MonoSingleton<GameMain> {

    List<MonoBehaviour> _behaviourList = new List<MonoBehaviour>();
    private void Awake()
    {

        Application.targetFrameRate = 30;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        DontDestroyOnLoad(gameObject);
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
      GameAssetManager.Instance.setLocalUrlFun(PatchManager.Instance.GetSignedFileLocalURL);
      GameAssetManager.ABManifest=AssetBundleConst.AssetBundleFolder;

	}
	void ResetManager(){
		//重置游戏管理者 OnRestartGame()
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
           yield return PatchManager.Instance.InitializePatch("http://192.168.1.230:8080/web");
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
        yield return LuaManager.Instance.InitStart();


        AvatarChar avatar= GameExample.Instance.CreatAvatar();	  
        AvatarChar avatar2= GameExample.Instance.CreatAvatar();	  
        avatar.gameObject.transform.Translate(new Vector3(-50,0,0));
        avatar2.gameObject.transform.Translate(new Vector3(50,0,0));
        yield return 0;
        avatar.ChangePart("Infility_limb_01");
        avatar2.ChangePart("Infility_limb_02");
        yield return new WaitForSeconds(0.5f);
        avatar.ChangePart("Infility_body_02");
        avatar2.ChangePart("Infility_body_01");
        yield return new WaitForSeconds(1);
        avatar.ChangePart("Infility_head_02");
        avatar2.ChangePart("Infility_head_02");
        yield return new WaitForSeconds(2);
        avatar.ChangeWeapon("Infility_weapon_01");
        avatar2.ChangeWeapon("Infility_weapon_03");
        yield return new WaitForSeconds(10);
        avatar.ChangeWeapon("Infility_weapon_02_01");
        avatar.ChangeWeapon("Infility_weapon_02_02",true);
        yield return new WaitForSeconds(10);
        avatar.ChangeWeapon("Infility_weapon_03");
        avatar.ChangeWeapon("",true);
        avatar2.ChangeWeapon("Infility_weapon_01");
        avatar2.ChangeWeapon("",true);
    }
    #if !UNITY_EDITOR
        private void OnGUI()
        {
            GUILayout.Space(100);

            if (GUILayout.Button("REPOORT", GUILayout.Width(60), GUILayout.Height(60)))
            {
                Reporter.Instance.doShow();
            }


        }
    #endif
}
