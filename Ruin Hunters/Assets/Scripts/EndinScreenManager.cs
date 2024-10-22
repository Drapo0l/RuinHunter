using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingScreenManager : MonoBehaviour
{
    public TMP_Text xpText;
    public TMP_Text goldText;
    public TMP_Text levelsText;
    public TMP_Text creditsText;
    public TMP_Text congratulationsText;
    public Button restartButton;
    public Button quitButton;

    private void Start()
    {
        // Populate XP and Gold
        PopulateEndingScreen();

        // Add listeners to buttons
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PopulateEndingScreen()
    {
        GameManager gameManager = GameManager.Instance;

        // Populate XP and Gold
        xpText.text = "Total XP: " + gameManager.expTotal;
        goldText.text = "Total Gold: " + gameManager.totalGold;

        // Populate Party Member Levels
        string levels = "";
        foreach (var member in gameManager.playerParty)
        {
            levels += member.nameOfCharacter + " - Level " + member.level + "\n"; // Ensure the correct property name is used
        }
        levelsText.text = levels;

        // Populate Credits
        creditsText.text = "Game Credits\n\n" +
                       "Developers:\n" +
                       "1. [Developer Name 1]\n" +
                       "2. [Developer Name 2]\n" +
                       "3. [Developer Name 3]\n" +
                       "4. [Developer Name 4]\n\n" +
                       "Art:\n" +
                       "Unity-chan\n";

        // Populate Congratulations Message
        congratulationsText.text = "Congratulations on completing the game!";
    }

    private void RestartGame()
    {
        // Hide the ending screen UI
        GameManager.Instance.endingScreenUI.SetActive(false);
    
            Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene("Map");
    }

    private void QuitGame()
    {
        Application.Quit(); // Quit the game

        // If we're in the Editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}