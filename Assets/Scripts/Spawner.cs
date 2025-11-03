using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;         // assign your stone prefab in the inspector
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 1f;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        // Instantiate the stone
        GameObject stone = Instantiate(prefab, transform.position, Quaternion.identity);

        // Move it randomly up/down
        stone.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

        // Make sure it has a collider and tag
        Collider2D col = stone.GetComponent<Collider2D>();
        if (col == null)
        {
            col = stone.AddComponent<BoxCollider2D>(); // or CircleCollider2D
            col.isTrigger = true;                      // set as trigger
        }
        else
        {
            col.isTrigger = true;
        }

        // Tag it as hazard
        stone.tag = "Hazard";

        // Optional: make invisible
        SpriteRenderer sr = stone.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.enabled = false; // make the sprite invisible
        }
    }
}

