using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public List<CharacterAttributes> characters; //list to hold enmies and allies
    private int currentTurnIndex = 0; // index of the current character's turn

    public bool combat = false;
    public List <CharacterAttributes> turnOrder; // list to hold the characters sorted by speed

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
        StartCombat();   
    }

    void StartCombat()
    {
        // Sort characters based on speed in descending order
        turnOrder = new List<CharacterAttributes>(characters);
        characters.Sort((a, b) => b.combatSpeed.CompareTo(a.combatSpeed));

        currentTurnIndex = 0; // start at the first character
        StartTurn(); // start the first character's turn
    }

    public void StartTurn()
    {
        combat = true;
        CharacterAttributes currentCharacter = turnOrder[currentTurnIndex];
        playerController player;
        // check if it's the player's turn or an enemy's turn
        if ( currentCharacter.isPlayerControlled)
        {
            //Get the correct player controller based on this character
            //playerController playerController = FindPlayerControllerByCharacter(currentCharacter);
            //playerController.StartTurn();
        }
        //else
        //{
        //    // get the enemy controller for this character
        //    EnemyAI enemy = FindEnemyByName(currentCharacter.nameOfCharacter);
        //    enemy.StartTurn(); 
        //}    
    }

    //playerController FindPlayerControllerByCharacter(CharacterAttributes character)
    //{
    //    // find the correct playerController based on the character's name
    //    return FindObjectOfType<playerController>().FirstOrDefault(pc => pc.characterAttributes.nameOfCharacter == character.nameOfCharacter);
    //}

    public void EndTurn()
    {
        //move to the next character in the list
        currentTurnIndex = (currentTurnIndex + 1) % characters.Count;

        //start the next character's turn
        StartTurn();
    }

}
