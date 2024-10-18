using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public GameObject winMenuUI; // Assign this in the Inspector
    public Button mainMenuButton;
    public Button quitButton;
    private GameObject finalBoss;

    private void Start()
    {
        winMenuUI.SetActive(false); // Ensure the menu is initially disabled
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(QuitGame);

        // Find the final boss in the scene
        finalBoss = GameObject.FindGameObjectWithTag("FinalBoss");
    }

    private void Update()
    {
        // Check if the final boss is defeated
        if (finalBoss == null)
        {
            ShowWinMenu();
        }
    }

    public void ShowWinMenu()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    void GoToMainMenu()
    {
        // Load the main menu scene
        // Example: SceneManager.LoadScene("MainMenu");
    }

    void QuitGame()
    {
        Application.Quit(); // Quit the game
    }
}