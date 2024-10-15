using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void Restar()
    {
        // Load the previous save state
       // SaveSystem.LoadPlayer();
        gameObject.SetActive(false); // Hide the death menu
    }

    public void Quit()
    {
        // Quit the game
        Application.Quit();
    }
}
