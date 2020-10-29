using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class charOutputData {
    public string matName;
    public string tagName;
    public string newMatPath;
    public string oldName;
    public string oldPath;
    public string newTexturePath;
}
public enum packCharType{
    partType=0,
    oneSetType=1,
    allSetType=2
}
public class CharacterBuild {

public static void CreateMaterials(string allPath,string DirName){
        List<Object> _pList = new List<Object>();
        List<string> _pathTex = new List<string>();
         List<string> _pathNTex = new List<string>();

     
        string pOut = allPath + "/Textures/";
        if (!Directory.Exists(pOut))
        {
           FileUtils.CreateDir(pOut);
        }
        AssetDatabase.Refresh();
         FileInfo[] fileList = new DirectoryInfo(allPath).GetFiles();
         //移动贴图
         for (int k = 0; k < fileList.Length; k++)
        {
              FileInfo fi = fileList[k];
              if (fi.Extension != ".tga") continue;
              AssetDatabase.StartAssetEditing();
              string oldTPath=allPath+"/"+fi.Name;
              string newTPath=allPath+"/Textures/"+fi.Name;
               TextureImporter textureImp = TextureImporter.GetAtPath(oldTPath) as TextureImporter;
               textureImp.textureType = TextureImporterType.Default;
               textureImp.mipmapEnabled = false;
               AssetDatabase.MoveAsset(oldTPath, newTPath);
               AssetDatabase.StopAssetEditing();
                 AssetDatabase.Refresh();
        }
        FileInfo[] fileListTex = new DirectoryInfo(allPath+"/Textures").GetFiles();
         //贴图 重命名复原
         for (int k = 0; k < fileListTex.Length; k++)
        {
              FileInfo fi = fileListTex[k];
              if (fi.Extension != ".tga") continue;
              AssetDatabase.StartAssetEditing();
              string oldTPath=allPath+"/Textures/"+fi.Name;
              string backName=fi.Name.Replace(DirName+"_","");
              backName=backName.Replace("_tga.tga","");
               TextureImporter textureImp = TextureImporter.GetAtPath(oldTPath) as TextureImporter;
               textureImp.textureType = TextureImporterType.Default;
               textureImp.mipmapEnabled = false;
               AssetDatabase.RenameAsset(oldTPath, backName);
               AssetDatabase.StopAssetEditing();
                 AssetDatabase.Refresh();
        }
        //重命名fbx
       for (int k = 0; k < fileList.Length; k++)
        {
              FileInfo fi = fileList[k];
             if (fi.Extension != ".FBX") continue;
             if (fi.Name.IndexOf("@")<0) continue;
            string oldTPath=allPath+"/"+fi.Name;
            int start=fi.Name.IndexOf("@")+1;
            int len=fi.Name.LastIndexOf(".")-start;
             string subname=fi.Name.Substring(start,len);
         //    DebugLog.Log(DirName+"@"+subname);
              AssetDatabase.StartAssetEditing();
               AssetDatabase.RenameAsset(oldTPath, DirName+"@"+subname);
                AssetDatabase.StopAssetEditing();
                  AssetDatabase.Refresh();
        }

        string meshStrPath = allPath + "/" + DirName + "@Mesh.FBX";
        float globalScale = 1;
        AssetDatabase.StartAssetEditing();
        //Modification des attribut du mesh avant de le pr茅fabriquer
        ModelImporter OBJI = ModelImporter.GetAtPath(meshStrPath) as ModelImporter;
        globalScale = OBJI.globalScale;
        OBJI.importAnimation = false;
        OBJI.importMaterials = true;
        OBJI.meshCompression = ModelImporterMeshCompression.High;
        OBJI.animationType = ModelImporterAnimationType.Legacy;
        OBJI.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
        OBJI.materialLocation=ModelImporterMaterialLocation.External;
        //AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
        AssetDatabase.ImportAsset(meshStrPath, ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
       
        // AssetDatabase.StartAssetEditing();
        // //Modification des attribut du mesh avant de le pr茅fabriquer
        // OBJI = ModelImporter.GetAtPath(meshStrPath) as ModelImporter;
        // globalScale = OBJI.globalScale;
        // OBJI.importAnimation = false;
        // OBJI.importMaterials = false;
        // OBJI.meshCompression = ModelImporterMeshCompression.High;
        // OBJI.animationType = ModelImporterAnimationType.Legacy;
        // OBJI.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
        // OBJI.materialLocation=ModelImporterMaterialLocation.External;
        // //AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
        // AssetDatabase.ImportAsset(meshStrPath, ImportAssetOptions.ForceSynchronousImport);
        // AssetDatabase.StopAssetEditing();
        // AssetDatabase.Refresh();


       //动画文件分离重命名.
        List<string> _pathAni = new List<string>();

       // Animation动画改格式......
        fileList = new DirectoryInfo(allPath).GetFiles();
        for (int k = 0; k < fileList.Length; k++)
        {
              FileInfo fi = fileList[k];
             if (fi.Extension != ".FBX") continue;
             if (fi.Name.IndexOf("@")<0) continue;
             if (fi.Name.Contains("@Mesh"))
            {
                continue;
            }
            string pppap = allPath+"/"+ fi.Name;
            _pathAni.Add(pppap);
        }


        for (int i = 0; i < _pathAni.Count; i++)
        {
            string ppath = _pathAni[i];
            AssetDatabase.StartAssetEditing();
            //Modification des attribut du mesh avant de le prefabriquer
            OBJI = ModelImporter.GetAtPath(ppath) as ModelImporter;
            OBJI.globalScale =globalScale;
            OBJI.importAnimation = true;
            if (!ppath.Contains(ProjectActionLabel.STAND_UP) && (ppath.Contains(ProjectActionLabel.STAND) || ppath.Contains(ProjectActionLabel.RUN) || ppath.Contains(ProjectActionLabel.Winner)))
            {

                OBJI.animationWrapMode = WrapMode.Loop;
            }
            else
            {
                OBJI.animationWrapMode = WrapMode.ClampForever;
            }
            for (int ii = 0; ii < OBJI.clipAnimations.Length; ii++)
            {
                ModelImporterClipAnimation mmAni = OBJI.clipAnimations[ii];
              if (OBJI.animationWrapMode == WrapMode.Loop)
              {
                  mmAni.wrapMode = WrapMode.Loop;
                  mmAni.loop = true;
              }
              else {
                  mmAni.wrapMode = WrapMode.ClampForever;
              }
            }
            OBJI.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;
            OBJI.meshCompression = ModelImporterMeshCompression.High;
            OBJI.animationType = ModelImporterAnimationType.Legacy;
            OBJI.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
            //AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
            AssetDatabase.ImportAsset(ppath, ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.StopAssetEditing();
                    AssetDatabase.Refresh();
        }
        List<AnimationClip> listClip = new List<AnimationClip>();
        //   AnimationEvent
        for (int i = 0; i < _pathAni.Count; i++)
        {
            string ppath = _pathAni[i];
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(ppath);
            foreach (Object o in objs)
            {
                if (o is AnimationClip&&!o.name.Contains("Take"))
                {
                    listClip.Add((AnimationClip)o);
       //             DebugLog.Log("ppath:"+ppath+ " name:  "+o.name + "is clip");
                }
            }
        }
        //_pathAni 所有动画
        UnityEngine.Object uo = AssetDatabase.LoadAssetAtPath(meshStrPath, typeof(GameObject));
        GameObject MeshClone = (GameObject)Object.Instantiate((GameObject)uo);
        MeshClone.name = uo.name;
        Animation anis=  MeshClone.GetComponent<Animation>();
        for (int p = 0; p < listClip.Count; p++)
        {
            anis.AddClip(listClip[p], listClip[p].name);
            if (listClip[p].name == ProjectActionLabel.STAND)
            {
                anis.clip = anis.GetClip(ProjectActionLabel.STAND);
            }
        }
        if( anis.clip==null&&listClip.Count>0){
            anis.clip = anis.GetClip(listClip[0].name);
        }
        AssetDatabase.DeleteAsset( allPath + "/" + DirName + ".prefab");
        AssetDatabase.Refresh();
        List<charOutputData> outMatList=new List<charOutputData>();
        List<charOutputData> outTgaList = new List<charOutputData>();
        charOutputData ooput;
      //  MeshClone
        // 如果Resource.Load()找到有和 新命名 同样名字的材质球  直接赋值
        if(MeshClone.GetComponentsInChildren<SkinnedMeshRenderer>()!=null){
            foreach (SkinnedMeshRenderer smr in MeshClone.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                string matName= DirName + "_" + smr.gameObject.name;
                string tagName = DirName + "_" + smr.gameObject.name + "_tga";

                string newMatPath = allPath + "/Materials/" + matName + ".mat";
                string newTexturePath = allPath + "/Textures/" + tagName + ".tga";
                //加载是否有该贴图...
                Texture texture = (Texture)AssetDatabase.LoadAssetAtPath(newTexturePath, typeof(Texture));
                if (texture != null)
                {

                }
                else
                {
                    //在检测是否有老贴图在 有才换名.....
                    ooput = new charOutputData();
                    ooput.matName = matName;
                    ooput.tagName = tagName;
                    ooput.newMatPath = newMatPath;
                    ooput.newTexturePath = newTexturePath;
                    ooput.oldName = smr.gameObject.name;
                    ooput.oldPath = allPath + "/Textures/" + ooput.oldName + ".tga";
                    texture=null;
                    texture = (Texture)AssetDatabase.LoadAssetAtPath(ooput.oldPath, typeof(Texture));
                    if (texture != null)
                    {
                        outTgaList.Add(ooput);
                    }
                }

                //先加载是否有该材质球..
                Material matt = (Material)AssetDatabase.LoadAssetAtPath(newMatPath, typeof(Material));
                if (matt != null)
                {
                    smr.material = matt;
                }
                else {
                    //在检测是否有老材质球在 有才换名.....
                    ooput = new charOutputData();
                    ooput.matName = matName;
                    ooput.tagName = tagName;
                    ooput.newMatPath = newMatPath;
                    ooput.oldName = smr.sharedMaterials[0].name;
                    ooput.oldPath = allPath + "/Materials/" + ooput.oldName + ".mat";
                    matt = null;
                    matt = (Material)AssetDatabase.LoadAssetAtPath(ooput.oldPath, typeof(Material));
                    if (matt != null)
                    {
                        outMatList.Add(ooput);
                    }
                }
            }
        }else{
            foreach (MeshRenderer smr in MeshClone.GetComponentsInChildren<MeshRenderer>())
            {
                string matName= DirName + "_" + smr.gameObject.name;
                string tagName = DirName + "_" + smr.gameObject.name + "_tga";

                string newMatPath = allPath + "/Materials/" + matName + ".mat";
                string newTexturePath = allPath + "/Textures/" + tagName + ".tga";
                //加载是否有该贴图...
                Texture texture = (Texture)AssetDatabase.LoadAssetAtPath(newTexturePath, typeof(Texture));
                if (texture != null)
                {

                }
                else
                {
                    //在检测是否有老贴图在 有才换名.....
                    ooput = new charOutputData();
                    ooput.matName = matName;
                    ooput.tagName = tagName;
                    ooput.newMatPath = newMatPath;
                    ooput.newTexturePath = newTexturePath;
                    ooput.oldName = smr.gameObject.name;
                    ooput.oldPath = allPath + "/Textures/" + ooput.oldName + ".tga";
                    texture=null;
                    texture = (Texture)AssetDatabase.LoadAssetAtPath(ooput.oldPath, typeof(Texture));
                    if (texture != null)
                    {
                        outTgaList.Add(ooput);
                    }
                }

                //先加载是否有该材质球..
                Material matt = (Material)AssetDatabase.LoadAssetAtPath(newMatPath, typeof(Material));
                if (matt != null)
                {
                    smr.material = matt;
                }
                else {
                    //在检测是否有老材质球在 有才换名.....
                    ooput = new charOutputData();
                    ooput.matName = matName;
                    ooput.tagName = tagName;
                    ooput.newMatPath = newMatPath;
                    ooput.oldName = smr.sharedMaterials[0].name;
                    ooput.oldPath = allPath + "/Materials/" + ooput.oldName + ".mat";
                    matt = null;
                    matt = (Material)AssetDatabase.LoadAssetAtPath(ooput.oldPath, typeof(Material));
                    if (matt != null)
                    {
                        outMatList.Add(ooput);
                    }
                }
            }

        }

        for (int p = 0; p < outMatList.Count; p++)
        {
             if(outMatList[p]==null)continue;
            AssetDatabase.RenameAsset(outMatList[p].oldPath, outMatList[p].matName);
      //      DebugLog.Log(outMatList[p].oldPath);
       //      DebugLog.Log(outMatList[p].matName);
            AssetDatabase.Refresh();
            Material matt = (Material)AssetDatabase.LoadAssetAtPath(outMatList[p].newMatPath, typeof(Material));
            //  if (matt==null){
            //         DebugLog.LogError(outMatList[p].newMatPath+" ,err Material");
            //  }
            if (matt!=null&&!matt.shader.name.Contains("Blue/Character/B"))
            {
                matt.shader = Shader.Find("Blue/Character/Base");
            }
           AssetDatabase.Refresh();
         }
        for (int p = 0; p < outTgaList.Count; p++)
        {
              if(outTgaList[p]==null)continue;
            AssetDatabase.RenameAsset(outTgaList[p].oldPath, outTgaList[p].tagName);
               AssetDatabase.Refresh();
            AssetDatabase.StartAssetEditing();
            TextureImporter textureImp = TextureImporter.GetAtPath(outTgaList[p].newTexturePath) as TextureImporter;
            textureImp.textureType = TextureImporterType.Default;
            textureImp.mipmapEnabled = false;
            AssetDatabase.ImportAsset(outTgaList[p].newTexturePath, ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.StopAssetEditing();
                    AssetDatabase.Refresh();
         }

        PrefabUtils.CreateNewPrefab(MeshClone, allPath + "/" + DirName + ".prefab");
        listClip.Clear();
        listClip = null;
        GameObject.DestroyImmediate(MeshClone);
        _pathAni.Clear();
         AssetDatabase.Refresh();
}


//重命名 动画配置 
 public static void PackageCharReName(string allPath,string DirName,packCharType ptype=packCharType.allSetType) {
    
        //加载Mesh
        string meshStrPath = allPath + "/" + DirName + "@Mesh.FBX";


    //     //Model......................................................................  
        //  //下面关闭 为了切断依赖 打包的图片 材质球 关联. 切断依赖

        AssetDatabase.StartAssetEditing();
        //Modification des attribut du mesh avant de le pr茅fabriquer
        ModelImporter OBJI = ModelImporter.GetAtPath(meshStrPath) as ModelImporter;
     //   globalScale = OBJI.globalScale;
        OBJI.importAnimation = false;
        OBJI.importMaterials = false;
        OBJI.meshCompression = ModelImporterMeshCompression.High;
        OBJI.animationType = ModelImporterAnimationType.Legacy;
        OBJI.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
    //    OBJI.materialLocation=ModelImporterMaterialLocation.External;
        //AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
        AssetDatabase.ImportAsset(meshStrPath, ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();


        AssetDatabase.StartAssetEditing();
        //Modification des attribut du mesh avant de le pr茅fabriquer
        OBJI = ModelImporter.GetAtPath(meshStrPath) as ModelImporter;
     //   globalScale = OBJI.globalScale;
        OBJI.importAnimation = false;
        OBJI.importMaterials = true;
        OBJI.meshCompression = ModelImporterMeshCompression.High;
        OBJI.animationType = ModelImporterAnimationType.Legacy;
        OBJI.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
        OBJI.materialLocation=ModelImporterMaterialLocation.External;
        //AssetDatabase.ImportAsset (T4MPrefabFolder+"Terrains/Meshes/"+FinalExpName+".obj", ImportAssetOptions.TryFastReimportFromMetaData);
        AssetDatabase.ImportAsset(meshStrPath, ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();


        string modelPath = allPath + "/" + DirName + ".prefab";
		GameObject model = AssetDatabase.LoadAssetAtPath(modelPath, typeof(GameObject)) as GameObject;
        string subPath= allPath.Replace("/"+DirName,"");

         switch(ptype){
             case packCharType.partType:
                PackageModelMesh(model, subPath);
                PackagePartMesh(model, subPath, DirName);
             break;
             case packCharType.oneSetType:
                CreatPartPrefab(model, subPath, DirName,false);
             break;
             case packCharType.allSetType:
                CreatPartPrefab(model, subPath, DirName,true);
             break;
         }    
    }

    //打包模型动作..
    private static void PackageModelMesh(GameObject characterFBX, string allPath)
    {
        // Create a directory to store the generated assetbundles.
        string pOut = allPath + "/" + characterFBX.name+"/Model/";
        if (!Directory.Exists(pOut))
        {
           FileUtils.CreateDir(pOut);
        }
            AssetDatabase.Refresh();
        //AssetDatabase.DeleteAsset(pOut+"/xx");
        // AssetDatabase.CreateFolder(pOut, "xx");
        // Save bones and animations to a seperate assetbundle. Any 
        // possible combination of CharacterElements will use these
        // assets as a base. As we can not edit assets we instantiate
        // the fbx and remove what we dont need. As only assets can be
        // added to assetbundles we save the result as a prefab and delete
        // it as soon as the assetbundle is created.
        GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);

        // postprocess animations: we need them animating even offscreen
        foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
        {
            anim.cullingType = AnimationCullingType.BasedOnRenderers;
        }

        foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            Object.DestroyImmediate(smr.gameObject);
        }
        AssetDatabase.DeleteAsset(pOut + characterFBX.name + ".prefab");
        AssetDatabase.Refresh();
        characterClone.AddComponent<SkinnedMeshRenderer>();
        Object characterBasePrefab = PrefabUtils.GetPrefab(characterClone, characterFBX.name);
      //  string path = strOutPath + characterFBX.name + ".rm";
        //BuildPipeline.BuildAssetBundle(characterBasePrefab, null, path, BuildAssetBundleOptions.CollectDependencies);
    //    AssetBoundleExport.SavePrefabObj((UnityEngine.Object)characterBasePrefab, ref path);
        string outEndPath = AssetDatabase.GetAssetPath(characterBasePrefab);
        AssetDatabase.MoveAsset(outEndPath, pOut + characterBasePrefab.name + ".prefab");
         AssetDatabase.Refresh();
        //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(characterBasePrefab));
    }


	//生成部件 prefab
    private static void PackagePartMesh(GameObject characterFBX, string allPath, string DirName)
    {
        // Create a directory to store the generated assetbundles.
        //string strOutPath = outputPath + "/" + fileName + "/" + DirName + "/";
        string pOut = allPath +"/"+DirName + "/Model/";
        if (!Directory.Exists(pOut))
        {
            FileUtils.CreateDir(pOut);
        }
            AssetDatabase.Refresh();
        // Collect materials.
     //   List<Material> materials = EditorHelper.CollectAll<Material>(CharacterMaterialsPath(characterFBX));
        GameObject cloneFBX= (GameObject) PrefabUtility.InstantiatePrefab(characterFBX);
          PrefabUtility.UnpackPrefabInstance(cloneFBX, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        // Create assetbundles for each SkinnedMeshRenderer.
        foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            List<Object> toinclude = new List<Object>();

            // Save the current SkinnedMeshRenderer as a prefab so it can be included
            // in the assetbundle. As instantiating part of an fbx results in the
            // entire fbx being instantiated, we have to dispose of the entire instance
            // after we detach the SkinnedMeshRenderer in question.
            GameObject rendererClone = (GameObject)Object.Instantiate(smr.gameObject);
             rendererClone.name = rendererClone.name.Replace("(Clone)", "");

            AssetDatabase.DeleteAsset(pOut + rendererClone.name + ".prefab");
                AssetDatabase.Refresh();
            Object rendererPrefab = PrefabUtils.GetPrefab(rendererClone, rendererClone.name);
            toinclude.Add(rendererPrefab);
            //     rendererClone.GetComponent<SkinnedMeshRenderer>().materials;

            JsonObject root = new JsonObject("ds");
            JsonArray mat = new JsonArray("materials");

            GameObject oooobj = (GameObject)GameObject.Instantiate(smr.gameObject);

            Material[] mttt = oooobj.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
            // Collect applicable materials.
            foreach (Material m in mttt)
            {
                //                 if (m.name.Contains(smr.name.ToLower()))
                //                 {
                string stt = m.name.Replace(" (Instance)", "");
                mat.Add(stt);
            }
            root["materials"] = mat;
            GameObject.DestroyImmediate(oooobj);
            // When assembling a character, we load SkinnedMeshRenderers from assetbundles,
            // and as such they have lost the references to their bones. To be able to
            // remap the SkinnedMeshRenderers to use the bones from the characterbase assetbundles,
            // we save the names of the bones used.
            JsonArray bones = new JsonArray("bones");
            foreach (Transform t in smr.bones)
            {
                bones.Add(t.name);
            }
            root["bones"] = bones;
            root["rootbones"] = smr.rootBone.name;

            // Save the assetbundle.
            //string stringholderpath = "Assets/bonenames.asset";
            //StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
            //holder.content = boneNames.ToArray();
            //AssetDatabase.CreateAsset(holder, stringholderpath);
            //toinclude.Add(AssetDatabase.LoadAssetAtPath(stringholderpath, typeof(StringHolder)));
            string bundleName = smr.name.ToLower();
            //bundleName = name + "_" + bundleName;

            string ppS = "Assets/Res/TXTBonesInfo/" + DirName+"_"+rendererPrefab.name + ".txt";
            AssetDatabase.DeleteAsset(ppS);
            RareJson.Serialize(root, ppS, true);

//             string strDescFile = strOutPath + characterFBX.name  + ".txt";
//             Engine.RareJson.Serialize(root, strDescFile, true);
//             int gg = strDescFile.IndexOf("Assets/ResRoot/Data/PlatformAssets/");
//             string assetUrl = strDescFile.Substring(gg);
            AssetDatabase.Refresh();
//             UnityEngine.Object uo = AssetDatabase.LoadAssetAtPath(assetUrl, typeof(UnityEngine.Object));
//             toinclude.Add(uo);

        //   string path = strOutPath + characterFBX.name + ".rs";
            //BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies);
           
            //打包....
//            AssetBoundleExport.SavePrefabObjs(ref toinclude, ref path);
//             string ppS = "Assets/Res/TXTBonesInfo/"+characterFBX.name  + ".txt";
//             AssetDatabase.DeleteAsset(ppS);
//             AssetDatabase.MoveAsset(assetUrl, ppS);
            string outEndPath = AssetDatabase.GetAssetPath(rendererPrefab);
            ppS = pOut + DirName+"_"+rendererPrefab.name+".prefab";
           
            AssetDatabase.MoveAsset(outEndPath, ppS);
            AssetDatabase.Refresh();
        }
        GameObject.DestroyImmediate(cloneFBX);
    }
	private static void CreatPartPrefab(GameObject characterFBX, string allPath, string DirName,bool onePakage){
		 string pOut = allPath +"/"+DirName + "/";
        if (!Directory.Exists(pOut))
        {
            FileUtils.CreateDir(pOut);
        }
            AssetDatabase.Refresh();
		pOut=allPath +"/"+DirName + "/";



        int Fcounts=0;

		//创建prefab
        foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true))
        {
            GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(smr.gameObject);
            GameObject rendererParent = rendererClone.transform.parent.gameObject;

            string exIdx=rendererClone.name.Substring(rendererClone.name.LastIndexOf("_")+1);

		    foreach (SkinnedMeshRenderer smr_b in rendererParent.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
                string exIdx2=smr_b.gameObject.name.Substring(smr_b.gameObject.name.LastIndexOf("_")+1);
				if(exIdx!=exIdx2){
                      Object.DestroyImmediate(smr_b.gameObject);
				}
			}
             //各自包中 未实现...
			string name1=rendererParent.name +"^"+exIdx;
            if(onePakage){
              //打在一个包中.
              name1=rendererParent.name +"_"+exIdx;
            }
            AssetDatabase.DeleteAsset(pOut + name1 + ".prefab");
             AssetDatabase.Refresh();
            Object rendererPrefab = PrefabUtils.GetPrefab(rendererParent,name1);
			string outEndPath = AssetDatabase.GetAssetPath(rendererPrefab);
            AssetDatabase.MoveAsset(outEndPath, pOut + name1 + ".prefab");
            Fcounts++;
             AssetDatabase.Refresh();
		}
        if(Fcounts==0){
            //没有 SkinnedMeshRenderer 的简单模型打包.
              GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(characterFBX.gameObject);
              Object rendererPrefab = PrefabUtils.GetPrefab(rendererClone,DirName+"_01");
              	string outEndPath = AssetDatabase.GetAssetPath(rendererPrefab);
                    AssetDatabase.MoveAsset(outEndPath, pOut + DirName+"_01" + ".prefab");
                    AssetDatabase.Refresh();
        }

	}
}

