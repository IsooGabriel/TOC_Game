using TMPro;
using UnityEngine;
using static DBManager_Gabu;

public class PlayerAssetsUISystem_Gabu : MonoBehaviour
{
    [SerializeField, Header("お金テキスト")]
    public TextMeshProUGUI moneyTMP;
    public GameObject moneySpace;
    [SerializeField, Header("スターのテキスト")]
    public TextMeshProUGUI starTMP;
    public GameObject starSpace;

    void Start()
    {

        if (DB.playerDBs[DB.AccountID].stars <= 0)
        {
            starSpace.SetActive(false);
        }
        else
        {
            starSpace.SetActive(true);
        }

        moneyTMP.text = DB.playerDBs[DB.AccountID].money.ToString("N0");
        starTMP.text = DB.playerDBs[DB.AccountID].stars.ToString("N0");
    }

    public void UpdateMoney()
    {
        moneyTMP.text = DB.playerDBs[DB.AccountID].money.ToString("N0");
    }

    public void UpdateStar()
    {
        if (DB.playerDBs[DB.AccountID].stars <= 0)
        {
            starSpace.SetActive(false);
        }
        else
        {
            starSpace.SetActive(true);
        }
        starTMP.text = DB.playerDBs[DB.AccountID].stars.ToString("N0");
    }
}
