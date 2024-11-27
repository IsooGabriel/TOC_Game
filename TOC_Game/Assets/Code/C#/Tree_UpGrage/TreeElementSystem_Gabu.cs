using UnityEngine;
using TMPro;
using static DBManager_Gabu;

public class TreeElementSystem_Gabu : MonoBehaviour
{
    [Header("アイコン")]
    public Sprite sprite;
    [Header("詳細")]
    public TextMeshProUGUI tmp;
    [Header("バフの種類と数値")]
    public FluctuateStatsDictionary fluctuateStatsDictionary;
}
