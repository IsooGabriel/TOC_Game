using UnityEngine;
using UnityEngine.UI;

public class TreeElementSystem_Gabu : MonoBehaviour
{
    [Header("アイコン")]
    public Image image;
    public Button button;
    public ImageAnimation_Gabu imageAnimation_Gabu;
    public TextAnimation_Gabu textAnimation_Gabu;
    public BaseUpGrageDB_Gabu baseUpGrageDB_Gabu;
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
        if (imageAnimation_Gabu == null)
        {
            imageAnimation_Gabu = GetComponentInChildren<ImageAnimation_Gabu>();
            Debug.LogWarning("imageAnimation_Gabuがnullだったため、子オブジェクトから割り当てました。");
        }
        if (textAnimation_Gabu == null)
        {
            textAnimation_Gabu = GetComponentInChildren<TextAnimation_Gabu>();
            Debug.LogWarning("textAnimation_Gabuがnullだったため、子オブジェクトから割り当てました。");
        }

        gameObject.name = baseUpGrageDB_Gabu.name;
        image.sprite = baseUpGrageDB_Gabu.prefab;
        image.color = baseUpGrageDB_Gabu.color;
    }
}
