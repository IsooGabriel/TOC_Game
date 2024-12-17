using UnityEngine;
using static DBManager_Gabu;

public class DataSaverLoader_Gabu : MonoBehaviour
{
    public static string FileName = "AccountDataBase";
    #region データセーバー、ローダ

    static public void DataSave()
    {
        PlayerDB_Gabu PData = DB.playerDBs[DB.AccountID];
        if (PData == null)
        {
            Debug.LogError("セーブするデータがありません");
            return;
        }
        string JsonData = JsonUtility.ToJson(PData);
        PlayerPrefs.SetString($"{FileName}+{DB.AccountID}", JsonData);
    }

    static public void DataLoad(int ID)
    {
        DB.AccountID = ID;
        string JsonData = PlayerPrefs.GetString($"{FileName}+{ID}", "");
        var PDataF = JsonUtility.FromJson<PlayerDB_Gabu>(JsonData);
        var PDB = new PlayerDB_Gabu();
        if (PDataF != null)
        {
            PDB.money = PDataF.money;
            PDB.stars = PDataF.stars;
            PDB.level = PDataF.level;
            PDB.playerID = PDataF.playerID;

            if (PDB.baseUpGrages.Count > 0)
            {
                PDB.baseUpGrages = PDataF.baseUpGrages;
            }
            if (PDB.inGameUpGrages.Count > 0)
            {
                PDB.inGameUpGrages = PDataF.inGameUpGrages;
            }
        }
        else
        {
            PDB = new PlayerDB_Gabu();
        }
        while (PDB.inGameUpGrages.Count < DB.inGameUpGrageDBs.Length) PDB.inGameUpGrages.Add(false);
        while (PDB.baseUpGrages.Count < DB.baseUpGrageDBs.Length) PDB.baseUpGrages.Add(false);
        DB.playerDBs[ID] = PDB;
    }

    #endregion
}
