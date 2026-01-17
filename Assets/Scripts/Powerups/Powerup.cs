using Unity.VisualScripting;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            activatePowerup();
            Debug.Log("Activated Powerup");
            Destroy(gameObject);
        }
    }

    protected abstract void activatePowerup();
}
