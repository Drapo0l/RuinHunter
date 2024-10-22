using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : NPCManager, NPCTalkable
{
    [SerializeField] public Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] GameObject playerGmaeOBject;
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
        if (QuestManager.instance != null)
        {
            if (completeQuest != null)
            {
                QuestManager.instance.CompleteQuest(completeQuest);
            }
            if (questForPlayer != null)
            {
                QuestManager.instance.AddQuest(questForPlayer);
            }

            // Only add to party if the playerGmaeOBject is not null
            if (playerGmaeOBject != null)
            {
                PartyManager.Instance.AddPartyMember(playerGmaeOBject);
            }
            else
            {
                Debug.Log("No playerGmaeOBject to add to the party.");
            }
        }
        else
        {
            Debug.LogError("QuestManager instance is null.");
        }
    }
}