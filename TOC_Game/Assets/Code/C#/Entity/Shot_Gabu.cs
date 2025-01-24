using UnityEngine;

public class Shot_Gabu : MonoBehaviour
{
    #region 変数

    public float speed = 1;

    public EntityBase attacker = null;
    public string targetTag = "Enemy";

    #endregion

    #region 関数

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            other.GetComponent<EntityBase>().TakeDamage(attacker.atk, attacker.level, attacker.criticalChance, attacker.criticalDamage);
            Destroy(gameObject);
        }
    }

    #endregion

    private void Start()
    {

    }

    public void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
