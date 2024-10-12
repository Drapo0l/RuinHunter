
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject playerCamera;
    public GameObject battleCamera;
    public int expTotal;
    private List<CharacterAttributes> playerParty; // list to hold player party
    public List<GameObject> battlePartyHealth = new List<GameObject>();
    public List<GameObject> battleParty = new List<GameObject>();
    private List<CharacterAttributes> characters; //list to hold enmies and allies

    public List<GameObject> playerHealths;          // list of player health/mana
    public GameObject playerHealthsParent;
    private int currentTurnIndex = 0; // index of the current character's turn

    [Header("Dependencies - No touching")]
    public bool combat = false;
    private List<CharacterAttributes> turnOrder;

    private List<CharacterAttributes> currentEnemies;// current enemies in combat
    public List<GameObject> enemyObj;
    private RegionEnemyPool colliderPool;

    public Vector3 lastPlayerPosition;

    private int amountDead = 0;
    private bool wasCombatInitialized = false;
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

    void Update()
    {
        if (combat && !wasCombatInitialized)
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
            characters[i].maxManaOG = characters[i].maxMana;
            characters[i].maxHealthOG = characters[i].maxHealth;
            characters[i].DefenceOG = characters[i].Defence;
            characters[i].combatSpeedOG = characters[i].combatSpeed;
            characters[i].skillDamageOG = characters[i].skillDamage;
            characters[i].attackDamageOG = characters[i].attackDamage;
            characters[i].critChanceOG = characters[i].critChance;
            characters[i].effectChanceOG = characters[i].effectChance;
            expTotal = characters[i].expGive + expTotal;
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
                enemy.GetComponent<EnemyAI>().postionOG = enemy.transform.position;
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
            player.GetComponent<Rigidbody>().velocity = Vector3.zero * 0;
            if (player.transform.localScale.x > 0)
                player.transform.localScale = new Vector3(Math.Abs(player.transform.localScale.x) * -1, player.transform.localScale.y, player.transform.localScale.z);
            player.SetActive(true);
            player.transform.SetParent(battleCamera.transform);
            player.transform.localPosition = new Vector3(3f + pos, 0f, 10f + pos);
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

        currentCharacter.isTurn = true;

    }

    public void EndTurn()
    {
        

        //move to the next character in the list
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count;

        //start the next character's turn
        StartTurn();
    }

    public void EnemyDeath(GameObject enemy)
    {
        currentTurnIndex--;
        enemyObj.Remove(enemy);
        turnOrder.Remove(enemy.GetComponent<EnemyAI>().enemyStats);
        if (enemyObj.Count == 0)
        {
            EndCombat();
        }
    }

    public void PlayerDeath(GameObject player)
    {
        currentTurnIndex--;
        battleParty.Remove(player);
        turnOrder.Remove(player.GetComponent<playerController>().playerStats);
        if (battleParty.Count == 0)
        {
            EndCombat();
        }
    }

    public void EndCombat()
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

        characters.Clear();
        playerParty.Clear();
        wasCombatInitialized = false;
        combat = false;
        battleCamera.SetActive(false);
        playerCamera.SetActive(true);
        playerHealthsParent.SetActive(false);

        foreach (GameObject player in battleParty)
        {
            player.SetActive(false);
            player.transform.localPosition = lastPlayerPosition;
            player.transform.SetParent(playerCamera.transform);

        }
        battleParty[0].SetActive(true);

        //move to the next character in the list






    }

}
