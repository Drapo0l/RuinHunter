using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public static WinMenu instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject winMenuUI; // Assign this in the Inspector
    public Button mainMenuButton;
    public Button quitButton;
    public GameObject finalBoss;

    private void Start()
    {
        winMenuUI.SetActive(false); // Ensure the menu is initially disabled
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(QuitGame);
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