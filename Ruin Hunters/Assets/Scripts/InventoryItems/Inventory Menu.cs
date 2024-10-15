using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    //To pause the screen
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuWeapon;
    [SerializeField] GameObject menuAmour;
    [SerializeField] GameObject menuAccessory;
    [SerializeField] GameObject menuPanel;
    public GameObject CheckPointPanel;

    public List<PartyManager> playerMems = new List<PartyManager>();

    public bool Ispause;
    float scaleorginaltime;
    public static InventoryMenu Instance;
    playerController player;
    public List<GameObject> battleParty = new List<GameObject>();
    [SerializeField]
    private GameObject DefaultButton;
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
        if (!GameManager.Instance.leveling && !GameManager.Instance.combat)
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

    public void returnToPartyInfo()  // turns off the other menus and goes back to the party info menu once you press the back button
    {
        menuAccessory.SetActive(false);
        menuAmour.SetActive(false);
        menuWeapon.SetActive(false);
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(DefaultButton, new BaseEventData(eventSystem));
        //menuPause.SetActive(true);
    }

   //private void AddPartyMembers()
   // {
   //     int index = 0;
   //     foreach (PartyManager player in playerMems)
   //     {
   //         GameObject menuPanel = menuPause.transform.GetChild(index).gameObject;
   //         CharacterAttributes characterAttributes = player.GetComponent<playerController>().playerStats;
   //         //sprite
   //         menuPanel.transform.GetChild(0).GetComponent<Image>().sprite = player.GetComponent<playerController>().Sprite;
   //         //name
   //         menuPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = characterAttributes.nameOfCharacter;
   //              //lvl
   //         menuPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "lvl: " + characterAttributes.level.ToString();
   //         //hp
   //         menuPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "hp: " + characterAttributes.maxHealthOG.ToString() + " -> " + characterAttributes.maxHealth.ToString();
   //                //mana
   //         menuPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "sp: " + characterAttributes.maxManaOG.ToString() + " -> " + characterAttributes.maxMana.ToString();
   //                //def
   //         menuPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "def: " + characterAttributes.DefenceOG.ToString() + " -> " + characterAttributes.Defence.ToString();
   //           //atk
   //         menuPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "atk: " + characterAttributes.attackDamage.ToString();

   //     }
   // }

    
}

    //private void ShowLevelUpScreen()
    //{
    //    {
    //        menuPause.SetActive(true);

    //        int child = 0;
    //        foreach (GameObject player in playerLeveled)
    //        {
    //            player.GetComponent<playerController>().actionSelector.HideMenu();
    //            player.GetComponent<SphereCollider>().enabled = true;
    //            levelUpScreen.transform.GetChild(child).gameObject.SetActive(true);
    //            GameObject playerPanel = levelUpScreen.transform.GetChild(child).gameObject;
    //            CharacterAttributes characterAttributes = player.GetComponent<playerController>().playerStats;
    //            //sprite
    //            playerPanel.transform.GetChild(0).GetComponent<Image>().sprite = player.GetComponent<playerController>().Sprite;
    //            //name
    //            playerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = characterAttributes.nameOfCharacter;
    //            //xp
    //            playerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "xp: " + characterAttributes.currentXP.ToString() + "/" + characterAttributes.xpToNextLevel.ToString();
    //            //lvl
    //            playerPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "lvl: " + characterAttributes.level.ToString();
    //            //hp
    //            playerPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "hp: " + characterAttributes.maxHealthOG.ToString() + " -> " + characterAttributes.maxHealth.ToString();
    //            //mana
    //            playerPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "sp: " + characterAttributes.maxManaOG.ToString() + " -> " + characterAttributes.maxMana.ToString();
    //            //def
    //            playerPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "def: " + characterAttributes.DefenceOG.ToString() + " -> " + characterAttributes.Defence.ToString();
    //            //atk
    //            playerPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "atk: " + characterAttributes.attackDamageOG.ToString() + " -> " + characterAttributes.attackDamage.ToString();
    //            //magic atk
    //            playerPanel.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = "matk: " + characterAttributes.skillDamageOG.ToString() + " -> " + characterAttributes.skillDamage.ToString();
    //            //combat speed
    //            playerPanel.transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = "speed: " + characterAttributes.combatSpeedOG.ToString() + " -> " + characterAttributes.combatSpeed.ToString();
    //            //crit
    //            playerPanel.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = "crit: " + characterAttributes.critChanceOG.ToString() + " -> " + characterAttributes.critChance.ToString();
    //            child++;
    //        }
    //    }
    //}
