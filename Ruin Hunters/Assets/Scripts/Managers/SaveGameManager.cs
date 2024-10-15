using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


[System.Serializable]
public class GameData
{
    public List<CharacterAttributes> partyStats;
    public List<Item> items;
    public List<EquipmentItem> equipmentItems;
    public List<WeaponItem> weaponItems;


}