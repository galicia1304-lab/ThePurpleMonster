using UnityEngine;
using System.Collections;

public class InfoPanelController : MonoBehaviour
{
    public GameObject infoPanel;
    public RectTransform pauseButton;
    public RectTransform menuPanel;

    [Header("Animation")]
    public float animationDuration = 0.4f;

    private bool isOpen = false;
    private bool isAnimating = false;

    private Vector2 menuCenterPosition;
    private Vector2 pauseButtonPosition;

    // This remembers your scale from the Inspector
    private Vector3 menuNormalScale;

    private void Start()
    {
        // Remember the scale you set in Unity.
        // In your case this will be (90, 90, 90).
        menuNormalScale = menuPanel.localScale;

        // Remember where the menu should be when fully open
        menuCenterPosition = menuPanel.anchoredPosition;

        // Remember the pause button position
        pauseButtonPosition = pauseButton.anchoredPosition;

        // Start the menu at the pause button
        menuPanel.anchoredPosition = pauseButtonPosition;
        menuPanel.localScale = Vector3.zero;

        infoPanel.SetActive(false);
    }

    public void ToggleInfo()
    {
        if (isAnimating)
            return;

        if (isOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        if (isAnimating)
            return;

        isOpen = true;
        isAnimating = true;

        infoPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;

        StartCoroutine(AnimateOpen());
    }

    public void CloseMenu()
    {
        if (isAnimating)
            return;

        isOpen = false;
        isAnimating = true;

        StartCoroutine(AnimateClose());
    }

    private IEnumerator AnimateOpen()
    {
        float elapsed = 0f;

        Vector2 startPosition = pauseButtonPosition;
        Vector2 endPosition = menuCenterPosition;

        Vector3 startScale = Vector3.zero;
        Vector3 endScale = menuNormalScale;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / animationDuration;

            // Smooth animation
            t = Mathf.SmoothStep(0f, 1f, t);

            menuPanel.anchoredPosition = Vector2.Lerp(
                startPosition,
                endPosition,
                t
            );

            menuPanel.localScale = Vector3.Lerp(
                startScale,
                endScale,
                t
            );

            yield return null;
        }

        // Make sure we finish at the exact values
        menuPanel.anchoredPosition = endPosition;
        menuPanel.localScale = menuNormalScale;

        isAnimating = false;
    }

    private IEnumerator AnimateClose()
    {
        float elapsed = 0f;

        Vector2 startPosition = menuCenterPosition;
        Vector2 endPosition = pauseButtonPosition;

        Vector3 startScale = menuNormalScale;
        Vector3 endScale = Vector3.zero;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / animationDuration;

            // Smooth animation
            t = Mathf.SmoothStep(0f, 1f, t);

            menuPanel.anchoredPosition = Vector2.Lerp(
                startPosition,
                endPosition,
                t
            );

            menuPanel.localScale = Vector3.Lerp(
                startScale,
                endScale,
                t
            );

            yield return null;
        }

        menuPanel.anchoredPosition = endPosition;
        menuPanel.localScale = Vector3.zero;

        infoPanel.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;

        isAnimating = false;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}