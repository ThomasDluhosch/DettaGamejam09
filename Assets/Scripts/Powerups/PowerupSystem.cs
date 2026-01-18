using System.Collections;
using UnityEngine;

public class PowerupSystem : MonoBehaviour
{
    public static PowerupSystem Instance { get; private set; }

    [Header("SpawnSettings")]
    [SerializeField] GameObject[] powerups;
    [SerializeField] float spawnInterval = 5f;

    [Header("SpawnArea")]
    [SerializeField] Vector2 minSpawnPos;
    [SerializeField] Vector2 maxSpawnPos;

    [Header("PlayerInstances")]
    [SerializeField] GameObject climber;
    [SerializeField] GameObject crane;


    [Header("Powerup Player Effects")]
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] float powerupEffectSpeed = 10f;
    [SerializeField] AudioClip powerupSound;


    private bool isGameRunning = false;

    //Singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameRunning = true;
        //StartCoroutine(spawnPowerups());
    }

    private IEnumerator spawnPowerups() {
        while (isGameRunning)
        {
            yield return new WaitForSeconds(spawnInterval);
            spawnPowerUp();
        }
    }

    private void spawnPowerUp() {
        Vector2 spawnPos = new Vector2(
           Random.Range(minSpawnPos.x, maxSpawnPos.x),
           Random.Range(minSpawnPos.y, maxSpawnPos.y)
       );

        GameObject prefab = powerups[
            Random.Range(0, powerups.Length)
        ];

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    public void playSpeedPowerup(float speedIncrease, float speedDuration) {
        StartCoroutine(speedRoutine(speedIncrease, speedDuration));
    }

    private IEnumerator speedRoutine(float speedIncrease, float speedDuration) {
        ClimberActions ca = climber.GetComponent<ClimberActions>();
        ca.setSpeed(ca.getSpeed() + speedIncrease);
        PowerupEffectOnPlayer();
        yield return new WaitForSeconds(speedDuration);
        ca.setSpeed(ca.getSpeed() - speedIncrease);
        ResetPowerupEffectOnPlayer();
    }

    public void playSlowPowerup(float speedDecrease, float slowDuration) {
        StartCoroutine(slowRoutine(speedDecrease, slowDuration));
    }

    private IEnumerator slowRoutine(float speedDecrease, float slowDuration) {
        CraneController cc = crane.GetComponent<CraneController>();
        cc.setMoveSpeed(cc.getMoveSpeed() - speedDecrease);
        PowerupEffectOnPlayer();
        yield return new WaitForSeconds(slowDuration);
        cc.setMoveSpeed(cc.getMoveSpeed() + speedDecrease);
        ResetPowerupEffectOnPlayer();
    }

    public void playJumpPowerup(float jumpIncrease, float jumpDuration) {
        StartCoroutine(jumpRoutine(jumpIncrease, jumpDuration));
    }

    private IEnumerator jumpRoutine(float jumpIncrease, float jumpDuration) {
        ClimberActions ca = climber.GetComponent<ClimberActions>();
        ca.setJumpForce(ca.getJumpForce() + jumpIncrease);
        PowerupEffectOnPlayer();
        yield return new WaitForSeconds(jumpDuration);
        ca.setJumpForce(ca.getJumpForce() - jumpIncrease);
        ResetPowerupEffectOnPlayer();
    }

    public void playShieldPowerup(float shieldDuration) {
        StartCoroutine(shieldRoutine(shieldDuration));
    }

    private IEnumerator shieldRoutine(float shieldDuration) {
        ClimberHealth ch = climber.GetComponent<ClimberHealth>();
        ch.SetShieldActive(true);
        PowerupEffectOnPlayer();
        yield return new WaitForSeconds(shieldDuration);
        ch.SetShieldActive(false);
        ResetPowerupEffectOnPlayer();
    }

    private void PowerupEffectOnPlayer()
    {
        if (playerSpriteRenderer != null)
        {
            SFXManager.Instance.PlaySFX(powerupSound);
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            playerSpriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_PowerUpSpeed", powerupEffectSpeed);
            playerSpriteRenderer.SetPropertyBlock(propBlock);
        }
    }

    private void ResetPowerupEffectOnPlayer()
    {
        if (playerSpriteRenderer != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            playerSpriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_PowerUpSpeed", 0f);
            playerSpriteRenderer.SetPropertyBlock(propBlock);
        }
    }
}
