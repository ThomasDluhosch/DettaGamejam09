using Unity.VisualScripting;
using UnityEngine;

public class TNTBlock : BaseBlock
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject explosionEffectPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnCraneDrop()
    {
        base.OnCraneDrop();
        if (spriteRenderer != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_DamageFlashSpeed", 4f);
            spriteRenderer.SetPropertyBlock(propBlock);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            Rigidbody2D rb = nearbyObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = rb.position - (Vector2)transform.position;
                rb.AddForce(direction.normalized * explosionForce);
            }
        }
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
