using UnityEngine;

public class ClimberActions : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;


    [Space(10)]
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpMultiplier = 2f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpTime = 0;
    [SerializeField] float jumpCounter;
    [SerializeField] private bool hasPressedJump = false;
    [SerializeField] bool isJumping;
    [SerializeField] private Vector2 vecGravity;



    [Space(10)]
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private float currentMoveDirection;
    private Rigidbody2D draggedBody;
    [SerializeField] LayerMask boxLayer;


    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
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


    void FixedUpdate()
    {
        if (hasPressedJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            hasPressedJump = false;
            isJumping = true;
        }

        if (rb.linearVelocity.y > 0 && isJumping)
        {
            jumpCounter += Time.fixedDeltaTime;
            if (jumpCounter >= jumpTime)
            {
                isJumping = false;

            }

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > .5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rb.linearVelocity += vecGravity * currentJumpM * Time.fixedDeltaTime;
        }

        if (rb.linearVelocity.y < 0 || !isJumping)
        {
            rb.linearVelocity -= vecGravity * fallMultiplier * Time.fixedDeltaTime;
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

    public void StopClimb()
    {
        isJumping = false;
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

}
