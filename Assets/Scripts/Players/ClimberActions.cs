using UnityEngine;

public class ClimberActions : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    private float currentMoveDirection;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Apply movement velocity
        if (rb != null) 
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.x = currentMoveDirection * moveSpeed;
            rb.linearVelocity = velocity;
        }
    }

    public void Move(float direction)
    {
        currentMoveDirection = direction;
    }

    public void Climb()
    {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Jumping (No Rigidbody assigned)");
        }
    }

    // todo
    public void Drag()
    {
        Debug.Log("Dragging");
    }

}
