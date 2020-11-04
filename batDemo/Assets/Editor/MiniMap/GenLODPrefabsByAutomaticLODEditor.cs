using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;

//自动 生成 LOD插件.
public class GenLODPrefabsByAutomaticLODEditor : Editor
{
    private   static List<AutomaticLOD> objList;
    private  static List<string> objPathList;
    private  static int doSave=0;

    private static float waitTime=0;
    [MenuItem("地图/选中Prefab目录生成BuildLOD",false,200)]
	public static void GenLODBuild()
	{
          GenLOD(true);
    }
	[MenuItem("地图/选中Prefab目录生成Lod",false,200)]
	public static void GenLODOther()
	{
           GenLOD(false);
    }
    public static void GenLOD(bool isbuilding)
	{    
           EditorApplication.update -= onUpdate;
         EditorApplication.update += onUpdate;

        string  path = Application.dataPath;
        Scene scene = EditorSceneManager.OpenScene(path+"/Scene/LodScene.unity");

        string allPath = "";
        string DirName = "";
        foreach (UnityEngine.Object o in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            DirName = o.name;
            allPath = AssetDatabase.GetAssetPath(o);
            break;
        }
        objList=new List<AutomaticLOD>();
        objPathList=new List<string>();
        doSave=0;
        FileInfo[] fileList = new DirectoryInfo(allPath).GetFiles("*.prefab", SearchOption.TopDirectoryOnly);
        for (int k = 0; k < fileList.Length; k++)
        {
            FileInfo fi = fileList[k];
             path = new FileInfo(path).FullName;
            DebugLog.Log(path);
            string objPath= fi.FullName.Replace(path,"");
            objPath="Assets"+objPath;
            DebugLog.Log(fi.FullName,objPath);
        //  //打开场景 选中目录.不递归. 查找预制.
            GameObject uo = AssetDatabase.LoadAssetAtPath(objPath, typeof(GameObject)) as GameObject;
            if(uo==null){
                continue;
            }

            GameObject MeshClone = (GameObject)UnityEngine.Object.Instantiate((GameObject)uo);
            MeshClone.name = uo.name;

            //判断 模型是否有父类节点.
            Renderer renderer=  MeshClone.GetComponent<Renderer>();
            if(renderer!=null){
               GameObject parent=new GameObject(uo.name);
                MeshClone.transform.parent=parent.transform;
                MeshClone.transform.localPosition=Vector3.zero;
                int len=MeshClone.transform.childCount;
                for (int i = 0; i < len; i++)
                {
                    MeshClone.transform.GetChild(0).parent=parent.transform;
                }
                MeshClone=parent;
            }
            
            AutomaticLOD automaticLOD= MeshClone.GetComponent<AutomaticLOD>();
            if(automaticLOD!=null){
                GameObject.DestroyImmediate(automaticLOD.gameObject);
                 continue;
            }
            automaticLOD = MeshClone.AddComponent<AutomaticLOD>();
            //判断顶点数 确认lod分多少个档.
            int vertexBufferCount=1000;
            MeshFilter mf= MeshClone.gameObject.GetComponentInChildren<MeshFilter>();
            if(mf==null){
                 SkinnedMeshRenderer skr= MeshClone.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                 if(skr!=null){
                      vertexBufferCount=  skr.sharedMesh.vertexCount;
                 }
                 skr=null;
            }else{
               vertexBufferCount= mf.sharedMesh.vertexCount;
               if(isbuilding){
                   //建筑自动加上4个点.
                   
               }
            }
            mf=null;
            
            vertexBufferCount= Mathf.RoundToInt(vertexBufferCount/1000);
            switch(vertexBufferCount){
                case 0:
                case 1:
                   automaticLOD.m_levelsToGenerate=AutomaticLOD.LevelsToGenerate._2;
                break;
                case 2:
                   automaticLOD.m_levelsToGenerate=AutomaticLOD.LevelsToGenerate._3;
                break;
                default:
                  automaticLOD.m_levelsToGenerate=AutomaticLOD.LevelsToGenerate._4;
                break;
            }
            objList.Add(automaticLOD);
            objPathList.Add(objPath);
            Selection.activeGameObject = automaticLOD.gameObject;
        }
	}
    private static void onUpdate(){
          if(doSave==0&&objList.Count>0){
               waitTime-=Time.deltaTime;
              if(waitTime<=0){
                  waitTime=3;
                  DebugLog.Log("GenerateLODData", objList[0].gameObject.name);
                  Selection.activeGameObject= objList[0].gameObject;
                  objList[0].GenerateLODData=true;
                  doSave=1;
                  AssetDatabase.Refresh();
                  return;
               }
          }
          if(doSave==1&&objList.Count>0){
              waitTime-=Time.deltaTime;
              if(waitTime<=0){
                     waitTime=3;
                    AutomaticLOD automaticLOD=objList[0];
                    automaticLOD.m_strAssetPath="Assets/Res/LODData/"+ "mesh_" + automaticLOD.gameObject.name  + ".asset";
                    automaticLOD.EnablePrefabUsage=true;
                    automaticLOD.m_bEnablePrefabUsage=true;
                    Selection.activeGameObject=automaticLOD.gameObject;
                    doSave=2;
                    DebugLog.Log("SaveData", objList[0].gameObject.name);
                    AssetDatabase.Refresh();
                   return;
              }
          }
            if(doSave==2&&objList.Count>0){
                waitTime-=Time.deltaTime;
                if(waitTime<=0){
                     waitTime=3;
                    AutomaticLOD automaticLOD=objList[0];


                 //   AssetDatabase.DeleteAsset(objPathList[0]);
                    PrefabUtility.SaveAsPrefabAsset(automaticLOD.gameObject,objPathList[0]);
                    GameObject.DestroyImmediate(automaticLOD.gameObject);

                    objList.RemoveAt(0);
                    objPathList.RemoveAt(0);
                    doSave=0;
                    if(objList.Count<=0){
                        EditorApplication.update -= onUpdate;
                    }
                    AssetDatabase.Refresh();
                    return;
                }
            }
    }
}
