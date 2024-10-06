using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEnums : Item
{
    
    public enum WeaponType
    {
        None,
        Sword,
        Dagger,
        Bow,
        Lance,
        
    }

    public enum ElementType
    {
        None,
        Fire,
        Ice,
        Lightning,
        Earth,
        
    }

    public enum Regions
    {
        None,
        desert,
        plains,
        caves,
    }

    public enum Effects // these are all of the effects attacks and skills can do
    {
        None,
        Crit,
        Heal,
        Stun,
        AttackUp,
        AttackDown,
        DefenceUp,
        DefenceDown,
        SpeedUp,
        SpeedDown,
        SkillPDown,
        SkillPUP,
    }

    public enum EnemyTypes // these are the enemy types 
    {
        Normal,
        Agressive,
        CasterA,
        CasterP,
        Support,


    }

    public enum ItemType
    {
        Consumable,
        Damageable,
        Weapon,
        Equipment
    }

    [CreateAssetMenu(fileName = "NewConsumable", menuName = "Items/Consumable")]
    public class ConsumableItem : Item
    {
        public int healingAmount; // Specific to consumables

        public void Use()
        {
            // Logic for using the consumable item
        }
    }

    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
    public class Weapon : Item
    {
        public int attackPower; // Specific to weapons

        public void Equip()
        {
            // Logic for equipping the weapon
        }
    }

    //You can now easily categorize and retrieve items:
    //var consumables = InventoryManager.instance.GetItems(ItemType.Consumable);
    //var weapons = InventoryManager.instance.GetItems(ItemType.Weapon);
}
