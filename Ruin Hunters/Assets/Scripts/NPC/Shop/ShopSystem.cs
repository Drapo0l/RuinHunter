using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopSystem : MonoBehaviour 
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
    }

    public void AddToShop(Item itemData, int amount)
    {
        if (ContainsItem(itemData, out ShopSlot shopSlot))
        {
            shopSlot.AddItem(amount);
        }
        else
        {
            var freeSlot = GetFreeSlot();
            freeSlot.itemData = itemData; // Assign the Item
            freeSlot.amount = amount; // Set the amount
        }
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.Find(i => i.GetItem == null);
        if (freeSlot == null)
        {
            freeSlot = new ShopSlot(null, 0);
            _shopInventory.Add(freeSlot);
        }
        return freeSlot;
    }

    public bool BuyItem(Item item, int amount, ref int playerGold)
    {
        float cost = item.itemPrice * _buyMarkUp * amount; // Calculate total cost
        if (playerGold >= cost)
        {
            playerGold -= (int)cost; // Deduct from player gold
            InventoryManager.instance.AddItem(item); // Make sure this is correct based on your InventoryManager logic
            return true; // Purchase successful
        }
        return false; // Not enough gold
    }

    public bool ContainsItem(Item itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = _shopInventory.Find(i => i.GetItem == itemToAdd);
        return shopSlot != null;
    }

    public void Initialize(ShopSystem shopSystem, int playerGold)
    {
        UpdatePlayerGold(playerGold); // Update player's gold display
        PopulateShopItems(shopSystem); // Populate the shop items in the UI
    }

    private void UpdatePlayerGold(int playerGold)
    {
     //   _playerGoldText.text = $"Gold: {playerGold}";
    }

    private void PopulateShopItems(ShopSystem shopSystem)
    {
        // Clear existing items
     //   foreach (Transform child in _itemListContentPanel.transform)
        {
     //       Destroy(child.gameObject);
        }

        // Loop through the items in the shop system and create UI elements
     //   foreach (var shopItem in shopSystem.GetShopItems())
        {
     //       ShopSlotUI shopSlotUI = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
     //       shopSlotUI.Initialize(shopItem, this); // Pass the shop item and the UI reference
        }
    }
}