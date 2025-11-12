using UnityEngine;

public class FireShooter2D : MonoBehaviour
{
    [Header("Fire Settings")]
    public GameObject firePrefab;   // Fireball prefab
    public Transform firePoint;     // Where fire spawns (e.g., gun barrel)
    public float fireForce = 10f;   // Fireball speed
    public float lifeTime = 2f;     // Fallback: destroy after X seconds

    private bool facingRight = true; // Tracks facing direction

    void Update()
    {
        // Shoot when left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            ShootFire();
        }

        // Detect facing direction based on local scale
        facingRight = transform.localScale.x > 0;
    }

    void ShootFire()
    {
        if (firePrefab != null && firePoint != null)
        {
            GameObject fireInstance = Instantiate(firePrefab, firePoint.position, Quaternion.identity);

            // Add velocity
            Rigidbody2D rb = fireInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = facingRight ? 1f : -1f;
                rb.linearVelocity = new Vector2(direction * fireForce, 0);
            }

            // Flip fireball sprite if facing left
            if (!facingRight)
            {
                Vector3 scale = fireInstance.transform.localScale;
                scale.x *= -1;
                fireInstance.transform.localScale = scale;
            }

            // Destroy fireball after a delay (safety cleanup)
            Destroy(fireInstance, lifeTime);
        }
        else
        {
            Debug.LogWarning("FireShooter2D: Missing firePrefab or firePoint!");
        }
    }
}



