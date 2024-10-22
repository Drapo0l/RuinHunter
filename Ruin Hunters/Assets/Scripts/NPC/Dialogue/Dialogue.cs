using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicEnums;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue Container")]
public class Dialogue : ScriptableObject
{
    public string NPCName;

    [TextArea(3, 10)]
    public string[] sentences;
    public List<DialogueChoice> choices; // List of choices associated with the dialogue
}

[System.Serializable]
public class DialogueChoice
{
    public string buttonText;
    public string playerText;
    public Dialogue nextDialogue;
    public ActionType actionType;
    public List<Condition> conditions;
    public List<Consequence> consequences;
    public bool choiceMade = false;
    public bool neverDisable = false; // Flag to mark choices that should never be disabled

    // Check if all nested choices are completed
    public bool AreAllNestedChoicesCompleted()
    {
        if (nextDialogue == null || nextDialogue.choices == null)
        {
            return true; // No next dialogue or choices means it's complete
        }

        foreach (var choice in nextDialogue.choices)
        {
            if (!choice.choiceMade)
            {
                return false; // If any nested choice is not made, return false
            }
        }

        return true; // All nested choices are made
    }
}

[System.Serializable]
public class Condition
{
    public string key; // Condition key
    public string value; // Condition value
}

[System.Serializable]
public class Consequence
{
    public string key; // Consequence key
    public string value; // Consequence value
    public ActionType actionType; // Action associated with the consequence
}
