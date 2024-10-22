using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PublicEnums;

public class InventoryManager : MonoBehaviour
{   

    public static InventoryManager instance { get; private set; }

    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<EquipmentItem> equipmentItems = new List<EquipmentItem>();
    [SerializeField] private List<WeaponItem> weaponItems = new List<WeaponItem>();


    private ItemType itemType;
    public int Gold;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // no duplicates
        }
    }

    public void AddItem(Item item)
    {
        Item existingItem = items.Find(i => i.name == item.name);
        if (existingItem != null)
        {
            existingItem.amountOfItem += item.amountOfItem;
        }
        else
        {
            items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public void AddItem(EquipmentItem item)
    {
        Item existingItem = items.Find(i => i.name == item.name);
        if (existingItem == null)
        {
            equipmentItems.Add(item);
        }
    }

    public void RemoveItem(EquipmentItem item)
    {
        equipmentItems.Remove(item);
    }
    public List<EquipmentItem> GetEquipmentItems()
    {
        return equipmentItems;
    }

    public void AddItem(WeaponItem item)
    {
        Item existingItem = items.Find(i => i.name == item.name);
        if (existingItem == null)
        {
            weaponItems.Add(item);
        }
    }

    public void RemoveItem(WeaponItem item)
    {
        weaponItems.Remove(item);
    }

    public List<WeaponItem> GetWeaponItems()
    {
        return weaponItems;
    }
}

