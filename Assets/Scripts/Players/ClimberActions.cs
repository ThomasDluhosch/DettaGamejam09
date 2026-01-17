using NUnit.Framework;
using UnityEngine;

public class ClimberActions : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float decelerationDrag = 15f;
    [SerializeField] private float movementAcceleration = 100f;
    private bool isMoving = false;


    [Space(10)]
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float jumpMultiplier = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpTime = 0.1f;
    [SerializeField] float jumpCounter;
    [SerializeField] private bool hasPressedJump = false;
    [SerializeField] bool isJumping;
    [SerializeField] private Vector2 vecGravity;


    [Space(10)]
    [Header("References")]
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    private float currentMoveDirection;
    private Rigidbody2D draggedBody;
    [SerializeField] LayerMask boxLayer;
    public bool IsGrounded { get; set; }


    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
    }


    void FixedUpdate()
    {
        // Moving
        if (Mathf.Abs(currentMoveDirection) > 0.01f)
        {
            rb.linearDamping = 0f;
            rb.AddForce(new Vector2(currentMoveDirection * movementAcceleration, 0), ForceMode2D.Force);

            Vector2 velocity = rb.linearVelocity;
            velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
            rb.linearVelocity = velocity;
            isMoving = true;
        }
        else
        {
            if (IsGrounded)
            {
                rb.linearDamping = decelerationDrag;
            }
            else
            {
                rb.linearDamping = 0f;
            }
        }

        //Setting Animator Properties
        if (isMoving)
        {
            animator.SetBool("isMoving", true);
        } else if (!isMoving)
        {
            animator.SetBool("isMoving", false);
        }


        // Jumping
        if (hasPressedJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            hasPressedJump = false;
            isJumping = true;
            animator.SetBool("isJumping", true);
        }

        if (rb.linearVelocity.y > 0 && isJumping)
        {
            rb.linearDamping = 0f;
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
        switch (direction)
        {
            case 1:
                Sprite.flipX = true;
                break;
            case -1:
                Sprite.flipX = false;
                break;
        }
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

    public float getSpeed()
    {
        return moveSpeed;
    }

    public void setSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void setJumpForce(float newJumpForce)
    {
        jumpForce = newJumpForce;
    }

    public float getJumpForce()
    {
        return jumpForce;
    }
}
