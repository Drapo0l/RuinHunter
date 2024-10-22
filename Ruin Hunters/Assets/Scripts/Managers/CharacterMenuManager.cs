using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//using static UnityEditor.Progress;


public class CharacterMenuManager : MonoBehaviour
{
    public static CharacterMenuManager Instance;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private List<WeaponItem> weaponItems;
    private List<EquipmentItem> equipmentItems;
    private List<Skill> playerSkills;

    private Stack<List<Button>> menuStack = new Stack<List<Button>>(); // Stack for managing menus
    private List<Button> equipmentButtons = new List<Button>();
    private List<Button> weaponButtons = new List<Button>();
    private List<Button> skillsButtons = new List<Button>();
    private List<Button> itemsButtons = new List<Button>();
    private List<Button> currentMenu;
    private List<Button> leftMenu = new List<Button>();
    private List<Button> rightMenu = new List<Button>();
    private List<Button> chooseMenu;
    public List<Button> subMenuButtons;
    public List<Button> playerButtons;

    public List<GameObject> stats;
    private List<GameObject> playerParty;
    public GameObject weaponStats;
    public GameObject equipmentStats;
    public GameObject itemStats;
    public GameObject skillStats;
    public GameObject choosePlayer;
    public GameObject playerRevivedAlive;

    public GameObject cursor;
    public GameObject statMenu;
    public GameObject subMenuParent;

    public Color defaultColor = Color.white;
    public Color equippedColor = Color.green;

    public bool inUI;
    private bool inMenu;
    private bool equipmentMenu;
    private bool weaponMenu;
    private bool skillMenu;
    private bool ItemMenu;
    private bool choosingPlayer;
    private int itemSelection;
    private int weaponScrollIndex;
    private int equipmentScrollIndex;
    private int skillScrollIndex;
    private int playerScrollIndex;
    private int itemScrollIndex;

    int currentSelection;

    [Header("Audio")]
    [SerializeField] AudioSource Aud;
    [SerializeField] AudioClip AudButtonPressed;
    [SerializeField] float ButtonPressedVol;
    [SerializeField] AudioClip AudClosemenu;
    [SerializeField] float ClosemenuVol;
    [SerializeField] AudioClip AudEquip;
    [SerializeField] float EquipVol;
    [SerializeField] AudioClip AudOpenMenu;
    [SerializeField] float OpenMenuVol;
    [SerializeField] AudioClip ButtonDownAud;
    [SerializeField] float ButtonDownVol;

   private bool isPlaying;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && statMenu.activeSelf)
        {
          
            cursor.transform.SetParent(statMenu.transform);
            weaponStats.SetActive(false);
            equipmentStats.SetActive(false);
            subMenuParent.SetActive(false);
            statMenu.SetActive(false);
            itemStats.SetActive(false);
            skillStats.SetActive(false);
            inUI = false;
            inMenu = false;
            equipmentMenu = false;
            weaponMenu = false;
            skillMenu = false;
            ItemMenu = false; 
            choosingPlayer = false;
            skillScrollIndex = 0;
            itemSelection = 0;
            weaponScrollIndex = 0;
            equipmentScrollIndex = 0;
            playerScrollIndex = 0;
            currentSelection = 0;
            leftMenu.Clear();
            rightMenu.Clear();
            itemsButtons.Clear();
            skillsButtons.Clear();
            weaponButtons.Clear();
            equipmentButtons.Clear();
            if (!isPlaying) Aud.PlayOneShot(AudClosemenu, ClosemenuVol);
        }
        else if (!GameManager.Instance.combat && !GameManager.Instance.leveling && statMenu.activeSelf && !weaponMenu && !equipmentMenu && !ItemMenu && !skillMenu) 
        {
            if (Input.GetKeyDown(KeyCode.W)) { Navigate(-1) ;}
            if (Input.GetKeyDown(KeyCode.S)) { Navigate(1);}
            if (Input.GetKeyDown(KeyCode.A)) {ChangeMenu();}
            if (Input.GetKeyDown(KeyCode.D)) { ChangeMenu();}

            if (Input.GetKeyDown(KeyCode.Return)) 
            { 
                if(currentMenu == leftMenu)
                {
                    if(currentSelection % 2 == 1)
                        OpenWeaponMenu();
                    else
                        PopulateItemMenu();
                }
                else
                {
                    if (currentSelection % 2 == 1)
                        OpenEquipmentMenu();
                    else
                        PopulateSkillMenu();

                }
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if(equipmentMenu && statMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateEquipmentItems(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateEquipmentItems(1); }
            if (Input.GetKeyDown(KeyCode.Return)) { EquipmArmor(); }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if(weaponMenu && statMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateWeaponItems(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateWeaponItems(1); }
            if (Input.GetKeyDown(KeyCode.Return)) { EquipWeapon(); }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if (choosingPlayer && statMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateParty(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateParty(1); }
            if (Input.GetKeyDown(KeyCode.Return)) 
            { 
                if(ItemMenu)
                    StartCoroutine(UseItem()); 
                else
                    StartCoroutine(UseSkill());
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspaceSmallerMenu(); }
        }
        else if(ItemMenu && statMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateItems(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateItems(1); }
            if (Input.GetKeyDown(KeyCode.Return)) { CheckForItemUse(); }


            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if(skillMenu && statMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateSkills(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateSkills(1); }
            if (Input.GetKeyDown(KeyCode.Return)) { CheckForSkillUse(); }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }        
        else if (!GameManager.Instance.combat && !GameManager.Instance.leveling && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu();
            
        }
    }

    private void ShowMenu()
    {
        inUI = true;
        inMenu = true;
        statMenu.SetActive(true);
        playerParty = PartyManager.Instance.GetPlayeGameObj();

        weaponItems = InventoryManager.instance.GetWeaponItems();
        equipmentItems = InventoryManager.instance.GetEquipmentItems();

        ShowStats();
        AssignButtonActions(weaponButtons, OpenWeaponMenu);
        AssignButtonActions(equipmentButtons, OpenEquipmentMenu);
        AssignButtonActions(itemsButtons, OpenWeaponMenu);
        AssignButtonActions(skillsButtons, OpenEquipmentMenu);
        if(leftMenu.Count == 0)
        {
            int index = 0;
            foreach (var button in itemsButtons)
            {
                leftMenu.Add(button);
                leftMenu.Add(weaponButtons[index]);
                index++;
            }
            index = 0;
            foreach (var button in skillsButtons)
            {
                rightMenu.Add(button);
                rightMenu.Add(equipmentButtons[index]);
                index++;
            }
            currentMenu = leftMenu;
        }               
        menuStack.Push(weaponButtons);
        UpdateHoverIndicator();
    }

    private void ChangeMenu()
    {
        if(currentMenu == leftMenu)
        {
            currentMenu = rightMenu;
            UpdateHoverIndicator();
        }
        else
        {
            currentMenu = leftMenu;
            UpdateHoverIndicator();
        }
    }

    private void ExecuteCurrentAction()
    {
        // Trigger the onClick event for the currently selected button
        Aud.PlayOneShot(AudButtonPressed, ButtonPressedVol);
        currentMenu[currentSelection].onClick.Invoke();
       
    }

    private void Navigate(int direction)
    {
     
        playerScrollIndex += direction;
        currentSelection += direction;
        if (currentSelection < 0)
        {
            Aud.PlayOneShot(AudButtonPressed, ButtonPressedVol);
            currentSelection = currentMenu.Count - 1; // Loop to end
            playerScrollIndex = currentMenu.Count - 1; // Loop to end
        }
        if (currentSelection >= playerParty.Count * 2)
        {
            Aud.PlayOneShot(AudButtonPressed, ButtonPressedVol);
            currentSelection = 0;      // Loop to start
            playerScrollIndex = 0;      // Loop to start
        }

        UpdateHoverIndicator();
    }

    private void NavigateParty(int direction)
    {
        playerScrollIndex += direction;
        currentSelection += direction;
        if (currentSelection < 0)
        {
            currentSelection = playerParty.Count - 1; // Loop to end
        }
        if (currentSelection >= playerParty.Count)
        {
            currentSelection = 0;      // Loop to start
        }

        UpdateHoverIndicator();
    }

    private void UpdateHoverIndicator()
    {
        Vector3 buttonPosition = Vector3.zero;
        if (choosingPlayer)
        {
            if (currentSelection < 0)
            {
                currentSelection = playerParty.Count - 1; 
            }
            else if (currentSelection >= playerParty.Count)
            {
                currentSelection = 0; 
            }

            int visibleIndex = currentSelection % currentMenu.Count;

            buttonPosition = currentMenu[visibleIndex].transform.position;
            
        }
        else if (inMenu)
        {
            int visibleIndex = currentSelection - itemSelection;

            if (visibleIndex >= 0 && visibleIndex < currentMenu.Count)
            {
                buttonPosition = currentMenu[visibleIndex].transform.position;
            }
        }
        else
        {
            if (currentSelection > currentMenu.Count - 1)
            { currentSelection = currentMenu.Count - 1; }
            buttonPosition = currentMenu[currentSelection].transform.position;
        }
        if (weaponMenu || equipmentMenu || skillMenu || ItemMenu)
        {
            cursor.transform.position = new Vector3(buttonPosition.x - 300f, buttonPosition.y, buttonPosition.z);
        }
        else
        {
            cursor.transform.position = new Vector3(buttonPosition.x - 150f, buttonPosition.y, buttonPosition.z);
        }
            
    }
    
    private void AssignButtonActions(List<Button> buttons, params UnityAction[] actions)
    {
        for (int i = 0; i < buttons.Count && i < actions.Length; i++)
        {
            buttons[i].onClick.AddListener(actions[i]);
 
        }
    }

    private void PopulateSkillMenu()
    {
        

        subMenuParent.SetActive(true);

        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        List<Skill> playerSkills = playerParty[floor].GetComponent<playerController>().playerStats.skills;
        this.playerSkills = playerSkills;

        //update buttons
        for (int i = 0; i < subMenuButtons.Count; i++)
        {
            int itemIndex = skillScrollIndex + i;

            if (itemIndex < playerSkills.Count)
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerSkills[itemIndex].skillName;
                subMenuButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }
        if (!skillMenu)
        {
            SwitchToMenu(subMenuButtons);
        }
        skillMenu = true;
        HideStats();
        skillStats.SetActive(true);
        UpdateHoverIndicator();
    }

    private void PopulateItemMenu()
    {
        

        subMenuParent.SetActive(true);

        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        List<Item> playerItems = InventoryManager.instance.GetItems();

        //update buttons
        for (int i = 0; i < subMenuButtons.Count; i++)
        {
            int itemIndex = itemScrollIndex + i;

            if (itemIndex < playerItems.Count)
            {
                if (playerItems[itemIndex] != null)
                {
                    subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[itemIndex].itemName;
                    subMenuButtons[i].interactable = true;
                    itemIndex++;
                }
            }
            else
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }
        if (!ItemMenu)
        {
            SwitchToMenu(subMenuButtons);
        }
        ItemMenu = true;
        itemStats.SetActive(true);
        HideStats();
        UpdateHoverIndicator();
    }

    private void NavigateSkills(int direction)
    {
        currentSelection += direction;
        skillScrollIndex += direction;

        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0) 
        {
            floor = 0;
        }
        List<Skill> playerSkills = playerParty[floor].GetComponent<playerController>().playerStats.skills;

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = playerSkills.Count - 1;
            skillScrollIndex = Mathf.Max(0, playerSkills.Count - 1);
            if (currentSelection > 6)
                PopulateSkillMenu();
        }
        else if (currentSelection >= playerSkills.Count)
        {
            //if player is at the bottom go to first skill
            currentSelection = 0;
            skillScrollIndex = 0;
            PopulateSkillMenu(); 
        }

        //check if need to scroll
        if (currentSelection < skillScrollIndex)
        {
            skillScrollIndex = Mathf.Max(0, skillScrollIndex - 1); //scroll up
            PopulateSkillMenu();
        }
        else if (currentSelection >= skillScrollIndex + 6)
        {
            skillScrollIndex = Mathf.Min(playerSkills.Count - 6, skillScrollIndex + 1); //scroll down
            //Aud.PlayOneShot(ButtonDownAud, ButtonDownVol);
            PopulateSkillMenu();
        }

        ShowSkillStats();
        UpdateHoverIndicator();
    }

    private void NavigateItems(int direction)
    {
        currentSelection += direction;
        itemScrollIndex += direction;

        List<Item> playerItems = InventoryManager.instance.GetItems();

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = playerItems.Count - 1;
            itemScrollIndex = Mathf.Max(0, playerItems.Count - 1);
            if (currentSelection > 6)
                PopulateItemMenu();
        }
        else if (currentSelection >= playerItems.Count)
        {
            //if player is at the bottom go to first skill
            currentSelection = 0;
            itemScrollIndex = 0;
            PopulateItemMenu();
        }

        //check if need to scroll
        if (currentSelection < itemScrollIndex)
        {
            itemScrollIndex = Mathf.Max(0, itemScrollIndex - 1); //scroll up
            PopulateItemMenu();
        }
        else if (currentSelection >= itemScrollIndex + 6)
        {
            itemScrollIndex = Mathf.Min(playerSkills.Count - 6, itemScrollIndex + 1); //scroll down
            PopulateItemMenu();
        }

        ShowItemsStats();
        UpdateHoverIndicator();
    }

    private void OpenWeaponMenu()
    {
        

        subMenuParent.SetActive(true);

        weaponStats.SetActive(true);

        cursor.transform.SetParent(subMenuParent.transform);

        List<WeaponItem> playerItems = new List<WeaponItem>(InventoryManager.instance.GetWeaponItems());
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        if (playerParty[floor].GetComponent<playerController>().playerStats.weapon != null)
            playerItems.Insert(0, playerParty[floor].GetComponent<playerController>().playerStats.weapon);

        weaponItems = playerItems;
        if (currentSelection > weaponItems.Count)
        {
            currentSelection = weaponItems.Count - 1;
        }
        if (weaponScrollIndex > weaponItems.Count)
        {
            weaponScrollIndex = weaponItems.Count - 1;
        }
        //update buttons
        for (int i = 0; i < subMenuButtons.Count; i++)
        {
            int itemIndex = weaponScrollIndex + i;

            if (itemIndex < playerItems.Count)
            {
                if (playerItems[itemIndex] != null)
                {
                    subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[itemIndex].itemName;
                    if (playerItems[i] == playerParty[floor].GetComponent<playerController>().playerStats.weapon)
                    {
                        subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.green;
                    }
                }
                else
                {
                    subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                }
                subMenuButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }
        if (!weaponMenu)
        {
            SwitchToMenu(subMenuButtons);
        }
        weaponMenu = true;
        ShowWeaponStats();
        HideStats();
        UpdateHoverIndicator();
    }

    private void OpenWeaponBottomMenu()
    {


        subMenuParent.SetActive(true);

        weaponStats.SetActive(true);

        cursor.transform.SetParent(subMenuParent.transform);

        List<WeaponItem> playerItems = new List<WeaponItem>(InventoryManager.instance.GetWeaponItems());
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        if (playerParty[floor].GetComponent<playerController>().playerStats.weapon != null)
            playerItems.Insert(0, playerParty[floor].GetComponent<playerController>().playerStats.weapon);

        weaponItems = playerItems;

        if (currentSelection > weaponItems.Count)
        {
            currentSelection = weaponItems.Count - 1;
        }
        if (weaponScrollIndex > weaponItems.Count)
        {
            weaponScrollIndex = weaponItems.Count - 1;
        }

        //update buttons
        for (int i = 0; i < subMenuButtons.Count; i--)
        {
            int itemIndex = weaponScrollIndex + i;

            if (itemIndex < playerItems.Count)
            {
                if (playerItems[itemIndex] != null)
                {
                    subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[itemIndex].itemName;
                    if (playerItems[i] == playerParty[floor].GetComponent<playerController>().playerStats.weapon)
                    {
                        subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.green;
                    }
                }
                else
                {
                    subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                }
                subMenuButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }
        if (!weaponMenu)
        {
            SwitchToMenu(subMenuButtons);
        }
        weaponMenu = true;
        ShowWeaponStats();
        HideStats();
        UpdateHoverIndicator();
    }

    private void OpenEquipmentMenu()
    {
        

        subMenuParent.SetActive(true);

        equipmentStats.SetActive(true);

        cursor.transform.SetParent(subMenuParent.transform);

        List<EquipmentItem> playerItems = new List<EquipmentItem>(InventoryManager.instance.GetEquipmentItems());
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        if (playerParty[floor].GetComponent<playerController>().playerStats.equipment != null)
            playerItems.Insert(0, playerParty[floor].GetComponent<playerController>().playerStats.equipment);

        equipmentItems = playerItems;
        if(currentSelection > equipmentItems.Count)
        {
            currentSelection = equipmentItems.Count - 1;
        }
        if (equipmentScrollIndex > equipmentItems.Count)
        {
            equipmentScrollIndex = equipmentItems.Count - 1;
        }
            //update buttons

        for (int i = 0; i < subMenuButtons.Count; i++)
        {
            int skillIndex = equipmentScrollIndex + i;

            if (skillIndex < playerItems.Count)
            {
                if (playerItems[skillIndex] != null)
                {
                    subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[skillIndex].itemName;
                    if (playerItems[i] == playerParty[floor].GetComponent<playerController>().playerStats.equipment)
                    {
                        subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.green;
                    }
                }
                else
                {
                    subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                }
                subMenuButtons[i].interactable = true;
                skillIndex++;
            }
            else
            {
                subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }
        

        if (!equipmentMenu)
        {
            SwitchToMenu(subMenuButtons);
        }
        equipmentMenu = true;
        ShowEquipmentStats();
        HideStats();
        UpdateHoverIndicator();
    }

    private void OpenEquipmentBottomMenu()
    {


        subMenuParent.SetActive(true);

        equipmentStats.SetActive(true);

        cursor.transform.SetParent(subMenuParent.transform);

        List<EquipmentItem> playerItems = new List<EquipmentItem>(InventoryManager.instance.GetEquipmentItems());
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        if (playerParty[floor].GetComponent<playerController>().playerStats.equipment != null)
            playerItems.Insert(0, playerParty[floor].GetComponent<playerController>().playerStats.equipment);
        //update buttons
        equipmentItems = playerItems;
        if (currentSelection > equipmentItems.Count)
        {
            currentSelection = equipmentItems.Count - 1;
        }
        if (equipmentScrollIndex > equipmentItems.Count)
        {
            equipmentScrollIndex = equipmentItems.Count - 1;
        }

        for (int i = 0; i < subMenuButtons.Count; i--)
        {
            int skillIndex = equipmentScrollIndex + i;

            if (skillIndex < playerItems.Count)
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[skillIndex].itemName;
                if (playerItems[i] == playerParty[floor].GetComponent<playerController>().playerStats.equipment)
                {
                    subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                }
                subMenuButtons[i].interactable = true;
                skillIndex++;
            }
            else
            {
                subMenuButtons[i].transform.parent.GetComponent<Image>().color = Color.grey;
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }          

        if (!equipmentMenu)
        {
            SwitchToMenu(subMenuButtons);
        }
        equipmentMenu = true;
        ShowEquipmentStats();
        HideStats();
        UpdateHoverIndicator();
    }

    private void NavigateEquipmentItems(int direction)
    {
        currentSelection += direction;
        equipmentScrollIndex += direction;

        List<EquipmentItem> playerItems = new List<EquipmentItem>(InventoryManager.instance.GetEquipmentItems());
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        if (playerParty[floor].GetComponent<playerController>().playerStats.equipment != null)
            playerItems.Insert(0, playerParty[floor].GetComponent<playerController>().playerStats.equipment);

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = playerItems.Count - 1;
            equipmentScrollIndex = Mathf.Max(0, playerItems.Count - 1);
            if(playerItems.Count > 6)
            { OpenEquipmentBottomMenu(); }
        }
        else if (currentSelection >= playerItems.Count)
        {
            //if player is at the bottom go to first skill
            currentSelection = 0;
            equipmentScrollIndex = 0;
            OpenEquipmentMenu();
        }

        //check if need to scroll
        if (currentSelection < equipmentScrollIndex)
        {
            equipmentScrollIndex = Mathf.Max(0, equipmentScrollIndex - 1); //scroll up
            OpenEquipmentMenu();
        }
        else if (currentSelection >= equipmentScrollIndex + 6)
        {
            equipmentScrollIndex = Mathf.Min(playerItems.Count - 6, equipmentScrollIndex + 1); //scroll down
            OpenEquipmentMenu();
        }

        ShowEquipmentStats();
        UpdateHoverIndicator();
    }

    private void NavigateWeaponItems(int direction)
    {
        currentSelection += direction;
        weaponScrollIndex += direction;

        List<WeaponItem> playerItems = new List<WeaponItem>(InventoryManager.instance.GetWeaponItems());
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        if (playerParty[floor].GetComponent<playerController>().playerStats.weapon != null)
            playerItems.Insert(0, playerParty[floor].GetComponent<playerController>().playerStats.weapon);

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = playerItems.Count - 1;
            weaponScrollIndex = Mathf.Max(0, playerItems.Count - 1);
            if (currentSelection > 6)
            { OpenWeaponBottomMenu(); }
        }
        else if (currentSelection >= playerItems.Count)
        {
            //if player is at the bottom go to first skill
            currentSelection = 0;
            weaponScrollIndex = 0;
            OpenWeaponMenu();
        }

        //check if need to scroll
        if (currentSelection < weaponScrollIndex)
        {
            weaponScrollIndex = Mathf.Max(0, weaponScrollIndex - 1); //scroll up
            OpenWeaponMenu();
        }
        else if (currentSelection >= weaponScrollIndex + 6)
        {
            weaponScrollIndex = Mathf.Min(playerItems.Count - 6, weaponScrollIndex + 1); //scroll down
            OpenWeaponMenu();
        }

        ShowWeaponStats();
        UpdateHoverIndicator();
    }

    private void CheckForItemUse()
    {
        Item item = InventoryManager.instance.GetItems()[itemScrollIndex];
        if(item != null)
        {
            if(item.potionEffect == PublicEnums.Effects.Heal || item.potionEffect == PublicEnums.Effects.Revive) // or heal
            {
                choosePlayer.SetActive(true);
                for (int i = 0; i < playerParty.Count; i++)
                {
                    choosePlayer.transform.GetChild(i).gameObject.SetActive(true);
                    playerButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerParty[i].GetComponent<playerController>().playerStats.name + " " + playerParty[i].GetComponent<playerController>().playerStats.health.ToString()
                        + "/" + playerParty[i].GetComponent<playerController>().playerStats.maxHealth.ToString();
                }
                choosingPlayer = true;
                currentMenu = playerButtons;
                currentSelection = 0;
                UpdateHoverIndicator();
            }
        }
    }

    private void CheckForSkillUse()
    {
        //tldr the way this works is by showing a new menu with the player names and choosing what player char to heal
        //then using the item on the selected player
        Skill skill = playerSkills[skillScrollIndex];
        if (skill != null)
        {
            if (skill.effect == PublicEnums.Effects.Heal || skill.effect == PublicEnums.Effects.Revive)
            {
                choosePlayer.SetActive(true);
                for (int i = 0; i < playerParty.Count; i++)
                {
                    choosePlayer.transform.GetChild(i).gameObject.SetActive(true);
                    chooseMenu.Add(choosePlayer.transform.GetChild(i).transform.GetChild(0).GetComponent<Button>());
                    chooseMenu[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerParty[i].GetComponent<playerController>().playerStats.name + " " + playerParty[i].GetComponent<playerController>().playerStats.health.ToString() 
                        + "/" + playerParty[i].GetComponent<playerController>().playerStats.maxHealth.ToString();
                }
                choosingPlayer = true;
                currentMenu = chooseMenu;
                currentSelection = 0;
            }
        }
    }
    
    private IEnumerator UseItem()
    {
        //I am assuming that we are only reviving or healing in the menu so I am not adjusting any other stat, if you want to do that go ahead
        //you can make it so revive is the only thing that heals the player, I have not pulled because I don't want to deal with merge errors
        //right now and I just want to keep coding
        Item item = InventoryManager.instance.GetItems()[itemScrollIndex];
        CharacterAttributes playerStats = playerParty[currentSelection].GetComponent<playerController>().playerStats;
        if (item != null)
        {
            if (item.potionEffect == PublicEnums.Effects.Revive && playerStats.health <= 0)
            {
                playerStats.health += item.effectAmount;
                if (playerStats.health > playerStats.maxHealth)
                    playerStats.health = playerStats.maxHealth;
                item.amountOfItem--;
            }
            if(item.potionEffect == PublicEnums.Effects.Party_Revive)
            {
                bool res;
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health <= 0)
                    {

                        playerParty[i].GetComponent<playerController>().playerStats.health = playerParty[i].GetComponent<playerController>().playerStats.maxHealth;


                    }
                }
                item.amountOfItem--;
            }
            if (item.potionEffect == PublicEnums.Effects.mana && playerStats.health <= 0)
            {
                playerStats.mana += item.effectAmount;
                if (playerStats.mana > playerStats.maxMana)
                    playerStats.mana = playerStats.maxMana;
                item.amountOfItem--;
            }
            if (item.potionEffect == PublicEnums.Effects.Party_mana && playerStats.health <= 0)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health > 0)
                    {
                      
                       
                        playerParty[i].GetComponent<playerController>().playerStats.mana += item.effectAmount;
                        if (playerParty[i].GetComponent<playerController>().playerStats.mana > playerParty[i].GetComponent<playerController>().playerStats.maxMana)
                            playerParty[i].GetComponent<playerController>().playerStats.mana = playerParty[i].GetComponent<playerController>().playerStats.maxMana;
                    }


                }
                item.amountOfItem--;
            }
            else if(item.potionEffect == PublicEnums.Effects.Heal && playerStats.health != playerStats.maxHealth)
            {
                playerStats.health += item.effectAmount;
                if (playerStats.health > playerStats.maxHealth)
                    playerStats.health = playerStats.maxHealth;
                item.amountOfItem--;
            }
            else if (item.potionEffect == PublicEnums.Effects.Party_Heal)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health > 0)
                    {
                       
                        playerParty[i].GetComponent<playerController>().playerStats.health += item.effectAmount;
                        if (playerParty[i].GetComponent<playerController>().playerStats.health > playerParty[i].GetComponent<playerController>().playerStats.maxHealth)
                            playerParty[i].GetComponent<playerController>().playerStats.health = playerParty[i].GetComponent<playerController>().playerStats.maxHealth;
                    }


                }
                item.amountOfItem--;
            }
            else if(item.potionEffect == PublicEnums.Effects.Revive && playerStats.health > 0)
            {
                playerRevivedAlive.SetActive(true);
                yield return new WaitForSeconds(1f);
                playerRevivedAlive.SetActive(false);
            }
        }
    }

    private IEnumerator UseSkill()
    {
        Skill skill = playerParty[currentSelection].GetComponent<playerController>().playerStats.skills[currentSelection];
        CharacterAttributes playerStats = playerParty[currentSelection].GetComponent<playerController>().playerStats;
        if (skill != null)
        {
            if (skill.effect == PublicEnums.Effects.Revive && playerStats.health <= 0)
            {
                playerStats.health += skill.baseDamage;
                if (playerStats.health > playerStats.maxHealth)
                    playerStats.health = playerStats.maxHealth;
                playerStats.mana -= skill.manaCost;
            }
            else if (skill.effect == PublicEnums.Effects.Heal)
            {
                playerStats.health += skill.baseDamage;
                if (playerStats.health > playerStats.maxHealth)
                    playerStats.health = playerStats.maxHealth;
                playerStats.mana -= skill.manaCost;
            }
            else if (skill.effect == PublicEnums.Effects.Revive && playerStats.health > 0)
            {
                playerRevivedAlive.SetActive(true);
                yield return new WaitForSeconds(1f);
                playerRevivedAlive.SetActive(false);
            }
        }
    }

    private void EquipWeapon()
    {
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        CharacterAttributes charStats = playerParty[floor].GetComponent<playerController>().playerStats;
        //unequip item
        if (charStats.weapon != null)
        {
            if (!isPlaying) Aud.PlayOneShot(AudEquip, EquipVol);
            InventoryManager.instance.AddItem(charStats.weapon);
            charStats.weapon = null;
        }
        
        
        currentSelection = 0;
        charStats.weapon = weaponItems[weaponScrollIndex];
        InventoryManager.instance.RemoveItem(weaponItems[weaponScrollIndex]);
        weaponScrollIndex = 0;
        OpenWeaponMenu();
    }

    private void EquipmArmor()
    {
        //unequip item
        int floor = (int)Math.Ceiling((double)playerScrollIndex / 2) - 1;
        if (floor < 0)
        {
            floor = 0;
        }
        CharacterAttributes charStats = playerParty[floor].GetComponent<playerController>().playerStats;
        
        if (charStats.equipment != null) 
        {
            if (!isPlaying) Aud.PlayOneShot(AudEquip, EquipVol);
            InventoryManager.instance.AddItem(charStats.equipment);
            charStats.equipment = null;
        }

        
        currentSelection = 0;
        charStats.equipment = equipmentItems[equipmentScrollIndex];
        InventoryManager.instance.RemoveItem(equipmentItems[equipmentScrollIndex]);
        equipmentScrollIndex = 0;
        OpenEquipmentMenu();
    }    

    private void HandleBackspace()
    {
        if (menuStack.Count > 1)
        {
            menuStack.Pop();
            currentMenu = leftMenu;
            playerScrollIndex = 0;
            currentSelection = 0;
            equipmentScrollIndex = 0;
            weaponScrollIndex = 0;
            skillScrollIndex = 0;
            itemScrollIndex = 0;
            subMenuParent.SetActive(false);
            inMenu = false;
            equipmentMenu = false;
            weaponMenu = false;
            inMenu = false;
            skillMenu = false;
            ItemMenu = false;
            ShowStats();
            weaponStats.SetActive(false);
            equipmentStats.SetActive(false);
            skillStats.SetActive(false);
            itemStats.SetActive(false);
            cursor.transform.SetParent(statMenu.transform);
            UpdateHoverIndicator();           
        }
    }

    private void HandleBackspaceSmallerMenu()
    {           
        currentMenu = subMenuButtons;
        playerScrollIndex = 0;
        currentSelection = 0;
        equipmentScrollIndex = 0;
        weaponScrollIndex = 0;
        skillScrollIndex = 0;
        itemScrollIndex = 0;
        choosePlayer.gameObject.SetActive(false);
        choosingPlayer = false;
        cursor.transform.SetParent(statMenu.transform);
        UpdateHoverIndicator();
    }

    private void SwitchToMenu(List<Button> newMenu)
    {      
        currentMenu = newMenu;
        menuStack.Push(newMenu);
        currentSelection = 0;
        equipmentScrollIndex = 0;
        weaponScrollIndex = 0;
        UpdateHoverIndicator();
        cursor.transform.SetParent(subMenuParent.transform);
        if (weaponMenu)
        {
            ShowWeaponStats();
        }
        else if (equipmentMenu)
        {
            ShowEquipmentStats();
        }
        else if(skillMenu)
        {
            ShowSkillStats();
        }
        else if(ItemMenu)
        {
            ShowItemsStats();
        }

    }

    private void HideStats()
    {
        foreach (var stat in stats)
        {
            Aud.PlayOneShot(AudClosemenu, ClosemenuVol);
            stat.SetActive(false);
        }
    }private void ShowStats()
    {
        int index = 0;
        foreach (var player in playerParty)
        {
            stats[index].SetActive(true);
           if(!isPlaying) Aud.PlayOneShot(AudOpenMenu, OpenMenuVol);
            CharacterAttributes playerStats = player.GetComponent<playerController>().playerStats;
            stats[index].transform.GetChild(0).gameObject.transform.GetComponent<Image>().sprite = player.GetComponent<playerController>().Sprite;
            stats[index].transform.GetChild(1).gameObject.transform.GetComponent<TextMeshProUGUI>().text = playerStats.name;
            stats[index].transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Lvl: " + playerStats.level;
            stats[index].transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "xp: " + playerStats.currentXP + "/" + playerStats.xpToNextLevel;
            stats[index].transform.GetChild(4).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Atk: " + playerStats.attackDamage;
            stats[index].transform.GetChild(5).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Skill Atk: " + playerStats.skillDamage;
            stats[index].transform.GetChild(6).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Defense: " + playerStats.Defence;
            stats[index].transform.GetChild(7).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Speed: " + playerStats.combatSpeed;
            if (leftMenu.Count == 0)
            {
                itemsButtons.Add(stats[index].transform.GetChild(8).transform.GetChild(0).gameObject.GetComponent<Button>());
                weaponButtons.Add(stats[index].transform.GetChild(9).transform.GetChild(0).gameObject.GetComponent<Button>());
                skillsButtons.Add(stats[index].transform.GetChild(10).transform.GetChild(0).gameObject.GetComponent<Button>());
                equipmentButtons.Add(stats[index].transform.GetChild(11).transform.GetChild(0).gameObject.GetComponent<Button>());
            }
            index++;
        }
    }

    private void ShowWeaponStats()
    {
        WeaponItem item = weaponItems[currentSelection];

        if (item != null)
        {
            weaponStats.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.name;
            weaponStats.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = item.Sprite;
            weaponStats.transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Type: " + item.weaponType.ToString();
            weaponStats.transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Damage: " + item.damage.ToString();
            weaponStats.transform.GetChild(4).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Skill Damage: " + item.skillDamage.ToString();
            weaponStats.transform.GetChild(6).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.description;      
        }
    }
    
    private void ShowSkillStats()
    {
        Skill skill = playerSkills[currentSelection];

        if (skill != null)
        {
            skillStats.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text = skill.skillName;
            skillStats.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = skill.sprite;
            skillStats.transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Element: " + skill.elementType.ToString();
            skillStats.transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Skill Damage: " + skill.baseDamage.ToString();
            skillStats.transform.GetChild(4).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Mana Cost: " + skill.manaCost.ToString();
            skillStats.transform.GetChild(6).gameObject.transform.GetComponent<TextMeshProUGUI>().text = skill.description;      
        }
    }
    private void ShowItemsStats()
    {
        Item item = InventoryManager.instance.GetItems()[currentSelection];

        if (item != null)
        {
            itemStats.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.itemName;
            itemStats.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = item.Sprite;
            itemStats.transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Element: " + item.Element.ToString();
            itemStats.transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Base Damage: " + item.effectAmount.ToString();
            itemStats.transform.GetChild(4).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Effect: " + item.potionEffect.ToString();
            itemStats.transform.GetChild(6).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.description;
            itemStats.transform.GetChild(7).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Amount: " + item.amountOfItem.ToString();
        }
    }

    private void ShowEquipmentStats()
    {
        EquipmentItem item = equipmentItems[currentSelection];

        if (item != null)
        {
            equipmentStats.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.name;
            equipmentStats.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = item.Sprite;
            equipmentStats.transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Defense: " + item.armor.ToString();
            equipmentStats.transform.GetChild(4).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.description;
        }
    }
}
