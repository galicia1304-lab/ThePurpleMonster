using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene switching

public class BossHealth : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("End Scene")]
    public string endSceneName = "EndScene"; // Replace with your actual scene name

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this when the boss takes damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Boss health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        // Optional: play death animation or sound before switching scenes
        SceneManager.LoadScene(endSceneName);
    }
}

