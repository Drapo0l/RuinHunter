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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !QuitRestartMenu.activeSelf)
        {
         
            OpenQuitMenu();
        }
    }

    public void OpenQuitMenu()
    {
        isQuit = true;
        QuitRestartMenu.SetActive(true);
        Time.timeScale = 0;    }

  
}
