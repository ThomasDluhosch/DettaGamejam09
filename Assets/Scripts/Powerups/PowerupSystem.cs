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
        yield return new WaitForSeconds(speedDuration);
        ca.setSpeed(ca.getSpeed() - speedIncrease);
    }

    public void playSlowPowerup(float speedDecrease, float slowDuration) {
        StartCoroutine(slowRoutine(speedDecrease, slowDuration));
    }

    private IEnumerator slowRoutine(float speedDecrease, float slowDuration) {
        CraneController cc = crane.GetComponent<CraneController>();
        cc.setMoveSpeed(cc.getMoveSpeed() - speedDecrease);
        yield return new WaitForSeconds(slowDuration);
        cc.setMoveSpeed(cc.getMoveSpeed() + speedDecrease);
    }
}
