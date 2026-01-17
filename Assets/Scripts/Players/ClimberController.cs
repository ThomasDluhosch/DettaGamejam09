using Unity.Mathematics;
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

        if (groundCheckCollider == null)
        {
            Debug.LogWarning("Ground Check Collider not assigned in ClimberController");
        }
    }


    void Update()
    {

        float moveDirection = Move.action.ReadValue<float>();
        ClimberActions.Move(moveDirection);

        if (Jump.action.WasPressedThisFrame() && isGrounded())
        {
            ClimberActions.Climb();
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



        if (Drag.action.IsPressed() && (blockInFront || blockInBack))
        {
            ClimberActions.Drag(blockColliders[0].gameObject, moveDirection);
        }

    }

    private bool isGrounded()
    {
        return groundCheckCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

}
