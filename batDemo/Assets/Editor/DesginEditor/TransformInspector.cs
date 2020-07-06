/*
 * @Description: 重写 Transform
 * @Version: 1.0
 * @Autor: xsddxr909
 * @Date: 2020-05-15 11:13:21
 * @LastEditors: xsddxr909
 * @LastEditTime: 2020-05-15 17:22:29
 */ 
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Text;
 
/// <summary>
/// 扩展Transform，添加复制坐标和旋转 缩放 的按钮
/// </summary>
[CustomEditor(typeof(Transform),true)]
public class TransformInspector : Editor {
 
    private Editor editor;
    private Transform transform;
    
   
    SerializedProperty mPos;

    void OnEnable()
    {
     //   editor = CreateEditor(target, Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.TransformInspector", true));
        try
        {
            var so = serializedObject;
            mPos = so.FindProperty("m_LocalPosition");
        }
        catch { }
    }
 
    public override void OnInspectorGUI()
    {
        transform = target as Transform;
  //      editor.OnInspectorGUI();
  
        EditorGUIUtility.labelWidth = 15f;
        serializedObject.Update();
        DrawWorldPosition();
        GUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        GUILayout.EndHorizontal();
        DrawLocalPosition();
        DrawLocalRotation();
        DrawLocalScale();
        serializedObject.ApplyModifiedProperties();
    }
 	void DrawLocalPosition ()
	{
		GUILayout.BeginHorizontal();
		bool reset = GUILayout.Button("R", GUILayout.Width(20f));
        bool resetInt = GUILayout.Button("Int", GUILayout.Width(30f));
        GUILayout.Label("Local_Pos ");
		EditorGUILayout.PropertyField(mPos.FindPropertyRelative("x"));
		EditorGUILayout.PropertyField(mPos.FindPropertyRelative("y"));
		EditorGUILayout.PropertyField(mPos.FindPropertyRelative("z"));
        bool copy = GUILayout.Button("C", GUILayout.Width(21f));
        bool paste = GUILayout.Button("P", GUILayout.Width(20f));
		GUILayout.EndHorizontal();

		//GUILayout.BeginHorizontal();
		//reset = GUILayout.Button("W", GUILayout.Width(20f));
		//EditorGUILayout.Vector3Field("", (target as Transform).position);

		if (reset) {
            mPos.vector3Value = Vector3.zero;
        }
        if (resetInt)
        {
            Vector3 pos = mPos.vector3Value;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);
            pos.z = Mathf.RoundToInt(pos.z);
            mPos.vector3Value = pos;
        }
        if(copy){
            TextEditor textEd = new TextEditor();
            StringBuilder str = new StringBuilder();
            str.Append(transform.localPosition.x + ",");
            str.Append(transform.localPosition.y + ",");
            str.Append(transform.localPosition.z);
            textEd.text = str.ToString();
            textEd.OnFocus();
            textEd.Copy();
        }
        if(paste){
         Vector3 vec = changeVec3Str(GUIUtility.systemCopyBuffer);
            if (vec.Equals(Vector3.zero))
                return;
            transform.localPosition = vec;
        }
		//GUILayout.EndHorizontal();
	}
     void DrawWorldPosition ()
	{
		GUILayout.BeginHorizontal();
		bool reset = GUILayout.Button("R", GUILayout.Width(20f));
        bool resetInt = GUILayout.Button("Int", GUILayout.Width(30f));
        GUILayout.Label("World_Pos");
		 Vector3 wPos = DrawVector3(transform.position);
        bool copy = GUILayout.Button("C", GUILayout.Width(21f));
        bool paste = GUILayout.Button("P", GUILayout.Width(20f));
		GUILayout.EndHorizontal();

		//GUILayout.BeginHorizontal();
		//reset = GUILayout.Button("W", GUILayout.Width(20f));
		//EditorGUILayout.Vector3Field("", (target as Transform).position);

         if (GUI.changed)
        {
            //NGUIEditorTools.RegisterUndo("Transform Change", trans);
            transform.position     =  Validate(wPos);
            
        }
		if (reset) {
            transform.position = Vector3.zero;
        }
        if (resetInt)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);
            pos.z = Mathf.RoundToInt(pos.z);
            transform.position = pos;
        }
        if(copy){
            TextEditor textEd = new TextEditor();
            StringBuilder str = new StringBuilder();
            str.Append(transform.position.x + ",");
            str.Append(transform.position.y + ",");
            str.Append(transform.position.z);
            textEd.text = str.ToString();
            textEd.OnFocus();
            textEd.Copy();
        }
        if(paste){
         Vector3 vec = changeVec3Str(GUIUtility.systemCopyBuffer);
            if (vec.Equals(Vector3.zero))
                return;
            transform.position = vec;
        }
	}
	void DrawLocalRotation ()
	{
		GUILayout.BeginHorizontal();
		
		bool reset = GUILayout.Button("R", GUILayout.Width(20f));
        GUILayout.Label("   Local_Rotate  ");
        Vector3 rot = DrawVector3(transform.localEulerAngles);
	 bool copy = GUILayout.Button("C", GUILayout.Width(21f));
        bool paste = GUILayout.Button("P", GUILayout.Width(20f));

		GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            //NGUIEditorTools.RegisterUndo("Transform Change", trans);
           transform.localEulerAngles  = Validate(rot);
            
        }
        
        if (reset)
        {
           transform.localEulerAngles = Vector3.zero;
        }
        if(copy){
             TextEditor textEd = new TextEditor();
            StringBuilder str = new StringBuilder();
            str.Append(transform.localRotation.eulerAngles.x + ",");
            str.Append(transform.localRotation.eulerAngles.y + ",");
            str.Append(transform.localRotation.eulerAngles.z);
            textEd.text = str.ToString();
            textEd.OnFocus();
            textEd.Copy();
        }
        if(paste){
             Vector3 qua = changeVec3Str(GUIUtility.systemCopyBuffer);
            if (qua.Equals(Vector3.zero))
                return;
            transform.localRotation = Quaternion.Euler(qua);
        }
	}
    void DrawLocalScale(){
        GUILayout.BeginHorizontal();
		
		bool reset = GUILayout.Button("R", GUILayout.Width(20f));
        GUILayout.Label("   Local_Scale   ");
        Vector3 scale = DrawVector3(transform.localScale);
		  bool copy = GUILayout.Button("C", GUILayout.Width(21f));
        bool paste = GUILayout.Button("P", GUILayout.Width(20f));

		GUILayout.EndHorizontal();

        if (GUI.changed)
        {
            //NGUIEditorTools.RegisterUndo("Transform Change", trans);
           transform.localScale  = Validate(scale);
            
        }
        
        if (reset)
        {
           transform.localScale = Vector3.one;
        }
        if(copy){
             TextEditor textEd = new TextEditor();
            StringBuilder str = new StringBuilder();
            str.Append(transform.localScale.x + ",");
            str.Append(transform.localScale.y + ",");
            str.Append(transform.localScale.z);
            textEd.text = str.ToString();
            textEd.OnFocus();
            textEd.Copy();
        }
        if(paste){
             Vector3 qua = changeVec3Str(GUIUtility.systemCopyBuffer);
            if (qua.Equals(Vector3.zero))
                return;
            transform.localScale = qua;
        }
    }

    //1,1,1
    Vector3 changeVec3Str(string str) {
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        string[] strs = str.Split(',');
        if (strs.Length != 3)
        {
            return Vector3.zero;
        }
        else {
            float x = float.Parse(strs[0].ToString());
            float y = float.Parse(strs[1].ToString());
            float z = float.Parse(strs[2].ToString());
 
            return new Vector3(x, y, z);
        }
    }
    static Vector3 Validate (Vector3 vector)
    {
        vector.x = float.IsNaN(vector.x) ? 0f : vector.x;
        vector.y = float.IsNaN(vector.y) ? 0f : vector.y;
        vector.z = float.IsNaN(vector.z) ? 0f : vector.z;
        return vector;
    }
    Vector3 DrawVector3 (Vector3 value)
    {
        GUILayoutOption opt = GUILayout.MinWidth(30f);
        value.x = EditorGUILayout.FloatField("X", value.x, opt);
        value.y = EditorGUILayout.FloatField("Y", value.y, opt);
        value.z = EditorGUILayout.FloatField("Z", value.z, opt);
        return value;
    }

 
}