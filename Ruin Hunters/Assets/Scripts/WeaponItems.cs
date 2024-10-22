using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Weapon")]
public class WeaponItem : ScriptableObject
{
    public UnityEngine.Sprite Sprite;
    public string itemName;
    public string description;
    public int damage;
    public int skillDamage;
    public int itemPrice; // item price
    public PublicEnums.WeaponType weaponType;
}
