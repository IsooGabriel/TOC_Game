using UnityEngine;

[CreateAssetMenu(menuName = "CreateData/DB")]
public class DBManager_Gabu : ScriptableObject
{
    private static DBManager_Gabu DBs;
    public int AccountID = 0;
    // ゲッタ
    public static DBManager_Gabu DB
    {
        get
        {
            if (DBs == null)
            {
                DBs = (DBManager_Gabu)Resources.Load("DB");
            }
            return DBs;
        }
    }

    public EnemyDB_Gabu[] enemyDBs;
    // ゲッタ
    public EnemyDB_Gabu GetEnemyDB(int ID)
    {
        if (ID < 0)
        {
            return enemyDBs[enemyDBs.Length - (int)Mathf.Repeat(ID, enemyDBs.Length + 1)];
        }
        if (ID > enemyDBs.Length)
        {
            Debug.LogError("IDがデータベースの長さを超えています. " + ID);
            return enemyDBs[0];
        }
        return enemyDBs[ID] == null ? enemyDBs[0] : enemyDBs[ID];
    }

    public InGameUpGrageDB_Gabu[] inGameUpGrageDBs;

    // ゲッタ
    public InGameUpGrageDB_Gabu GetInGameUpGrageDB(int ID)
    {
        if (ID < 0)
        {
            return inGameUpGrageDBs[inGameUpGrageDBs.Length - (int)Mathf.Repeat(ID, inGameUpGrageDBs.Length + 1)];

        }
        if (ID > inGameUpGrageDBs.Length)
        {
            Debug.LogError("IDがデータベースの長さを超えています. " + ID);
            return inGameUpGrageDBs[0];
        }
        return inGameUpGrageDBs[ID] == null ? inGameUpGrageDBs[0] : inGameUpGrageDBs[ID];
    }

    public BaseUpGradeDB_StatChangeSkill_Gabu[] baseUpGrageDBs;
    // ゲッタ
    public BaseUpGradeDB_StatChangeSkill_Gabu GetBaseUpGrageDB(int ID)
    {
        if (ID < 0)
        {
            return baseUpGrageDBs[baseUpGrageDBs.Length - (int)Mathf.Repeat(ID, baseUpGrageDBs.Length + 1)];
        }
        if (ID > baseUpGrageDBs.Length)
        {
            Debug.LogError("IDがデータベースの長さを超えています. " + ID);
            return baseUpGrageDBs[0];
        }
        return baseUpGrageDBs[ID] == null ? baseUpGrageDBs[0] : baseUpGrageDBs[ID];
    }

    public Sprite arrow;

    public PlayerDB_Gabu[] playerDBs;
    // ゲッタ
    public PlayerDB_Gabu GetPlayerDB(int ID)
    {
        if (ID < 0)
        {
            return playerDBs[playerDBs.Length - (int)Mathf.Repeat(ID, playerDBs.Length + 1)];
        }
        if (ID > playerDBs.Length)
        {
            Debug.LogError("IDがデータベースの長さを超えています. " + ID);
            return playerDBs[0];
        }
        return playerDBs[ID] == null ? playerDBs[0] : playerDBs[ID];
    }



    /// <summary>
    /// バフの種類と数値の辞書
    /// </summary>
    [System.Serializable]
    public class FluctuateStatsDictionary
    {
        public E_FLUCTUATE_STATS fluctuateStats;
        public float value;
        [Header("変動させる優先度。数値が低い程先に計算される")]
        public int priority;
    }

    public Sprite[] UIStatusIcons = new Sprite[6];

    public Material[] gradationMaterials = new Material[999];


    #region 列挙型

    /// <summary>
    /// 変更させるステータスとその計算方法
    /// </summary>
    public enum E_FLUCTUATE_STATS
    {
        HP_ADD = 0,
        HP_MULTIPLY,
        HP_SUBTRACT,
        HP_DIVIDE,

        ATK_ADD,
        ATK_MULTIPLY,
        ATK_SUBTRACT,
        ATK_DIVIDE,

        ATK_SPEED_ADD,
        ATK_SPEED_MULTIPLY,
        ATK_SPEED_SUBTRACT,
        ATK_SPEED_DIVIDE,

        SPEED_ADD,
        SPEED_MULTIPLY,
        SPEED_SUBTRACT,
        SPEED_DIVIDE,

        DEFENSE_ADD,
        DEFENSE_MULTIPLY,
        DEFENSE_SUBTRACT,
        DEFENSE_DIVIDE,

        REROLL_SPEED_ADD,
        REROLL_SPEED_MULTIPLY,
        REROLL_SPEED_SUBTRACT,
        REROLL_SPEED_DIVIDE,

        LEVEL_ADD,
        LEVEL_MULTIPLY,
        LEVEL_SUBTRACT,
        LEVEL_DIVIDE,

        BUFF_ADD,
        BUFF_MULTIPLY,
        BUFF_SUBTRACT,
        BUFF_DIVIDE,

        AMMO_ADD,
        AMMO_MULTIPLY,
        AMMO_SUBTRACT,
        AMMO_DIVIDE,

        CRITICAL_CHANCE_ADD,
        CRITICAL_CHANCE_MULTIPLY,
        CRITICAL_DAMAGE_ADD,
        CRITICAL_DAMAGE_MULTIPLY,

    }

    public enum E_ENEMY_TYPE
    {
    }

    public enum E_IN_GAME_UPGRADE
    {
    }

    public enum E_BASE_UPGRADE
    {
    }

    public enum E_GRADATION_MATERIAL : int
    {
        NONE = -1,
        WHITE = 0,
        RED,
        RtoO,
        GREEN,
        BLUE,
        CYAN,
        MAGENTA,
        YELLOW,
        PURPLE,
        ORANGE,
        OtoO,
        BLACK,
        BtoC,
        BGI_BtoC,
        矢印,
        Star,
        Gold
    }

    public enum UIStatusType
    {
        None = 0,
        確認済,
        未確認,
        ロック,
        新規,
        注意,
        重要,
    }

    #endregion
}