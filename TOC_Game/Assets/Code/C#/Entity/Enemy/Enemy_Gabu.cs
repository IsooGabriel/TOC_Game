using UnityEngine;

public class Enemy_Gabu : EntityBase
{
    public Rigidbody2D rb;

    #region 関数


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<EntityBase>().TakeDamage(atk, level);
        }
    }

    public override void Reroll()
    {
        // リロール処理
        return;
    }

    public override void Die()
    {
        // ディド処理
    }

    #endregion

    private void Start()
    {
        currentHP = maxHP;
        rerollSpeed = 999;
        ammo = 999999;
    }
}
