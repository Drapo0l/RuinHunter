using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class QuitMenu : MonoBehaviour
{
    public GameObject QuitRestartMenu;
    public Button QuitButton;
    private bool isQuit;


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !QuitRestartMenu.activeSelf)
        {
         
            OpenQuitMenu();
        }
        else if (Input.GetKeyDown(KeyCode.T) && QuitRestartMenu.activeSelf)
        {
            isQuit = false;
            QuitRestartMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    

    public void OpenQuitMenu()
    {
        isQuit = true;
        QuitRestartMenu.SetActive(true);
        Time.timeScale = 0;    }

  
}
