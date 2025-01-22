using UnityEngine;
using static DBManager_Gabu;

public abstract class UpGrade_Gabu : ScriptableObject
{
    [Header("アップグレードの名前")]
    public string UpGradeName = "Diamond";
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
    public BaseUpGradeDB_StatChangeSkill_Gabu[] premises;
    [Header("変動させるステータス")]
    public FluctuateStatsDictionary[] fluctuateStats;
    [Header("優先度")]
    public int priority = 0;
    //[Header("固有のシステム)")]
    //public CustomUpGrade_Gabu customUpGrade = null;
    [Header("固有オブジェクト")]
    public GameObject customObject = null;
    [Header("グラデーションのマテリアル")]
    public E_GRADATION_MATERIAL material = E_GRADATION_MATERIAL.NONE;


    public abstract void Execute(Player_Gabu player);
}
