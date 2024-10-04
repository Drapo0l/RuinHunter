using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : NPCManager, NPCTalkable, NPCShop
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private ShopItem shopItemsHeld;
    private ShopSystem shopSystem;

    public bool isAShopNPC = false;


    public override void Interact()
    {
        Talk(dialogue);

        if (isAShopNPC)
        {
            Shop(dialogue);
        }
    }

    public void Shop(Dialogue dialogueText)
    {
        throw new System.NotImplementedException();
    }

    public void Talk(Dialogue dialogueText)
    {
        //start conversation
        dialogueManager.DisplayNextSentence(dialogueText);
    }
}
