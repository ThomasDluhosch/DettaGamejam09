using Unity.VisualScripting;
using UnityEngine;

public abstract class Powerup : BaseBlock
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            activatePowerup(collision.gameObject);
            Debug.Log("Activated Powerup");
            Destroy(gameObject);
        }
    }

    protected abstract void activatePowerup(GameObject player);
}
