using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private BaseBlock[] blockPrefabs;
    [SerializeField] private Powerup[] powerupPrefabs;
    [SerializeField] private float spawnInterval = 2f;

    private BaseBlock currentBlock;
    private float timer;

    private int powerUpSpawnThreshhold = 0;
    private int currentBlockSpawns = 0;


    private void Start()
    {
        powerUpSpawnThreshhold = 3;
        //Random.Range(7, 11);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBlock != null)
        {
            timer = 0f;
            return;
        }
        
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnBlock();
            timer = 0f;
        }
        
    }

    void SpawnBlock()
    {
        if (currentBlockSpawns >= powerUpSpawnThreshhold)
        {
            if (powerupPrefabs.Length == 0)
            {
                currentBlockSpawns = 0;
                return;
            }
            int randomIndexPowerups = Random.Range(0, powerupPrefabs.Length);
            currentBlock = Instantiate(powerupPrefabs[randomIndexPowerups], transform.position, Quaternion.identity);
            currentBlockSpawns = 0;
        }
        else
        {
            if (blockPrefabs.Length == 0)
                return;
                
            int randomIndex = Random.Range(0, blockPrefabs.Length);
            currentBlock = Instantiate(blockPrefabs[randomIndex], transform.position, Quaternion.identity);
            currentBlockSpawns++;
        }
    }

    public BaseBlock TakeBlock()
    {
        var block = currentBlock;
        currentBlock = null;
        return block;
    }
}