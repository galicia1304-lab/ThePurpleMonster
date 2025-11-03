using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject stonePrefab;
    public float spawnRate = 2f;
    public float minY = -1f;
    public float maxY = 2f;
    public float gapSize = 3f; // distance between top and bottom stones
    public float stoneHeight = 1f; // approximate height of one stone
    public float stoneSpeed = 2f; // how fast stones move left

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPair), 1f, spawnRate);
    }

    void SpawnPair()
    {
        float gapCenterY = Random.Range(minY, maxY);

        // Calculate top and bottom positions using fixed stone height
        Vector3 topPos = transform.position + Vector3.up * (gapCenterY + gapSize / 2f + stoneHeight / 2f);
        Vector3 bottomPos = transform.position + Vector3.up * (gapCenterY - gapSize / 2f - stoneHeight / 2f);

        // Spawn both stones
        GameObject topStone = Instantiate(stonePrefab, topPos, Quaternion.identity);
        GameObject bottomStone = Instantiate(stonePrefab, bottomPos, Quaternion.identity);

        SetupStone(topStone);
        SetupStone(bottomStone);
    }

    void SetupStone(GameObject stone)
    {
        // Ensure it has a collider
        Collider2D col = stone.GetComponent<Collider2D>();
        if (col == null)
            col = stone.AddComponent<BoxCollider2D>();
        col.isTrigger = true;


        // Add movement component
        StoneMover mover = stone.AddComponent<StoneMover>();
        mover.speed = stoneSpeed;

        // Auto-destroy after 10 seconds
        Destroy(stone, 10f);
    }
}

public class StoneMover : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    // Optional: Destroy if it goes far off-screen (extra safety)
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
