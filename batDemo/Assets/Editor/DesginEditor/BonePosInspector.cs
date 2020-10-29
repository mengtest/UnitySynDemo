/*
 * @Description: 重写 Transform
 * @Version: 1.0
 * @Autor: xsddxr909
 * @Date: 2020-05-15 11:13:21
 * @LastEditors: xsddxr909
 * @LastEditTime: 2020-10-30 06:35:39
 */ 
using UnityEngine;
using UnityEditor;
 
/// <summary>
/// BonePos
/// </summary>
[CustomEditor(typeof(BonePos),true)]
public class BonePosInspector : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
          EditorGUILayout.Space();
       	bool init = GUILayout.Button("InitBonePos", GUILayout.Width(180f));
        if (init) {
            (target as BonePos).initAni();
        }
    }
 	
 
}