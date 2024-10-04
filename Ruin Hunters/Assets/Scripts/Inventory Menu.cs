using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class InventoryMenu : MonoBehaviour
{
    //To pause the screen
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuWeapon;
    [SerializeField] GameObject menuAmour;
    [SerializeField] GameObject menuAccessory;
    public bool Ispause;
    float scaleorginaltime;
    public static InventoryMenu Instance;


    void Start()
    {
        scaleorginaltime = Time.timeScale;
        Instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                startPause();
                menuActive = menuPause;
                menuPause.SetActive(Ispause);
            }
            else if (menuActive == menuPause)
            {
                startunPause();
            }

        }
    }

    public void startPause()
    {
        Ispause = !Ispause;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void startunPause()
    {
        Ispause = !Ispause;
        Time.timeScale = scaleorginaltime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(Ispause);
        menuActive = null;
    }

    public void Weaponmenu()
    {
        menuWeapon.SetActive(true);
        menuPause.SetActive(false);
    }

    public void Amourmenu()
    {
        menuAmour.SetActive(true);
        menuPause.SetActive(false);
    }

    public void Accessoryymenu()
    {
        menuAccessory.SetActive(true);
        menuPause.SetActive(false);
    }

    public void returnToPartyInfo()
    {
        menuAccessory.SetActive(false);
        menuAmour.SetActive(false);
        menuWeapon.SetActive(false);
        menuPause.SetActive(true);
    }


}
