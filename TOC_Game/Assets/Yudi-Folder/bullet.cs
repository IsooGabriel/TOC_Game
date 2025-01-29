using UnityEngine;

public class bullet : MonoBehaviour
{
    public float lifeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
        Destroy(gameObject,lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
