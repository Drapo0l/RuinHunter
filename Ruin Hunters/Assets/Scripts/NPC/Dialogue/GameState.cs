using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicEnums;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; } // Singleton instance

    public Dictionary<string, string> stateVariables = new Dictionary<string, string>(); // Store state variables
    public List<string> playerChoices = new List<string>();
    public bool questUnlocked = false; // Example flag for unlocking quests
    private Dictionary<string, bool> npcStates = new Dictionary<string, bool>(); // Track NPC states
    private List<Item> inventory = new List<Item>(); // Player's inventory

    private void Awake()
    {
        // Ensure that there's only one instance of GameState
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep this instance when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void AddChoice(string choice)
    {
        playerChoices.Add(choice);
    }

    public void EvaluateEndings()
    {
        // Simple evaluation example
        if (playerChoices.Contains("helped the villagers") && playerChoices.Contains("defeated the dragon"))
        {
            // Trigger good ending
            Debug.Log("Good ending triggered!");
        }
        else
        {
            // Trigger bad ending
            Debug.Log("Bad ending triggered!");
        }
    }

    public void UnlockQuest(string questId)
    {
        questUnlocked = true; // Set quest unlocked to true (add more logic as needed)
        //Debug.Log($"Quest {questId} unlocked!");
    }

    public void UpdateInventory(Item item)
    {
        inventory.Add(item);
        Debug.Log($"{item.name} added to inventory!");
    }

    public void ChangeNPCState(string npcID, bool newState)
    {
        if (npcStates.ContainsKey(npcID))
        {
            npcStates[npcID] = newState;
        }
        else
        {
            npcStates.Add(npcID, newState);
        }
        Debug.Log($"NPC {npcID} state changed to {newState}!");
    }

    public bool GetNPCState(string npcID)
    {
        if (npcStates.ContainsKey(npcID))
        {
            return npcStates[npcID];
        }
        return false; // Default state if NPC ID not found
    }

    public void SetStateVariable(string key, string value)
    {
        if (stateVariables.ContainsKey(key))
        {
            stateVariables[key] = value;
        }
        else
        {
            stateVariables.Add(key, value);
        }
    }

    public string GetStateVariable(string key)
    {
        stateVariables.TryGetValue(key, out string value);
        return value;
    }

    public void ApplyConsequences(List<Consequence> consequences)
    {
        foreach (var consequence in consequences)
        {
            SetStateVariable(consequence.key, consequence.value);
            ExecuteAction(consequence.actionType);
        }
    }

    private void ExecuteAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.UnlockQuest:
                UnlockQuest("QuestID"); // Replace with actual quest ID
                break;
            case ActionType.ChangeNPCState:
                ChangeNPCState("NPCID", true); // Replace with actual NPC ID and state
                break;
            case ActionType.UpdateInventory:
               // UpdateInventory(new Item("ItemName")); // Replace with actual item
                break;
            case ActionType.None:
                break;
        }
    }
}