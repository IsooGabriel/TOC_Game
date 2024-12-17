using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DBManager_Gabu;

public class TreeManager_Gabu : MonoBehaviour
{
    TreeElementSystem_Gabu[] elements;
    public GameObject elementPrefab;
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI detaliTMP;
    public TextMeshProUGUI moneyCostTMP;
    public TextMeshProUGUI starCostTMP;
    public TextMeshProUGUI unlockTMP;
    public Button unlickButton;
    // prefabに変更を加え複製する、
    // DBからデータを取得して、TreeElementSystem_Gabuの配列に格納する、解放の有無を確認する
    private void Awake()
    {
        if (elementPrefab.GetComponent<TreeElementSystem_Gabu>() == null)
        {
            Debug.LogWarning("TreeElementSystem_Gabuがアタッチされていません");
            elementPrefab.AddComponent<TreeElementSystem_Gabu>();
        }

        foreach (var element in DB.baseUpGrageDBs)  // アップグレードをインスタンス
        {
            GameObject obj = Instantiate(elementPrefab, transform);
            TreeElementSystem_Gabu tes = obj.GetComponent<TreeElementSystem_Gabu>();
            obj.name = element.name;
            obj.transform.localPosition = new Vector3(element.treePosition.x, element.treePosition.y, 0);
            tes.image.sprite = element.prefab;
            tes.image.color = element.color;
            tes.baseUpGrageDB_Gabu = element;
            if (DB.playerDBs[DB.AccountID].baseUpGrages[element.UpGrageID])
            {

            }
        }
    }

    public void SerectElement()
    {

    }
}
