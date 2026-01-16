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

    private bool isGrounded = true;
    private bool blockInFront = false;
    private bool blockInBack = false;


    void Start()
    {
        Jump.action.Enable();
        Move.action.Enable();
        Drag.action.Enable();

        ClimberActions = GetComponent<ClimberActions>();
    }


    void Update()
    {
        ///Ground check
        if (groundCheckCollider != null)
        {
            isGrounded = groundCheckCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        }

        //Grab Check
        if (grabFrontCheckCollider != null)
        {
            blockInFront = grabFrontCheckCollider.IsTouchingLayers(LayerMask.GetMask("Block"));
        }

        if (grabBackCheckCollider != null)
        {
            blockInBack = grabBackCheckCollider.IsTouchingLayers(LayerMask.GetMask("Block"));
        }

        if (Jump.action.WasPressedThisFrame() && isGrounded)
        {
            ClimberActions.Climb();
        }

        if (Drag.action.WasPressedThisFrame() && (blockInFront || blockInBack))
        {
            ClimberActions.Drag();
        }

        float moveDirection = Move.action.ReadValue<float>();
        ClimberActions.Move(moveDirection);

    }
}
