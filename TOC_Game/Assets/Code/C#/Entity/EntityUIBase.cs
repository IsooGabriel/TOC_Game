using DG.Tweening;
using UnityEngine;

public abstract class EntityUIBase : ColorSystem
{
    #region 変数

    public bool isDash = false;
    public bool isDie = false;
    
    public  EntityBase entity = null;
    
    public  GameObject damageEfect = null; 
    
    public  SpriteRenderer entityImage = null;
            
            
    public Color normalColor = new Color(255f, 255f, 255f);
    public float normalSaturation = 0f;
    public Vector3 normalScale = Vector3.zero;
            
    public readonly float damageSaturation = 10f;
    public readonly float damageEffectTime = 0.4f;
    public readonly Ease damageEase = Ease.InFlash;

    public readonly float attackSaturation = 50f;
    public readonly float attackEffectTime = 0.7f;
    public readonly Ease attackEase = Ease.InOutBounce;
            
    public readonly float dieSaturation = 50f;
    public readonly Ease dieColorEase = Ease.OutCubic;
    public readonly float dieEffectTime = 0.7f;
    public readonly Ease dieScaleEase = Ease.InBack;
    public readonly float dieRotate = 720f;
    public readonly Ease dieRotateEase = Ease.InQuint;
            
    public readonly float buffSaturation = 50f;
    public readonly float buffEffectTime = 0.7f;
    public readonly Ease buffEase = Ease.InOutCirc;
    public readonly float buffHueChengeAmount = 20f;
    public readonly Ease buffHueEase = Ease.InOutCirc;
    public readonly float buffValueChengeAmount = 20f;
    public readonly Ease buffValueEase = Ease.InOutCirc;
    public readonly string buffAnimationId = "BuffedAnimation"; // buffで使われるアニメーションID
            
    public readonly float criticalEffectTime = 0.7f;
    public readonly Ease criticalEase = Ease.InOutCirc;
    public readonly long criticalBlinkDamage = 10000L;

    #endregion

    #region 関数

    public abstract void Dash(Vector2 direction);
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
        if (isDie)
        {
            return;
        }
        isDie = true;
        entityImage.transform.DOShakeScale(dieEffectTime, 10f, 15, 15f, true).SetEase(dieScaleEase);
        entityImage.transform.DOScale(Vector3.zero, dieEffectTime/2).SetEase(dieScaleEase).SetDelay(dieEffectTime/2);
        entityImage.DOColor(new Color(180f, 180f, 180f), dieEffectTime / 2).SetEase(dieColorEase).SetDelay(dieEffectTime/2);
        entityImage.transform.DORotate(new Vector3(0, 0, dieRotate), dieEffectTime).SetEase(dieRotateEase);
    }

    public virtual void Critical()
    {
        Camera.main.DOShakePosition(criticalEffectTime, 3f, 10, 1, true);
    }

    public virtual void Buffed(float value)
    {

        // `value == 0` の場合、現在のアニメーションだけを停止して終了
        if (value == 0)
        {
            DOTween.Kill(buffAnimationId, complete: true); // この関数のアニメーションのみ停止
            return;
        }

        // アニメーション用シーケンスを作成
        Sequence sequence = DOTween.Sequence();

        if (value > 0)
        {
            // バフの場合: 色相の変更
            sequence.Append(entityImage.DOColor(AddHue(normalColor, buffHueChengeAmount), buffEffectTime / 2)
                .SetEase(buffHueEase)
                .SetId(buffAnimationId));
            sequence.Append(entityImage.DOColor(SubtractionHue(normalColor, buffHueChengeAmount), buffEffectTime / 2)
                .SetEase(buffEase)
                .SetId(buffAnimationId));
        }
        else
        {
            // デバフの場合: 明度の変更
            sequence.Append(entityImage.DOColor(AddValue(normalColor, buffValueChengeAmount), buffEffectTime / 2)
                .SetEase(buffValueEase)
                .SetId(buffAnimationId));
            sequence.Append(entityImage.DOColor(SubtractionValue(normalColor, buffValueChengeAmount), buffEffectTime / 2)
                .SetEase(buffValueEase)
                .SetId(buffAnimationId));
        }

        // 彩度のアニメーションを追加
        sequence.Append(entityImage.DOColor(SubtractionSaturation(normalColor, buffSaturation), buffEffectTime / 2)
            .SetEase(buffEase)
            .SetId(buffAnimationId));
        sequence.Append(entityImage.DOColor(AddSaturation(normalColor, buffSaturation), buffEffectTime / 2)
            .SetEase(buffEase)
            .SetId(buffAnimationId));

        // 繰り返しアニメーション設定
        sequence.SetLoops(-1, LoopType.Restart);
        sequence.SetId(buffAnimationId); // シーケンス全体にもIDを設定
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
        normalScale = entityImage.transform.localScale;
        isDie = false;
        VitualStart();
    }
    public abstract void VitualStart();
}
