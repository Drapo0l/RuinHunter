using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.Experimental.GraphView;

public class PlayerActionSelector : MonoBehaviour
{
    public List<Button> mainMenuButtons;          // Main menu buttons
    public GameObject hoverIndicator;             // Visual indicator for current selection
    public Transform itemsMenuParent;             // Parent where item buttons will be added
    public Transform skillsMenuParent;            // Parent where SKILL buttons will be added
    public GameObject menuPanel;                  //Panel for the menu UI
    public Camera battleCamera;               // battle cam
    public GameObject actionMenu;                 //action menu buttons
    public List<Button> subMenuButtons;           //buttons for skill/item menu
    public GameObject subMenuParent;

    private int currentSelection = 0;              // Index of the currently selected button
    private Stack<List<Button>> menuStack = new Stack<List<Button>>(); // Stack for managing menus
    public List<GameObject> playerHealths;          // list of player health/mana
    private List<Button> currentMenu;               // Reference to the currently active menu

    private playerController playerController;
    private List<CharacterAttributes> characterAttributes; // references to character attributes
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
    private bool tutorial = true;

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
        AssignButtonActions(mainMenuButtons, PerformAttack, OpenSkillMenu, OpenItemMenu, DefendFunc, FleeFunc);

        characterAttributes = PartyManager.Instance.GetCurrentPartyComponent();
        

        
    }

    void Update()
    {
        {
            // Handle navigation and selection input
            if (menuPanel.activeSelf && !attacking && !skillAttack && !usingItem)
            {
                if (Input.GetKeyDown(KeyCode.W)) { Navigate(-1); }
                if (Input.GetKeyDown(KeyCode.S)) { Navigate(1); }
                if (Input.GetKeyDown(KeyCode.Return)) { ExecuteCurrentAction(); }
                if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
            }
            else if (menuPanel.activeSelf && attacking)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    selectedEnemyIndex--;
                    if (selectedEnemyIndex < 0) selectedEnemyIndex = enemies.Count - 1;
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
                    StartCoroutine(DamageEnemy());
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
                if (Input.GetKeyDown(KeyCode.W)) { NavigateItems(-1); }
                if (Input.GetKeyDown(KeyCode.S)) { NavigateItems(1); }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    UseItem();
                }
                if (Input.GetKeyDown(KeyCode.Backspace))
                { HandleBackspace(); }
            }
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

        menuPanel.SetActive(true); // enable the menu

        //SetMenuPositionToLeftOfPlayer();
    }

    //private void SetMenuPositionToLeftOfPlayer()
    //{
    //    Vector3 playerWorldPos = playerTransform.position;

    //    Vector3 offset = new Vector3(-2f, 0, 0);

    //    Vector3 screenPos = battleCamera.WorldToScreenPoint(playerWorldPos + offset);

    //    if (screenPos.z < 0) // if it's behind the camera
    //    {           
    //        return;
    //    }
    //    RectTransform canvasRectTransform = actionMenu.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //    canvasRectTransform,
    //    screenPos,
    //    battleCamera,
    //    out Vector2 localPoint);
    //    //localPoint.x += 1100;
    //    //localPoint.y += 600;
    //    actionMenu.GetComponent<RectTransform>().anchoredPosition = localPoint;
    //}

    public void HideMenu()
    {
        actionMenu.SetActive(true);
        menuPanel.SetActive(false);
        targetIndicator.SetActive(false);
        attacking = false;
        targetingParty = false;
        usingItem = false;
        skillAttack = false;
        HandleBackspace();
    }

    void HideActionMenu()
    {
        actionMenu.SetActive(false);
        subMenuParent.SetActive(false);
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

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = playerSkills.Count - 1;
            skillScrollIndex = Mathf.Max(0, playerSkills.Count - 5);
            PopulateSkillsMenu();
        }
        else if (currentSelection >= playerSkills.Count)
        {
            //if player is at the bottom go to first skill
            currentSelection = 0; 
            skillScrollIndex = 0;
            PopulateSkillsMenu();
        }

        //check if need to scroll
        if(currentSelection < skillScrollIndex)
        {
            skillScrollIndex = Mathf.Max(0, skillScrollIndex - 1); //scroll up
            PopulateSkillsMenu();
        }
        else if(currentSelection >= skillScrollIndex + 5)
        {
            skillScrollIndex = Mathf.Min(playerSkills.Count - 5, skillScrollIndex + 1); //scroll down
            PopulateSkillsMenu();
        }

        while (!subMenuButtons[currentSelection % subMenuButtons.Count].interactable)
        {
            currentSelection += direction;
            if (currentSelection < 0) currentSelection = currentMenu.Count - 1;
            if (currentSelection >= currentMenu.Count) currentSelection = 0;
        }

        UpdateHoverIndicator();
    }

    private void NavigateItems(int direction)
    {
        currentSelection += direction;

        List<Item> playerItems = InventoryManager.instance.GetItems();

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = playerItems.Count - 1;
            itemScrollIndex = Mathf.Max(0, playerItems.Count - 5);
            PopulateItemMenu();
        }
        else if (currentSelection >= playerSkills.Count)
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
        else if (currentSelection >= itemScrollIndex + 5)
        {
            itemScrollIndex = Mathf.Min(playerSkills.Count - 5, itemScrollIndex + 1); //scroll down
            PopulateItemMenu();
        }

        while (!subMenuButtons[currentSelection % subMenuButtons.Count].interactable)
        {
            currentSelection += direction;
            if (currentSelection < 0) currentSelection = currentMenu.Count - 1;
            if (currentSelection >= currentMenu.Count) currentSelection = 0;
        }

        UpdateHoverIndicator();
    }

    private void UpdateHoverIndicator()
    {
        Vector3 buttonPosition = Vector3.zero;
        if (skillAttack)
        {
            int visibleIndex = currentSelection - skillScrollIndex;

            if (visibleIndex >= 0 && visibleIndex < currentMenu.Count)
            {
                buttonPosition = currentMenu[visibleIndex].transform.position;                
            }
        }
        else if (usingItem)
        {
            int visibleIndex = currentSelection - itemScrollIndex;

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
            attacking = false;
            targetingParty = false;
            usingItem = false;
            skillAttack = false;
            skillScrollIndex = 0;
            itemScrollIndex = 0;
            subMenuButtons[0].transform.parent.gameObject.SetActive(false);

            UpdateHoverIndicator();

        }
    }

    private void HandleAttackBackspace()
    {
        if (skillAttack)
        {
            subMenuParent.SetActive(true);
            usingItem = false;
        }
        else if(usingItem)
        {
            subMenuParent.SetActive(true);
            skillAttack = false;
        }  
        else { subMenuParent.SetActive(false); }       
        attacking = false;
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
        SwitchToMenu(subMenuButtons);
    }

    public void OpenSkillMenu()
    {
        PopulateSkillsMenu();
        SwitchToMenu(subMenuButtons);
    }

    private void PopulateItemMenu()
    {
        usingItem = true;

        subMenuButtons[0].transform.parent.gameObject.SetActive(true);

        List<Item> playerItems = InventoryManager.instance.GetItems();

        //update buttons
        for (int i = 0; i < subMenuButtons.Count; i++)
        {
            int itemIndex = itemScrollIndex + i;

            if (itemIndex < playerItems.Count)
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[itemIndex].itemName;
                subMenuButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
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

        subMenuButtons[0].transform.parent.gameObject.SetActive(true);
        
        //update buttons
        for (int i = 0; i < subMenuButtons.Count; i++) 
        {
            int skillIndex = skillScrollIndex + i;

            if (skillIndex < playerSkills.Count)
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerSkills[skillIndex].skillName;
                subMenuButtons[i].interactable = true;
                skillIndex++;
            }
            else
            {
                subMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                subMenuButtons[i].interactable = false;
            }
        }

        currentMenu = subMenuButtons;

        //enable scrolling
        ScrollRect scrollRect = skillsMenuParent.GetComponentInParent<ScrollRect>();
        if (scrollRect != null ) 
        {
            scrollRect.verticalScrollbar.value = 1; //reset scroll to top
        }

        UpdateHoverIndicator();
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

    public IEnumerator DamageEnemy()
    {
        
        if (!usingItem)
        {
            Vector3 origininalPosition = playerTransform.position;
            Transform enemyTransform = enemies[selectedEnemyIndex].transform;
            
            Vector3 directionToEnemy = (enemyTransform.position - playerTransform.position).normalized;

            float distanceInFrontOfEnemy = 3f;
            Vector3 targetPosition = enemyTransform.position - directionToEnemy * distanceInFrontOfEnemy;

            playerController.playerAnimator.SetBool("moving", true);
            while (Vector3.Distance(playerTransform.position, targetPosition) > 0.1f)
            {
               playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, playerController.speed * Time.deltaTime);
               yield return null;
            }                     

            playerController.playerAnimator.SetTrigger("Attack");

            yield return new WaitForSeconds(playerController.playerAnimator.GetCurrentAnimatorStateInfo(0).length);

            if (skillAttack)
            {
                if (playerSkills[skillScrollIndex].AOE)
                {
                    int currentMax = enemies.Count;
                    for (int i = 0; i < currentMax; i++)
                    {
                        if(enemies.Count != currentMax)
                        {
                            i--;
                            currentMax--;
                        }
                        enemies[i].GetComponent<EnemyAI>().TakeSkillDamage(playerSkills[skillScrollIndex].baseDamage, playerSkills[skillScrollIndex].elementType);
                    }
                }
                else
                {
                    enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(playerSkills[skillScrollIndex].baseDamage, playerSkills[skillScrollIndex].elementType);
                }
            }            
            else
            {
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeMeleeDamage(playerController.playerStats.attackDamage, playerController.playerWeapon);
            }


            playerController.playerAnimator.SetBool("moving", true);
            while (Vector3.Distance(playerTransform.position, origininalPosition) > 0.1f)
            {
                playerTransform.position = Vector3.MoveTowards(playerTransform.position, origininalPosition, playerController.speed * Time.deltaTime);
                yield return null;
            }
            playerController.playerAnimator.SetBool("moving", false);
        }
        else
        {
            consumeItem();
        }
        HideMenu();
        if (enemies.Count != 0)        
            GameManager.Instance.EndTurn();
    }
    
    private void consumeItem()
    {
        EnemyAI pause = new EnemyAI();
        Item item = InventoryManager.instance.GetItems()[itemScrollIndex];
        if (item.damageable == false)
        {
            if (item.potionEffect == PublicEnums.Effects.Heal)
            {
                DamageNumberManager.Instance.ShowNumbers(playerParty[selectedPartyIndex].transform.position, item.effectAmount, Color.green);
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.health += item.effectAmount;
                if (playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.health > playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.maxHealth)
                    playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.health = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.maxHealth;
            }
            if (item.potionEffect == PublicEnums.Effects.AttackUp)
            {
                DamageNumberManager.Instance.ShowString(playerParty[selectedPartyIndex].transform.position, "ATT UP", Color.red);
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage * 2;
            }
            if (item.potionEffect == PublicEnums.Effects.DefenceUp)
            {
                DamageNumberManager.Instance.ShowString(playerParty[selectedPartyIndex].transform.position, "DEF UP", Color.blue);
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage * 2;
            }
            if (item.potionEffect == PublicEnums.Effects.SpeedUp)
            {
                DamageNumberManager.Instance.ShowString(playerParty[selectedPartyIndex].transform.position, "SPD UP", Color.green);
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage * 2;
            }
            if (item.potionEffect == PublicEnums.Effects.SkillPUP)
            {
                DamageNumberManager.Instance.ShowString(playerParty[selectedPartyIndex].transform.position, "SKL UP", Color.cyan);
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage * 2;
            }
            if (item.potionEffect == PublicEnums.Effects.Clense)
            {
                DamageNumberManager.Instance.ShowString(playerParty[selectedPartyIndex].transform.position, "Clense", Color.gray);
                playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage = playerParty[selectedPartyIndex].GetComponent<playerController>().playerStats.attackDamage * 2;
            }
            if (item.potionEffect == PublicEnums.Effects.Revive)
            {
                DamageNumberManager.Instance.ShowString(playerParty[selectedPartyIndex].transform.position, "REBORN", Color.yellow);
                GameManager.Instance.PlayerReborn(playerParty[selectedPartyIndex]);
            }
            if (item.potionEffect == PublicEnums.Effects.Party_AttackUp)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health > 0)
                    {
                        DamageNumberManager.Instance.ShowString(playerParty[i].transform.position, "ATT UP", Color.red);
                        playerParty[i].GetComponent<playerController>().playerStats.attackDamage = playerParty[i].GetComponent<playerController>().playerStats.attackDamage * 2;
                    }


                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_DefenceUp)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health > 0)
                    {
                        DamageNumberManager.Instance.ShowString(playerParty[i].transform.position, "DEF UP", Color.blue);
                        playerParty[i].GetComponent<playerController>().playerStats.Defence = playerParty[i].GetComponent<playerController>().playerStats.Defence * 2;
                    }


                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_SpeedUp)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health > 0)
                    {
                        DamageNumberManager.Instance.ShowString(playerParty[i].transform.position, "SPD UP", Color.green);
                        playerParty[i].GetComponent<playerController>().playerStats.combatSpeed = playerParty[i].GetComponent<playerController>().playerStats.combatSpeed * 2;
                    }


                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_SkillUp)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health > 0)
                    {
                        DamageNumberManager.Instance.ShowString(playerParty[i].transform.position, "SKL UP", Color.cyan);
                        playerParty[i].GetComponent<playerController>().playerStats.skillDamage = playerParty[i].GetComponent<playerController>().playerStats.skillDamage * 2;
                    }


                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_Revive)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (playerParty[i].GetComponent<playerController>().playerStats.health <= 0)
                    {
                        DamageNumberManager.Instance.ShowString(playerParty[i].transform.position, "REBORN", Color.yellow);
                        GameManager.Instance.PlayerReborn(playerParty[i]);
                    }
                }
            }
        }
        else
        {
            if (item.potionEffect == PublicEnums.Effects.None)
            {
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
            }
            if (item.potionEffect == PublicEnums.Effects.Party_None)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (enemies[i].GetComponent<playerController>().playerStats.health <= 0)
                    {
                        enemies[i].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
                    }
                }
                
            }
            if (item.potionEffect == PublicEnums.Effects.AttackDown)
            {
                DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "ATT DOWN", Color.black);
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().enemyStats.attackDamage = enemies[selectedEnemyIndex].GetComponent<playerController>().playerStats.attackDamage / 2;
                pause.Epause();
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
            }
            if (item.potionEffect == PublicEnums.Effects.DefenceDown)
            {
                DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "DEF DOWN", Color.black);
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().enemyStats.Defence = enemies[selectedEnemyIndex].GetComponent<playerController>().playerStats.attackDamage / 2;
                pause.Epause();
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
            }
            if (item.potionEffect == PublicEnums.Effects.SpeedDown)
            {
                DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "SPD DOWN", Color.black);
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().enemyStats.combatSpeed = enemies[selectedEnemyIndex].GetComponent<playerController>().playerStats.attackDamage / 2;
                pause.Epause();
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
            }
            if (item.potionEffect == PublicEnums.Effects.SkillPDown)
            {
                DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "SKL DOWN", Color.black);
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().enemyStats.skillDamage = enemies[selectedEnemyIndex].GetComponent<playerController>().playerStats.attackDamage / 2;
                pause.Epause();
                enemies[selectedEnemyIndex].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
            }
            if (item.potionEffect == PublicEnums.Effects.Party_AttackDown)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (enemies[i].GetComponent<playerController>().playerStats.health <= 0)
                    {
                        DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "ATT DOWN", Color.black);
                        pause.Epause();
                        enemies[i].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
                    }
                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_DefenceDown)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (enemies[i].GetComponent<playerController>().playerStats.health <= 0)
                    {
                        DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "DEF DOWN", Color.black);
                        pause.Epause();
                        enemies[i].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
                    }
                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_SpeedDown)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (enemies[i].GetComponent<playerController>().playerStats.health <= 0)
                    {
                        DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "SPD DOWN", Color.black);
                        pause.Epause();
                        enemies[i].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
                    }
                }
            }
            if (item.potionEffect == PublicEnums.Effects.Party_SkillDown)
            {
                for (int i = 0; i < playerParty.Count; i++)
                {
                    if (enemies[i].GetComponent<playerController>().playerStats.health <= 0)
                    {
                        DamageNumberManager.Instance.ShowString(enemies[selectedEnemyIndex].transform.position, "SKL DOWN", Color.black);
                        pause.Epause();
                        enemies[i].GetComponent<EnemyAI>().TakeSkillDamage(item.effectAmount, item.Element);
                    }
                }
            }
        }
        
        //continue if statements for other item types
        //leave this at the end to consume the item
        item.amountOfItem--;
        if(item.amountOfItem <= 0 )
        {
            InventoryManager.instance.RemoveItem(item);
        }
        HideMenu();
    }
    private void DefendFunc()
    {
        playerController.defended = playerController.playerStats.Defence;
        playerController.playerStats.Defence = playerController.playerStats.Defence * 2;
        HideMenu();
        GameManager.Instance.EndTurn();
    }

    private void FleeFunc()
    {
        if(Random.value > 0.5f)
        {
            HideMenu();
            GameManager.Instance.EndCombat();
        }
    }


    public void RemoveEnemy(GameObject enemy) 
    {
        enemies.Remove(enemy);
    }

  
}
