using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEntry2D : MonoBehaviour
{
    [Header("Next Scene Name")]
    public string nextSceneName = "SpaceWorldMinigame";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Save portal position for next scene
            PlayerPrefs.SetFloat("PortalExitX", transform.position.x);
            PlayerPrefs.SetFloat("PortalExitY", transform.position.y);
            PlayerPrefs.SetFloat("PortalExitZ", transform.position.z);
            PlayerPrefs.Save();

            Debug.Log("Player entered 2D portal — loading " + nextSceneName);
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }
}

