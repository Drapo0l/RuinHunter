using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerActionSelector : MonoBehaviour
{
    public List<Button> mainMenuButtons;          // Main menu buttons
    public GameObject hoverIndicator;             // Visual indicator for current selection
    public Transform itemsMenuParent;             // Parent where item buttons will be added
    public Transform skillsMenuParent;            // Parent where SKILL buttons will be added
    public GameObject buttonPrefab;               // Prefab for dynamically created buttons
    public GameObject menuPanel;                  //Panel for the menu UI

    private int currentSelection = 0;              // Index of the currently selected button
    private Stack<List<Button>> menuStack = new Stack<List<Button>>(); // Stack for managing menus
    private List<Button> currentMenu;               // Reference to the currently active menu

    private List<Button> itemsMenuButtons = new List<Button>();         // Dynamically populated item buttons
    private List<Button> skillsMenuButtons = new List<Button>();        // Dynamically populated skill buttons

    private playerController playerController;
    private List<CharacterComponent> characterAttributes; // references to character attributes
    private InventoryManager inventoryManager;       // reference to inventory manager

    private List<GameObject> enemies; //list of all enemies
    private int selectedEnemyIndex = 0;
    public GameObject targetIndicator;

    private Transform playerTransform; // track player position

    private bool attacking = false;

    void Start()
    {
        currentMenu = mainMenuButtons;
        menuStack.Push(mainMenuButtons);
        UpdateHoverIndicator();

        // Assign button actions
        AssignButtonActions(mainMenuButtons, PerformAttack, OpenItemMenu, OpenSkillMenu);

        characterAttributes = PartyManager.Instance.GetCurrentParty();
        inventoryManager = InventoryManager.instance;

        //hide the menu
        menuPanel.SetActive(false);       
    }

    void Update()
    {
        // Handle navigation and selection input
        if (menuPanel.activeSelf && !attacking)
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
                if (selectedEnemyIndex >= 0) selectedEnemyIndex = 0;
                SelectEnemy(selectedEnemyIndex);
            }
            if (Input.GetKeyDown(KeyCode.Return)) 
            {
                PerformAttack(); 
            
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) 
            {
                HandleBackspace();             
            }
        }
    }

    public void ShowMenu(Transform player, playerController _playerController)
    {
        playerTransform = player;
        playerController = _playerController;
        enemies = GameManager.Instance.enemyObj;

        // set the position of the menu to the left of the player
        Vector3 menuPosition = playerTransform.position + new Vector3(-2f, 0, 0); //adjust offset if needed
        menuPanel.transform.position = Camera.main.WorldToScreenPoint(menuPosition);

        menuPanel.SetActive(true); // enable the menu
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
    }

    private void Navigate(int direction)
    {
        currentSelection += direction;
        if (currentSelection < 0) currentSelection = currentMenu.Count - 1; // Loop to end
        if (currentSelection >= currentMenu.Count) currentSelection = 0;      // Loop to start
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
        }
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
        targetIndicator.transform.position = Camera.main.WorldToScreenPoint(indicatorPosition);

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
        //clear existing buttons
        foreach ( Button button in itemsMenuButtons ) 
        {
            Destroy(button.gameObject);
        }
        itemsMenuButtons.Clear();

        //populate item from player's inventory
        foreach ( Item item in inventoryManager.GetItems())
        {
            GameObject newButtonObj = Instantiate(buttonPrefab, itemsMenuParent);
            Button newButton = newButtonObj.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = item.itemName;
            newButton.onClick.AddListener(() => UseItem(item));

            itemsMenuButtons.Add(newButton);
        }

        //enable scrolling
        ScrollRect scrollRect = itemsMenuParent.GetComponentInParent<ScrollRect>();
        if ( scrollRect != null ) 
        {
            scrollRect.verticalScrollbar.value = 1; //reset scroll at the top
        }
    }

    private void PopulateSkillsMenu()
    {
        foreach ( Button button in skillsMenuButtons )
        {
            Destroy(button.gameObject );
        }
        skillsMenuButtons.Clear();

        foreach (Skill skill in characterAttributes[0].stats.skills )
        {
            GameObject newButtonOBj = Instantiate(buttonPrefab, skillsMenuParent);
            Button newButton = newButtonOBj.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = skill.skillName;
            newButton.onClick.AddListener(() => UseSkill(skill));

            skillsMenuButtons.Add(newButton);
        }
    }

    private void UseItem(Item item)
    {
        
    }

    private void UseSkill(Skill skill)
    {

    }

    // Actions for main menu
    public void PerformAttack()
    {
        attacking = true;
        
    }

    public void DamageEnemy()
    {
        enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeMeleeDamage(playerController.playerStats.attackDamage, playerController.playerWeapon);
    }
   
    public void RemoveEnemy(GameObject enemy) 
    {
        enemies.Remove(enemy);
    }
}
