using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private GameObject deathMenuUI; // Reference to the Death Menu UI

    private void Start()
    {
        deathMenuUI.SetActive(false); // Ensure the menu is hidden at the start
    }

    public void ShowDeathMenu()
    {
        deathMenuUI.SetActive(true); // Show the menu
        Time.timeScale = 0f; // Pause the game
    }

    public void Respawn()
    {
        deathMenuUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f; // Resume the game
        // Call respawn logic here, e.g., RespawnManager.Instance.RespawnPlayer(player);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Resume the game before quitting
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
    }
}