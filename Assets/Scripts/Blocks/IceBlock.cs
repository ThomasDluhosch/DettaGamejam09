using UnityEngine;

public class IceBlock : BaseBlock
{
    [SerializeField] private GameObject iceBreakEffectPrefab;
    [SerializeField] private float breakForceMultiplier = 2f;
    [SerializeField] private float breakVelocityMultiplier = 2f;

    private float iceHealth = 100f;

    void Update()
    {
        if (iceHealth <= 0)
        {
            BreakIce();
            return;
        }

    }

    void FixedUpdate()
    {

        iceHealth -= Rigidbody.totalForce.magnitude * breakForceMultiplier;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        iceHealth -= collision.relativeVelocity.magnitude * breakVelocityMultiplier;
        
    }

    private void BreakIce()
    {
        if (iceBreakEffectPrefab != null)
        {
            Instantiate(iceBreakEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
