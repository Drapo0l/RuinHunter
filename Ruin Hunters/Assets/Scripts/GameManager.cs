using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    List<CharacterAttributes> playerParty; // list to hold player party
    public List<CharacterAttributes> characters; //list to hold enmies and allies
    private int currentTurnIndex = 0; // index of the current character's turn

    public bool combat = false;
    public List<CharacterAttributes> turnOrder;

    public List<RegionEnemyPool> enemyPools; //enemy pool for every region
    public List<CharacterAttributes> currentEnemies; // current enemies in combat

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

    void Start()
    {
        //begin combat if necessary
        playerParty = PartyManager.Instance.GetCurrentParty();
        StartCombat();   
    }

    void StartCombat()
    {
        characters = new List<CharacterAttributes>();
        
        characters.AddRange(playerParty);

        AddRandomEnemies(playerParty[0].regions);

        // Sort characters based on speed in descending order
        turnOrder = new List<CharacterAttributes>(characters);
        characters.Sort((a, b) => b.combatSpeed.CompareTo(a.combatSpeed));

        currentTurnIndex = 0; // start at the first character
        StartTurn(); // start the first character's turn
    }

    void AddRandomEnemies(PublicEnums.Regions region)
    {
        // clear enemy list
        currentEnemies.Clear();

        //find the region
        RegionEnemyPool pool = enemyPools.Find(r => r.region == region);

        if(pool != null)
        {
            List<GameObject> enemies = pool.GetEnemies();

            foreach (var enemy in enemies) 
            {
                enemy.transform.position = GetSpawnPosition();
                characters.Add(enemy.GetComponent<CharacterAttributes>());
            }
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

}
