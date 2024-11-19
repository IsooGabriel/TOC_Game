using UnityEngine;

public class movimento : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f; // Smoothing factor

    private Vector2 movement;
    private Vector2 velocity = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get player input for movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector
        movement = new Vector2(moveX, moveY).normalized;

        // Smoothly move the player
        transform.position = Vector2.SmoothDamp(transform.position, transform.position + (Vector3)movement, ref velocity, smoothTime, moveSpeed, Time.deltaTime);
    }
}
