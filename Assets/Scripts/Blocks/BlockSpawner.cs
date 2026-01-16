using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private BaseBlock[] blockPrefabs;
    [SerializeField] private float spawnInterval = 2f;

    private BaseBlock currentBlock;
    private float timer;


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
        int randomIndex = Random.Range(0, blockPrefabs.Length);
        currentBlock = Instantiate(blockPrefabs[randomIndex], transform.position, Quaternion.identity);
    }

    public BaseBlock TakeBlock()
    {
        var block = currentBlock;
        currentBlock = null;
        return block;
    }
}