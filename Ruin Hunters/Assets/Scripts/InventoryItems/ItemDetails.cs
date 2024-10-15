using UnityEngine;
using UnityEngine.UI;
public class ItemDetails : MonoBehaviour
{
    [SerializeField]
    private InventoryItem Item = default;
    [SerializeField]
    private Sprite ItemIMAGE;
    [SerializeField]
    private UnityEngine.UI.Text Text = default;
    public void SetItem(InventoryItem item)  // tells what item you selected and the details for it
    {
        Item = item;
        Text.text = Item.label;
        ItemIMAGE = item.ItemImage;
    }
}
