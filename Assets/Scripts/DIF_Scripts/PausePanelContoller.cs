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

    private Vector3 menuNormalScale;

    private void Start()
    {
        // Remember the scale set in the Inspector
        menuNormalScale = menuPanel.localScale;

        // Remember the menu's open position
        menuCenterPosition = menuPanel.anchoredPosition;

        // Remember the pause button's position
        pauseButtonPosition = pauseButton.anchoredPosition;

        // Start with the menu hidden at the pause button
        menuPanel.anchoredPosition = pauseButtonPosition;
        menuPanel.localScale = Vector3.zero;

        infoPanel.SetActive(false);

        // Game starts normally
        Time.timeScale = 1f;
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

    // OPEN PAUSE MENU
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

    // CLOSE PAUSE MENU AND RESUME GAME
    public void CloseMenu()
    {
        if (isAnimating)
            return;

        isOpen = false;
        isAnimating = true;

        StartCoroutine(AnimateClose(true));
    }

    // CLOSE PAUSE MENU BUT KEEP GAME PAUSED
    // Used when opening Settings
    public void CloseMenuForSettings()
    {
        if (isAnimating)
            return;

        isOpen = false;
        isAnimating = true;

        StartCoroutine(AnimateClose(false));
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
        menuPanel.localScale = menuNormalScale;

        isAnimating = false;
    }

    private IEnumerator AnimateClose(bool resumeGame)
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

        if (resumeGame)
        {
            // Continue playing
            Time.timeScale = 1f;
        }
        else
        {
            // Going to settings, so remain paused
            Time.timeScale = 0f;
        }

        isAnimating = false;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}