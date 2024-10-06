using UnityEngine.Events;

[System.Serializable]   
// be able to talk to other menus and when you click on the weapon show the details on the right and be able to equip it
public class ActiveInventoryItemChangeEvent : UnityEvent<InventoryItem> { }

