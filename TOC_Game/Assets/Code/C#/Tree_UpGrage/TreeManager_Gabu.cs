using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private void Start()
    {
        foreach(var element in elements)
        {

        }
    }

    public void SerectElement()
    {

    }
}
