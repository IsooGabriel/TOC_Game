using UnityEngine;
using UnityEngine.InputSystem;

public class movimento : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f; // Smoothing factor

    private Vector2 movement; // Input movement vector
    private Vector2 velocity = Vector2.zero;
    [SerializeField]

    private PlayerInput playerInput; // Reference to PlayerInput component

    // Called when the object is created
    void Start()
    {
        // Get the PlayerInput component attached to the player GameObject
        playerInput = GetComponent<PlayerInput>();
    }

    // This method is called automatically by the Input System when input changes
    public void Move(InputAction.CallbackContext context)
    {
        // Read the movement input as a Vector2
        movement = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // Smoothly move the player based on the input vector
        MovePlayer();
    }

    // Method to handle player movement
    void MovePlayer()
    {
        // Smoothly transition between current and target position
        transform.position = Vector2.SmoothDamp(transform.position, transform.position + (Vector3)movement, ref velocity, smoothTime, moveSpeed, Time.deltaTime);
    }
}

