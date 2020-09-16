using UnityEngine;
using UnityEditor;

public  class PrefabUtils
{
       // 生成对象的预制
    public static UnityEngine.Object CreatePrefab(GameObject go, string name, string path = null)
    {
        if (path == null)
        {
            path = "Assets/" + name + ".prefab";
        }
        bool isSucess = false;
        GameObject tempPrefab = PrefabUtility.SaveAsPrefabAsset(go, path, out isSucess);
        if (!isSucess)
        {
            DebugLog.LogError("创建预制体错误", path);
            return null;
        }
        return tempPrefab;
    }

    //创建预置
    public static GameObject CreateNewPrefab(GameObject go, string path, bool connectToPrefab = true)
    {
        UnityEngine.Object emptyPrefab = PrefabUtility.CreateEmptyPrefab(path);
        ReplacePrefabOptions option;
        if (connectToPrefab)
        {
            option = ReplacePrefabOptions.ConnectToPrefab;
        }
        else
        {
            option = ReplacePrefabOptions.Default;
        }
        GameObject prefab = PrefabUtility.ReplacePrefab(go, emptyPrefab, option);
        EditorUtility.SetDirty(prefab);
        AssetDatabase.Refresh();
        return prefab;
    }
    // 生成对象的预制
    public static UnityEngine.Object GetPrefab(GameObject go, string name, bool bRemoveSrc = true)
    {
        UnityEngine.Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        if (bRemoveSrc)
        {
            UnityEngine.Object.DestroyImmediate(go);
        }
        AssetDatabase.Refresh();
        return tempPrefab;
    }
}
