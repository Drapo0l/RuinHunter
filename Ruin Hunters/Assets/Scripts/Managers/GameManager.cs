
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEngine.UIElements;
//using static UnityEditor.Progress;
//using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject playerCamera;
    public GameObject battleCamera;
    public GameObject playerParent;
    public GameObject battleUI;
    public GameObject worldEnemyParent;
    public GameObject deadMenu;
    public GameObject cursor;
    public List<Button> deadButtons;
    private int deadMenuIndex;
    public int expTotal;
    private List<CharacterAttributes> playerParty; // list to hold player party
    public List<GameObject> Grave_Yard = new List<GameObject>();
    public List<GameObject> battlePartyHealth = new List<GameObject>();
    public List<GameObject> battleParty = new List<GameObject>();
    private List<CharacterAttributes> characters; //list to hold enmies and allies

    public List<GameObject> playerHealths;  // list of player health/mana
    public GameObject playerHealthsParent;
    private int currentTurnIndex = 0; // index of the current character's turn

    [SerializeField] GameObject levelUpScreen;

    [SerializeField] CinemachineVirtualCamera playerCam;

    [Header("Dependencies - No touching")]
    public bool combat = false;
    public bool leveling = false;
    private List<CharacterAttributes> turnOrder;

    private List<CharacterAttributes> currentEnemies;// current enemies in combat
    public List<GameObject> enemyObj;
    private RegionEnemyPool colliderPool;

    public Vector3 lastPlayerPosition;
    public List<GameObject> playerLeveled = new List<GameObject>();
    private List<AudioClip[]> Effect_Sounds = new List<AudioClip[]>();
    private int totalXpForParty;
    private bool wasCombatInitialized = false;

    private List<Item> randomItems = new List<Item>();
    private int totalGold;

    public Vector3 lastSavedPosition;
    //Polo Angel's code
    // Player Spawn Location
    //public GameObject PlayerSpawnLoc;

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
        //PlayerSpawnLoc = GameObject.FindGameObjectWithTag("PlaySpawnPos");
        Cursor.visible = false;
    }

    void Update()
    {
        if (leveling)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                leveling = false;
                for (int i = 0; i < 4; i++)
                {
                    levelUpScreen.transform.GetChild(i).gameObject.SetActive(true);
                }
                levelUpScreen.SetActive(false);
            }
        }
        else if (deadMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.D)) { Navigate(-1); }
            if (Input.GetKeyDown(KeyCode.A)) { Navigate(1); }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(deadButtons[deadMenuIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == "Quit")
                {
                    Application.Quit();
                }
                else
                {
                    Continue();
                }
            }
        }
        else if (combat && !wasCombatInitialized)
        {
            playerHealthsParent.SetActive(true);
            playerParty = PartyManager.Instance.GetCurrentPartyComponent();
            StartCombat();
            wasCombatInitialized = true;
        }
        else if (combat)
        {
            SetHealthBars();
            
        }
    }

    private void Navigate(int direction)
    {
        deadMenuIndex += direction;
        if (deadMenuIndex < 0)
        {
            deadMenuIndex = deadButtons.Count - 1; // Loop to end
        }
        if (deadMenuIndex >= deadButtons.Count)
        {
            deadMenuIndex = 0;      // Loop to start
        }

        UpdateHoverIndicator();
    }

    private void UpdateHoverIndicator()
    {
        Vector3 buttonPosition = Vector3.zero;
        
        if (deadMenuIndex > deadButtons.Count - 1)
        { deadMenuIndex = deadButtons.Count - 1; }
        buttonPosition = deadButtons[deadMenuIndex].transform.position;        
       
        cursor.transform.position = new Vector3(buttonPosition.x - 150f, buttonPosition.y, buttonPosition.z);
        
    }

    public void SetEnemyPool(RegionEnemyPool _colliderPool)
    {
        colliderPool = _colliderPool;
    }

    void StartCombat()
    {
        worldEnemyParent.SetActive(false);
        QuestManager.instance.questParent.SetActive(false);

        characters = new List<CharacterAttributes>();        

        for (int i = 0; i < playerParty.Count; i++)
        {
            playerHealths[i].SetActive(true);
        }

        AddRandomEnemies();
        for (int i = 0; i < characters.Count; i++) // makes it so that your og stats are now saved 
        {
            characters[i].maxManaOG = characters[i].maxMana;
            characters[i].maxHealthOG = characters[i].maxHealth;
            characters[i].DefenceOG = characters[i].Defence;
            characters[i].combatSpeedOG = characters[i].combatSpeed;
            characters[i].skillDamageOG = characters[i].skillDamage;
            characters[i].attackDamageOG = characters[i].attackDamage;
            characters[i].critChanceOG = characters[i].critChance;
            characters[i].effectChanceOG = characters[i].effectChance;
            expTotal = characters[i].expGive + expTotal;
            if(characters[i].equipment != null)
            {
                characters[i].Defence += characters[i].equipment.armor;
            }           
        }
        SetupBattleField();

        int count = battleParty.Count;
        for (int i = 0; i < count;)
        {
            if (battleParty[i].GetComponent<playerController>().playerStats.health > 0)
            {
                characters.Add(battleParty[i].GetComponent<playerController>().playerStats);
                i++;
            }
            else
            {
                PlayerDeath(battleParty[i]);
                count--;
            }
        }

        // Sort characters based on speed in descending order
        turnOrder = new List<CharacterAttributes>(characters);
        characters.Sort((a, b) => b.combatSpeed.CompareTo(a.combatSpeed));
        turnOrder = characters;
        currentTurnIndex = 0; // start at the first character
        StartTurn(); // start the first character's turn
    }

    void AddRandomEnemies()
    {
        // clear enemy list
        if (currentEnemies != null)
        {
            currentEnemies.Clear();
        }

        if (currentEnemies == null)
        {
            currentEnemies = new List<CharacterAttributes> { };
        }

        if (colliderPool != null)
        {
            enemyObj = colliderPool.GetEnemies();
            foreach (var enemy in enemyObj)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    enemy.GetComponent<EnemyAI>().enemyStats.maxMana = enemy.GetComponent<EnemyAI>().enemyStats.maxManaOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.maxHealth = enemy.GetComponent<EnemyAI>().enemyStats.maxHealthOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.Defence = enemy.GetComponent<EnemyAI>().enemyStats.DefenceOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.combatSpeed = enemy.GetComponent<EnemyAI>().enemyStats.combatSpeedOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.skillDamage = enemy.GetComponent<EnemyAI>().enemyStats.skillDamageOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.attackDamage = enemy.GetComponent<EnemyAI>().enemyStats.attackDamageOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.critChance = enemy.GetComponent<EnemyAI>().enemyStats.critChanceOG;
                    enemy.GetComponent<EnemyAI>().enemyStats.effectChance = enemy.GetComponent<EnemyAI>().enemyStats.effectChanceOG;
                    characters.Add(enemy.GetComponent<EnemyAI>().enemyStats);
                    enemy.GetComponent<EnemyAI>().postionOG = enemy.transform.position;
                }
            }
        }

        foreach (CharacterAttributes enemyObj in currentEnemies)
        {
            characters.Add(enemyObj);
        }

    }

    void SetupBattleField()
    {
        battleCamera.SetActive(true);
        playerCamera.SetActive(false);
        battlePartyHealth = PartyManager.Instance.GetPlayeGameObj();
        battleParty = new List<GameObject>(battlePartyHealth);
        int pos = 0;
        foreach (GameObject player in battleParty)
        {
            player.GetComponent<SphereCollider>().enabled = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero * 0;
            if (player.transform.localScale.x > 0)
                player.transform.localScale = new Vector3(Math.Abs(player.transform.localScale.x) * -1, player.transform.localScale.y, player.transform.localScale.z);
            player.SetActive(true);
            player.transform.SetParent(battleCamera.transform);
            player.transform.localPosition = new Vector3(2.03f + pos, -1.28f, 7.5f + pos);
            pos++;
        }
        pos = 0;
   
        foreach (GameObject enemy in enemyObj)
        {
            enemy.SetActive(true);
            enemy.transform.SetParent(battleCamera.transform);
            enemy.transform.localPosition = new Vector3(-7.25f + pos, -1.28f, 10.5f + pos);
            pos++;
        }
    }
    private void SetHealthBars()
    {

        int index = 0;
        foreach (var playerChar in battlePartyHealth)
        {
            playerChar.gameObject.SetActive(true);
            //ManaNumber
            playerHealths[index].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = playerChar.GetComponent<playerController>().playerStats.mana.ToString() + " / " + playerChar.GetComponent<playerController>().playerStats.maxMana.ToString();
            //ManaBar
            float manabar = (float)playerChar.GetComponent<playerController>().playerStats.mana / playerChar.GetComponent<playerController>().playerStats.maxMana;
            playerHealths[index].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = manabar;
            //HealthNumber            
            playerHealths[index].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = playerChar.GetComponent<playerController>().playerStats.health.ToString() + " / " + playerChar.GetComponent<playerController>().playerStats.maxHealth.ToString();
            //HealthBar
            float healthbar = (float)playerChar.GetComponent<playerController>().playerStats.health / playerChar.GetComponent<playerController>().playerStats.maxHealth;
            playerHealths[index].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = healthbar;
            //CharacterName
            playerHealths[index].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = playerChar.GetComponent<playerController>().playerStats.nameOfCharacter;

            index++;
        }
    }

    Vector3 GetSpawnPosition()
    {
        //spawn at certain location
        return Vector3.zero; //placeholder
    }

    public void StartTurn()
    {

        foreach (var chara in turnOrder)
        {
            chara.isTurn = false;
        }

        combat = true;

        CharacterAttributes currentCharacter = turnOrder[currentTurnIndex];
        if (currentCharacter.isStuned == true)
        {

            EndTurn();
        }
        else
        {
            currentCharacter.isTurn = true;
        }


    }

    public void EndTurn()
    {
        //move to the next character in the list
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count;

        //start the next character's turn
        StartTurn();
    }

    public void EnemyDeath(GameObject enemy, Item item, int gold)
    {
        totalGold += gold;
        randomItems.Add(item);
        if (turnOrder[currentTurnIndex].combatSpeed < enemy.GetComponent<EnemyAI>().enemyStats.combatSpeed && turnOrder[currentTurnIndex] != turnOrder[turnOrder.Count - 1])
            currentTurnIndex--;
        totalXpForParty += enemy.GetComponent<EnemyAI>().enemyStats.currentXP;
        enemyObj.Remove(enemy);
        turnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
        if (enemyObj.Count == 0)
        {
            StartCoroutine(EndCombat());
        }
    }
    public void EnemyDeath(GameObject enemy, int gold)
    {
        totalGold += gold;
        if (turnOrder[currentTurnIndex].combatSpeed < enemy.GetComponent<EnemyAI>().enemyStats.combatSpeed && turnOrder[currentTurnIndex] != turnOrder[turnOrder.Count - 1])
            currentTurnIndex--;
        totalXpForParty += enemy.GetComponent<EnemyAI>().enemyStats.currentXP;
        enemyObj.Remove(enemy);
        turnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
        if (enemyObj.Count == 0)
        {
            StartCoroutine(EndCombat());
        }
    }

    public IEnumerator PlayerDeath(GameObject player)
    {
        yield return new WaitForSeconds(1f);
        currentTurnIndex--;
        battleParty.Remove(player);
        if(turnOrder != null)
            turnOrder.Remove(player.GetComponent<playerController>().playerStats);
        Grave_Yard.Add(player);
        if (battleParty.Count == 0)
        {
            deadMenu.SetActive(true);
        }
    }

    public void PlayerReborn(GameObject player)
    {
        currentTurnIndex++;
        battleParty.Add(player);
        turnOrder.Add(player.GetComponent<playerController>().playerStats);
        Grave_Yard.Remove(player);

    }

    public IEnumerator EndCombat()
    {


        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].maxMana = characters[i].maxManaOG;
            characters[i].maxHealth = characters[i].maxHealthOG;
            characters[i].Defence = characters[i].DefenceOG;
            characters[i].combatSpeed = characters[i].combatSpeedOG;
            characters[i].skillDamage = characters[i].skillDamageOG;
            characters[i].attackDamage = characters[i].attackDamageOG;
            characters[i].critChance = characters[i].critChanceOG;
            characters[i].effectChance = characters[i].effectChanceOG;
            characters[i].AddExperience(expTotal);
        }



        yield return new WaitForSeconds(2f);
        combat = false;
        battleUI.SetActive(false);
        InventoryManager.instance.Gold += totalGold;
        foreach (var player in battleParty)
        {
            DamageNumberManager.Instance.ShowNumbers(player.transform.position, totalGold, Color.yellow);
        }

        yield return new WaitForSeconds(1f);

        foreach (var item in randomItems)
        {
            InventoryManager.instance.AddItem(item);
            foreach (var player in battleParty)
            {
                DamageNumberManager.Instance.ShowString(player.transform.position, item.itemName, Color.yellow);
            }
            yield return new WaitForSeconds(1f);

        }

        foreach (var player in battleParty)
        {

            player.GetComponent<playerController>().playerStats.AddExperience((totalXpForParty / battleParty.Count));
            DamageNumberManager.Instance.ShowNumbers(player.transform.position, (totalXpForParty / battleParty.Count), Color.blue);
            if (player.GetComponent<playerController>().playerStats.currentXP < (totalXpForParty / battleParty.Count))
            {
                playerLeveled.Add(player);
            }
        }
        yield return new WaitForSeconds(1f);


        if (playerLeveled.Count != 0)
        {
            ShowLevelUpScreen();
            playerLeveled.Clear();
        }

        characters.Clear();
        playerParty.Clear();
        wasCombatInitialized = false;

        battleCamera.SetActive(false);
        playerCamera.SetActive(true);
        playerHealthsParent.SetActive(false);

        foreach (GameObject player in battleParty)
        {
            player.SetActive(false);
            player.transform.localPosition = lastPlayerPosition;
            player.transform.SetParent(playerParent.transform);
            player.transform.GetComponent<SphereCollider>().enabled = true;
        }
        battleParty[0].SetActive(true);
        playerCam.Follow = battleParty[0].transform;
        playerCam.LookAt = battleParty[0].transform;
        QuestManager.instance.questParent.SetActive(true);
        worldEnemyParent.SetActive(true);
        //move to the next character in the list
    }
    public void FleeCombat()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].maxMana = characters[i].maxManaOG;
            characters[i].maxHealth = characters[i].maxHealthOG;
            characters[i].Defence = characters[i].DefenceOG;
            characters[i].combatSpeed = characters[i].combatSpeedOG;
            characters[i].skillDamage = characters[i].skillDamageOG;
            characters[i].attackDamage = characters[i].attackDamageOG;
            characters[i].critChance = characters[i].critChanceOG;
            characters[i].effectChance = characters[i].effectChanceOG;
            characters[i].AddExperience(expTotal);
        }
        combat = false;
        battleUI.SetActive(false);
        characters.Clear();
        playerParty.Clear();
        wasCombatInitialized = false;
       
        battleCamera.SetActive(false);
        playerCamera.SetActive(true);
        playerHealthsParent.SetActive(false);

        foreach (GameObject player in battleParty)
        {
            player.SetActive(false);
            player.transform.localPosition = lastPlayerPosition;
            player.transform.SetParent(playerParent.transform);
            player.transform.GetComponent<SphereCollider>().enabled = true;
        }
        battleParty[0].SetActive(true);
        playerCam.Follow = battleParty[0].transform;
        playerCam.LookAt = battleParty[0].transform;
        QuestManager.instance.questParent.SetActive(true);
        worldEnemyParent.SetActive(true);
    }

    private void ShowLevelUpScreen()
    {
        levelUpScreen.SetActive(true);
        leveling = true;
        int child = 0;
        foreach (GameObject player in playerLeveled)
        {
            player.GetComponent<SphereCollider>().enabled = true;
            levelUpScreen.transform.GetChild(child).gameObject.SetActive(true);
            GameObject playerPanel = levelUpScreen.transform.GetChild(child).gameObject;
            CharacterAttributes characterAttributes = player.GetComponent<playerController>().playerStats;
            //sprite
            playerPanel.transform.GetChild(0).GetComponent<Image>().sprite = player.GetComponent<playerController>().Sprite;
            //name
            playerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = characterAttributes.nameOfCharacter;
            //xp
            playerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "xp: " + characterAttributes.currentXP.ToString() + "/" + characterAttributes.xpToNextLevel.ToString();
            //lvl
            playerPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "lvl: " + characterAttributes.level.ToString();
            //hp
            playerPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "hp: " + characterAttributes.maxHealthOG.ToString() + " -> " + characterAttributes.maxHealth.ToString();
            //mana
            playerPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "sp: " + characterAttributes.maxManaOG.ToString() + " -> " + characterAttributes.maxMana.ToString();
            //def
            playerPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = "def: " + characterAttributes.DefenceOG.ToString() + " -> " + characterAttributes.Defence.ToString();
            //atk
            playerPanel.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = "atk: " + characterAttributes.attackDamageOG.ToString() + " -> " + characterAttributes.attackDamage.ToString();
            //magic atk
            playerPanel.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = "matk: " + characterAttributes.skillDamageOG.ToString() + " -> " + characterAttributes.skillDamage.ToString();
            //combat speed
            playerPanel.transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = "speed: " + characterAttributes.combatSpeedOG.ToString() + " -> " + characterAttributes.combatSpeed.ToString();
            //crit
            playerPanel.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = "crit: " + characterAttributes.critChanceOG.ToString() + " -> " + characterAttributes.critChance.ToString();
            child++;
        }
    }

    private void Continue()
    {
        foreach (var player in Grave_Yard)
        {
            player.transform.position = lastSavedPosition;
            player.GetComponent<playerController>().playerStats.health = player.GetComponent<playerController>().playerStats.maxHealth;
            battleParty.Add(player);
        }
        deadMenu.SetActive(false);
        FleeCombat();
    }

    //public void playerEndCombat()
    //{
    //    FleeCombat();
    //    StartCoroutine(EndCombatRoutine());
    //}

    //private IEnumerator EndCombatRoutine()
    //{
    //    // Perform any clean-up needed to end combat
    //    foreach (var player in battleParty)
    //    {
    //        player.SetActive(false); // Disable player game objects
    //    }
    //    foreach (var enemy in enemyObj)
    //    {
    //        enemy.SetActive(false); // Disable enemy game objects
    //    }
    //    combat = false;
    //    battleUI.SetActive(false);
    //    playerHealthsParent.SetActive(false);

    //    // Activate the death menu
    //    //deathMenu.SetActive(true);

    //    yield return null;

    //}
}
