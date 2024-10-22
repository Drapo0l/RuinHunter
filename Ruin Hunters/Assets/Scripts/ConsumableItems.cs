using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicEnums;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Consumable")]
public class Item : ScriptableObject
{
    public UnityEngine.Sprite Sprite;
    public string itemName;
    public string description;
    public int effectAmount; // For example, healing amount
    public int amountOfItem;
    public int itemPrice; // item price
    public bool damageable;
    public PublicEnums.Effects potionEffect;
    public PublicEnums.ElementType Element;
    // New variables for categorization
    public ItemType itemType; // Enum to categorize items
}
