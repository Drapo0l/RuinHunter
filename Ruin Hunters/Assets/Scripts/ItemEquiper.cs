using UnityEngine;
using UnityEngine.Events; 

public class ItemEquiper : MonoBehaviour
{
    [SerializeField]
    private ActiveInventoryItemChangeEvent activeInventoryItemChangeEvent = default;

    public InventoryItem Item;



    private bool EquipingArmour = false;
    private bool EquipingWeaon = false;
    private bool EquipingAccessory = false;
    public void ChooseItem()
    {
        if (Item == null)
        {
            Debug.LogError("No item selected to equip!");
            return;
        }

        CharacterAttributes player = FindObjectOfType<CharacterAttributes>();
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        activeInventoryItemChangeEvent.Invoke(Item);
        player.Equip(Item);
        Debug.Log($"You equipped {Item.label}!");

    }

    
}
