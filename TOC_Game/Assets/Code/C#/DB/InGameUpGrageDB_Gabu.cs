using System.Numerics;
using UnityEngine;
using static DBManager_Gabu;

[CreateAssetMenu(menuName = "CreateData/InGameUpGrage")]
public class InGameUpGrageDB_Gabu : ScriptableObject
{
    [Header("アップグレードの名前")]
    public string UpGradeName = "Diamond";
    public int UpGrageID = 0;
    [Header("金のコスト")]
    public long moneyCost = 0;
    [Header("スターのコスト")]
    public long starCost = 0;
    //public int enemyLocalID = 0;
    [TextArea, Header("詳細情報")]
    public string infometion;   
    [Header("画像")]
    public Sprite prefab;
    [Header("変動させるステータス")]
    public E_FLUCTUATE_STATS[] fluctuateStats;
}