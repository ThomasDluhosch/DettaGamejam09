using UnityEngine;

public class JumpPowerup : Powerup
{
    [SerializeField] private float jumpIncrease;
    [SerializeField] private float powerupDuration;


    protected override void activatePowerup(GameObject player)
    {
        Debug.Log("Activate Jump Boost");
        // std value +/- jumpIncrease
        PowerupSystem.Instance.playJumpPowerup(jumpIncrease, powerupDuration);
        Destroy(gameObject);
    }
}
