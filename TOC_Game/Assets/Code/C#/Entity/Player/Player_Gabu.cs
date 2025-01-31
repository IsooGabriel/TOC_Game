using System.Threading;
using UnityEngine;
using static DBManager_Gabu;

public class Player_Gabu : EntityBase
{
    #region 変数

    public PlayerUISystem_Gabu uiSystem;
    public SkillManager_Gabu skillManager;
    public PlayerInputManager playerMovement;
    public EnemyManager enemyManager;
    public SceneLoad_Gabu sceneLoad;
    public PlayerAssetsUISystem_Gabu playerAssetsUISystem;

    public float holdTime = 0f;
    public readonly float openShopTime = 0.45f;

    #endregion


    #region 関数

    public void AttackButton(Quaternion rotation)
    {
        if(entityUIBase.isDie) { return; }
        GameObject shotObj;
        if (!isInBase)
        {
            if (ammo <= 0 || atkCT > 0)
            {
                return;
            }
            shotObj = Attack(rotation);
            enemyManager.SetShot(shotObj);
            shotObj.GetComponent<Shot_Gabu>().enemyManager = enemyManager;
            ammo--;
            uiSystem.UpdateAmmo(ammo);
            return;
        }

        holdTime += Time.deltaTime;
        if (holdTime >= openShopTime)
        {
            uiSystem.Shop();
            return;
        }
        shotObj = Attack(rotation);
        enemyManager.SetShot(shotObj);
        shotObj.GetComponent<Shot_Gabu>().enemyManager = enemyManager;
        ammo--;
        uiSystem.UpdateAmmo(ammo);
    }

    public override void TakeDamage(long opponentAtk, long opponentLevel, float opponentCriticalChance, float opponentCriticalDamage, float buff)
    {
        base.TakeDamage(opponentAtk, opponentLevel, opponentCriticalChance, opponentCriticalDamage, buff);
        uiSystem.UpdataHpSlider();
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

    public void UpdateMoney()
    {
        playerAssetsUISystem.UpdateMoney();
    }

    public void SetIsInBase(bool binary)
    {
        isInBase = binary;
    }

    public override void Die()
    {
        if(entityUIBase.isDie) { return; }
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
        atk = DB.playerDBs[DB.AccountID].atk;
        atkSpeed = DB.playerDBs[DB.AccountID].atkSpeed;
        speed = DB.playerDBs[DB.AccountID].speed;
        defense = DB.playerDBs[DB.AccountID].defense;
        rerollSpeed = DB.playerDBs[DB.AccountID].rerollSpeed;
        ammo = DB.playerDBs[DB.AccountID].ammo;
        isInBase = false;
        Buff = 100;

        if (skillManager == null)
        {
            skillManager = new SkillManager_Gabu();
        }
        skillManager.SetSkills();
        skillManager.UseAllSkills(this);

        if (entityUIBase == null)
        {
            entityUIBase = uiSystem;
        }

        playerMovement.player = this;
        currentHP = maxHP;
    }
}
