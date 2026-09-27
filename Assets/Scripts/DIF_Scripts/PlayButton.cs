using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public AudioSource playSound;
    public AudioSource hoverSound;

    public string nextSceneName = "GameScene";

    public void PlayGame()
    {
        StartCoroutine(PlaySoundThenLoad());
    }

    IEnumerator PlaySoundThenLoad()
    {
        playSound.Play();

        // Wait until the play sound finishes
        yield return new WaitWhile(() => playSound.isPlaying);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    public void HoverSound()
    {
        hoverSound.Play();
    }
}
