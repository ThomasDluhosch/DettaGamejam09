using System.Collections;
using UnityEngine;

public class ShieldPowerup : Powerup
{
    [SerializeField] private float powerupDuration;

    protected override void activatePowerup(GameObject player)
    {
        Debug.Log("Activate Shield");
        PowerupSystem.Instance.playShieldPowerup(powerupDuration);
        Destroy(gameObject);
    }
}
