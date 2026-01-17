using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Powerup : BaseBlock
{
    int durationTime = 5;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            activatePowerup(collision.gameObject);
            Debug.Log("Activated Powerup");
            Destroy(gameObject);
        }
    }

    public override void OnCraneDrop() {
        base.OnCraneDrop();
        StartCoroutine(liveTimer());

    }

    private IEnumerator liveTimer() {
        yield return new WaitForSeconds(durationTime);
        Destroy(gameObject);
    }

    protected abstract void activatePowerup(GameObject player);
}
