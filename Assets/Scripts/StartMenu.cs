using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnMouseDown()
    {
        SceneManager.LoadScene("ShootingFlower1"); // Use the exact name of your next scene
    }
}

