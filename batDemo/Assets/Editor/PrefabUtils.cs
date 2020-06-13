using UnityEngine;
using UnityEditor;

public  class PrefabUtils
{

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
