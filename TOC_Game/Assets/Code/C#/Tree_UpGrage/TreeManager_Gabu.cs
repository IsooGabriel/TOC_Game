using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DBManager_Gabu;
using static DataSaverLoader_Gabu;
using Unity.VisualScripting;

public class TreeManager_Gabu : MonoBehaviour
{

    #region 変数

    public List<TreeElementSystem_Gabu> elements;
    public GameObject elementPrefab;
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI detaliTMP;
    public TextMeshProUGUI moneyCostTMP;
    public GameObject moneyTextObject;
    public TextMeshProUGUI starCostTMP;
    public GameObject starTextObject;
    public TextMeshProUGUI unlockTMP;
    public Button unlockButton;

    public PlayerAssetsUISystem_Gabu playerAssetsUISystem;

    #endregion

    #region 関数

    // クリックされたアップグレードの情報を表示する
    public void SerectElement(int upgradeID)
    {
        titleTMP.text = DB.baseUpGrageDBs[upgradeID].name;
        detaliTMP.text = DB.baseUpGrageDBs[upgradeID].infometion;

        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(() => UnlockElement(upgradeID));

        // お金とスターのコストを表示する
        if (DB.baseUpGrageDBs[upgradeID].moneyCost == 0)
        {
            moneyTextObject.SetActive(false);
        }
        else
        {
            moneyTextObject.SetActive(true);
        }
        moneyCostTMP.text = DB.baseUpGrageDBs[upgradeID].moneyCost.ToString("N0");
        if (DB.baseUpGrageDBs[upgradeID].starCost == 0)
        {
            starTextObject.SetActive(false);
        }
        else
        {
            starTextObject.SetActive(true);
        }
        starCostTMP.text = DB.baseUpGrageDBs[upgradeID].starCost.ToString("N0");

        if (DB.playerDBs[DB.AccountID].baseUpGrages[upgradeID])
        {
            unlockTMP.text = "解放済み";
            unlockButton.interactable = false;
        }
        else
        {
            // お金とスターが足りているか確認する
            (bool, bool) tuple = (
               (DB.baseUpGrageDBs[upgradeID].moneyCost <= (int)DB.playerDBs[DB.AccountID].money)
               ? true : false,
               (DB.baseUpGrageDBs[upgradeID].starCost <= (int)DB.playerDBs[DB.AccountID].stars)
               ? true : false);

            switch (tuple)
            {
                case (true, true):
                    unlockTMP.text = "解放可能";
                    unlockButton.interactable = true;
                    break;
                case (false, true):
                    unlockTMP.text = "お金が足りません";
                    unlockButton.interactable = false;
                    break;
                case (true, false):
                    unlockTMP.text = "スターが足りません";
                    unlockButton.interactable = false;
                    break;
                case (false, false):
                    unlockTMP.text = "お金とスターが足りません";
                    unlockButton.interactable = false;
                    break;
            }
        }

        foreach (var element in elements)
        {
            if (element.baseUpGrageDB.UpGrageID == upgradeID)
            {
                element.CheckStats();
            }
        }
    }

    public void UnlockElement(int upgradeID)
    {
        if (DB.playerDBs[DB.AccountID].money < (ulong)DB.baseUpGrageDBs[upgradeID].moneyCost || DB.playerDBs[DB.AccountID].stars < (ulong)DB.baseUpGrageDBs[upgradeID].starCost)
        {
            return;
        }

        foreach (var element in elements)
        {
            if (element.baseUpGrageDB.UpGrageID == upgradeID)
            {
                element.button.interactable = true;
                element.SetOpened();
                break;
            }
        }

        DB.playerDBs[DB.AccountID].money -= (ulong)DB.baseUpGrageDBs[upgradeID].moneyCost;
        DB.playerDBs[DB.AccountID].stars -= (ulong)DB.baseUpGrageDBs[upgradeID].starCost;
        DB.playerDBs[DB.AccountID].baseUpGrages[upgradeID] = true;

        SerectElement(upgradeID);

        // DBの保存
        DataSaverLoader_Gabu.DataSave();

        playerAssetsUISystem.UpdateMoney();
        playerAssetsUISystem.UpdateStar();
    }

    #endregion

    // prefabに変更を加え複製する、
    // DBからデータを取得して、TreeElementSystem_Gabuの配列に格納する、解放の有無を確認する
    private void Awake()
    {
        if (elementPrefab.GetComponent<TreeElementSystem_Gabu>() == null)
        {
            Debug.LogWarning("TreeElementSystem_Gabuがアタッチされていません");
            elementPrefab.AddComponent<TreeElementSystem_Gabu>();
        }

        foreach (var element in DB.baseUpGrageDBs)  // アップグレードをインスタンス
        {
            GameObject obj = Instantiate(elementPrefab, transform);
            TreeElementSystem_Gabu tes = obj.GetComponent<TreeElementSystem_Gabu>();
            elements.Add(tes);
            obj.name = element.name;
            obj.transform.localPosition = new Vector3(element.treePosition.x, element.treePosition.y, 0);
            tes.image.sprite = element.prefab;
            tes.image.color = element.color;
            tes.image.material = DB.gradationMaterials[(int)element.material];
            tes.baseUpGrageDB = element;
            tes.button.onClick.AddListener(() => SerectElement(element.UpGrageID));// クリックされたアップグレードの情報を表示する
            tes.CheckStats();

            if (!DB.playerDBs[DB.AccountID].baseUpGrages[element.UpGrageID])
            {
                tes.SetUnreleased();
            }
            else
            {
                tes.button.interactable = true;
            }
        }

        SerectElement(0);

        if (playerAssetsUISystem == null)
        {
            playerAssetsUISystem = gameObject.GetComponent<PlayerAssetsUISystem_Gabu>();
            Debug.LogWarning("PlayerAssetsUISystem_Gabuがnullだったため、アセットから探しました。");
        }
    }

}
