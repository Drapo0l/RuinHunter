using UnityEngine;
using UnityEngine.UI;
public class ItemDetails : MonoBehaviour
{
    [SerializeField]
    private InventoryItem Item = default;
    [SerializeField]
    private UnityEngine.UI.Text Text = default;
    public void SetItem(InventoryItem item)
    {
        Item = item;
        Text.text = Item.label;
    }
}
