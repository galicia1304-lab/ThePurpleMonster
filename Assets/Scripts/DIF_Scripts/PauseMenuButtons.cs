using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    public InfoPanelController pauseMenuController;

    [Header("Settings")]
    public GameObject settingsMenu;

    [Header("Main Menu")]
    public string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        // Settings starts hidden
        settingsMenu.SetActive(false);

        // Game starts playing
        Time.timeScale = 1f;
    }

    // CONTINUE BUTTON
    public void ContinueGame()
    {
        // Slide the pause menu back to the pause button
        // and resume the game.
        pauseMenuController.CloseMenu();
    }

    // RETRY BUTTON
    public void RetryGame()
    {
        Time.timeScale = 1f;

        // Restart the current scene
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // SETTINGS BUTTON
    public void OpenSettings()
    {
        // Slide pause menu back to the pause button
        // without unpausing the game.
        pauseMenuController.CloseMenuForSettings();

        // Show settings
        settingsMenu.SetActive(true);

        // Keep the game paused
        Time.timeScale = 0f;
    }

    // CLOSE SETTINGS BUTTON
    public void CloseSettings()
    {
        // Hide settings
        settingsMenu.SetActive(false);

        // Show pause menu again
        pauseMenuController.OpenMenu();

        // Keep game paused
        Time.timeScale = 0f;
    }

    // QUIT BUTTON
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;

        // Load main menu
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
