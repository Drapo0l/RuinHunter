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
            Shop();
        }
    }

    public void Shop()
    {
            shopSystem = new ShopSystem(shopItemsHeld.Items.Count, shopItemsHeld.MaxAllowedGold, shopItemsHeld.BuyMarkUp, shopItemsHeld.SellMarkUp );
        foreach ( var item in shopItemsHeld.Items )
        {
            shopSystem.AddToShop(item.itemData, item.Amount);
        }
    }

    public void Talk(Dialogue dialogueText)
    {
        //start conversation
        dialogueManager.DisplayNextSentence(dialogueText);
    }
}
