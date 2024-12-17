using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateData/Player")]
public class PlayerDB_Gabu : ScriptableObject
{
    [Header("プレイヤの名前")]
    public string playerName = "Diamond";
    public int playerID = 0;
    //public int enemyLocalID = 0;
    [TextArea, Header("詳細情報")]
    public string infometion;
    public GameObject prefab;
    public int level = 1;
    public int hp = 5;
    public int atk = 1;
    public float atkSpeed = 1;
    public float speed = 1;
    public float defense = 10;
    public float rerollSpeed = 1;
    public ulong money = 0;
    public ulong stars = 0;
    public List<bool> baseUpGrages = new List<bool>();
    public List<bool> inGameUpGrages = new List<bool>();
}