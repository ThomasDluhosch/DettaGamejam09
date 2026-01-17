using UnityEngine;

public class SlowPowerup : Powerup
{
    [SerializeField] private float powerupDuration;
    [SerializeField] private float powerupSlow;

    protected override void activatePowerup(GameObject player)
    {
        Debug.Log("Activate Slow");
        PowerupSystem.Instance.playSlowPowerup(powerupSlow, powerupDuration);
        Destroy(gameObject);
    }
}
