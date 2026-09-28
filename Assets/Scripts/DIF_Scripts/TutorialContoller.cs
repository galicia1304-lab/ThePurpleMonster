using UnityEngine;
using System.Collections;

public class TutorialContoller : MonoBehaviour
{
    [Header("Tutorial")]
    public GameObject tutorialPanel;
    public GameObject[] tutorialPages;

[Header("Animation")]
    public float targetScale = 117f;
    public float openDuration = 0.3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Tutorial Button Sounds")]
    public AudioClip tutorialHoverSound;
    public AudioClip tutorialPressSound;

    [Header("Close Button Sounds")]
    public AudioClip closeHoverSound;
    public AudioClip closePressSound;

    [Header("Next Button Sounds")]
    public AudioClip nextHoverSound;
    public AudioClip nextPressSound;

    private int currentPage = 0;

    private void Start()
    {
        tutorialPanel.SetActive(false);
        ShowPage(0);
    }

    public void OpenTutorial()
    {
        PlaySound(clickSound);

        currentPage = 0;

        tutorialPanel.SetActive(true);
        ShowPage(0);

        tutorialPanel.transform.localScale = Vector3.zero;

        StopAllCoroutines();
        StartCoroutine(ScaleOpen());
    }

    public void NextPage()
    {
        Debug.Log("NEXT PRESSED");

        PlaySound(clickSound);

        if (currentPage < tutorialPages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
        else
        {
            CloseTutorial();
        }
    }

    public void CloseTutorial()
    {
        Debug.Log("CLOSE PRESSED");

        PlaySound(clickSound);

        tutorialPanel.SetActive(false);
    }

    // =========================
    // HOVER SOUNDS
    // =========================

    public void TutorialHover()
    {
        PlaySound(tutorialHoverSound);
    }

    public void CloseHover()
    {
        PlaySound(closeHoverSound);
    }

    public void NextHover()
    {
        PlaySound(nextHoverSound);
    }

    // =========================
    // PRESS SOUNDS
    // =========================

    public void TutorialPress()
    {
        PlaySound(tutorialPressSound);
    }

    public void ClosePress()
    {
        PlaySound(closePressSound);
    }

    public void NextPress()
    {
        PlaySound(nextPressSound);
    }

    // =========================
    // EXISTING CODE
    // =========================

    private void ShowPage(int pageIndex)
    {
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].SetActive(i == pageIndex);
        }
    }

    private IEnumerator ScaleOpen()
    {
        float elapsed = 0f;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one * targetScale;

        while (elapsed < openDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / openDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            tutorialPanel.transform.localScale =
                Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        tutorialPanel.transform.localScale = endScale;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
