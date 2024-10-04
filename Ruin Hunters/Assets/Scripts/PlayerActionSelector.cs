using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PlayerActionSelector : MonoBehaviour
{
    public List<Button> mainMenuButtons;          // Main menu buttons
    public GameObject hoverIndicator;             // Visual indicator for current selection
    public Transform itemsMenuParent;             // Parent where item buttons will be added
    public Transform skillsMenuParent;            // Parent where SKILL buttons will be added
    public GameObject buttonPrefab;               // Prefab for dynamically created buttons
    public GameObject menuPanel;                  //Panel for the menu UI
    public GameObject battleCamera;               // battle cam
    public GameObject actionMenu;                 //action menu buttons

    private int currentSelection = 0;              // Index of the currently selected button
    private Stack<List<Button>> menuStack = new Stack<List<Button>>(); // Stack for managing menus
    public List<GameObject> playerHealths;          // list of player health/mana
    private List<Button> currentMenu;               // Reference to the currently active menu

    private List<Button> itemsMenuButtons = new List<Button>();         // Dynamically populated item buttons
    private List<Button> skillsMenuButtons = new List<Button>();        // Dynamically populated skill buttons

    private playerController playerController;
    private List<CharacterComponent> characterAttributes; // references to character attributes
    private InventoryManager inventoryManager;       // reference to inventory manager

    private List<GameObject> playerParty;
    private List<GameObject> enemies; //list of all enemies
    private int selectedPartyIndex = 0;
    private int selectedEnemyIndex = 0;
    public GameObject targetIndicator;

    private Transform playerTransform; // track player position

    private bool attacking = false;
    private bool skillAttack = false;
    private bool targetingParty = false;
    private bool usingItem = false;

    private List<Skill> playerSkills;
    public int visibleSkillCount = 4;
    private int skillScrollIndex = 0;
    public int visibleItemCount = 4;
    private int itemScrollIndex = 0;

    void Start()
    {
        currentMenu = mainMenuButtons;
        menuStack.Push(mainMenuButtons);
        UpdateHoverIndicator();

        // Assign button actions
        AssignButtonActions(mainMenuButtons, PerformAttack, OpenSkillMenu, OpenItemMenu);

        characterAttributes = PartyManager.Instance.GetCurrentPartyComponent();
        

        
    }

    void Update()
    {
        // Handle navigation and selection input
        if (menuPanel.activeSelf && !attacking && !skillAttack && !usingItem)
        { 
            if (Input.GetKeyDown(KeyCode.W)) { Navigate(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { Navigate(1); }
            if (Input.GetKeyDown(KeyCode.Return)) { ExecuteCurrentAction(); }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if(menuPanel.activeSelf && attacking)
        {
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                selectedEnemyIndex--;
                if(selectedEnemyIndex < 0) selectedEnemyIndex = enemies.Count - 1;
                SelectEnemy(selectedEnemyIndex);
            }
            if (Input.GetKeyDown(KeyCode.S)) 
            {
                selectedEnemyIndex++;
                if (selectedEnemyIndex >= enemies.Count) selectedEnemyIndex = 0;
                SelectEnemy(selectedEnemyIndex);
            }
            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                DamageEnemy();
            
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) 
            {
                HandleAttackBackspace();             
            }
        }
        else if (menuPanel.activeSelf && skillAttack)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateSkills(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateSkills(1); }
            if (Input.GetKeyDown(KeyCode.Return)) 
            { 
                PerformAttack();
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if (targetingParty)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                selectedPartyIndex--;
                if (selectedPartyIndex < 0) selectedPartyIndex = playerParty.Count - 1;
                targetParty(selectedPartyIndex);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                selectedPartyIndex++;
                if (selectedPartyIndex >= playerParty.Count) selectedPartyIndex = 0;
                targetParty(selectedPartyIndex);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                consumeItem();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                HandleAttackBackspace();
            }
        }
        else if (menuPanel.activeSelf && usingItem)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateSkills(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateSkills(1); }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                UseItem();
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) 
            { HandleBackspace(); }
        }
        
    }

    public void ShowMenu(Transform player, playerController _playerController, List<Skill> currentSkills)
    {
        playerParty = PartyManager.Instance.GetPlayeGameObj();
        inventoryManager = InventoryManager.instance;
        playerSkills = currentSkills;
        playerTransform = player;
        playerController = _playerController;
        enemies = GameManager.Instance.enemyObj;        

        // set the position of the menu to the left of the player
        Vector3 menuPosition = playerTransform.position + new Vector3(-2f, 0, 0); //adjust offset if needed
        menuPanel.transform.position = battleCamera.transform.position;

        menuPanel.SetActive(true); // enable the menu

        
    }

    

    public void HideMenu()
    {
        menuPanel.SetActive(false);
        targetIndicator.SetActive(false);
        attacking = false;
    }

    void HideActionMenu()
    {
        actionMenu.SetActive(false);
    }

    private void Navigate(int direction)
    {
        currentSelection += direction;
        if (currentSelection < 0) currentSelection = currentMenu.Count - 1; // Loop to end
        if (currentSelection >= currentMenu.Count) currentSelection = 0;      // Loop to start
        UpdateHoverIndicator();
    }

    private void NavigateSkills(int direction)
    {
        currentSelection += direction;

        if (currentSelection < 0) currentSelection = currentMenu.Count - 1; // Loop to end
        if (currentSelection >= currentMenu.Count) currentSelection = 0;      // Loop to start

        //check if need to scroll
        if(currentSelection == 0 && direction == -1 && skillScrollIndex > 0)
        {
            skillScrollIndex--; //scroll up
            PopulateSkillsMenu();
        }
        else if(currentSelection == currentMenu.Count - 1 && direction == 1  && skillScrollIndex + visibleSkillCount < playerSkills.Count)
        {
            skillScrollIndex++; //scroll up
            PopulateSkillsMenu();
        }

        UpdateHoverIndicator();
    }

    private void UpdateHoverIndicator()
    {
        Vector3 buttonPosition = currentMenu[currentSelection].transform.position;

        hoverIndicator.transform.position = new Vector3(buttonPosition.x - 150f, buttonPosition.y, buttonPosition.z);
    }

    private void ExecuteCurrentAction()
    {
        // Trigger the onClick event for the currently selected button
        currentMenu[currentSelection].onClick.Invoke();
    }

    private void HandleBackspace()
    {
        if (menuStack.Count > 1)
        {
            menuStack.Pop();
            currentMenu = menuStack.Peek();
            currentSelection = 0;
            UpdateHoverIndicator();
            attacking = false;
            targetingParty = false;
            usingItem = false;

            if (skillsMenuButtons.Count > 0) 
            {
                ClearSkillMenu();
            }
            else if (itemsMenuButtons.Count > 0) 
            {
                ClearItemsMenu();
            }

        }
    }

    private void HandleAttackBackspace()
    {
        attacking = false;
        skillAttack = false;
        targetingParty = false;
        targetIndicator.SetActive(false);
        actionMenu.SetActive(true);
    }

    private void SwitchToMenu(List<Button> newMenu)
    {
        currentMenu = newMenu;
        menuStack.Push(newMenu);
        currentSelection = 0;
        UpdateHoverIndicator();
    }

    void SelectEnemy(int index)
    {           
        selectedEnemyIndex = index;

        GameObject selectedEnemy = enemies[selectedEnemyIndex];

        Vector3 enemyPosition = selectedEnemy.transform.position;
        Vector3 indicatorPosition = new Vector3(enemyPosition.x, enemyPosition.y + 2f, enemyPosition.z);

        targetIndicator.transform.position = indicatorPosition;

        targetIndicator.SetActive(true);        
    }

    private void AssignButtonActions(List<Button> buttons, params UnityAction[] actions)
    {
        for (int i = 0; i < buttons.Count && i < actions.Length; i++)
        {
            buttons[i].onClick.AddListener(actions[i]);
        }
    }

    public void OpenItemMenu()
    {
        PopulateItemMenu();
        SwitchToMenu(itemsMenuButtons);
    }

    public void OpenSkillMenu()
    {
        PopulateSkillsMenu();
        SwitchToMenu(skillsMenuButtons);
    }

    private void PopulateItemMenu()
    {
        usingItem = true;

        foreach (Button button in itemsMenuButtons)
        {
            Destroy(button.gameObject);
        }
        itemsMenuButtons.Clear();

        float horizontalOffset = 65f; //distance between buttons in x axis

        RectTransform itemsMenuParentRect = itemsMenuParent.GetComponent<RectTransform>();
        Vector3 initialPosition = itemsMenuParentRect.position;

        //create visible buttons
        for (int i = itemScrollIndex; i < Mathf.Min(itemScrollIndex + visibleItemCount, inventoryManager.GetItems().Count); i++)
        {
            //instantiate the button
            GameObject newButtonOBj = Instantiate(buttonPrefab, itemsMenuParent);
            Button newButton = newButtonOBj.GetComponent<Button>();
            RectTransform newButtonRect = newButtonOBj.GetComponent<RectTransform>();

            //set the text
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = inventoryManager.GetItems()[i].itemName;

            //adjust position
            Vector3 newPosition = new Vector3(
                initialPosition.x + 180f,
                initialPosition.y + (i * -horizontalOffset),
                initialPosition.z);
            newButtonRect.localPosition = newPosition;

            itemsMenuButtons.Add(newButton);
        }

        //enable scrolling
        ScrollRect scrollRect = itemsMenuParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar.value = 1; //reset scroll to top
        }

        UpdateHoverIndicator();
    }

    private void PopulateSkillsMenu()
    {
        skillAttack = true;

        foreach ( Button button in skillsMenuButtons )
        {
            Destroy(button.gameObject );
        }
        skillsMenuButtons.Clear();

        float horizontalOffset = 65f; //distance between buttons in x axis

        RectTransform skillsMenuParentRect = skillsMenuParent.GetComponent<RectTransform>();
        Vector3 initialPosition = skillsMenuParentRect.position;

        //create visible buttons
        for ( int i = skillScrollIndex; i < Mathf.Min(skillScrollIndex + visibleSkillCount, playerSkills.Count); i++ )
        {
            //instantiate the button
            GameObject newButtonOBj = Instantiate(buttonPrefab, skillsMenuParent);
            Button newButton = newButtonOBj.GetComponent<Button>();
            RectTransform newButtonRect = newButtonOBj.GetComponent<RectTransform>();

            //set the text
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = playerSkills[i].skillName;

            //adjust position
            Vector3 newPosition = new Vector3(
                initialPosition.x + 180f,
                initialPosition.y + (i * -horizontalOffset),
                initialPosition.z);
            newButtonRect.localPosition = newPosition;

            skillsMenuButtons.Add(newButton);
        }

        //enable scrolling
        ScrollRect scrollRect = skillsMenuParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null ) 
        {
            scrollRect.verticalScrollbar.value = 1; //reset scroll to top
        }

        UpdateHoverIndicator();
    }

    void ClearSkillMenu()
    {
        foreach (Button button in skillsMenuButtons) 
        {
            Destroy(button.gameObject);
        }
        skillsMenuButtons.Clear();
        skillAttack = false;
    }

    void ClearItemsMenu()
    {
        foreach (Button button in itemsMenuButtons)
        {
            Destroy (button.gameObject);
        }
        itemsMenuButtons.Clear();
    }

    private void UseItem()
    {
        if (InventoryManager.instance.GetItems()[itemScrollIndex] == null) return;
        if (InventoryManager.instance.GetItems()[itemScrollIndex].damageable)
        {
            PerformAttack();
        }
        else
        {
            HideActionMenu();
            targetingParty = true;
            targetIndicator.SetActive(true);
            targetParty(0);
        }
    }

    private void targetParty(int index)
    {
        selectedPartyIndex = index;

        GameObject selectedCharacter = playerParty[selectedPartyIndex];

        Vector3 characterPosition = selectedCharacter.transform.position;
        Vector3 indicatorPosition = new Vector3(characterPosition.x, characterPosition.y + 2f, characterPosition.z);

        targetIndicator.transform.position = indicatorPosition;

        targetIndicator.SetActive(true);
    }

    // Actions for main menu
    public void PerformAttack()
    {
        HideActionMenu();
        attacking = true;
        targetIndicator.SetActive(true);
        SelectEnemy(0);
    }

    public void DamageEnemy()
    {
        
        if (skillAttack)
        {
            enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(playerSkills[skillScrollIndex].baseDamage, playerSkills[skillScrollIndex].elementType);
        }
        else if(usingItem)
        {
            consumeItem();
        }
        else
        {
            enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeMeleeDamage(playerController.playerStats.attackDamage, playerController.playerWeapon);
        }        
        HideMenu();
    }
    
    private void consumeItem()
    {
        Item item = InventoryManager.instance.GetItems()[itemScrollIndex];
        if (item.potionEffect == PublicEnums.Effects.Heal)
        {
            playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.health += item.effectAmount;
            if (playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.health > playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.maxHealth)
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.health = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.maxHealth;
        }
        //continue if statements for other item types
        //leave this at the end to consume the item
        item.amountOfItem--;
        if(item.amountOfItem <= 0 )
        {
            InventoryManager.instance.RemoveItem(item);
        }
    }

    public void RemoveEnemy(GameObject enemy) 
    {
        enemies.Remove(enemy);
    }
}
