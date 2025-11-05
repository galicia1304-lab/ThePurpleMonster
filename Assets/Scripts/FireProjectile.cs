using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 moveDirection;

    void Start()
    {
        Destroy(gameObject, 5f); // destroy after 5 seconds

        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;

        // Destroy if off-camera
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;
    }
}


