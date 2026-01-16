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


    void Start()
    {
        Jump.action.Enable();
        Move.action.Enable();
        Drag.action.Enable();

        ClimberActions = GetComponent<ClimberActions>();
    }


    void Update()
    {
        if (Jump.action.WasPressedThisFrame())
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
