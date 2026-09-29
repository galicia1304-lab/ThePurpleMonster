using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip clickSound;
    public AudioClip hoverSound;

    // Add this to the Button's On Click()
    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // Add this to Event Trigger -> Pointer Enter
    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
