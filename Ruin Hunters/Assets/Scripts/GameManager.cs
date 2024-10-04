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

    private List<CharacterComponent> playerParty; // list to hold player party
    private List<GameObject> battleParty;
    private List<CharacterComponent> characters; //list to hold enmies and allies
    public List<GameObject> playerHealths;          // list of player health/mana
    private int currentTurnIndex = 0; // index of the current character's turn

    public bool combat = false;
    private List<CharacterComponent> turnOrder;

    public List<RegionEnemyPool> enemyPools; //enemy pool for every region
    private List<CharacterComponent> currentEnemies;// current enemies in combat
    public List<GameObject> enemyObj;

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

    void StartCombat()
    {
        characters = new List<CharacterComponent>();
        
        characters.AddRange(playerParty);

        for (int i = 0; i < playerParty.Count; i++)
        {
            playerHealths[i].SetActive(true);
        }

        AddRandomEnemies(playerParty[0].stats.regions);

        SetupBattleField();

        // Sort characters based on speed in descending order
        turnOrder = new List<CharacterComponent>(characters);
        characters.Sort((a, b) => b.stats.combatSpeed.CompareTo(a.stats.combatSpeed));

        currentTurnIndex = 0; // start at the first character
        StartTurn(); // start the first character's turn
    }

    void AddRandomEnemies(PublicEnums.Regions region)
    {
        // clear enemy list
        if (currentEnemies != null)
        {  
            currentEnemies.Clear(); 
        }
        

        //find the region
        PublicEnums.Regions playerPool = playerParty[0].stats.regions;
        RegionEnemyPool currentRegionPool = null;

        foreach (var pool in enemyPools) 
        {
            if(pool.region == playerPool)
            {
                currentRegionPool = pool;
                break;
            }
        }
        if (currentEnemies == null) 
        {
            currentEnemies = new List<CharacterComponent> { };
        }
       
        if (currentRegionPool != null)
        {
            enemyObj = currentRegionPool.GetEnemies();
            foreach (var enemy in enemyObj) 
            {
                CharacterComponent currEnemy = new CharacterComponent(enemy.GetComponent<EnemyAI>().enemyStats);
                currentEnemies.Add(currEnemy);
            }          
        }

        foreach (CharacterComponent enemyObj in currentEnemies) 
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
            chara.stats.isTurn = false;
        }

        CharacterComponent currentCharacter = turnOrder[currentTurnIndex];
        if (currentCharacter.stats.isStuned == true) // you are stunned 
        {
            currentCharacter.stats.isStuned = false;
            EndTurn();
        }
        currentCharacter.stats.isTurn = true;
    }

    public void EndTurn()
    {
        //move to the next character in the list
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count;

        //start the next character's turn
        StartTurn();
    }

}
