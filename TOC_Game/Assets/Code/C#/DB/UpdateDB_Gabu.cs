using UnityEditor;
using UnityEngine;

public class UpdateDB_Gabu : MonoBehaviour
{
#if UNITY_EDITOR

    // エディタ上でのみ実行される
    [SerializeField]
    BaseUpGrageDB_Gabu[] DBs = null;

    private void Start()
    {
        foreach (var db in DBs)
        {
            // ここで必要な処理を行う
            db.UpGrageName = "Updated Name"; // 例: 名前を更新

            // ScriptableObject を保存
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
        }
        DBs = null;
    }

    private void Update()
    {
        if (DBs == null)
        {
            return;
        }

        foreach (var db in DBs)
        {
            // ここで必要な処理を行う
            db.UpGrageName = "Updated Name"; // 例: 名前を更新
                                             // ScriptableObject を保存
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
        }
    }
#endif
}
