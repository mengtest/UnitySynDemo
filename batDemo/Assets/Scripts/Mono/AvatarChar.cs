using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarChar : MonoBehaviour
{

    //多个部件 通过名字拆分
    private string _charName;
    private Dictionary<string, string> _modelDic = new Dictionary<string, string>();
    private Dictionary<string, GameAssetRequest> _modelReqs = new Dictionary<string, GameAssetRequest>();

    private Dictionary<string, ModelObj> _modelObjDic = new Dictionary<string, ModelObj>();
    //时装合并贴图  必须 贴图开启readenable 的选项 必须重新打包. 这样内存开销会加大很多 减少drawcall
    public bool combineTexture = false;

    public bool combineMesh = false;
    // 最终材质（合并所有模型后使用的材质）
    public Material material;
    //
    private string _aniUrl;
    private GameAssetRequest _aniReqs;
    private GameObject mainObj;

    private string _weaponR_Url;
    private GameAssetRequest _weaponR_Reqs;
    private GameObject _weaponR;
    private string _weaponL_Url;
    private GameAssetRequest _weaponL_Reqs;
    private GameObject _weaponL;
    private bool inited = false;
    public void Init(string aniUrl, string[] modelpaths)
    {
        if (string.IsNullOrEmpty(aniUrl))
        {
            DebugLog.LogError("aniUrl null");
            return;
        }
        this._modelDic.Clear();
        this._modelReqs.Clear();
        for (int i = 0; i < modelpaths.Length; i++)
        {
            string part = modelpaths[i].Split('_')[1];
            this._modelDic[part] = (modelpaths[i]);
        }
        this._aniUrl = aniUrl;
        this._charName = this._aniUrl;
        this.gameObject.name = this._charName;
        this.material = null;
        inited = false;
        StartCoroutine(LoadAvatar());
    }
    IEnumerator LoadAvatar()
    {
        _aniReqs = GameAssetManager.Instance.LoadAsset<GameObject>(GetLoadPath(this._aniUrl), null);
        while (!_aniReqs.IsLoadComplete())
        {
            yield return 0;
        }
        this.mainObj = GameObject.Instantiate(_aniReqs.GetAsset(0)) as GameObject;
        this.mainObj.name = this._charName;
        this.mainObj.transform.parent = this.gameObject.transform;
        this.mainObj.transform.localPosition = new Vector3(0, 0, 0);

        Dictionary<string, string>.Enumerator iter = this._modelDic.GetEnumerator();
        while (iter.MoveNext())
        {
            GameAssetRequest gameReq = GameAssetManager.Instance.LoadAsset<GameObject>(GetLoadPath(iter.Current.Value), null);
            _modelReqs[iter.Current.Key] = gameReq;
            while (!gameReq.IsLoadComplete())
            {
                yield return 0;
            }
            TextAsset txtAs;
            if (!GameSettings.Instance.useAssetBundle)
            {
                txtAs = GameAssetManager.LoadGameAssetInEditor(GetBoneInfoPath(iter.Current.Value), typeof(TextAsset)) as TextAsset;
            }
            else
            {
                AssetBundleInfo info = gameReq.GetAssetBundleInfo(0);
                AssetBundle bundle = info.assetBundle;
                AssetBundleRequest req = bundle.LoadAssetAsync<TextAsset>(gameReq.GetAsset(0).name);
                yield return req;
                txtAs = req.asset as TextAsset;
            }

            JsonNode root = RareJson.ParseJson(txtAs.text);
            JsonArray bone = (JsonArray)root["bones"];
            List<string> bones = new List<string>();
            for (int j = 0; j < bone.Count; ++j)
            {
                JsonNode node = bone[j];
                bones.Add(node.Value);
            }
            string[] m_BonesName = bones.ToArray();
            bones = null;
            root = null;
            bone = null;
            //	GameObject.Destroy(txtAs);
            if (combineMesh)
            {
                _modelObjDic[iter.Current.Key] = new ModelObj(m_BonesName, gameReq.GetAsset(0) as GameObject);
            }
            else
            {
                GameObject partObj = GameObject.Instantiate(gameReq.GetAsset(0)) as GameObject;
                partObj.name = iter.Current.Value;
                partObj.transform.parent = mainObj.transform;
                partObj.transform.position =Vector3.zero;
                partObj.transform.rotation=Quaternion.Euler(0, 0, 0);

                List<Transform> bonesT = new List<Transform>();
                Transform[] transforms = mainObj.GetComponentsInChildren<Transform>();
                for (int i = 0, len = m_BonesName.Length; i < len; i++)
                {
                    for (int j = 0, jlen = transforms.Length; j < jlen; j++)
                    {
                        if (transforms[j].name != m_BonesName[i]) continue;
                        bonesT.Add(transforms[j]);
                        break;
                    }
                }

                SkinnedMeshRenderer r = partObj.GetComponent<SkinnedMeshRenderer>();
                if (r != null)
                {
                    r.bones = bonesT.ToArray();
                    r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                    r.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                    r.receiveShadows = false;
                    Bounds bound = r.bounds;
                    bound.center = new Vector3(0, 1, 0);
                    bound.extents = new Vector3(0.5f, 1, 0.5f);
                    bonesT.Clear();
                    transforms = null;
                }
                if (GameSettings.Instance.useAssetBundle)
                {
                    RenderHelper.RefreshShader(ref mainObj);
                }
            }
        }
        if (combineMesh)
        {
            CombineMeshs();
        }
        else
        {
            SkinnedMeshRenderer smr = mainObj.GetComponent<SkinnedMeshRenderer>();
            if (smr != null)
            {
                GameObject.Destroy(smr);
                // smr.sharedMaterials = new Material[] { };
                // smr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                // smr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                // smr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                // smr.receiveShadows = false;
            }
            resetAni();
        }
        inited = true;
    }
    private void resetAni(){
        Animation ani = mainObj.GetComponent<Animation>();
        ani.cullingType=AnimationCullingType.BasedOnRenderers;
    }
    // 合并Mesh
    private void CombineMeshs()
    {
        float startTime = Time.realtimeSinceStartup;

        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Material> materials = new List<Material>();
        List<Transform> bones = new List<Transform>();
        Transform[] transforms = mainObj.GetComponentsInChildren<Transform>();

        int width = 0;
        int height = 0;
        int uvCount = 0;
        List<Vector2[]> uvList = new List<Vector2[]>();
        List<Texture2D> textures = new List<Texture2D>();

        Dictionary<string, ModelObj>.Enumerator _dic = _modelObjDic.GetEnumerator();
        while (_dic.MoveNext())
        {
            ModelObj model = _dic.Current.Value;
            if (model.prefab == null)
            {
                continue;
            }
            SkinnedMeshRenderer smr = model.prefab.GetComponent<SkinnedMeshRenderer>();
            materials.AddRange(smr.sharedMaterials);
            for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                //              ci.transform = smr.transform.localToWorldMatrix;
                combineInstances.Add(ci);
            }

            if (combineTexture)
            {
                uvList.Add(smr.sharedMesh.uv);
                uvCount += smr.sharedMesh.uv.Length;
                //手机游戏一般单材质
                if (smr.sharedMaterials[0].mainTexture != null)
                {
                    if (material == null)
                    {
                        material = GameObject.Instantiate(smr.sharedMaterials[0]);
                        material.mainTexture = null;
                    }
                    Texture2D tex = smr.sharedMaterials[0].mainTexture as Texture2D;
                    textures.Add(tex);
                    width += tex.width;
                    height += tex.height;
                }
            }

            // As the SkinnedMeshRenders are stored in assetbundles that do not
            // contain their bones (those are stored in the characterbase assetbundles)
            // we need to collect references to the bones we are using
            for (int i = 0, len = model.bones.Length; i < len; i++)
            {
                for (int j = 0, jlen = transforms.Length; j < jlen; j++)
                {
                    if (transforms[j].name != model.bones[i]) continue;
                    bones.Add(transforms[j]);
                    break;
                }
            }
        }
        // Obtain and configure the SkinnedMeshRenderer attached to
        // the character base.
        if (combineInstances.Count > 0)
        {
            SkinnedMeshRenderer r = mainObj.GetComponent<SkinnedMeshRenderer>();
            if (r != null)
            {
                if (r.sharedMesh != null)
                {
                    GameObject.DestroyImmediate(r.sharedMesh);
                }
                r.sharedMesh = new Mesh();
                if (combineTexture)
                {
                    r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, false);
                    r.bones = bones.ToArray();
                    Texture2D skinnedMeshTexture = new Texture2D(get2Pow(width), get2Pow(height));
                    Rect[] packingResult = skinnedMeshTexture.PackTextures(textures.ToArray(), 0);
                    Vector2[] atlasUVs = new Vector2[uvCount];
                    r.sharedMaterials = new Material[] { material };

                    // 因为将贴图都整合到了一张图片上，所以需要重新计算UV
                    int j = 0;
                    for (int i = 0; i < uvList.Count; i++)
                    {
                        foreach (Vector2 uv in uvList[i])
                        {
                            atlasUVs[j].x = Mathf.Lerp(packingResult[i].xMin, packingResult[i].xMax, uv.x);
                            atlasUVs[j].y = Mathf.Lerp(packingResult[i].yMin, packingResult[i].yMax, uv.y);
                            j++;
                        }
                    }

                    // 设置贴图和UV
                    if (material.mainTexture != null)
                    {
                        GameObject.Destroy(material.mainTexture);
                    }
                    material.mainTexture = skinnedMeshTexture;
                    r.sharedMesh.uv = atlasUVs;
                }
                else
                {
                    r.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
                    r.bones = bones.ToArray();
                    r.sharedMaterials = materials.ToArray();
                }
                r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                r.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
                Bounds bound = r.bounds;
                bound.center = new Vector3(0, 1, 0);
                bound.extents = new Vector3(0.5f, 1, 0.5f);
                materials.Clear();
                bones.Clear();
                combineInstances.Clear();
                transforms = null;
            }
        }

        if (GameSettings.Instance.useAssetBundle)
        {
            RenderHelper.RefreshShader(ref mainObj);
        }
        DebugLog.Log("合并耗时 : " + (Time.realtimeSinceStartup - startTime) * 1000 + " ms");
    }
    IEnumerator AddPart(string partKey)
    {
        inited = false;
        string partPath;
        if (!this._modelDic.TryGetValue(partKey, out partPath))
        {
            yield return 0;
        }
        GameAssetRequest gameReq = GameAssetManager.Instance.LoadAsset<GameObject>(GetLoadPath(partPath), null);
        _modelReqs[partKey] = gameReq;
        while (!gameReq.IsLoadComplete())
        {
            yield return 0;
        }

        TextAsset txtAs;
        if (!GameSettings.Instance.useAssetBundle)
        {
            txtAs = GameAssetManager.LoadGameAssetInEditor(GetBoneInfoPath(partPath), typeof(TextAsset)) as TextAsset;
        }
        else
        {
            AssetBundleInfo info = gameReq.GetAssetBundleInfo(0);
            AssetBundle bundle = info.assetBundle;
            AssetBundleRequest req = bundle.LoadAssetAsync<TextAsset>(gameReq.GetAsset(0).name);
            yield return req;
            txtAs = req.asset as TextAsset;
        }

        JsonNode root = RareJson.ParseJson(txtAs.text);
        JsonArray bone = (JsonArray)root["bones"];
        List<string> bones = new List<string>();
        for (int j = 0; j < bone.Count; ++j)
        {
            JsonNode node = bone[j];
            bones.Add(node.Value);
        }
        string[] m_BonesName = bones.ToArray();
        bones = null;
        root = null;
        bone = null;
        //	GameObject.Destroy(txtAs);

        if (combineMesh)
        {
            _modelObjDic[partKey] = new ModelObj(m_BonesName, gameReq.GetAsset(0) as GameObject);
            CombineMeshs();
        }
        else
        {
            GameObject partObj = GameObject.Instantiate(gameReq.GetAsset(0)) as GameObject;
            partObj.name = gameReq.GetAsset(0).name;
            partObj.transform.parent = mainObj.transform;
            partObj.transform.position =Vector3.zero;
            partObj.transform.rotation=Quaternion.Euler(0, 0, 0);

            List<Transform> bonesT = new List<Transform>();
            Transform[] transforms = mainObj.GetComponentsInChildren<Transform>();
            for (int i = 0, len = m_BonesName.Length; i < len; i++)
            {
                for (int j = 0, jlen = transforms.Length; j < jlen; j++)
                {
                    if (transforms[j].name != m_BonesName[i]) continue;
                    bonesT.Add(transforms[j]);
                    break;
                }
            }

            SkinnedMeshRenderer r = partObj.GetComponent<SkinnedMeshRenderer>();
            if (r != null)
            {
                r.bones = bonesT.ToArray();
                r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                r.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
                Bounds bound = r.bounds;
                bound.center = new Vector3(0, 1, 0);
                bound.extents = new Vector3(0.5f, 1, 0.5f);
                bonesT.Clear();
                transforms = null;
            }
            resetAni();
            if (GameSettings.Instance.useAssetBundle)
            {
                RenderHelper.RefreshShader(ref mainObj);
            }
        }
        inited = true;
    }

    //更换部件.
    public void ChangePart(string partPath)
    {
        StartCoroutine(ChangePartCoroutine(partPath));
    }
    IEnumerator ChangePartCoroutine(string partPath)
    {
        while(!inited)
        {
            yield return 0;
        }
        string part = partPath.Split('_')[1];
        string oldPartPath;
        if (this._modelDic.TryGetValue(part, out oldPartPath))
        {
            if (oldPartPath == partPath)
            {
                //同样部件不用换.
            }
            else
            {
                if (_modelObjDic.ContainsKey(part))
                {
                    _modelObjDic[part].Release();
                    _modelObjDic.Remove(part);
                }
                if (_modelReqs.ContainsKey(part))
                {
                    GameAssetRequest gameReq = _modelReqs[part];
                    gameReq.Unload();
                    _modelReqs.Remove(part);
                }
                if (!combineMesh && mainObj != null)
                {
                    Transform tf = mainObj.transform.Find(oldPartPath);
                    if (tf != null)
                    {
                        GameObject.Destroy(tf.gameObject);
                    }
                }
                this._modelDic[part] = partPath;
                inited=false;
                yield return AddPart(part);
            }
        }
        else
        {
            //没有当前部件.
            this._modelDic[part] = partPath;
            inited=false;
            yield return AddPart(part);
        }
    }
    //换武器.  武器不合贴图
    public void ChangeWeapon(string partPath, bool isleft = false)
    {
        StartCoroutine(ChangeWeaponCoroutine(partPath, isleft));
    }
    private void RemoveWeapon(bool isleft = false)
    {
        if (isleft)
        {
            if (_weaponL_Reqs != null)
            {
                _weaponL_Reqs.Unload();
                _weaponL_Reqs = null;
            }
            if (_weaponL != null)
            {
                GameObject.DestroyImmediate(_weaponL);
                _weaponL = null;
            }

        }
        else
        {
            if (_weaponR_Reqs != null)
            {
                _weaponR_Reqs.Unload();
                _weaponR_Reqs = null;
            }
            if (_weaponR != null)
            {
                GameObject.DestroyImmediate(_weaponR);
                _weaponR = null;
            }

        }
    }
    IEnumerator AddWeapon(bool isleft)
    {
        inited = false;
        string url = isleft ? _weaponL_Url : _weaponR_Url;
        GameAssetRequest gameReq = GameAssetManager.Instance.LoadAsset<GameObject>(GetLoadPath(url), null);
        if (isleft)
        {
            _weaponL_Reqs = gameReq;
        }
        else
        {
            _weaponR_Reqs = gameReq;
        }
        while (!gameReq.IsLoadComplete())
        {
            yield return 0;
        }

        TextAsset txtAs;
        if (!GameSettings.Instance.useAssetBundle)
        {
            txtAs = GameAssetManager.LoadGameAssetInEditor(GetBoneInfoPath(url), typeof(TextAsset)) as TextAsset;
        }
        else
        {
            AssetBundleInfo info = gameReq.GetAssetBundleInfo(0);
            AssetBundle bundle = info.assetBundle;
            AssetBundleRequest req = bundle.LoadAssetAsync<TextAsset>(gameReq.GetAsset(0).name);
            yield return req;
            txtAs = req.asset as TextAsset;
        }

        JsonNode root = RareJson.ParseJson(txtAs.text);
        JsonArray bone = (JsonArray)root["bones"];
        List<string> bonesS = new List<string>();
        for (int j = 0; j < bone.Count; ++j)
        {
            JsonNode node = bone[j];
            bonesS.Add(node.Value);
        }
        string[] m_BonesName = bonesS.ToArray();
        bonesS = null;
        root = null;
        bone = null;
        //	GameObject.Destroy(txtAs);
        GameObject weapon;
        if (isleft)
        {
            this._weaponL = GameObject.Instantiate(gameReq.GetAsset(0)) as GameObject;
            weapon = _weaponL;
            weapon.name = _weaponL_Url;
        }
        else
        {
            this._weaponR = GameObject.Instantiate(gameReq.GetAsset(0)) as GameObject;
            weapon = _weaponR;
            weapon.name = _weaponR_Url;
        }
        weapon.transform.parent = mainObj.transform;
        weapon.transform.position =Vector3.zero;
        weapon.transform.rotation=Quaternion.Euler(0, 0, 0);

        List<Transform> bones = new List<Transform>();
        Transform[] transforms = mainObj.GetComponentsInChildren<Transform>();

        // As the SkinnedMeshRenders are stored in assetbundles that do not
        // contain their bones (those are stored in the characterbase assetbundles)
        // we need to collect references to the bones we are using
        for (int i = 0, len = m_BonesName.Length; i < len; i++)
        {
            for (int j = 0, jlen = transforms.Length; j < jlen; j++)
            {
                if (transforms[j].name != m_BonesName[i]) continue;
                bones.Add(transforms[j]);
                break;
            }
        }

        SkinnedMeshRenderer r = weapon.GetComponent<SkinnedMeshRenderer>();
        if (r != null)
        {
            r.bones = bones.ToArray();
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            r.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            r.receiveShadows = false;
            Bounds bound = r.bounds;
            bound.center = new Vector3(0, 1, 0);
            bound.extents = new Vector3(0.5f, 1, 0.5f);
            bones.Clear();
            transforms = null;
        }
        resetAni();
        if (GameSettings.Instance.useAssetBundle)
        {
            RenderHelper.RefreshShader(ref mainObj);
        }

        inited = true;
    }

    IEnumerator ChangeWeaponCoroutine(string partPath, bool isleft = false)
    {
        while(!inited)
        {
            yield return 0;
        }
        if (string.IsNullOrEmpty(partPath))
        {
            //卸载武器.
            RemoveWeapon(isleft);
        }
        else
        {
            if (isleft)
            {
                if (partPath != _weaponL_Url)
                {
                    RemoveWeapon(isleft);
                    _weaponL_Url = partPath;
                    inited=false;
                    //加载武器.
                    yield return AddWeapon(isleft);
                }
            }
            else
            {
                if (partPath != _weaponR_Url)
                {
                    RemoveWeapon(isleft);
                    _weaponR_Url = partPath;
                    inited=false;
                    //加载武器.
                    yield return AddWeapon(isleft);
                }
            }
        }
    }

    private string GetLoadPath(string path)
    {
        //	Avatar/xx/Model/xx
        return "Avatar/" + this._charName + "/Model/" + path;
    }
    private string GetBoneInfoPath(string path)
    {
        //	Avatar/xx/Model/xx
        return "TXTBonesInfo/" + path;
    }

    /// <summary>
    /// 获取最接近输入值的2的N次方的数，最大不会超过1024，例如输入320会得到512
    /// </summary>
    private int get2Pow(int into)
    {
        int outo = 1;
        for (int i = 0; i < 10; i++)
        {
            outo *= 2;
            if (outo > into)
            {
                break;
            }
        }

        return outo;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        //释放部件 	
        _modelDic = null;
        foreach (ModelObj model in _modelObjDic.Values)
        {
            model.Release();
        }
        _modelObjDic.Clear();

        foreach (GameAssetRequest gameAssetRequest in _modelReqs.Values)
        {
            gameAssetRequest.Unload();
        }
        _modelReqs.Clear();
        if (material != null)
        {
            // if(material.mainTexture!=null){
            //       GameObject.Destroy(material.mainTexture);
            // }
            GameObject.Destroy(material);
            material = null;
        }

        if (_aniReqs != null)
        {
            _aniReqs.Unload();
            _aniReqs = null;
        }
        if (_weaponL_Reqs != null)
        {
            _weaponL_Reqs.Unload();
            _weaponL_Reqs = null;
        }
        if (_weaponR_Reqs != null)
        {
            _weaponR_Reqs.Unload();
            _weaponR_Reqs = null;
        }
        if (_weaponL != null)
        {
            _weaponL = null;
        }
        if (_weaponR != null)
        {
            _weaponR = null;
        }
        if (mainObj != null)
        {
            GameObject.DestroyImmediate(mainObj);
            mainObj = null;
        }
    }
}

public class ModelObj
{
    public string[] bones { get; private set; }
    public GameObject prefab { get; private set; }
    public ModelObj(string[] bones, GameObject prefab)
    {
        this.bones = bones;
        this.prefab = prefab;
    }
    public void Release()
    {
        this.bones = null;
        this.prefab = null;
    }
}

public enum AvartarPart
{
    body = 0,
    head,
    limb,
    weaponL,
    weaponR,
}
