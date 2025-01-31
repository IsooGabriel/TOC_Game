using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    #region 変数

    public int level = 1;
    public long maxHP = 5;
    public long currentHP;
    public long atk = 10;
    public float atkSpeed = 1;
    [SerializeField, Header("会心の確率、%で計算つまり100で確定会心")]
    public float criticalChance = 0;
    [SerializeField, Header("会心のダメージ、%で計算つまり100で2倍のダメージ")]
    public float criticalDamage = 0f;
    public float speed = 1;
    public float defense = 10;
    public float rerollSpeed = 1;
    public long ammo = 1;
    [SerializeField, Header("バフの値、%で計算つまり100で2倍のダメージ")]
    public float Buff = 0;
    public bool isInBase = false;

    public float atkCT = 0;
    public float rerollTime = 0;

    public GameObject shotTemplete = null;
    public EntityUIBase entityUIBase = null;

    #endregion


    #region 関数

    public virtual void TakeDamage(long opponentAtk, long opponentLevel, float opponentCriticalChance, float opponentCriticalDamage, float buff)
    {
        if (currentHP <= 0)
        {
            return;
        }

        float levelMultiplier = Mathf.Pow(1.1f, level - opponentLevel); // 1レベル差ごとに10%増減
        long damage = (long)Mathf.Max(1, (opponentAtk - defense) * levelMultiplier);
        damage += (long)(damage * (buff / 100));

        if(Random.Range(0f, 1f) < opponentCriticalChance/100)
        {
            damage += (long)(damage * opponentCriticalDamage/100);
            entityUIBase.Critical();
        }

        currentHP -= damage;

        // エフェクトやアニメーションの処理
        entityUIBase.TakeDamage();

        if (currentHP <= 0)
        {
            Die();
        }
    }
    public virtual GameObject Attack(Quaternion rotation)
    {
        if (!CanAttack())
        {
            return null;
        }

        // 攻撃処理
        GameObject shotObj = Instantiate(shotTemplete, transform.position, rotation);
        shotObj.transform.parent = transform.parent;
        shotObj.GetComponent<Shot_Gabu>().attacker = this;
        entityUIBase.Attack();
        atkCT = atkSpeed;
        return shotObj;
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
