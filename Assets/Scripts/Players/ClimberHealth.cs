using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClimberHealth : MonoBehaviour
{
    [SerializeField] private float health = 1f;
    [SerializeField] private float maxDamageForce = 40f;
    [SerializeField] private float hitAngleThreshold = 80f;
    [SerializeField] private UnityEvent onDeath;
    [SerializeField] private UnityEvent<float> onHealthChanged;

    private bool isDead = false;
    private List<Collider2D> alreadyCollidedBlocks = new List<Collider2D>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onHealthChanged.Invoke(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(float damage)
    {
        if (isDead) return;

        Debug.Log($"Climber took {damage} damage.");
        
        health -= damage;
        onHealthChanged.Invoke(health);
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Climber has died.");
        onDeath.Invoke();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            if (alreadyCollidedBlocks.Contains(collision.collider)) return;

            // check if collision is from above
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -Mathf.Cos(hitAngleThreshold * Mathf.Deg2Rad))
            {
                float hitForce = collision.relativeVelocity.magnitude;

                TakeDamage(hitForce / maxDamageForce);

                alreadyCollidedBlocks.Add(collision.collider);
            }
            
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 from = transform.position;
        Vector3 to = from + Quaternion.Euler(0, 0, hitAngleThreshold) * Vector3.down * 1f;
        Gizmos.DrawLine(from, to);
        to = from + Quaternion.Euler(0, 0, -hitAngleThreshold) * Vector3.down * 1f;
        Gizmos.DrawLine(from, to);
    }
}
