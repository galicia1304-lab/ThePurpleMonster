using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipHealth : MonoBehaviour
{
    public int lives = 3; // starting lives
    private bool isDead = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            LoseLife();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        lives--;
        Debug.Log("Spaceship hit! Lives remaining: " + lives);

        if (lives <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    void Die()
    {
        Debug.Log("All lives lost! Returning to first scene...");
        // Load the first scene (index 0 in the build settings)
        SceneManager.LoadScene(0);
    }
}

