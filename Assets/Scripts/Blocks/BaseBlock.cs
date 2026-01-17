using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }
}
