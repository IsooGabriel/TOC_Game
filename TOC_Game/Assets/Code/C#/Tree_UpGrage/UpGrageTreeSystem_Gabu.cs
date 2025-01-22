using UnityEngine;
using static DBManager_Gabu;

public class UpGrageTreeSystem_Gabu : MonoBehaviour
{
    [SerializeField,Header("スキルツリーを構成するアップグレード一覧")]
    private BaseUpGradeDB_StatChangeSkill_Gabu[] baseUpGrageDBs;
    [SerializeField]
    private GameObject prefab;

    private void Start()
    {
        if(baseUpGrageDBs == null)
        {
            Debug.LogWarning("baseUpGrageDBs is null");
            return;
        }
        foreach(var upGrage in baseUpGrageDBs)
        {
            GameObject item = Instantiate(prefab);
            if (upGrage.treePosition.x == 0)
            {
                upGrage.treePosition = new Vector2Int(0, 0);
            }
        }
    }
}
