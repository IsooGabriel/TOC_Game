using UnityEngine;
using UnityEditor;

// GameObjectReferenceスクリプト用のカスタムエディタ
[CustomEditor(typeof(EnemyDB_Gabu))]
public class GameObjectReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 元のInspector内容を表示
        base.OnInspectorGUI();

        // スクリプトの対象オブジェクトを取得
        EnemyDB_Gabu refScript = (EnemyDB_Gabu)target;

        // ゲームオブジェクトが設定されている場合にプレビューを表示
        if (refScript.prefab != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("GameObject Preview", EditorStyles.boldLabel);

            // ゲームオブジェクトのプレビューを描画
            Texture2D preview = AssetPreview.GetAssetPreview(refScript.prefab);

            if (preview != null)
            {
                GUILayout.Box(preview, GUILayout.Width(100), GUILayout.Height(100));
            }
            else
            {
                GUILayout.Label("Preview not available", EditorStyles.helpBox);
            }
        }
        else
        {
            GUILayout.Space(10);
            GUILayout.Label("No GameObject Assigned", EditorStyles.helpBox);
        }
    }
}
