using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private InventoryManager Items;
    private List<Item> inventoryitems;
    private InventoryItem item;
    public PublicEnums.WeaponType WType;
    public PublicEnums.ArmourTypes AType;
    public PublicEnums.AccessoryTypes AccessoryType;
    public int MaxRange;
    public int MinRange;
    void Start()
    {
        // Optionally initialize the Items reference
        if (Items == null)
        {
            Items = FindObjectOfType<InventoryManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RandomItemsOpen();
    }

    public void RandomItemsOpen()
    {
        int itemCount = Random.Range(MinRange, MaxRange);

        List<Item> ChestItems = new List<Item>();

        for (int i = 0; i < itemCount; i++)
        {
            Item randomItem = inventoryitems[Random.Range(0, inventoryitems.Count)];

            ChestItems.Add(randomItem);
        }
        if(item.weaponType == WType)
        {

        }
        foreach (var item in ChestItems)
        {
            Items.AddItem(item);
        }

      
    }

  

}
