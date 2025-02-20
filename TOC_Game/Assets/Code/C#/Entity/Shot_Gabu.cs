﻿using UnityEngine;

public class Shot_Gabu : MonoBehaviour
{
    #region 変数

    public float speed = 1;

    public EntityBase attacker = null;
    public string targetTag = "Enemy";
    public EnemyManager enemyManager = null;

    #endregion

    #region 関数

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            other.GetComponent<EntityBase>().TakeDamage(attacker.atk, attacker.level, attacker.criticalChance, attacker.criticalDamage, attacker.Buff);
            Destroy(gameObject);
        }
    }

    public void OnAttacked()
    {
        Debug.Log("ショットしたよ");
        enemyManager.RmoveShot(gameObject);
        Destroy(gameObject);
    }

    public void OnBecameInvisible()
    {
        Debug.Log("消えた");
        OnAttacked();
    }

    #endregion


    public void Update()
    {
        if(gameObject.transform.position.x > 1000 || gameObject.transform.position.x < -1000 || gameObject.transform.position.y > 1000 || gameObject.transform.position.y < -1000)
        {
            OnAttacked();
        }
        if(Vector2.Distance(transform.position, attacker.transform.position) > 20)
        {
            OnAttacked();
        }
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
