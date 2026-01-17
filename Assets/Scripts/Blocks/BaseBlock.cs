using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BaseBlock : MonoBehaviour
{
    [SerializeField] private GameObject hitGroundEffect;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (hitGroundEffect != null)
            {
                Vector2 spawnPosition = new Vector2(transform.position.x, collision.contacts[0].point.y);
                var vfx = Instantiate(hitGroundEffect, spawnPosition, Quaternion.identity);

                Destroy(vfx, 2f);
            }
        }
    }
}
