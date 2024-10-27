
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
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
    public GameObject endingScreenUI; 
    public List<Button> deadButtons;
    private int deadMenuIndex;
    public int expTotal;
    public List<CharacterAttributes> playerParty; // list to hold player party
    public List<GameObject> Grave_Yard = new List<GameObject>();
    public List<GameObject> battlePartyHealth = new List<GameObject>();
    public List<GameObject> battleParty = new List<GameObject>();
    public List<GameObject> weaknessBars;
    private List<CharacterAttributes> characters; //list to hold enmies and allies

    public List<GameObject> turnOrderSprytes;
    public GameObject turnOrderDividor;
    public List<GameObject> playerHealths;          // list of player health/mana
    public GameObject playerHealthsParent;
    public GameObject turnOrderParent;

    private int currentTurnIndex = 0; // index of the current character's turn

    [SerializeField] GameObject levelUpScreen;

    [SerializeField] CinemachineVirtualCamera playerCam;

    [Header("Sounds")]
    [SerializeField] AudioSource Aud;
    [SerializeField] AudioClip defeatSound;
    [SerializeField] float AudiodefeatVol;
    [SerializeField] AudioClip defeatMusic;
    [SerializeField] float AudiodefeatMVol;
    [SerializeField] AudioClip levelUpSound;
    [SerializeField] float AudioLevelUpVol;
    [SerializeField] AudioClip levelUpMusic;
    [SerializeField] float AudioLevelUpMVol;
    [SerializeField] AudioClip winSound;
    [SerializeField] float AudioWinVol;
    [SerializeField] List<AudioClip> fightMusic;
    [SerializeField] float AudioFightVol;

    private int randomSound;

    [Header("Dependencies - No touching")]
    public bool combat = false;
    public bool leveling = false;
    private List<CharacterAttributes> turnOrder;
    private List<CharacterAttributes> futureTurnOrder;

    private List<CharacterAttributes> currentEnemies;// current enemies in combat
    public List<GameObject> enemyObj;
    private RegionEnemyPool colliderPool;

    public Vector3 lastPlayerPosition;
    public List<GameObject> playerLeveled = new List<GameObject>();

    private int totalXpForParty;
    private bool wasCombatInitialized = false;

    private List<Item> randomItems = new List<Item>();
    public int totalGold;

    public Vector3 lastSavedPosition;

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
        Cursor.visible = false;
    }

    void Update()
    {
        if (leveling)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Aud.Stop();
                leveling = false;
                for (int i = 0; i < 4; i++)
                {
                    levelUpScreen.transform.GetChild(i).gameObject.SetActive(false);
                }
                levelUpScreen.SetActive(false);
                worldEnemyParent.SetActive(true);
                QuestManager.instance.UpdateQuestDisplay();
                foreach (GameObject player in battleParty)
                {
                    if (player != null)
                    {
                        player.SetActive(false);
                        player.transform.position = lastPlayerPosition;
                        player.transform.SetParent(playerParent.transform);
                        player.transform.GetComponent<SphereCollider>().enabled = true;
                    }
                }
                battleParty[0].SetActive(true);
                characters.Clear();
                playerParty.Clear();
                wasCombatInitialized = false;
                battleCamera.SetActive(false);
                playerCamera.SetActive(true);
                playerHealthsParent.SetActive(false);

            }
            if (!Aud.isPlaying)
            {
                if (Aud.clip != null)
                {
                    Aud.clip = levelUpMusic;
                    Aud.Play();
                }
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
            wasCombatInitialized = true;
            StartCombat();
           
        }
        else if (combat)
        {
            if (!Aud.isPlaying)
            {
                Aud.Play();
            }
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
        playerCam.gameObject.SetActive(false);
        battleCamera.SetActive(true);
        battleUI.SetActive(true);
        randomSound = UnityEngine.Random.Range(0, fightMusic.Count);
        Aud.volume = AudioFightVol;
        Aud.clip = fightMusic[randomSound];

        //BattleMusic.Play();
        worldEnemyParent.SetActive(false);
        QuestManager.instance.questParent.SetActive(false);

        characters = new List<CharacterAttributes>();        

        for (int i = 0; i < playerParty.Count; i++)
        {
            playerHealths[i].SetActive(true);
        }

        AddRandomEnemies();
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
                Grave_Yard.Add(battleParty[i]);
                count--;
            }
        }


        for (int i = 0; i < characters.Count; i++) // makes it so that your og stats are now saved 
        {
            characters[i].maxManaOG = characters[i].maxMana;
            characters[i].maxHealthOG = characters[i].maxHealth;
            characters[i].DefenceOG = characters[i].Defence;
            characters[i].combatSpeedOG = characters[i].combatSpeed;
            characters[i].skillDamageOG = characters[i].skillDamage;
            characters[i].attackDamageOG = characters[i].attackDamage;
            characters[i].critChanceOG = characters[i].critChance;
            expTotal = characters[i].expGive + expTotal;
            if (characters[i].equipment != null)
            {
                characters[i].Defence += characters[i].equipment.armor;
            }
        }

        

        turnOrder = new List<CharacterAttributes>(characters);
        characters.Sort((a, b) => b.combatSpeed.CompareTo(a.combatSpeed));
        turnOrder = characters;
        futureTurnOrder = new List<CharacterAttributes>(turnOrder);
        ShowCurrentTurnOrder();
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
            int index = 0;
            foreach (var enemy in enemyObj)
            {
                enemy.GetComponent<EnemyAI>().setUpStats();
                currentEnemies.Add(enemy.GetComponent<EnemyAI>().enemyStats);


                currentEnemies[index].maxManaOG = currentEnemies[index].maxMana;
                currentEnemies[index].maxHealthOG = currentEnemies[index].maxHealth;
                currentEnemies[index].DefenceOG = currentEnemies[index].Defence;
                currentEnemies[index].combatSpeedOG = currentEnemies[index].combatSpeed;
                currentEnemies[index].skillDamageOG = currentEnemies[index].skillDamage;
                currentEnemies[index].attackDamageOG = currentEnemies[index].attackDamage;
                currentEnemies[index].critChanceOG = currentEnemies[index].critChance;
                characters.Add(enemy.GetComponent<EnemyAI>().enemyStats);
                enemy.GetComponent<EnemyAI>().postionOG = enemy.transform.position;
                index++;
            }
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
            player.GetComponent<playerController>().playerAnimator.SetBool("moving", false);
            player.GetComponent<SphereCollider>().enabled = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero * 0;
            if (player.transform.localScale.x > 0)
                player.transform.localScale = new Vector3(Math.Abs(player.transform.localScale.x) * -1, player.transform.localScale.y, player.transform.localScale.z);
            player.SetActive(true);
            player.transform.SetParent(battleCamera.transform);
            player.transform.localPosition = new Vector3(2.03f + pos, -1.28f, 7.5f + pos);
            pos++;

        }
        Vector3[] positions = new Vector3[]
        {
            new Vector3(629.849976f,3.51999998f,297.48999f),   // Enemy 1: Top
            new Vector3(628.929993f,3.16000009f,294.589996f),  // Enemy 2: Middle-left
            new Vector3(634.309998f,3.17000008f,294.179993f),   // Enemy 3: Middle-right
            new Vector3(632.200012f,2.52999997f,291.799988f)   // Enemy 4: Bottom
        };

        pos = 0;
   
        foreach (GameObject enemy in enemyObj)
        {
            enemy.transform.localPosition = positions[pos];
            enemy.GetComponent<EnemyAI>().weaknesBar = weaknessBars[pos];
            weaknessBars[pos].SetActive(true);
            enemy.GetComponent<EnemyAI>().weaknesBar.GetComponent<WeknessManager>().weaknessBar = weaknessBars[pos];
            enemy.GetComponent<EnemyAI>().weaknesBar.GetComponent<WeknessManager>().SettupWeakness(enemy.GetComponent<EnemyAI>().enemyStats.weaknessIcons, enemy.transform.position, battleCamera.GetComponent<Camera>(), enemy.GetComponent<EnemyAI>());
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

        foreach (var chara in futureTurnOrder)
        {
            chara.isTurn = false;
        }

        combat = true;

        CharacterAttributes currentCharacter = turnOrder[0];
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
        if (futureTurnOrder != null)
            futureTurnOrder.Sort((a, b) => b.combatSpeed.CompareTo(a.combatSpeed));
        turnOrder.RemoveAt(0);
        //move to the next character in the list
        if (turnOrder.Count == 0)
        {
            SetNextTurnOrder();
        }
        ShowCurrentTurnOrder();
        //start the next character's turn
        StartTurn();
    }

    private void SetNextTurnOrder()
    {
        turnOrder = new List<CharacterAttributes>(futureTurnOrder);
    }
    private void ShowCurrentTurnOrder()
    {
        int index = 0;
        foreach (var chara in turnOrder)
        {
            turnOrderSprytes[index].SetActive(true);
            if (chara.Sprite != null)
            {
                turnOrderSprytes[index].GetComponent<Image>().sprite = chara.Sprite;
                turnOrderSprytes[index].GetComponent<Image>().SetNativeSize();
            }
            index++;
        }
        turnOrderDividor.transform.position = turnOrderSprytes[index].transform.position;
        turnOrderDividor.transform.position -= new Vector3(40, 0, 0);
        foreach (var chara in futureTurnOrder)
        {
            turnOrderSprytes[index].SetActive(true);
            if (chara.Sprite != null)
            {
                turnOrderSprytes[index].GetComponent<Image>().sprite = chara.Sprite;
                turnOrderSprytes[index].GetComponent<Image>().SetNativeSize();
            }
            index++;
        }
        while (index != turnOrderSprytes.Count)
        {
            turnOrderSprytes[index].SetActive(false);
            index++;
        }
    }

    public void EnemyDeath(GameObject enemy, Item item, int gold)
    {
        totalGold += gold;
        randomItems.Add(item);
        if (turnOrder[currentTurnIndex].combatSpeed < enemy.GetComponent<EnemyAI>().enemyStats.combatSpeed && turnOrder[currentTurnIndex] != turnOrder[turnOrder.Count - 1])
            currentTurnIndex--;
        totalXpForParty += enemy.GetComponent<EnemyAI>().enemyStats.currentXP;
        enemyObj.Remove(enemy);
        if (turnOrder.Contains(enemy.GetComponent<EnemyAI>().enemyStats))
            turnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
        futureTurnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
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
        if (turnOrder.Contains(enemy.GetComponent<EnemyAI>().enemyStats))
            turnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
        futureTurnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
        if (enemyObj.Count == 0)
        {
            Aud.clip = winSound;
            Aud.volume = AudioWinVol;
            Aud.Play();
            StartCoroutine(EndCombat());
        }
    }

    public void PlayerDeath(GameObject player)
    {
        //yield return new WaitForSeconds(1f);

        if (turnOrder.Contains(player.GetComponent<playerController>().playerStats))
            turnOrder.Remove(player.GetComponent<playerController>().playerStats);
        if (futureTurnOrder != null)
            futureTurnOrder.Remove(player.GetComponent<playerController>().playerStats);
        Grave_Yard.Add(player);
        battleParty.Remove(player);
        if (battleParty.Count == 0)
        {
            Aud.clip = defeatSound;
            Aud.volume = AudiodefeatVol;
            Aud.Play();
            foreach(var health in playerHealths)
            {
                health.SetActive(false);
            }
            battleUI.SetActive(false);

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
        playerCam.gameObject.SetActive(true);
        foreach (var bar in weaknessBars)
        {
            bar.GetComponent<WeknessManager>().ClearWeakness();
        }

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

        //foreach (var item in randomItems)
        //{
        //    InventoryManager.instance.AddItem(item);
        //    foreach (var player in battleParty)
        //    {
        //        DamageNumberManager.Instance.ShowString(player.transform.position, item.itemName, Color.yellow);
        //    }
        //    yield return new WaitForSeconds(1f);

        //}

        foreach (var player in battleParty)
        {

            player.GetComponent<playerController>().playerStats.AddExperience((totalXpForParty / battleParty.Count));
            DamageNumberManager.Instance.ShowNumbers(player.transform.position, (totalXpForParty / battleParty.Count), Color.blue);
            if (player.GetComponent<playerController>().playerStats.currentXP < (totalXpForParty / battleParty.Count))
            {
                playerLeveled.Add(player);
            }
        }
        if (playerLeveled.Count != 0)
        {
            Aud.clip = levelUpSound;
            Aud.volume = AudioLevelUpVol;
            Aud.Play();
        }
        yield return new WaitForSeconds(1.5f);


        if (playerLeveled.Count != 0)
        {
            Aud.clip = levelUpMusic;
            Aud.volume = AudioLevelUpMVol;
            Aud.Play();
            ShowLevelUpScreen();
            playerLeveled.Clear();
        }

        if (playerCam != null)
        {
            playerCam.Follow = battleParty[0].transform;
            playerCam.LookAt = battleParty[0].transform;
        }
        QuestManager.instance.questParent.SetActive(true);
        if (!leveling)
        {
            worldEnemyParent.SetActive(true);
            foreach (GameObject player in battleParty)
            {
                if (player != null)
                {
                    player.SetActive(false);
                    player.transform.position = lastPlayerPosition;
                    player.transform.SetParent(playerParent.transform);
                    player.transform.GetComponent<SphereCollider>().enabled = true;
                }
            }
            battleParty[0].SetActive(true);
            characters.Clear();
            playerParty.Clear();
            wasCombatInitialized = false;
            battleCamera.SetActive(false);
            playerCamera.SetActive(true);
            playerHealthsParent.SetActive(false);
        }
        //move to the next character in the list
    }
    public void FleeCombat()
    {
        playerCam.gameObject.SetActive(true);
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
        while (enemyObj.Count != 0 )
        {
            Destroy(enemyObj[0]);
            enemyObj.RemoveAt(0);
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
            player.transform.position = lastPlayerPosition;
            player.transform.SetParent(playerParent.transform);
            player.transform.GetComponent<SphereCollider>().enabled = true;
        }
        battleParty[0].SetActive(true);
        playerCam.Follow = battleParty[0].transform;
        playerCam.LookAt = battleParty[0].transform;
        QuestManager.instance.questParent.SetActive(true);
        if (!leveling)
            worldEnemyParent.SetActive(true);
            QuestManager.instance.UpdateQuestDisplay();        
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

    public void PlayerBeatsGame()
    {
        // Show the ending screen UI
        endingScreenUI.SetActive(true);
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
