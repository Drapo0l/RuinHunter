using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class InventoryMenu : MonoBehaviour
{
    //To pause the screen
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuWeapon;
    [SerializeField] GameObject menuAmour;
    [SerializeField] GameObject menuAccessory;
    [SerializeField] GameObject menuItem;
    [SerializeField] GameObject menuSkills;
    [SerializeField] GameObject menuPanel;


    public bool Ispause;
    float scaleorginaltime;
    public static InventoryMenu Instance;

    //Show Player's stats
    public List<GameObject> playerHealths;          // list of player health
    public TMP_Text HP_text;
    public List<GameObject> playerMana;          // list of player Mana
    public TMP_Text Mana_text;
    public List<GameObject> playerStrength;          // list of player Strentgh
    public TMP_Text Strength_text;
    public List<GameObject> playerSpeed;          // list of player speed
    public TMP_Text Speed_text;
    public List<GameObject> playerDefense;          // list of player Defense
    public TMP_Text Defense_text;


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

    public void Weaponmenu() // turns off the Party Info menu and switches and turns on the weapon menu
    {
        menuWeapon.SetActive(true);
        //menuPause.SetActive(false);
    }

    public void Amourmenu() // turns off the Party Info menu and switches and turns on the Amour menu
    {
        menuAmour.SetActive(true);
        //menuPause.SetActive(false);
    }

    public void Accessoryymenu() // turns off the Party Info menu and switches and turns on the Accessory menu
    {
        menuAccessory.SetActive(true);
        //menuPause.SetActive(false);
    }

    public void ItemMenu() // turns off the Party Info menu and switches and turns on the weapon menu
    {
        menuItem.SetActive(true);
        //menuPause.SetActive(false);
    }

    public void Skillmenu() // turns off the Party Info menu and switches and turns on the weapon menu
    {
        menuSkills.SetActive(true);
        //menuPause.SetActive(false);
    }
    public void returnToPartyInfo()  // turns off the other menus and goes back to the party info menu once you press the back button
    {
        menuAccessory.SetActive(false);
        menuAmour.SetActive(false);
        menuWeapon.SetActive(false);
        menuSkills.SetActive(false);
        menuItem.SetActive(false);
        // menuPause.SetActive(true);
    }

    private void ShowHealth()
    {
        List<GameObject> playerParty = PartyManager.Instance.GetPlayeGameObj();
        int index = 0;
        foreach (var playerChar in playerParty)
        {
            playerChar.transform.GetChild(0).gameObject.SetActive(true);
            //HealthNumber
            playerHealths[index].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = playerChar.GetComponent<playerController>().playerStats.health.ToString() + " / " + playerChar.GetComponent<playerController>().playerStats.maxHealth.ToString();

            index++;
        }
    }


}
