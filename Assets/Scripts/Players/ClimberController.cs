using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private Collider2D groundCheckCollider, grabCheckCollider;
    private Collider2D[] blockColliders;

    [Header("Scripts")]
    [SerializeField] ClimberActions climberActions;

    float moveDirection;

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

        SetUpInput();
    }


    void Update()
    {

        moveDirection = Move.action.ReadValue<float>();
        ClimberActions.Move(moveDirection);
        ClimberActions.IsGrounded = isGrounded();

        if (Jump.action.WasPressedThisFrame() && isGrounded())
        {
            ClimberActions.Climb();
        }

        if(Jump.action.WasReleasedThisFrame())
        {
            ClimberActions.StopClimb();
        }



    }

    private void FixedUpdate()
    {
        int count = grabCheckCollider.Overlap(filter, blockColliders);
        bool hasBlock = count > 0;

        // Start dragging
        if (Drag.action.WasPressedThisFrame() && hasBlock && !climberActions.isDragging)
        {
            ClimberActions.Drag(blockColliders[0].gameObject);
        }

        // Stop dragging
        if (!Drag.action.IsPressed() && climberActions.isDragging)
        {
            ClimberActions.ReleaseDrag();
        }
    }

    private bool isGrounded()
    {
        return groundCheckCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || groundCheckCollider.IsTouchingLayers(LayerMask.GetMask("Block"));
    }


    void SetUpInput()
    {
        List<InputDevice> devices = new List<InputDevice>
        {
            Keyboard.current
        };

        if (Gamepad.all.Count > 1)
        {
            devices.Add(Gamepad.all[1]);
        }
        Jump.asset.devices = devices.ToArray();
    }
}
