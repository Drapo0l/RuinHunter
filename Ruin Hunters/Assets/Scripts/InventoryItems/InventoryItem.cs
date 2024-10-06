using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Item", order = 0)]
public class InventoryItem : ScriptableObject
{
    public string label = "Weapon";  // what you call and can change the name of the weapon or amour or accessory in the inscpetor of the ScriptableObject  
    public PublicEnums.WeaponType weaponType;

    public PublicEnums.WeaponType GetWeaponType()    // gets the weapon type you want the weapon to be and you get it in the inscpetor of the ScriptableObject  
    {
        return weaponType;  
       
    }
}