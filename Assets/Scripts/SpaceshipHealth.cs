using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int lives = 3;          // total lives
    private bool isDead = false;   // has the player died?

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.5f;   // seconds between registering hits
    private float lastHitTime = -1f;

    // Only using trigger collisions for hazards
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            // Check cooldown
            if (Time.time - lastHitTime > hitCooldown)
            {
                LoseLife();
                lastHitTime = Time.time;
            }
        }
    }

    private void LoseLife()
    {
        lives--;
        Debug.Log("Spaceship hit! Lives remaining: " + lives);

        if (lives <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("All lives lost! Reloading first scene...");
        SceneManager.LoadScene(0);  // reload the first scene
    }
}

