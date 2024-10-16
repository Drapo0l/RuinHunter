using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    
    public static SaveGameManager SavedScene { get; set; }


}



[System.Serializable]
public class GameData
{
    public List<CharacterAttributes> partyStats;
    public List<Item> items;
    public List<EquipmentItem> equipmentItems;
    public List<WeaponItem> weaponItems;

}