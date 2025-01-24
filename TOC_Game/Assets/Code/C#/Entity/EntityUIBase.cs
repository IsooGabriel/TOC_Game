using UnityEngine;
using UnityEngine.UI;

public abstract class EntityUIBase : ColorSystem
{
    public EntityBase entity = null;
    public GameObject damageEfect = null;
    public Image entityImage = null;

    public abstract void Dash(Quaternion direction);
    public abstract void stopDash();
    public abstract void TakeDamage();
    public abstract void Attack();
    public abstract void Buffed(int value);
    public abstract void Die();
}
