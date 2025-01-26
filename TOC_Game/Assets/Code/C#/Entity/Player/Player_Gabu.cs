using UnityEngine;
using static DBManager_Gabu;

public class Player_Gabu : EntityBase
{
    #region 変数

    public PlayerUISystem_Gabu uiSystem;
    public SkillManager_Gabu skillManager;
    public PlayerInputManager playerMovement;

    public float holdTime = 0f;
    public readonly float openShopTime = 0.45f;

    #endregion


    #region 関数

    public void AttackButton(Quaternion rotation)
    {
        if (!isInBase)
        {
            Attack(rotation);
            return;
        }

        holdTime += Time.deltaTime;
        if(holdTime >= openShopTime)
        {
            uiSystem.Shop();
            return;
        }
        Attack(rotation);
    }

    public void Move(Vector2 direction)
    {
        uiSystem.Dash(direction);
    }

    public void UseSkill(int skillID)
    {
        skillManager.UseSkill(skillID, this);
    }

    public void ApplyPowerUp(PowerUpItem_Gabu powerUp)
    {
        powerUp.Apply(this);
    }

    public void Aim()
    {
        uiSystem.Aim();
    }

    public override void Reroll()
    {
        // DBに保存されている弾数より多い場合はリロールしない
        if (ammo >= DB.playerDBs[DB.AccountID].ammo)
        {
            return;
        }
        if (!isInBase)
        {
            return;
        }

        // リロール中の処理
        if (rerollTime > 0)
        {
            rerollTime -= Time.deltaTime;
            uiSystem.Reroll(rerollTime);

            if (rerollTime <= 0)
            {
                uiSystem.Rerolled();
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
        entityUIBase.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Base")
        {
            isInBase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Base")
        {
            isInBase = false;
        }
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

        if (entityUIBase == null)
        {
            entityUIBase = uiSystem;
        }

        playerMovement.player = this;
    }
}
