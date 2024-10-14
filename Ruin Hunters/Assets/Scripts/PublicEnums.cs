using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicEnums : Item
{
    
    public enum WeaponType
    {
        None =0,
        Sword = 15,
        Dagger=10,
        Bow =15,
        Lance =15,
        
    }

    public enum ArmourTypes
    {
        None,
        leather,
        Bronze,
        Iron,
       Diamond,
    }

    public enum AccessoryTypes
    {
        None,
        strength,
        Health,
        Mana,
        Defense,
        Speed  
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
        Clense,
    }

    public enum EnemyTypes // these are the enemy types 
    {
        Normal,
        Agressive,
        CasterA,
        CasterP,
        Support,
        Elite_dessert_1,
        Elite_forest_1,
        Elite_ice_1,
        Elite_ruin_1,
        Boss_1_Main,
        Boss_1_R_arm,
        Boss_1_L_arm,


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
