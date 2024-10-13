using UnityEngine;
using UnityEngine.Events; 

public class ItemEquiper : MonoBehaviour
{
    [SerializeField]
    private ActiveInventoryItemChangeEvent activeInventoryItemChangeEvent = default;

    private CharacterAttributes PlayerDamage;
    public PublicEnums.WeaponType weaponType;
    public PublicEnums.ArmourTypes ArmourType;
    public InventoryItem Item;
    public void ChooseItem()  
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
        player.Equip(Item);
        Debug.Log($"You equipped {Item.label}!");

    }

    private void AddWeaponDamage() // Adds the damage of the weapon to the player damage
    {
        PlayerDamage.attackDamage = (int)GetWeaponDamage();  
    }

    public PublicEnums.WeaponType GetWeaponDamage()    // gets the weapon's damage you 
    {
        return weaponType;

    }

}
