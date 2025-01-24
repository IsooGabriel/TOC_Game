using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    #region 変数

    public int level = 1;
    public long maxHP = 100;
    public long currentHP;
    public long atk = 10;
    public float atkSpeed = 1;
    public float criticalChance = 0;
    public float criticalDmg = 1f;
    public float speed = 1;
    public float defense = 10;
    public float rerollSpeed = 1;
    public long ammo = 1;
    public float Buff = 0;
    public bool isInBase = false;

    public float atkCT = 0;
    public float rerollTime = 0;

    public GameObject shotTemplete = null;
    public EntityUIBase entityUIBase = null;

#endregion

    #region 関数

    public virtual void TakeDamage(long opponentAtk, long opponentLevel)
    {
        if (currentHP <= 0)
        {
            return;
        }

        float levelMultiplier = Mathf.Pow(1.1f, level - opponentLevel); // 1レベル差ごとに10%増減
        long damage = (long)Mathf.Max(1, (opponentAtk - defense) * levelMultiplier);
        damage = (long)(damage * (Buff / 100));
        currentHP -= damage;

        // エフェクトやアニメーションの処理
        entityUIBase.TakeDamage();

        if (currentHP <= 0)
        {
            Die();
        }
    }
    public virtual void Attack(Quaternion rotation)
    {
        if (!CanAttack())
        {
            return;
        }

        // 攻撃処理
        GameObject shotObj = Instantiate(shotTemplete, transform.position, rotation);
        shotObj.transform.parent = transform.parent;
        shotObj.GetComponent<Shot_Gabu>().attacker = this;
        entityUIBase.Attack();
    }

    public virtual bool CanAttack()
    {
        if (atkCT > 0)
        {
            return false;
        }
        if (isInBase)
        {
            return true;
        }
        return ammo > 0;
    }

    public abstract void Reroll();

    public virtual void ChengeBuff(float value)
    {
        Buff += value;
    }

    public abstract void Die();

#endregion

    public virtual void Update()
    {
        if (atkCT > 0)
        {
            atkCT -= Time.deltaTime;
        }
    }
}
