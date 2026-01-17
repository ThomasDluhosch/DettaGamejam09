using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.DefaultInputActions;

[RequireComponent(typeof(PlayerActions))]
public class ClimberController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private ClimberActions ClimberActions;

    [Space(10)]
    [Header("Input Actions")]
    [SerializeField] private InputActionReference Jump;
    [SerializeField] private InputActionReference Move;
    [SerializeField] private InputActionReference Drag;
    
    
    [Space(10)]
    [Header("Colliders")]
    [SerializeField] private Collider2D groundCheckCollider, grabFrontCheckCollider, grabBackCheckCollider;
    private Collider2D[] blockColliders;

    private bool isGrounded = true;
    private bool blockInFront = false;
    private bool blockInBack = false;

    ContactFilter2D filter = new ContactFilter2D();

    int count = 0;


    void Start()
    {
        Jump.action.Enable();
        Move.action.Enable();
        Drag.action.Enable();

        ClimberActions = GetComponent<ClimberActions>();
        blockColliders = new Collider2D[1];
        filter.SetLayerMask(LayerMask.GetMask("Block"));
    }


    void Update()
    {

        float moveDirection = Move.action.ReadValue<float>();
        ClimberActions.Move(moveDirection);

        ///Ground check
        if (groundCheckCollider != null)
        {
            isGrounded = groundCheckCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        }

        //Grab Check
        if (grabFrontCheckCollider != null)
        {
            blockInFront = grabFrontCheckCollider.IsTouchingLayers(LayerMask.GetMask("Block"));
            count = grabFrontCheckCollider.Overlap(filter, blockColliders);
        }

        if (grabBackCheckCollider != null)
        {
            blockInBack = grabBackCheckCollider.IsTouchingLayers(LayerMask.GetMask("Block"));
            count = grabBackCheckCollider.Overlap(filter, blockColliders);
        }

        if (Jump.action.WasPressedThisFrame() && isGrounded)
        {
            ClimberActions.Climb();
        }

        if (Drag.action.IsPressed() && (blockInFront || blockInBack))
        {
            ClimberActions.Drag(blockColliders[0].gameObject, moveDirection);
        }

    }

}
