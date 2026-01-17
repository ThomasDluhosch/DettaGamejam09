using System.Collections;
using UnityEngine;

public class SpeedPowerup : Powerup
{
    [SerializeField] private float powerupDuration;
    [SerializeField] private float powerupSpeed;

    protected override void activatePowerup(GameObject player)
    {
        Debug.Log("Activate Speed");
        PowerupSystem.Instance.playSpeedPowerup(powerupSpeed, powerupDuration);
        Destroy(gameObject);
    }
}
