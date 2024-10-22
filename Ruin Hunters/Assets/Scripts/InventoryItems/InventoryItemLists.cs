//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;


//public class InventoryItemLists : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject _content = default; // where the items being stored in
//    [SerializeField]
//    private GameObject _menuItemTemplate = default; // for what format the new items take in the menu list of the item, like a button for example
//    [SerializeField]
//    private List<InventoryItem> inventoryItems = default;  // list for the items and how much you want in the menu for it
//    [SerializeField]
//    private ActiveInventoryItemChangeEvent AIICE = default; // to be able communicate with item details and shows what item you selected and what else

//    void Start()
//    {
//        AddMenuItems();
//        ActivateFirstItem();
//    }

//    void AddMenuItems() // to add new items in the list 
//    {
//        for (int index = 0; index < inventoryItems.Count; index++)
//        {
//            AddMenuItem(inventoryItems[index]);
//        }
//    }

//    void AddMenuItem(InventoryItem item) // Makes the new button for the item and sets it in the content and gets the name for the new item too
//    {
//        GameObject newMenuItem;
//        string label = $"   {item.label}";
//        newMenuItem = Instantiate(_menuItemTemplate, transform.position, transform.rotation);
//        newMenuItem.name = label;
//        newMenuItem.transform.SetParent(_content.transform, true);
//        newMenuItem.SetActive(true);
//        newMenuItem.GetComponentInChildren<TextMeshProUGUI>().text = label;
//        //newMenuItem.GetComponent<Item>().Item = item;    
//    }

//    void ActivateFirstItem() // always starts with the first item in the list and shows the name and details of the item on top
//    {
//        InventoryItem Activeitem = inventoryItems[0];

//        if (Activeitem != null)
//        {
//            AIICE.Invoke(Activeitem);
//        }
//    }
//}
