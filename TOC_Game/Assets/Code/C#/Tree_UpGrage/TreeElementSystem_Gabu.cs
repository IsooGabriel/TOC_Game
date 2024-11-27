using UnityEngine;
using TMPro;
using static DBManager_Gabu;
using UnityEngine.UI;

public class TreeElementSystem_Gabu : MonoBehaviour
{
    [Header("アイコン")]
    public Sprite sprite;
    [Header("詳細")]
    public TextMeshProUGUI tmp;
    [Header("バフの種類と数値")]
    public FluctuateStatsDictionary fluctuateStatsDictionary;
    public Button button;
    public ImageAnimation_Gabu imageAnimation_Gabu;
    public TextAnimation_Gabu textAnimation_Gabu;

    private void Start()
    {
        if (sprite == null)
        {
            sprite = GetComponentInChildren<SpriteRenderer>()?.sprite;
            Debug.LogWarning("spriteがnullだったため、子オブジェクトから割り当てました。");
        }
        if (tmp == null)
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
            Debug.LogWarning("tmpがnullだったため、子オブジェクトから割り当てました。");
        }
        if (fluctuateStatsDictionary == null)
        {
            fluctuateStatsDictionary = GetComponentInChildren<FluctuateStatsDictionary>();
            Debug.LogWarning("fluctuateStatsDictionaryがnullだったため、子オブジェクトから割り当てました。");
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
    }
}
