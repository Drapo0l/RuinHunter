using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private List<ShopSlot> _shopInventory;
    private int _availableGold;
    private float _buyMarkUp;
    private float _sellMarkUp;

  public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
       _availableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {

    }
}
