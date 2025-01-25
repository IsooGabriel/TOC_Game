using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.Purchasing;
using UnityEngine.UI;

public class PlayerUISystem_Gabu : EntityUIBase
{
    #region 関数

    //public SpriteRenderer spriteRenderer = null;
    public TextMeshProUGUI ammoTmpro = null;

    public GameObject messageTemprete = null;
    public GameObject messageBox = null;

    public Slider rerollSlider = null;

    public GameObject aimObject = null;

    public Ease attackEase = Ease.InCubic;

    public float dashTime = 0.5f;
    public float dashScale = 1.5f;
    public Ease dashEase = Ease.InOutCubic;
    public string dashYAnimationID = "DashYAnimation";
    public string dashXAnimationID = "DashXAnimation";

    public float stopDashTime = 0.5f;
    public Ease stopDashEase = Ease.OutElastic;

    #endregion

    #region 関数

    public void UpdateAmmo(long ammo)
    {
        ammoTmpro.text = ammo.ToString();
    }

    public override void Attack()
    {
        base.Attack();
        UpdateAmmo(entity.ammo);
    }

    public override void Dash(Vector2 direction)
    {
        // ダッシュアニメーション、完全上下以外はentityImageを斜めに変形させる、上下のアニメーションは縦幅の拡大縮小を繰り返す
        if (direction.x != 0)
        {
            DOTween.Kill(dashYAnimationID);
            entityImage.transform.DOScaleX(normalScale.x * dashScale, dashTime).SetLoops(-1, LoopType.Yoyo).SetId(dashXAnimationID);
        }
        else if (direction.y != 0)
        {
            DOTween.Kill(dashXAnimationID);
            entityImage.transform.DOScaleY(normalScale.y * dashScale, dashTime).SetLoops(-1, LoopType.Yoyo).SetId(dashYAnimationID);
        }
        //else if (direction.x < 0)
        //{
        //    DOTween.Kill(dashYAnimationID);
        //    entityImage.transform.DOScaleX(normalScale.x / dashScale, dashTime).SetLoops(-1, LoopType.Yoyo).SetId(dashXAnimationID);
        //}
        //else if(direction.y < 0)
        //{
        //    DOTween.Kill(dashXAnimationID);
        //    entityImage.transform.DOScaleY(normalScale.y / dashScale, dashTime).SetLoops(-1, LoopType.Yoyo).SetId(dashYAnimationID);
        //}
        DOTween.Kill(dashYAnimationID);
        DOTween.Kill(dashXAnimationID);
        Debug.LogWarning($"ダッシュの方向がおかしいです: {direction}");
    }

    public override void stopDash()
    {
        DOTween.Kill(dashYAnimationID);
        DOTween.Kill(dashXAnimationID);
        entityImage.transform.DOScale(normalScale, stopDashTime).SetEase(stopDashEase);
    }

    public void Reroll(float percent)
    {
        rerollSlider.gameObject.SetActive(true);
        rerollSlider.value = percent;
    }

    public void Rerolled()
    {
        rerollSlider.value = 0;
        rerollSlider.gameObject.SetActive(false);
    }

    public void Aim()
    {
        aimObject.SetActive(true);
        // エイムオブジェクトをカーソルの方向に向ける
        Vector3 mousePos = Input.mousePosition;
        aimObject.transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10)));
    }

    public void Message(string message)
    {
        GameObject messageObj = Instantiate(messageTemprete, messageBox.transform);
    }

    #endregion
}
