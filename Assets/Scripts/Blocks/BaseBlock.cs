using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BaseBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    public Rigidbody2D Rigidbody => rb;
    public Collider2D Collider => col;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        col.isTrigger = true;
    }
    
    /// <summary>
    /// Called every frame when the crane is holding this block.
    /// </summary>
    public virtual void OnCraneUpdate() { }

    /// <summary>
    /// Called when the crane picks up this block.
    /// </summary>
    public virtual void OnCranePickup() { }

    /// <summary>
    /// Called when the crane drops this block.
    /// </summary>
    public virtual void OnCraneDrop()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        col.isTrigger = false;
    }
}
