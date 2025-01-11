using UnityEngine;
using UnityEngine.UI;

public class TreeElementSystem_Gabu : MonoBehaviour
{
    [Header("アイコン")]
    public Image image;
    public Button button;
    public ImageAnimation_Gabu imageAnimation;
    public TextAnimation_Gabu textAnimation;
    public BaseUpGrageDB_Gabu baseUpGrageDB;
    public readonly float elementSize = 100;

    // baseUpGrageDB_Gabuから情報を取得して、表示する

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

        gameObject.name = baseUpGrageDB.name;
        image.sprite = baseUpGrageDB.prefab;
        image.color = baseUpGrageDB.color;

        // 開放条件グレードアップへの座標にDBManager_Gabuのarrowを表示する。矢印のスプライトはSlicedでインスタンス
        if (baseUpGrageDB.premises.Length > 0)
        {
            for (int i = 0; i < baseUpGrageDB.premises.Length; i++)
            {
                // 矢印のGameObjectを生成し、親をrectTransformに設定します
                GameObject imageGameObject = Instantiate(image.gameObject, transform);
                imageGameObject.name = "Arrow";
                RectTransform arrowRectTransform = imageGameObject.GetComponent<RectTransform>();
                arrowRectTransform.SetParent(transform);
                arrowRectTransform.pivot = new Vector2(0.5f, 0);
                Image arrowImage = imageGameObject.GetComponent<Image>();
                arrowImage.sprite = DBManager_Gabu.DB.arrow;
                arrowImage.type = Image.Type.Sliced;

                // baseUpGrageDB.premises[i]とelementSizeの差に基づいて、imageGameObjectのscale.yを設定します
                float distance = Vector2.Distance(gameObject.transform.position, baseUpGrageDB.premises[i].treePosition) - elementSize;
                Vector3 scale = arrowRectTransform.localScale;
                scale.y = distance;
                arrowRectTransform.localScale = scale;

                // baseUpGrageDB.premises[i]の方向に合わせて、imageGameObjectの回転を設定します
                Vector2 direction = ((Vector2)baseUpGrageDB.premises[i].treePosition).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);
                arrowRectTransform.rotation = rotation;
            }
        }
    }
}
