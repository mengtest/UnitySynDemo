/*
 * @Description: 重写 Transform
 * @Version: 1.0
 * @Autor: xsddxr909
 * @Date: 2020-05-15 11:13:21
 * @LastEditors: xsddxr909
 * @LastEditTime: 2020-09-18 17:25:00
 */ 
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// 扩展Transform，添加复制坐标和旋转 缩放 的按钮
/// </summary>
[CustomEditor(typeof(Weapon_Gun),true)]
public class Weapon_GunInspector : Editor {
 
    public string savePath = "Data/GunData";
    private Editor editor;
    private Weapon_Gun mono;

    private  WeaponGun_Serializable _data;     


    List<Transform> m_DotsList = new List<Transform>();

    Transform m_DotsParent;
    Transform m_CenterDot;
    int layer;
    
    private void Init(){
        if(m_DotsParent==null){
            layer = LayerMask.NameToLayer("Ignore Raycast");
            m_DotsParent = GameObject.Find("UIRoot/guide").transform;
            m_CenterDot = m_DotsParent.Find("RecoilCenterDot").transform;
            m_CenterDot.gameObject.SetActive(true);
            m_CenterDot.SetAsFirstSibling();

            for (int i = 0; i < m_DotsParent.childCount; i++)
            {
                if (i == 0) continue;//CenterDots
                m_DotsList.Add(m_DotsParent.GetChild(i));
            }
        }
    }
 
    public override void OnInspectorGUI()
    {
        mono = target as Weapon_Gun;
        chkData();
        if (!_data){
            if(GUILayout.Button("创建数据--CreatData", GUILayout.Height(30))){
              this.chkCreatData();
            }
            return;
        }
        base.OnInspectorGUI();
        Init();
        
        this.UpdateFun();

        GUILayout.Space(15);
        bool isSave = GUILayout.Button("更新保存--Save", GUILayout.Height(30));
        GUILayout.Space(15);
        bool isReload = GUILayout.Button("还原配置--Reload", GUILayout.Height(30));
        GUILayout.Space(40);

        bool strToList = GUILayout.Button(new GUIContent("后坐力-Generate", "字符转成数组数据"));
        GUILayout.Space(5);
        bool opReFlash = GUILayout.Button(new GUIContent("后坐力-UI-ReFlash", "修改不保存对应Asset文件"));
        GUILayout.Space(5);
        bool opClear = GUILayout.Button(new GUIContent("后坐力-UI-Clear", "清除"));

        //GUILayout.FlexibleSpace();
        GUILayout.Space(10);
        bool opCopyYawData = GUILayout.Button(new GUIContent("Copy Yaw 后坐力", "复制Yaw数据到粘贴板"));
        GUILayout.Space(10);
        bool opCopyPitchData = GUILayout.Button(new GUIContent("Copy Pitch 后坐力", "复制Pitch数据到粘贴板"));
        GUILayout.Space(20);


        if(isSave){
            SaveData();
        }
        if(isReload){
            LoadData();
            ClearRecoilData();
        }


        if (strToList) GenerateRecoilData();
        if (opReFlash) ReFlashRecoilData();
        if (opClear) ClearRecoilData();
        if (opCopyYawData) CopyRecoilData(true);
        if (opCopyPitchData) CopyRecoilData(false);

    }
    private void chkData(){
        if (!_data)
        {
            string path = Path.Combine("Assets", "Res", savePath, mono.name + ".asset");
            _data = AssetDatabase.LoadAssetAtPath<WeaponGun_Serializable>(path);
        //    this.LoadData();
        }else{
            if (!mono.data)
            {
                mono.data = _data;
            }
        }
    }
    private void chkCreatData(){
        if (!_data)
        {
            string path = Path.Combine("Assets", "Res", savePath, mono.name + ".asset");
            if (string.IsNullOrWhiteSpace(path))
                path = EditorUtility.SaveFilePanelInProject("Save settings", mono.name + ".asset", "asset",
                    "Please enter a file name to save the settings to");

            var settingAsset = ScriptableObject.CreateInstance<WeaponGun_Serializable>();

            settingAsset.Name = mono.name;

            AssetDatabase.CreateAsset(settingAsset, path);

            mono.data = settingAsset;
            _data = settingAsset;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            this.LoadData();
        }
        else
        {
            if (!mono.data)
            {
                mono.data = _data;
            }
        }
    }
    public void LoadData(){
        mono.LoadData();
        mono.getGunPath();
    }
    public void SaveData(){
         mono.SaveData();
         mono.getGunPath();
         EditorUtility.SetDirty(_data);
         AssetDatabase.SaveAssets();
         AssetDatabase.Refresh();
    }
    
    private void UpdateFun(){
        if(mono.muzzleTrans==null){
            mono.muzzleTrans= mono.gameObject.transform.Find("muzzle");
            if(mono.muzzleTrans==null){
            GameObject muzzle= new GameObject("muzzle"); 
            mono.muzzleTrans=muzzle.transform;
            mono.muzzleTrans.parent=mono.transform;
            mono.muzzleTrans.localPosition=Vector3.zero;
            }
        }else{
            mono.muzzlePos=mono.muzzleTrans.localPosition;
        }
        CreateInteractiveRadius();
    }
    private void CreateInteractiveRadius()
	{
        if(mono.col==null){
              mono.col=mono.gameObject.GetComponent<BoxCollider>();
          if(mono.col==null){
               mono.col = mono.gameObject.AddComponent<BoxCollider>();
           }
           mono.col.enabled=false;
        }
        if(mono.interactiveRadius==null){
           mono.interactiveRadius=mono.gameObject.GetComponent<SphereCollider>();
           if(mono.interactiveRadius==null){
		      mono.interactiveRadius = mono.gameObject.AddComponent<SphereCollider>();
              mono.interactiveRadius.center = mono.col.center;
              mono.interactiveRadius.radius = 1f;
              mono.interactiveRadius.isTrigger = true;
           }
        }
        mono.BoxSize=mono.col.size;
        mono.BoxCenter=mono.col.center;
        mono.SphereColRadius=mono.interactiveRadius.radius;
        mono.SphereColCenter=mono.interactiveRadius.center;
	}
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~后坐力~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
     private void ClearRecoilData()
    {
        for (int i = 0; i < m_DotsList.Count; i++)
        {
             GameObject.DestroyImmediate(m_DotsList[i].gameObject);
        }
        m_DotsList.Clear();
     //   mono.recoilList.Clear();
    }

    private void GenerateRecoilData()
    {
        //生成数据.
        GreanerateVet2();
        //UI 上 从新生成 点坐标.
        DrawCanvas();
    }
     private void ReFlashRecoilData()
    {
        mono.recoilList.Clear();
        m_DotsList.Clear();
        for (int i = 0; i < m_DotsParent.childCount; i++)
        {
            if (i == 0) continue;//CenterDots
            m_DotsList.Add(m_DotsParent.GetChild(i));
        }

        m_DotsList.Sort(DotsComparison);

        for (int i = 0; i < m_DotsList.Count; i++)
        {
            m_DotsList[i].SetSiblingIndex(i + 1);
            Text txtOrder = m_DotsList[i].GetComponentInChildren<Text>();
            if ((int)float.Parse(txtOrder.text.Trim()) != i || !m_DotsList[i].name.Replace("Dot","").Equals(i.ToString()))
            {
                txtOrder.text = i.ToString();
                m_DotsList[i].name = "Dot" + i;
            }
            RectTransform rectTransform = m_DotsList[i].GetComponent<RectTransform>();
            Vector2 recoil = new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y);
            mono.recoilList.Add(recoil);
        }

        mono.Yaw_params = GetYawDatas();
        mono.Pitch_params = GetPitchDatas();
    }
   public string GetYawDatas()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0; i < mono.recoilList.Count; i++)
        {
            builder.Append(GetYaw(mono.recoilList[i]));
            if (i != mono.recoilList.Count - 1)
                builder.Append("|");
        }
        return builder.ToString();
    }
    private void CopyRecoilData(bool isYaw)
    {
        GUIUtility.systemCopyBuffer = isYaw ? mono.Yaw_params : mono.Pitch_params;
    }
    public string GetPitchDatas()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0; i < mono.recoilList.Count; i++)
        {
            builder.Append(GetPitch(mono.recoilList[i]));
            if (i != mono.recoilList.Count - 1)
                builder.Append("|");
        }
        return builder.ToString();
    }
    float GetYaw(Vector2 recoil)
    {
        return (float)Math.Round(recoil.x, 3);
    }

    float GetPitch(Vector2 recoil)
    {
        return (float)Math.Round(recoil.y, 3);
    }

    int DotsComparison(Transform x, Transform y)
    {
        if (x.GetComponentInChildren<Text>().text.Trim().Equals(y.GetComponentInChildren<Text>().text.Trim()))
            return x.name.CompareTo(y.name);
        return float.Parse(x.GetComponentInChildren<Text>().text.Trim()).CompareTo(float.Parse(y.GetComponentInChildren<Text>().text.Trim()));
    }
    public void DrawCanvas()
    {
        if (mono.recoilList.Count == 0) return;
        m_DotsList.Clear();
        int create = mono.recoilList.Count - m_DotsList.Count;

        for (int i = 0; i < create; i++)
        {
            string name = "Dot" + i;
            Transform dot = m_DotsParent.Find(name);
            if (dot == null)
            {
                dot = UnityEngine.Object.Instantiate(m_CenterDot, m_DotsParent);
                dot.gameObject.name = name;
                Text txt = dot.GetComponentInChildren<Text>();
                if (txt.gameObject.layer != layer) txt.gameObject.layer = layer;
                txt.text = i.ToString();
                dot.GetComponent<Image>().color = Color.red;
                dot.GetComponent<RectTransform>().localScale = Vector3.one * 0.002f;
            }
            m_DotsList.Add(dot);
        }

        for (int i = 0; i <  mono.recoilList.Count; i++)
        {
            m_DotsList[i].localPosition =  mono.recoilList[i];
        }
    }

    private void GreanerateVet2(){
        if(mono.Yaw_params==""||mono.Pitch_params=="")
        {
            return;
        }
        mono.recoilList.Clear();

        string[] datasYaw = mono.Yaw_params.Split('|');
        string[] datasPitch = mono.Pitch_params.Split('|');

        if (datasYaw.Length != datasPitch.Length)
        {
            Debug.LogError("Gun:" + mono.Name + "  Yaw_params's Length not equals Pitch_params's!!!!!!!");

            if (datasYaw.Length > datasPitch.Length)
            {
                for (int i = 0; i < datasYaw.Length; i++)
                {
                    if (i >= datasPitch.Length)
                    {
                        Vector2 recoil = new Vector2() { x = float.Parse(datasYaw[i]), y = float.Parse(datasYaw[i]) };
                        mono.recoilList.Add(recoil);
                    }
                    else
                    {
                        Vector2 recoil = new Vector2() { x = float.Parse(datasYaw[i]), y = float.Parse(datasPitch[i]) };
                        mono.recoilList.Add(recoil);
                    }
                }
            }
            else
            {
                for (int i = 0; i < datasPitch.Length; i++)
                {
                    if (i >= datasYaw.Length)
                    {
                        Vector2 recoil = new Vector2() { x = float.Parse(datasPitch[i]), y = float.Parse(datasPitch[i]) };
                        mono.recoilList.Add(recoil);
                    }
                    else
                    {
                        Vector2 recoil = new Vector2() { x = float.Parse(datasYaw[i]), y = float.Parse(datasPitch[i]) };
                        mono.recoilList.Add(recoil);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < datasYaw.Length; i++)
            {
                Vector2 recoil = new Vector2() { x = float.Parse(datasYaw[i]), y = float.Parse(datasPitch[i]) };
                mono.recoilList.Add(recoil);
            }
        }
    }
}