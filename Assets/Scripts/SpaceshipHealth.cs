using TMPro; // Only if using TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int lives = 3;
    private bool isDead = false;

    [Header("UI Reference")]
    public TextMeshProUGUI livesText;  // Use Text if not using TMP

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.5f;
    private float lastHitTime = -1f;

    private void Start()
    {
        UpdateLivesUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazard"))
        {
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
        UpdateLivesUI();

        if (lives <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = ": " + lives;
    }

    private void Die()
    {
        Debug.Log("All lives lost! Reloading first scene...");
        SceneManager.LoadScene(0);
    }
}


