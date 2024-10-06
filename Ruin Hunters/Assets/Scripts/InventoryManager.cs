using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicEnums;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    [SerializeField]  private List<Item> items = new List<Item>();
    private ItemType itemType;

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
        items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public List<Item> GetItems()
    {
        return items.FindAll(item => item.itemType == itemType);
    }

}

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public int effectAmount; // For example, healing amount
    public int amountOfItem;
    public int itemPrice; // item price
    public bool damageable;
    public PublicEnums.Effects potionEffect;

    // New variables for categorization
    public ItemType itemType; // Enum to categorize items
}

