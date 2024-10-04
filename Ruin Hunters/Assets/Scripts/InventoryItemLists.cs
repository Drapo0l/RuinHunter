using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemLists : MonoBehaviour
{
    [SerializeField]
    private GameObject _content = default;
    [SerializeField]
    private GameObject _menuItemTemplate = default;
    [SerializeField]
    private List<InventoryItem> inventoryItems = default;

  



    void Awake()
    {
        AddMenuItems();
 
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
        if (item == null || _menuItemTemplate == null || _content == null)
        {
            Debug.LogError("AddMenuItem: Missing item, template, or content!");
            return;
        }

        GameObject newMenuItem = Instantiate(_menuItemTemplate);
        string label = $"  {item.label}";
        newMenuItem.name = label;

        // Set parent to content and reset local position/rotation
        newMenuItem.transform.SetParent(_content.transform, false); // false to maintain local scale
        newMenuItem.SetActive(true);

        if (newMenuItem.TryGetComponent<TextMeshProUGUI>(out var textComponent))
        {
            textComponent.text = label;
        }
        else
        {
            Debug.LogError("Text component not found in the new menu item!");
        }


    }
}
