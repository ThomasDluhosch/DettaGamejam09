using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ClimberActions : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    private float currentMoveDirection;

    private Rigidbody2D draggedBody;

    [SerializeField] LayerMask boxLayer;

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

        hasPressedJump = true;
        isJumping = true;
        jumpCounter = 0f;

    }

    // todo
    public void Drag(GameObject block, float moveDirection)
    {
       Rigidbody2D blockRb = block.GetComponent<Rigidbody2D>();
        if (blockRb == null) return;

        if (Mathf.Abs(moveDirection) < 0.01f) return;

        blockRb.linearVelocity = new Vector2(
            moveDirection * 4f,
            blockRb.linearVelocity.y
        );
    }

    public void setSpeed(float speed) {
        moveSpeed = speed;
    }

    public float getSpeed() { return moveSpeed; }

}
