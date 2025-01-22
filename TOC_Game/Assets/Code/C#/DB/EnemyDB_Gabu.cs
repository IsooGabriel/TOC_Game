using UnityEngine;

[CreateAssetMenu(menuName = "CreateData/Enemy")]
public class EnemyDB_Gabu : ScriptableObject
{
    [Header("敵の名前")]
    public string enemyName = "Diamond";
    public int enemyID = 0;
    //public int enemyLocalID = 0;
    [TextArea, Header("詳細情報")]
    public string infometion;
    public GameObject prefab;
    public uint dropMoney = 1;
    public uint dropStar = 0;
    public int level = 1;
    public int hp = 5;
    public float speed = 1;
    public float speedDown = 0.8f;
    public float defense = 10;
    public float baseDamage = 1;
}