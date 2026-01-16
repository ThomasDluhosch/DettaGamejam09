using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private Collider2D groundCheckCollider;

    private bool isGrounded = true;


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

        if (Jump.action.WasPressedThisFrame() && isGrounded)
        {
            ClimberActions.Climb();
        }

        if (Drag.action.WasPressedThisFrame())
        {
            ClimberActions.Drag();
        }

        float moveDirection = Move.action.ReadValue<float>();
        ClimberActions.Move(moveDirection);

    }
}
