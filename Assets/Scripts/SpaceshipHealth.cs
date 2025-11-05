using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int lives = 3;
    private bool isDead = false;

    [Header("UI Reference")]
    public TextMeshProUGUI livesText;  // Assign in Inspector

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.5f;
    private float lastHitTime = -1f;

    [Header("Restart Settings")]
    public float respawnDelay = 2f; // seconds to wait before restarting

    [Header("Effects")]
    public GameObject explosionEffect; // Optional: particle prefab or animation

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
            StartCoroutine(HandleDeath());
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = ": " + lives;
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        Debug.Log("All lives lost! Playing explosion and restarting minigame...");

        // Play explosion effect at spaceship position
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Optional: wait a short delay so player can see the explosion
        yield return new WaitForSeconds(respawnDelay);

        // Reload the current scene to restart the minigame
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}



