using UnityEngine;
using static DBManager_Gabu;

public class SeveDB_Gabu : MonoBehaviour
{
    public DBManager_Gabu dbManagerGabu;

    private void Awake()
    {
        SaveScriptableObject();
    }

    private void SaveScriptableObject()
    {
        if (dbManagerGabu != null)
        {
            string path = "Assets/Resources/DBManager_Gabu.asset";
            UnityEditor.AssetDatabase.CreateAsset(dbManagerGabu, path);
            UnityEditor.AssetDatabase.SaveAssets();
            Debug.Log("DBManager_Gabu has been saved to " + path);
        }
        else
        {
            Debug.LogError("dbManagerGabu is not assigned.");
        }
    }
}
