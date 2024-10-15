//using System.Collections.Generic;
//using System.Xml.Serialization;
//using UnityEngine;
//using UnityEngine.Events;

//public class ItemEquiper : MonoBehaviour
//{
//    [SerializeField]
//    private ActiveInventoryItemChangeEvent activeInventoryItemChangeEvent = default;

//    private CharacterAttributes PlayerDamage;
//    private PublicEnums.WeaponType weaponType;
//    private PublicEnums.ArmourTypes armourTypes;
//    private PublicEnums.AccessoryTypes accessoryTypes;
//    private playerController Player;
//    private Dictionary<PublicEnums.WeaponType, int> weaponDamageMap;
//    private Dictionary<PublicEnums.ArmourTypes, int> ArmourDefenseMap;
//    private Dictionary<PublicEnums.AccessoryTypes, int> AccessoryBuffsMap;

//    public InventoryItem Item;

//    void Start()
//    {
//        Player = FindObjectOfType<playerController>();

//        WeaponDamageFORPLAYER();
//        ArmourDefenseFORPLAYER();
//    }

//    public void WeaponDamageFORPLAYER()
//    {
//        // Initialize the weapon damage mapping
//        weaponDamageMap = new Dictionary<PublicEnums.WeaponType, int>
//        {
//            { PublicEnums.WeaponType.None, 0 },
//            { PublicEnums.WeaponType.Sword, 18 },// Add other weapon types and their damage values here
//            { PublicEnums.WeaponType.Lance, 15 },
//            { PublicEnums.WeaponType.Bow, 12 },
//              { PublicEnums.WeaponType.Dagger, 10 },
//        };
//    }
//    public void ArmourDefenseFORPLAYER()
//    {
//        // Initialize the Armour damage mapping
//        ArmourDefenseMap = new Dictionary<PublicEnums.ArmourTypes, int>
//        {
//            { PublicEnums.ArmourTypes.None, 0 },
//            {  PublicEnums.ArmourTypes.leather,8 },// Add other Armour types and their damage values here
//            {  PublicEnums.ArmourTypes.Bronze,12 },
//            {  PublicEnums.ArmourTypes.Iron,18 },
//            {  PublicEnums.ArmourTypes.Diamond,23 },

//        };
//    }
//    public void AddWeaponDamage(PublicEnums.WeaponType weaponType)
//    {

//        if (weaponDamageMap == null)
//        {
//            Debug.LogError("weaponDamageMap is not initialized!");
//            return;
//        }
//        if (Player.playerStats.attackDamage == 0)
//        {
//            Debug.LogError("PlayerDamage is not initialized!");
//            return;
//        }
//        if (weaponDamageMap.TryGetValue(weaponType, out int weaponDamage))
//        {
//            Player.playerStats.attackDamage += weaponDamage;
//            Debug.Log($"Added {weaponDamage} damage for weapon type {weaponType}.");
//        }
//        else
//        {
//            Debug.LogError("Weapon type not found in damage map!");
//        }
//    }




//    public void AddArmourDefense(PublicEnums.ArmourTypes ArmourType)
//    {
//        if (ArmourDefenseMap == null)
//        {
//            Debug.LogError("weaponDamageMap is not initialized!");
//            return;
//        }

//        if (ArmourDefenseMap.TryGetValue(ArmourType, out int ArmourDefense))
//        {
//            Player.playerStats.Defence += ArmourDefense;
//            Debug.Log($"Removed {ArmourDefense} damage for weapon type {ArmourType}.");
//        }
//        else
//        {
//            Debug.LogError("Weapon type not found in damage map!");
//        }

//    }


//    public void ChooseWeapon()
//    {

//        if (Item == null)  // if not selected,will give a warning and return nothinh
//        {
//            Debug.LogError("No item selected to equip!");
//            return;
//        }

//        playerController player = FindObjectOfType<playerController>();
//        if (player == null) // if couldn't find the player, gives a error message
//        {
//            Debug.LogError("Player not found!");
//            return;
//        }

//        //Remove damage from the current weapon if there is one
//        if (player.equippedWeapon.weaponType != PublicEnums.WeaponType.None)
//            {
//                RemoveWeaponDamage(player.equippedWeapon.weaponType);

//            }


//        activeInventoryItemChangeEvent.Invoke(Item);  // Once you click on the button for a weapon for a example, it will be equiped on the player
//        player.EquipWeapon(Item);
//        AddWeaponDamage(Item.weaponType);
//        Debug.Log($"You equipped {Item.label}!");


//    }

//    public void ChooseArmour()
//    {

//        //playerController player = FindObjectOfType<playerController>();
//        if (Player == null) // if couldn't find the player, gives a error message
//        {
//            Debug.LogError("Player not found!");
//            return;
//        }
//        // Remove damage from the current weapon if there is one
//        if (Player.equippedArmour.ArmourType != PublicEnums.ArmourTypes.None)
//        {
//            RemoveArmourDefense(Player.equippedArmour.ArmourType);

//        }
//        activeInventoryItemChangeEvent.Invoke(Item);  // Once you click on the button for a weapon for a example, it will be equiped on the player
//        Player.EquipArmour(Item);
//        AddArmourDefense(Item.ArmourType);
//        Debug.Log($"You equipped {Item.label}!");
//    }



//    public void ChooseAccessory()
//    {

//        if (Item == null)  // if not selected,will give a warning and return nothinh
//        {
//            Debug.LogError("No item selected to equip!");
//            return;
//        }

//        playerController player = FindObjectOfType<playerController>();
//        if (player == null) // if couldn't find the player, gives a error message
//        {
//            Debug.LogError("Player not found!");
//            return;
//        }

//        activeInventoryItemChangeEvent.Invoke(Item);  // Once you click on the button for a weapon for a example, it will be equiped on the player
//        player.EquipAccessory(Item);
//        //AddWeaponDamage(Item.weaponType);
//        Debug.Log($"You equipped {Item.label}!");

//    }
//    public void RemoveWeaponDamage(PublicEnums.WeaponType weaponType)
//    {
//        if (weaponDamageMap == null)
//        {
//            Debug.LogError("weaponDamageMap is not initialized!");
//            return;
//        }

//        if (weaponDamageMap.TryGetValue(weaponType, out int weaponDamage))
//        {
//            Player.playerStats.attackDamage -= weaponDamage;
//            Debug.Log($"Removed {weaponDamage} damage for weapon type {weaponType}.");
//        }
//        else
//        {
//            Debug.LogError("Weapon type not found in damage map!");
//        }
//    }

//    public void RemoveArmourDefense(PublicEnums.ArmourTypes ArmourType)
//    {
//        if (ArmourDefenseMap == null)
//        {
//            Debug.LogError("weaponDamageMap is not initialized!");
//            return;
//        }

//        if (ArmourDefenseMap.TryGetValue(ArmourType, out int ArmourDefense))
//        {
//            Player.playerStats.Defence -= ArmourDefense;
//            Debug.Log($"Removed {ArmourDefense} damage for weapon type {ArmourType}.");
//        }
//        else
//        {
//            Debug.LogError("Weapon type not found in damage map!");
//        }
//    }

//}


