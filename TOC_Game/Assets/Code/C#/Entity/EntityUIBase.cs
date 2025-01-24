using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityUIBase : ColorSystem
{
    #region 変数

    public EntityBase entity = null;
    public GameObject damageEfect = null;
    public SpriteRenderer entityImage = null;

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

    #endregion

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
    public abstract void Buffed(long value);
}
