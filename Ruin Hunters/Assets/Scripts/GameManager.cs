using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject playerCamera;
    public GameObject battleCamera;

    private List<CharacterAttributes> playerParty; // list to hold player party
    private List<GameObject> battleParty;
    private List<CharacterAttributes> characters; //list to hold enmies and allies
    private List<CharacterAttributes> characterAttributes;
    public List<GameObject> playerHealths;          // list of player health/mana
    private int currentTurnIndex = 0; // index of the current character's turn

    [Header("Dependencies - No touching")]
    public bool combat = false;
    private List<CharacterAttributes> turnOrder;

    private List<CharacterAttributes> currentEnemies;// current enemies in combat
    public List<GameObject> enemyObj;
    private RegionEnemyPool colliderPool;

    private bool collisionEnemy;
    private bool wasCombatInitialized = false;
    private void Awake ()
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

    void Update()
    {
        if (combat && !wasCombatInitialized)
        {
            //begin combat if necessary
            playerParty = PartyManager.Instance.GetCurrentPartyComponent();
            StartCombat();
            wasCombatInitialized = true;
        }
        else if (combat)
        {
            SetHealthBars();
        }
    }

    public void SetEnemyPool(RegionEnemyPool _colliderPool)
    {
        colliderPool = _colliderPool;
    }

    void StartCombat()
    {
        characters = new List<CharacterAttributes>();
        
        characters.AddRange(playerParty);
       
        for (int i = 0; i < playerParty.Count; i++)
        {
            playerHealths[i].SetActive(true);
        }

        AddRandomEnemies();
        for (int i = 0; i < characters.Count; i++) // makes it so that your og stats are now saved 
        {
            characters[i].maxMana = characters[i].maxManaOG;
            characters[i].maxHealth = characters[i].maxHealthOG;
            characters[i].Defence = characters[i].DefenceOG;
            characters[i].combatSpeed = characters[i].combatSpeedOG;
            characters[i].skillDamage = characters[i].skillDamageOG;
            characters[i].attackDamage = characters[i].attackDamageOG;
            characters[i].critChance = characters[i].critChanceOG;
            characters[i].effectChance = characters[i].effectChanceOG;
        }
        SetupBattleField();

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
        if (characterAttributes == null)
        {
            characterAttributes = new List<CharacterAttributes>();
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
                characters.Add(enemy.GetComponent<EnemyAI>().enemyStats);
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
        battleParty = PartyManager.Instance.GetPlayeGameObj();
        int pos = 0;
        foreach (GameObject player in battleParty)
        {           
            player.SetActive(true);
            player.transform.SetParent(battleCamera.transform);
            player.transform.localPosition = new Vector3(3f + pos, -1.5f, 10f + pos);
            pos++;
        }
        pos = 0;

        foreach (GameObject enemy in enemyObj)
        {
            enemy.SetActive(true);
            enemy.transform.SetParent(battleCamera.transform);
            enemy.transform.localPosition = new Vector3(-7.25f + pos, -1.5f, 10.5f + pos);
            pos++;
        }
    }
    private void SetHealthBars()
    {
        List<GameObject> playerParty = PartyManager.Instance.GetPlayeGameObj();
        int index = 0;
        foreach (var playerChar in playerParty)
        {
            playerChar.transform.GetChild(0).gameObject.SetActive(true);
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
        combat = true;

        foreach (var chara in turnOrder)
        {
            chara.isTurn = false;
        }

        CharacterAttributes currentCharacter = turnOrder[currentTurnIndex];

        currentCharacter.isTurn = true;
    }

    public void EndTurn()
    {
        //move to the next character in the list
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count;
        if (currentEnemies.Count <= 0)
        {
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
            }
        }
        else
        {
            //start the next character's turn
            StartTurn();
        }
       
    }

}
