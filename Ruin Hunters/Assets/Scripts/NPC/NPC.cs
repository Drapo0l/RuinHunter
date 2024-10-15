using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NPCManager, NPCTalkable
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;

    public bool isAShopNPC = false;

    public override void Interact()
    {
        if (dialogueManager.gameObject.activeSelf)
        {
            // If the dialogue is already active, advance to the next sentence
            dialogueManager.DisplayNextSentence(dialogue);
        }
        else
        {
            // Start the dialogue if it isn't active
            Talk(dialogue);
        }
    }

    public void Talk(Dialogue dialogueText)
    {
        if (dialogueText != null && dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogueText);
        }
    }

}