using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int heartCount = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoseLife()
    {
        heartCount--;
        Debug.Log("Player hit! Lives left: " + heartCount);

        if (heartCount <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");

        // Option 1: Reload the first scene by index (0)
        SceneManager.LoadScene(0);

        // Option 2: Reload by name
        // SceneManager.LoadScene("MainScene");

        // Reset lives so player starts fresh
        heartCount = 3;
    }
}



