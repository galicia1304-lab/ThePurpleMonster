using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSource;

    public AudioClip clickSound;
    public AudioClip hoverSound;

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(hoverSound);
    }
}