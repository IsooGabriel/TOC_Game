using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UIElements;

public class MessageManager_Gabu : MonoBehaviour
{
    public TextMeshProUGUI tmpro;

    public float startpositon = 0;
    public float duration = 2;
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;
    public Ease fadeInEase = Ease.InOutSine;
    public Ease fadeOutEase = Ease.InOutSine;

    private float time = 0;

    public void SetMessage(string message)
    {
        tmpro.text = message;
    }

    private void Start()
    {
        startpositon = gameObject.transform.position.y;
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, startpositon + 50);
        tmpro.DOFade(1, fadeInDuration).SetEase(fadeInEase);
        gameObject.transform.DOLocalMoveY(startpositon, fadeInDuration).SetEase(fadeInEase);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > duration)
        {
            tmpro.DOFade(0, fadeOutDuration).SetEase(fadeOutEase);
            gameObject.transform.DOLocalMoveY(gameObject.transform.position.y + 100, fadeOutDuration).SetEase(fadeOutEase);
        }
        if (time > duration + fadeOutDuration)
        {
            Destroy(gameObject);
        }
    }
}
