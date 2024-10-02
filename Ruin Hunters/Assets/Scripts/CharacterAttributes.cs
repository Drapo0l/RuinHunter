using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "NewCharacterAttributes", menuName = "Character/Attributes")]
public class CharacterAttributes : ScriptableObject
{
    public int health;
    public int maxHealth;
    public int maxMana;
    public int mana;
    public string nameOfCharacter;
    public int combatSpeed;
    public int skillDamage;
    public int attackDamage;
    public int critChance;
    public int effectChance;
    public int Defence;
    public List<Skill> skills; // List of skills specific to the character    
    public PublicEnums.Regions regions;
    public bool isTurn;
    public bool isStuned;
   

}

public class CharacterComponent : MonoBehaviour
{
    public CharacterAttributes stats;
    public CharacterComponent(CharacterAttributes _stats)
    {
        stats = _stats;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    private List<Item> items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keep inventory manager acdross scenes
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
        return items;
    }
}


[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill")]
public class AddSkill : ScriptableObject
{
    public string skillName;
    public PublicEnums.ElementType elementType;
    public int baseDamage;
    public int manaCost;
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public int effectAmount; // For example, healing amount
}