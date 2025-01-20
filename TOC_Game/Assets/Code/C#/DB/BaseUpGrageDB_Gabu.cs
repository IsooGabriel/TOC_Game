using System.Numerics;
using UnityEngine;
using static DBManager_Gabu;

[CreateAssetMenu(menuName = "CreateData/BaseUpGrage")]
public class BaseUpGrageDB_Gabu : ScriptableObject
{

    [Header("アップグレードの名前")]
    public string UpGrageName = "Diamond";
    public int UpGradeID = 0;
    [Header("金のコスト")]
    public long moneyCost = 0;
    [Header("スターのコスト")]
    public long starCost = 0;
    [TextArea, Header("詳細情報")]
    public string infometion;
    [Header("画像")]
    public Sprite prefab;
    [Header("イメージ色")]
    public Color color;
    [Header("ツリー上での配置")]
    public Vector2Int treePosition;
    [Header("前提条件(必要なアップグレード)")]
    public BaseUpGrageDB_Gabu[] premises;
    [Header("変動させるステータス")]
    public FluctuateStatsDictionary[] fluctuateStats;
    [Header("グラデーションのマテリアル")]
    public E_GRADATION_MATERIAL material = E_GRADATION_MATERIAL.NONE;
}