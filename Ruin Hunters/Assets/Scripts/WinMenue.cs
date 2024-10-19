using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WinMenu : MonoBehaviour
{
    public static WinMenu instance;
    public GameObject winMenuUI; // Assign this in the Inspector
   // public Button restartButton;
    public Button quitButton;

    private int currentSelection = 0;
    private Button[] buttons;
    private EventSystem eventSystem;

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

    private void Start()
    {
        winMenuUI.SetActive(false); // Ensure the menu is initially disabled
       // restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);

        buttons = new Button[] { /*restartButton*/ quitButton };
        eventSystem = EventSystem.current; // Get the current EventSystem
    }

    private void Update()
    {
        if (winMenuUI.activeSelf)
        {
            HandleNavigation();
        }
    }

    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed: invoking button click");
            buttons[currentSelection].onClick.Invoke();
        }
    }

    private void Navigate(int direction)
    {
        currentSelection += direction;
        if (currentSelection < 0) currentSelection = buttons.Length - 1;
        if (currentSelection >= buttons.Length) currentSelection = 0;

        // Highlight the selected button
        eventSystem.SetSelectedGameObject(buttons[currentSelection].gameObject);
    }

    public void ShowWinMenu()
    {
        winMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game

        // Highlight the first button by default
        eventSystem.SetSelectedGameObject(buttons[currentSelection].gameObject);
    }

    //void RestartGame()
    //{
    //    Debug.Log("Restart Game button clicked");
    //    Time.timeScale = 1f; // Unpause the game
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene
    //    winMenuUI.SetActive(false);

    //}

    void QuitGame()
    {
        Debug.Log("Quit Game button clicked");
        Application.Quit(); // Quit the game

        // If we're in the Editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}