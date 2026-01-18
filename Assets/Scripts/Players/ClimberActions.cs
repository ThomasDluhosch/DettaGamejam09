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

    private FixedJoint2D dragJoint;
    private Rigidbody2D draggedBlockRb;
    [SerializeField] private float grabOffsetX = 0.5f;


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

    public bool isDragging;

    // todo
    public void Drag(GameObject block)
    {
        if (!IsGrounded) return;
        if (!isDragging) {
            isDragging = true;
            setSpeed(moveSpeed / 2f);
        }

        if (dragJoint != null) return;

        Rigidbody2D blockRb = block.GetComponent<Rigidbody2D>();
        if (blockRb == null) return;

        dragJoint = block.AddComponent<FixedJoint2D>();
        dragJoint.connectedBody = rb;
        dragJoint.enableCollision = true;
        dragJoint.autoConfigureConnectedAnchor = false;

        Vector2 worldOffset = blockRb.position - rb.position;


        dragJoint.anchor = blockRb.transform.InverseTransformPoint(blockRb.position);
        dragJoint.connectedAnchor = rb.transform.InverseTransformPoint(rb.position + worldOffset);

        draggedBlockRb = blockRb;

    }

    public void ReleaseDrag()
    {
        if (dragJoint == null) return;

        Destroy(dragJoint);
        dragJoint = null;
        draggedBlockRb = null;
        if (isDragging) {
            isDragging = false;
            setSpeed(moveSpeed * 2f);
        }
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
