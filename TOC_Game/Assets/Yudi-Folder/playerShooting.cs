using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;    // Bullet prefab to instantiate
    public float bulletSpeed = 10f;    // Bullet speed
    public Transform shootingPoint;    // The point where the bullet will spawn (e.g., player's gun)

    void Update()
    {
        // Check for mouse click to shoot
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }
    }

    // Method to shoot a bullet towards the mouse position
    void ShootBullet()
    {
        // Instantiate bullet at the shooting point
        GameObject bullet = InstantiateBullet();

        // Get direction to mouse position
        Vector2 direction = GetDirectionToMouse();

        // Set bullet's velocity in that direction
        MoveBullet(bullet, direction);

        // Rotate bullet to face direction
        RotateBullet(bullet, direction);
    }

    // Method to instantiate the bullet at the shooting point
    GameObject InstantiateBullet()
    {
        return Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
    }

    // Method to calculate the direction from the shooting point to the mouse position
    Vector2 GetDirectionToMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mousePosition - (Vector2)shootingPoint.position).normalized;
    }

    // Method to set the bullet's velocity towards the mouse position
    void MoveBullet(GameObject bullet, Vector2 direction)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        //rb.linearVelocity = direction * bulletSpeed;
    }

    // Method to rotate the bullet to face the mouse direction
    void RotateBullet(GameObject bullet, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
