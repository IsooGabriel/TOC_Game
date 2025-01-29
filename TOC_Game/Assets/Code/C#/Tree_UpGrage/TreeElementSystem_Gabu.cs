using UnityEngine;
using UnityEngine.UI;

public class TreeElementSystem_Gabu : MonoBehaviour
{
    #region 変数
    [Header("アイコン")]
    public Image image;
    public Button button;
    public Image statsIcone;
    public ImageAnimation_Gabu imageAnimation;
    public TextAnimation_Gabu textAnimation;
    public BaseUpGradeDB_StatChangeSkill_Gabu baseUpGrageDB;
    public LineRenderer LineRenderer;
    public STATS stats = 0;
    public readonly float elementSize = 0.75f;
    public readonly float arrowSizeX = 10f;
    public readonly float arrowSizeY = 10f;
    public readonly float lineStartWidth = 0.05f;
    public readonly float lineEndWidth = 0.07f;

    // baseUpGrageDB_Gabuから情報を取得して、表示する

    public enum STATS
    {
        None = 0,
        未開放 = 1,
        開放済み = 2,
        開放不可 = 3,
        新規 = 4,
    }

    #endregion

    #region 関数

    public int SetOpened()
    {
        stats = STATS.開放済み;
        button.interactable = true;
        statsIcone.sprite = DBManager_Gabu.DB.UIStatusIcons[(int)stats];

        if (statsIcone.sprite == null)
        {
            statsIcone.gameObject.SetActive(false);
        }

        return (int)stats;
    }

    public int SetUnreleased()
    {
        stats = STATS.未開放;
        button.interactable = true;
        statsIcone.sprite = DBManager_Gabu.DB.UIStatusIcons[(int)stats];

        if (statsIcone.sprite == null)
        {
            statsIcone.gameObject.SetActive(false);
        }

        return (int)stats;
    }

    public int SetUnopenable()
    {
        stats = STATS.開放不可;
        button.interactable = false;
        statsIcone.sprite = DBManager_Gabu.DB.UIStatusIcons[(int)stats];

        if (statsIcone.sprite == null)
        {
            statsIcone.gameObject.SetActive(false);
        }

        return (int)stats;
    }

    public int SetNew()
    {
        stats = STATS.新規;
        button.interactable = true;
        statsIcone.sprite = DBManager_Gabu.DB.UIStatusIcons[(int)stats];

        if (statsIcone.sprite == null)
        {
            statsIcone.gameObject.SetActive(false);
        }

        return (int)stats;
    }

    public void CheckStats()
    {

        if (DBManager_Gabu.DB.playerDBs[DBManager_Gabu.DB.AccountID].baseUpGrages[baseUpGrageDB.UpGradeID])
        {
            SetOpened();
        }
        else
        {
            if (baseUpGrageDB.premises.Length > 0)
            {
                bool isUnopenable = false;
                foreach (var premise in baseUpGrageDB.premises)
                {
                    if (!DBManager_Gabu.DB.playerDBs[DBManager_Gabu.DB.AccountID].baseUpGrages[premise.UpGradeID])
                    {
                        isUnopenable = true;
                        break;
                    }
                }
                if (isUnopenable)
                {
                    SetUnopenable();
                }
                else
                {
                    SetUnreleased();
                }
            }
            else
            {
                SetUnreleased();
            }
        }
    }


    void DrawConnection(Vector3 startPos, Vector3 endPos)
    {
        LineRenderer.positionCount = 2;
        LineRenderer.startWidth = lineStartWidth;
        LineRenderer.endWidth = lineEndWidth;
        LineRenderer.SetPosition(0, startPos);
        LineRenderer.SetPosition(1, endPos);
        LineRenderer.startColor = new Color32(25, 25, 25, 255); // 始点の色
        LineRenderer.endColor = new Color32(25, 25, 25, 240); // 終点の色
        LineRenderer.material = DBManager_Gabu.DB.gradationMaterials[(int)DBManager_Gabu.E_GRADATION_MATERIAL.矢印];

        // 矢印を配置
        PlaceArrow(startPos, endPos);
    }

    void PlaceArrow(Vector3 startPos, Vector3 endPos)
    {
        GameObject arrowInstance = Instantiate(image.gameObject, transform);
        Image arrowImage = arrowInstance.GetComponent<Image>();
        RectTransform arrowRectTransform = arrowInstance.GetComponent<RectTransform>();
        arrowImage.sprite = DBManager_Gabu.DB.arrow;
        arrowImage.material = null;
        arrowRectTransform.pivot = new Vector2(0.5f, 0.5f);

        // ピボットを中央下（Center-Mid）に設定
        arrowRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        arrowRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        arrowRectTransform.sizeDelta = new Vector2(arrowSizeX, arrowSizeY);

        // 矢印の位置を線の中心に設定
        Vector3 midPoint = (startPos + endPos) / 2;
        arrowInstance.transform.position = midPoint;

        // 矢印の回転を設定
        Vector3 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion

    private void Start()
    {
        if (image == null)
        {
            image = GetComponentInChildren<Image>();
            Debug.LogWarning("spriteがnullだったため、子オブジェクトから割り当てました。");
        }
        if (button == null)
        {
            button = GetComponentInChildren<Button>();
            Debug.LogWarning("buttonがnullだったため、子オブジェクトから割り当てました。");
        }
        if (imageAnimation == null)
        {
            imageAnimation = GetComponentInChildren<ImageAnimation_Gabu>();
            Debug.LogWarning("imageAnimation_Gabuがnullだったため、子オブジェクトから割り当てました。");
        }
        if (textAnimation == null)
        {
            textAnimation = GetComponentInChildren<TextAnimation_Gabu>();
            Debug.LogWarning("textAnimation_Gabuがnullだったため、子オブジェクトから割り当てました。");
        }
        if (LineRenderer == null)
        {
            LineRenderer = gameObject.AddComponent<LineRenderer>();
            Debug.LogWarning("LineAlignmentがnullだったため、インスタンスしました。");
        }

        gameObject.name = baseUpGrageDB.name;
        image.sprite = baseUpGrageDB.prefab;
        image.color = baseUpGrageDB.color;

        transform.localScale = new Vector3(transform.localScale.x * elementSize, transform.localScale.y * elementSize, 1);


        // 開放条件グレードアップへの座標にDBManager_Gabuのarrowを表示する。矢印のスプライトはSlicedでインスタンス
        if (baseUpGrageDB.premises.Length > 0)
        {
            Canvas canvas = GetComponentInParent<Canvas>(); // 親Canvasを取得
            if (canvas == null)
            {
                Debug.LogError("Canvasが見つかりません");
                return;
            }

            for (int i = 0; i < baseUpGrageDB.premises.Length; i++)
            {
                // baseUpGrageDB.premises[i]とelementSizeの差に基づいて、imageGameObjectのscale.yを設定します
                Vector2 currentPosition = gameObject.transform.position;
                Vector2 targetPosition = baseUpGrageDB.premises[i].treePosition;
                float distance = Vector2.Distance(currentPosition, targetPosition) - elementSize;

                DrawConnection(currentPosition, targetPosition);

            }
        }
        CheckStats();
    }
}
