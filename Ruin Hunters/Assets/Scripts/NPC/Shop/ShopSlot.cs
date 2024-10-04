using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopSlot : InventoryManager
{
    public ShopSlot()
    {
       //Inventory slot kis missing
    }

    public InventoryManager GetItem { get; internal set; }
}
