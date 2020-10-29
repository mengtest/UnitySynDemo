using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
public class Gen2DMapByCameraEditor : Editor
{
	static string path = Application.dataPath;
	[MenuItem("地图/生成2d小地图(jpg)",false,100)]
	public static void Gen2DMap()
	{
       // Scene scene=SceneManager.GetActiveScene();
	//	Scene scene = EditorSceneManager.OpenScene(path+"/Scenes/mncj/mncj.unity");
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		if (cam == null)
		{
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/MiniMap/EditorCamera.prefab");
			cam = GameObject.Instantiate(prefab);
		}
		Camera camera = cam.GetComponent<Camera>();
		CaptureCamera(camera, new Rect(0,0,1920,1080));
		AssetDatabase.Refresh();
	}

	static Texture2D CaptureCamera(Camera camera, Rect rect)
	{
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 16);
		camera.targetTexture = rt;
		camera.Render();
		RenderTexture.active = rt;
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		screenShot.ReadPixels(rect, 0, 0);
		screenShot.Apply();
		camera.targetTexture = null;
		RenderTexture.active = null;  
		GameObject.DestroyImmediate(rt);
		byte[] bytes = screenShot.EncodeToPNG();
		string filename = Application.dataPath + "/Editor/MiniMap/Mini" + "Map.jpg";
		System.IO.File.WriteAllBytes(filename, bytes);
		Debug.Log(string.Format("截屏了一张照片: {0}", filename));
		return screenShot;
	}
}
