
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler
{
    [Header("Scene")]
    public string mainMenuScene = "MainMenu";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Settings")]
    public bool playHoverSound = true;
    public bool playClickSound = true;
    public bool waitForClickSound = true;

    // Call this from Button -> On Click()
    public void GoToMainMenu()
    {
        if (waitForClickSound && clickSound != null)
        {
            StartCoroutine(PlayClickSoundThenLoad());
        }
        else
        {
            PlayClickSound();
            LoadMainMenu();
        }
    }

    private IEnumerator PlayClickSoundThenLoad()
    {
        // Play the click sound
        if (playClickSound && audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);

            // Wait until the sound has finished
            yield return new WaitForSeconds(clickSound.length);
        }

        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    // Called automatically when the mouse moves over the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    // You can also call this from Event Trigger -> Pointer Enter
    public void PlayHoverSound()
    {
        if (playHoverSound && audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    // You can also call this manually from Button -> On Click()
    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}

