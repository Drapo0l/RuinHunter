using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class InventoryItemLists : MonoBehaviour
{
    [SerializeField]
    private GameObject _content = default;
    [SerializeField]
    private GameObject _menuItemTemplate = default;
    [SerializeField]
    private List<InventoryItem> inventoryItems = default;
    [SerializeField]
    private ActiveInventoryItemChangeEvent AIICE = default;

    void Awake()
    {
        AddMenuItems();
        ActivateFirstItem();


    }

    void AddMenuItems()
    {
        for (int index = 0; index < inventoryItems.Count; index++)
        {
            AddMenuItem(inventoryItems[index]);
        }
    }

    void AddMenuItem(InventoryItem item)
    {
        GameObject newMenuItem;
        string label = $"   {item.label}";
        newMenuItem = Instantiate(_menuItemTemplate, transform.position, transform.rotation);
        newMenuItem.name = label;
        newMenuItem.transform.SetParent(_content.transform, true);
        newMenuItem.SetActive(true);
        newMenuItem.GetComponentInChildren<TextMeshProUGUI>().text = label;
        newMenuItem.GetComponent<ItemEquiper>().Item = item;    
    }

    void ActivateFirstItem()
    {
        InventoryItem Activeitem = inventoryItems[0];

        if (Activeitem != null)
        {
            AIICE.Invoke(Activeitem);   
        }
    }
}
