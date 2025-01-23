using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Pool;
using static DBManager_Gabu;

public class Player_Gabu : MonoBehaviour
{
    public int level = 1;
    public long maxHP = 100;
    public long currentHP;
    public long atk = 10;
    public float atkSpeed = 1;
    public float speed = 1;
    public float defense = 10;
    public float rerollSpeed = 1;
    public long ammo = 1;
    public float Buff = 0;
    public float criticalChance = 0;
    public float criticalDmg = 0;

    public float atkCT = 0;
    public float rerollTime = 0;

    public SkillManager_Gabu skillManager;

    void Start()
    {
        level = DB.playerDBs[DB.AccountID].level;
        maxHP = DB.playerDBs[DB.AccountID].hp;
        currentHP = maxHP;
        atk = DB.playerDBs[DB.AccountID].atk;
        atkSpeed = DB.playerDBs[DB.AccountID].atkSpeed;
        speed = DB.playerDBs[DB.AccountID].speed;
        defense = DB.playerDBs[DB.AccountID].defense;
        rerollSpeed = DB.playerDBs[DB.AccountID].rerollSpeed;
        ammo = DB.playerDBs[DB.AccountID].ammo;

        skillManager = new SkillManager_Gabu();
        skillManager.UseAllSkills(this);
    }

    void Update()
    {
        if (atkCT > 0)
        {
            atkCT -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (!CanAttack())
        {
            return;
        }

        // 攻撃処理をここに記述
        Debug.Log("Player attacks with power: " + atk);
    }

    public bool CanAttack()
    {
        return true;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 死亡処理をここに記述
        Debug.Log("Player has died");
    }

    public void UseSkill(int skillID)
    {
        skillManager.UseSkill(skillID, this);
    }

    public void ApplyPowerUp(PowerUpItem_Gabu powerUp)
    {
        powerUp.Apply(this);
    }
}
