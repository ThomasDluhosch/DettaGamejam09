using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CraneController : MonoBehaviour
{
    [Header("Crane Components")]
    [SerializeField] private Transform craneArm;
    [SerializeField] private Transform startPoint;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float horizontalLimit = 8f;

    [Header("Events")]
    public UnityEvent onCranePickedUp;
    public UnityEvent onCraneDropped;

    private CraneInput craneInput;

    private void Awake()
    {
        craneInput = new CraneInput();
    }

    private void OnEnable()
    {
        craneInput.Enable();
    }

    private void OnDisable()
    {
        craneInput.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUpInput();
    }

    void SetUpInput()
    {
        List<InputDevice> devices = new List<InputDevice>
        {
            Keyboard.current
        };

        if (Gamepad.all.Count > 0)
        {
            devices.Add(Gamepad.all[0]);
        }
        craneInput.devices = devices.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = craneInput.Crane.Movement.ReadValue<float>();

        HandleMovement(horizontal);

        if (craneInput.Crane.Pickup.WasPerformedThisFrame())
        {
            onCranePickedUp.Invoke();
        }
        if (craneInput.Crane.Drop.WasPerformedThisFrame())
        {
            onCraneDropped.Invoke();
        }
    }

    void HandleMovement(float horizontal)
    {
        if ((craneArm.position.x <= -horizontalLimit && horizontal < 0) ||
            (craneArm.position.x >= horizontalLimit && horizontal > 0))
        {
            return;
        }

        craneArm.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (startPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-horizontalLimit, startPoint.position.y, startPoint.position.z),
                            new Vector3(horizontalLimit, startPoint.position.y, startPoint.position.z));
        }
    }

    public float getMoveSpeed() { return moveSpeed; }

    public void setMoveSpeed(float moveSpeed) { this.moveSpeed = moveSpeed; }
}
