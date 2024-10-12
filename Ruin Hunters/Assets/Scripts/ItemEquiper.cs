using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class ItemEquiper : MonoBehaviour
{
    [SerializeField]
    private ActiveInventoryItemChangeEvent activeInventoryItemChangeEvent = default;

    private CharacterAttributes PlayerDamage;
    private PublicEnums.WeaponType weaponType;
    private PublicEnums.ArmourTypes armourTypes;    
    private PublicEnums.AccessoryTypes accessoryTypes;
    private playerController player; 
    private Dictionary<PublicEnums.WeaponType, int> weaponDamageMap;

    public InventoryItem Item;

    void Start()
    {
         player = FindObjectOfType<playerController>();
        //PlayerDamage = FindObjectOfType<CharacterAttributes>();
        //if (PlayerDamage == null)
        //{
        //    Debug.LogError("CharacterAttributes instance not found in the scene!");
        //    return; // Exit early if PlayerDamage is null
        //}
        WeaponDamageFORPLAYER();
    }

    public void WeaponDamageFORPLAYER()
    {
        // Initialize the weapon damage mapping
        weaponDamageMap = new Dictionary<PublicEnums.WeaponType, int>
        {
            { PublicEnums.WeaponType.None, 0 },
            { PublicEnums.WeaponType.Sword, 18 },// Add other weapon types and their damage values here
            { PublicEnums.WeaponType.Lance, 15 },// Add other weapon types and their damage values here
            { PublicEnums.WeaponType.Bow, 10 },

        };
    }

    public void AddWeaponDamage(PublicEnums.WeaponType weaponType)
    {
       
        if (weaponDamageMap == null)
        {
            Debug.LogError("weaponDamageMap is not initialized!");
            return;
        }
        if (player.playerStats.attackDamage == 0)
        {
            Debug.LogError("PlayerDamage is not initialized!");
            return;
        }

        if (weaponDamageMap.TryGetValue(weaponType, out int weaponDamage))
        {
            player.playerStats.attackDamage += weaponDamage; 
            Debug.Log($"Added {weaponDamage} damage for weapon type {weaponType}.");
        }
        else
        {
            Debug.LogError("Weapon type not found in damage map!");
        }
    }
    public void ChooseWeapon()  
    {
   
        if (Item == null)  // if not selected,will give a warning and return nothinh
        {
            Debug.LogError("No item selected to equip!");
            return;
        }

        playerController player = FindObjectOfType<playerController>();
        if (player == null) // if couldn't find the player, gives a error message
        {
            Debug.LogError("Player not found!");
            return;
        }
     
       activeInventoryItemChangeEvent.Invoke(Item);  // Once you click on the button for a weapon for a example, it will be equiped on the player
        player.EquipWeapon(Item);
        AddWeaponDamage(Item.weaponType);
        Debug.Log($"You equipped {Item.label}!");

    }

    public void ChooseArmour()
    {

        if (Item == null)  // if not selected,will give a warning and return nothinh
        {
            Debug.LogError("No item selected to equip!");
            return;
        }

        playerController player = FindObjectOfType<playerController>();
        if (player == null) // if couldn't find the player, gives a error message
        {
            Debug.LogError("Player not found!");
            return;
        }

        activeInventoryItemChangeEvent.Invoke(Item);  // Once you click on the button for a weapon for a example, it will be equiped on the player
        player.EquipArmour(Item);
        //AddWeaponDamage(Item.weaponType);
        Debug.Log($"You equipped {Item.label}!");

    }

    public void ChooseAccessory()
    {

        if (Item == null)  // if not selected,will give a warning and return nothinh
        {
            Debug.LogError("No item selected to equip!");
            return;
        }

        playerController player = FindObjectOfType<playerController>();
        if (player == null) // if couldn't find the player, gives a error message
        {
            Debug.LogError("Player not found!");
            return;
        }

        activeInventoryItemChangeEvent.Invoke(Item);  // Once you click on the button for a weapon for a example, it will be equiped on the player
        player.EquipAccessory(Item);
        //AddWeaponDamage(Item.weaponType);
        Debug.Log($"You equipped {Item.label}!");

    }



}
