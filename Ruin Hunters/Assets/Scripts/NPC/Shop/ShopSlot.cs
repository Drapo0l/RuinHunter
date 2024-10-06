using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopSlot
{
    public Item itemData; // The inventory item
    public int amount; // Quantity of the item


    public Item GetItem => itemData;

    public ShopSlot(Item item, int amount)
    {
        this.itemData = item;
        this.amount = amount;
    }

    public void AddItem(int amountToAdd)
    {
        amount += amountToAdd;
    }

    public void RemoveItem(int amountToRemove)
    {
        amount -= amountToRemove;
        if (amount <= 0)
        {
            itemData = null; // Clear the slot if empty
        }
    }
}