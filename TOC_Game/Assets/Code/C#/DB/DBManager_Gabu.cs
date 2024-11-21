using System.Net;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateData/DB")]
public class DBManager_Gabu : ScriptableObject
{
    static DBManager_Gabu DBs;
    public static DBManager_Gabu DB
    {
        get
        {
            if (DBs == null)
            {
                DBs = (DBManager_Gabu)Resources.Load("DataBase");
            }
            return DBs;
        }
    }

    public EnemyDB_Gabu[] enemyDBs;
    public EnemyDB_Gabu GetEnemyDB(int ID)
    {
        if (ID < 0)
        {
            Debug.LogError("IDは0以上である必要があります. " + ID);
            return enemyDBs[0];
        }
        if (ID > enemyDBs.Length)
        {
            Debug.LogError("IDがデータベースの長さを超えています. " + ID);
            return enemyDBs[0];
        }
        return enemyDBs[ID] == null ? enemyDBs[0] : enemyDBs[ID];
    }


}
