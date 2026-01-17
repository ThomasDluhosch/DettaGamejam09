using System.Collections;
using UnityEngine;

public class PowerupSystem : MonoBehaviour
{
    [Header("SpawnSettings")]
    [SerializeField] GameObject[] powerups;
    [SerializeField] float spawnInterval = 5f;

    [Header("SpawnArea")]
    [SerializeField] Vector2 minSpawnPos;
    [SerializeField] Vector2 maxSpawnPos;

    private bool isGameRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameRunning = true;
        StartCoroutine(spawnPowerups());
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
}
