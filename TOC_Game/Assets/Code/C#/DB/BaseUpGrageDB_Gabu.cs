using System.Numerics;
using UnityEngine;
using static DBManager_Gabu;

[CreateAssetMenu(menuName = "CreateData/BaseUpGrage")]
public class BaseUpGrageDB_Gabu : ScriptableObject
{

    [Header("アップグレードの名前")]
    public string UpGrageName = "Diamond";
    public int UpGrageID = 0;
    [Header("金のコスト")]
    public BigInteger moneyCost = 0;
    [Header("スターのコスト")]
    public BigInteger starCost = 0;
    [TextArea, Header("詳細情報")]
    public string infometion;
    [Header("画像")]
    public Sprite prefab;
    [Header("ツリー上での配置")]
    public Vector2Int treePosition;
    [Header("前提条件(必要なアップグレード)")]
    public BaseUpGrageDB_Gabu[] premises;
    [Header("変動させるステータス")]
    public E_FluctuateStats[] fluctuateStats;
}