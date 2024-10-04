using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopSystem 
{
    private List<ShopSlot> _shopInventory;
    private int _availableGold;
    private float _buyMarkUp;
    private float _sellMarkUp;

  public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
       _availableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);

        for (int i = 0; i < size; i++)
        {
            _shopInventory.Add(item: new ShopSlot());
        }
    }

    public void AddToShop(InventoryManager data, int amount)
    {
        if (ContainsItem(data, out ShopSlot shopSlot))
        {
           // shopSlot.AddItem( amount);
        }

        var freeSlot = GetFreeSlot();
       // freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.Find(i => i.GetItem == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }
        return freeSlot;
    }

    public bool ContainsItem(InventoryManager ItemToAdd, out ShopSlot shopSlot)
    {
       shopSlot = _shopInventory.Find(i => i.GetItem == ItemToAdd);
       return shopSlot != null;
    }

}
