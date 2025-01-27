using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DBManager_Gabu;

public class PlayerUISystem_Gabu : EntityUIBase
{
    #region 変数

    //public SpriteRenderer spriteRenderer = null;
    public TextMeshProUGUI ammoTmpro = null;

    public GameObject messageTemprete = null;
    public GameObject messageBox = null;

    public Slider rerollSlider = null;

    public GameObject aimObject = null;

    public readonly float dashTime = 0.5f;
    public float dashTimeMultiplier = 0.01f;
    public readonly float dashScale = 1.2f;
    public readonly Ease dashEase = Ease.InOutCubic;
    public readonly string dashYAnimationID = "DashYAnimation";
    public readonly string dashXAnimationID = "DashXAnimation";
    public (bool x,bool y) isDashStats = (false,false);

    public readonly float stopDashTime = 0.5f;
    public readonly Ease stopDashEase = Ease.OutElastic;

    public readonly float openShopTime = 0.45f;
    public float openShopHoldTime = 0f;
    public GameObject shopObject = null;

    #endregion

    #region 関数

    public void UpdateAmmo(long ammo)
    {
        ammoTmpro.text = ammo.ToString();
    }

    public override void Dash(Vector2 direction)
    {
        return;
        // アニメーション用シーケンスを作成
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        float tweenTime = dashTime * dashTimeMultiplier / 2;

        // ダッシュアニメーション、完全上下以外はentityImageを斜めに変形させる、上下のアニメーションは縦幅の拡大縮小を繰り返す
        if (direction.x != 0)
        {
            if (isDashStats == (x:true, y:false))
            {
                return;
            }

            isDashStats = (x:true, y:false);

            DOTween.Kill(dashYAnimationID);
            sequence.Append(entityImage.transform.DOScaleX(normalScale.x * dashScale, tweenTime)
                .SetEase(dashEase));
            sequence.Append(entityImage.transform.DOScaleX(normalScale.x / dashScale, tweenTime)
                .SetEase(dashEase));

            sequence.SetLoops(-1, LoopType.Yoyo);// 繰り返しアニメーション設定
            sequence.SetId(dashXAnimationID); // シーケンス全体にもIDを設定
            return;
        }
        else if (direction.y != 0)
        {
            if (isDashStats == (x:false, y:true))
            {
                return;
            }
            isDashStats = (x: false, y: true);

            DOTween.Kill(dashXAnimationID);
            sequence.Append(entityImage.transform.DOScaleY(normalScale.y * dashScale, tweenTime)
                .SetEase(dashEase)
                .SetId(dashYAnimationID));
            sequence.Append(entityImage.transform.DOScaleY(normalScale.y / dashScale, tweenTime)
                .SetEase(dashEase)
                .SetId(dashYAnimationID));

            // 繰り返しアニメーション設定
            sequence.SetLoops(-1, LoopType.Restart);
            sequence.SetId(dashYAnimationID); // シーケンス全体にもIDを設定

            return;
        }

        isDashStats = (false,false);

        entityImage.transform.DOScale(normalScale, stopDashTime).SetEase(stopDashEase);
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

    public void Shop()
    {
        openShopHoldTime += Time.deltaTime;

        if (openShopHoldTime < openShopTime)
        {
            return;
        }
        shopObject.SetActive(true);
    }

    #endregion

    public override void VitualStart()
    {
        dashTimeMultiplier = DB.playerDBs[DB.AccountID].speed;
        UpdateAmmo(entity.ammo);
    }

}
