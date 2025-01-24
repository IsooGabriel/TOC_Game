using UnityEngine;
using static DBManager_Gabu;

public class Player_Gabu : EntityBase
{
    #region 変数

    public SkillManager_Gabu skillManager;

    #endregion


    #region 関数


    public void UseSkill(int skillID)
    {
        skillManager.UseSkill(skillID, this);
    }

    public void ApplyPowerUp(PowerUpItem_Gabu powerUp)
    {
        powerUp.Apply(this);
    }

    public override void Reroll()
    {
        // DBに保存されている弾数より多い場合はリロールしない
        if (ammo >= DB.playerDBs[DB.AccountID].ammo)
        {
            return;
        }

        // リロール中の処理
        if (rerollTime > 0)
        {
            rerollSpeed -= Time.deltaTime;
            if (rerollSpeed <= 0)
            {
                ammo += 1;
            }
            return;
        }

        // リロール時間が0以下の場合は新しく弾をリロールする
        rerollTime = rerollSpeed;
    }


    public void SetIsInBase(bool binary)
    {
        isInBase = binary;
    }

    public override void Die()
    {
        entityUIBase    .Die();
    }

    #endregion

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
        isInBase = false;
        Buff = 0;

        skillManager = new SkillManager_Gabu();
        skillManager.UseAllSkills(this);
    }
}
