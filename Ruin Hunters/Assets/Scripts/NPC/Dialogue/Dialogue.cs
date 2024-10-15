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
    public string buttonText; // Text displayed on the button
    public string playerText; // Text for the player to choose
    public Dialogue nextDialogue; // The next dialogue to display after the choice
    public ActionType actionType; // Action associated with the choice
    public List<Condition> conditions; // Conditions to check for this choice
    public List<Consequence> consequences; // Consequences of this choice
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
