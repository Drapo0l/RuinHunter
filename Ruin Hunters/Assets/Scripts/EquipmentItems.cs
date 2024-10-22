using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Equipment")]
public class EquipmentItem : ScriptableObject
{
    public UnityEngine.Sprite Sprite;
    public string itemName;
    public string description;
    public int armor;
    public int itemPrice; // item price
    //public PublicEnums.ArmourTypes armourType;
}
