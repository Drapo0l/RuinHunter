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

    private CharacterAttributes characterAttributes; // references to character attributes
    private InventoryManager inventoryManager;       // reference to inventory manager

    private Transform playerTransform; // track player position

    void Start()
    {
        currentMenu = mainMenuButtons;
        menuStack.Push(mainMenuButtons);
        UpdateHoverIndicator();

        // Assign button actions
        AssignButtonActions(mainMenuButtons, PerformAttack, OpenItemMenu, OpenSkillMenu);

        characterAttributes = GetComponent<playerController>().characterAttributes;
        inventoryManager = InventoryManager.instance;

        //hide the menu
        menuPanel.SetActive(false);
    }

    void Update()
    {
        // Handle navigation and selection input
        if (menuPanel.activeSelf)
        { 
            if (Input.GetKeyDown(KeyCode.W)) { Navigate(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { Navigate(1); }
            if (Input.GetKeyDown(KeyCode.Return)) { ExecuteCurrentAction(); }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
    }

    public void ShowMenu(Transform player)
    {
        playerTransform = player;

        // set the position of the menu to the left of the player
        Vector3 menuPosition = playerTransform.position + new Vector3(-2f, 0, 0); //adjust offset if needed
        menuPanel.transform.position = Camera.main.WorldToScreenPoint(menuPosition);

        menuPanel.SetActive(true); // enable the menu
        UpdateHoverIndicator();
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
        hoverIndicator.transform.position = currentMenu[currentSelection].transform.position;
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
    }

    private void PopulateSkillsMenu()
    {
        foreach ( Button button in skillsMenuButtons )
        {
            Destroy(button.gameObject );
        }
        skillsMenuButtons.Clear();

        foreach (Skill skill in characterAttributes.skills )
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
        
        // Insert attack logic here
    }
    
}
