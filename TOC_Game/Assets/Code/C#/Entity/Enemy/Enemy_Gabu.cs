using System.Threading;

public class Enemy_Gabu : EntityBase
{
    #region 変数

    public uint ID = 0;
    public EnemyManager enemyManager;

    #endregion

    #region 関数

    public override void Reroll()
    {
        // リロール処理
        return;
    }

    public override void Die()
    {
        if(entityUIBase.isDie) { return; }
        
        entityUIBase.Die();
        Thread.Sleep((int)entityUIBase.dieEffectTime * 1000); // エフェクトが終わるまで待機、1000倍してミリ秒に変換
        Reset();
        enemyManager.ResetEnemy(ID);
    }

    public void Reset()
    {
        currentHP = maxHP;
        rerollSpeed = 999;
        ammo = 999999;
        Buff = 100;
    }

    #endregion

    private void Start()
    {
        currentHP = maxHP;
        rerollSpeed = 999;
        ammo = 999999;
    }
}
