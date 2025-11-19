using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 5f;

    private Vector2 direction;  // NEW: the direction the fireball moves

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // NEW: Set a movement direction
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // Move in the assigned direction
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}






