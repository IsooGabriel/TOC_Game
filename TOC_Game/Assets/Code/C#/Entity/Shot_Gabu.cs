using UnityEngine;

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
        enemyManager.DessetShot(gameObject);
        Destroy(gameObject);
    }

    public void OnBecameInvisible()
    {
       Destroy(gameObject);
    }

    #endregion

    public void Update()
    {
        if(gameObject.transform.position.x > 1000 || gameObject.transform.position.x < -1000 || gameObject.transform.position.y > 1000 || gameObject.transform.position.y < -1000)
        {
            Destroy(gameObject);
        }
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
