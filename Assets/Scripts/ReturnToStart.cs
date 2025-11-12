using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    void OnMouseDown()
    {
        // Optional: add a sound or animation here
        SceneManager.LoadScene("ButtonMenu"); // Make sure this matches your start menu scene name
    }
}

