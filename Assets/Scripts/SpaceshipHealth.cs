using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipHealth : MonoBehaviour
{
    [Header("Lives Settings")]
    public int lives = 3;
    private bool isDead = false;

    [Header("UI Reference")]
    public TextMeshProUGUI livesText;

    [Header("Hit Cooldown")]
    public float hitCooldown = 0.5f;
    private float lastHitTime = -1f;

    [Header("Restart Settings")]
    public float respawnDelay = 2f;

    [Header("Effects")]
    public GameObject explosionEffect;
    public SpriteRenderer spaceshipRenderer;

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
        UpdateLivesUI();

        if (spaceshipRenderer != null)
            StartCoroutine(Blink());

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

    private System.Collections.IEnumerator Blink()
    {
        float blinkDuration = 0.5f;
        float blinkInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            spaceshipRenderer.enabled = !spaceshipRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spaceshipRenderer.enabled = true;
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(respawnDelay);

        // Reload scene, then PortalExitManager will detect the new spaceship
        SceneManager.sceneLoaded += OnSceneReloaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnSceneReloaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneReloaded;
        // Notify PortalExitManager
        if (PortalExitManager.Instance != null)
            PortalExitManager.Instance.OnSpaceshipRespawn();
    }
}



