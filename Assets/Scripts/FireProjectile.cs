using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f; // how long before fire destroys itself
    private Transform target;
    private Vector3 randomOffset;

    void Start()
    {
        // Random offset to make movement uneven
        randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);

        // Destroy the fire after some time automatically
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position + randomOffset,
                speed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // destroy only the fireball, not the boss
        }
    }

}





