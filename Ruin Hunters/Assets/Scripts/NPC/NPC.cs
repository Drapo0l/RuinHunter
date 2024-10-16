using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : NPCManager, NPCTalkable
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField]  GameObject playerGmaeOBject;
    public Quest questForPlayer;
    public Quest completeQuest;

    public bool isPartyMemeber;
    public bool isAShopNPC = false;

    public override void Interact()
    {
        if (dialogueManager.gameObject.activeSelf)
        {
            // If the dialogue is already active, advance to the next sentence
            dialogueManager.DisplayNextSentence(dialogue, this);
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

    public void GiveQuest()
    {
        QuestManager.instance.CompleteQuest(completeQuest);
        QuestManager.instance.AddQuest(questForPlayer);        
        PartyManager.Instance.AddPartyMember(playerGmaeOBject);
    }

}