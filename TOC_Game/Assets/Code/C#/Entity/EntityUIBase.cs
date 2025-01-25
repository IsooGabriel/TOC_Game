using DG.Tweening;
using UnityEngine;

public abstract class EntityUIBase : ColorSystem
{
    #region 変数

    public EntityBase entity = null;
    public GameObject damageEfect = null;
    public SpriteRenderer entityImage = null;

    public Color normalColor = new Color(255f, 255f, 255f);
    public float normalSaturation = 0f;
    public float damageSaturation = 50f;
    public float damageEffectTime = 0.4f;
    public Ease damageEase = Ease.InFlash;

    public float attackSaturation = 50f;
    public float attackEffectTime = 0.7f;
    public Ease attackEase = Ease.InOutBounce;

    public float dieSaturation = 50f;
    public Ease dieColorEase = Ease.OutCubic;
    public float dieEffectTime = 0.7f;
    public Ease dieScaleEase = Ease.InBack;
    public float dieRotate = 720f;
    public Ease dieRotateEase = Ease.InQuint;

    public float buffSaturation = 50f;
    public float buffEffectTime = 0.7f;
    public Ease buffEase = Ease.InOutBounce;
    public float buffHueChengeAmount = 20f;
    public Ease buffHueEase = Ease.InOutBounce;
    public float buffValueChengeAmount = 20f;
    public Ease buffValueEase = Ease.InOutBounce;

    #endregion

    #region 関数

    public abstract void Dash(Quaternion direction);
    public abstract void stopDash();
    public virtual void TakeDamage()
    {
        entityImage.DOColor(ChengeSaturation(entityImage.color, damageSaturation), damageEffectTime).SetEase(damageEase);
        entityImage.DOColor(ChengeSaturation(entityImage.color, normalSaturation), damageEffectTime / 2).SetDelay(damageEffectTime);
    }
    public virtual void Attack()
    {
        entityImage.DOColor(ChengeSaturation(entityImage.color, attackSaturation), attackEffectTime).SetEase(attackEase);
        entityImage.DOColor(ChengeSaturation(entityImage.color, normalSaturation), attackEffectTime / 2).SetDelay(attackEffectTime);
    }
    public virtual void Die()
    {
        entityImage.transform.DOScale(Vector3.zero, dieEffectTime).SetEase(dieScaleEase);
        entityImage.DOColor(new Color(180f, 180f, 180f), dieEffectTime / 2).SetEase(dieColorEase);
        entityImage.transform.DORotate(new Vector3(0, 0, dieRotate), dieEffectTime).SetEase(dieRotateEase);
    }
    public virtual void Buffed(float value)
    {
        // この関数で生成されるアニメーション専用のID
        string animationId = "BuffedAnimation";

        // `value == 0` の場合、現在のアニメーションだけを停止して終了
        if (value == 0)
        {
            DOTween.Kill(animationId, complete: true); // この関数のアニメーションのみ停止
            return;
        }

        // アニメーション用シーケンスを作成
        Sequence sequence = DOTween.Sequence();

        if (value > 0)
        {
            // バフの場合: 色相の変更
            sequence.Append(entityImage.DOColor(AddHue(normalColor, buffHueChengeAmount), buffEffectTime / 2)
                .SetEase(buffHueEase)
                .SetId(animationId));
            sequence.Append(entityImage.DOColor(SubtractionHue(normalColor, buffHueChengeAmount), buffEffectTime / 2)
                .SetEase(buffEase)
                .SetId(animationId));
        }
        else
        {
            // デバフの場合: 明度の変更
            sequence.Append(entityImage.DOColor(AddValue(normalColor, buffValueChengeAmount), buffEffectTime / 2)
                .SetEase(buffValueEase)
                .SetId(animationId));
            sequence.Append(entityImage.DOColor(SubtractionValue(normalColor, buffValueChengeAmount), buffEffectTime / 2)
                .SetEase(buffValueEase)
                .SetId(animationId));
        }

        // 彩度のアニメーションを追加
        sequence.Append(entityImage.DOColor(SubtractionSaturation(normalColor, buffSaturation), buffEffectTime / 2)
            .SetEase(buffEase)
            .SetId(animationId));
        sequence.Append(entityImage.DOColor(AddSaturation(normalColor, buffSaturation), buffEffectTime / 2)
            .SetEase(buffEase)
            .SetId(animationId));

        // 繰り返しアニメーション設定
        sequence.SetLoops(-1, LoopType.Restart);
        sequence.SetId(animationId); // シーケンス全体にもIDを設定
    }
    public virtual float BuffChengeHue(float value)
    {
        // バフの量に応じて変動色彩の量を変更
        if (value < 0.5)
        {
            return 4 * value * value * value;
        }
        else
        {
            float temp = -2 * value + 2;
            return 1 - Mathf.Pow(temp, 3) / 2;
        }
    }

    #endregion

    public virtual void Start()
    {
        normalSaturation = GetSaturation(entityImage.color);
        normalColor = entityImage.color;
    }
}
