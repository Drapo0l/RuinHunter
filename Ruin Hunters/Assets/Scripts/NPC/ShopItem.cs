using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Shop System/Shop Item List")]

public class ShopItem : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> items;
    [SerializeField] private int maxGold;
    [SerializeField] private float sellMarkUp;
    [SerializeField] private float buyMarkUp;


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
public struct ShopInventoryItem
{
    //public InventoryItemData itemData;
    public int Amount;
}