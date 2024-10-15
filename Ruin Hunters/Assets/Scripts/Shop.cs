using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<Item> Items;
    public List<EquipmentItem> Armor;
    public List<WeaponItem> Weapons;

    public List<Button> ShopButtons;
    public List<Button> ItemButtons;
    public List<Button> YesNo;
    private Stack<List<Button>> menuStack = new Stack<List<Button>>(); // Stack for managing menus
    private List<Button> currentMenu;

    public TextMeshProUGUI playerGold;
    public GameObject itemShowcase;
    public GameObject weaponStats;
    public GameObject equipmentStats;
    public GameObject buyChoice;
    public GameObject cursor;
    public GameObject shop;
    public GameObject noGold;

    private int currentItemList;
    private int currentSelection;
    private int itemSelection;
    private int itemScrollIndex;

    private bool itemMenu;
    private bool ShopOpened;
    private bool choice;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenShop();
        }
        else if (ShopOpened && !itemMenu && !choice)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                currentItemList = (currentItemList + 1) % 3;
                Navigate(-1);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentItemList = (currentItemList - 1) % 3;
                Navigate(1);
            }
            if (Input.GetKeyDown(KeyCode.Return)) { OpenItemMenu(); }
            if (Input.GetKeyDown(KeyCode.Backspace)) { HandleBackspace(); }
        }
        else if (shop.activeSelf && itemMenu)
        {
            if (Input.GetKeyDown(KeyCode.W)) { NavigateItems(-1); }
            if (Input.GetKeyDown(KeyCode.S)) { NavigateItems(1); }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                WillBuy();
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            { HandleBackspace(); }
        }
        else if (shop.activeSelf && choice)
        {
            if (Input.GetKeyDown(KeyCode.D)) { Navigate(-1); }
            if (Input.GetKeyDown(KeyCode.A)) { Navigate(1); }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Buy());
            }
        }
        
    }

    public void OpenShop()
    {
        playerGold.text = InventoryManager.instance.Gold.ToString();
        ShopOpened = true;
        shop.SetActive(true);
        menuStack.Push(ShopButtons);
        currentMenu = ShopButtons;
    }

    public void OpenItemMenu()
    {
        itemMenu = true;
        WhatMenu();
        SwitchToMenu(ItemButtons);
    }

    private void SwitchToMenu(List<Button> newMenu)
    {
        currentMenu = newMenu;
        menuStack.Push(newMenu);
        currentSelection = 0;
        UpdateHoverIndicator();
    }

    private void Navigate(int direction)
    {
        currentSelection += direction;
        if (currentSelection < 0)
        {
            currentSelection = currentMenu.Count - 1; // Loop to end
        }
        if (currentSelection >= currentMenu.Count)
        {
            currentSelection = 0;      // Loop to start
        }

        UpdateHoverIndicator();
    }

    private void UpdateHoverIndicator()
    {
        Vector3 buttonPosition = Vector3.zero;
        if (itemMenu)
        {
            int visibleIndex = currentSelection - itemSelection;

            if (visibleIndex >= 0 && visibleIndex < currentMenu.Count)
            {
                buttonPosition = currentMenu[visibleIndex].transform.position;
            }
        }
        else
        {
            if (currentSelection > currentMenu.Count - 1)
            { currentSelection = currentMenu.Count - 1; }
            buttonPosition = currentMenu[currentSelection].transform.position;
        }
        if (itemMenu)
        {
            cursor.transform.position = new Vector3(buttonPosition.x - 300f, buttonPosition.y, buttonPosition.z);
        }
        else
        {
            cursor.transform.position = new Vector3(buttonPosition.x - 150f, buttonPosition.y, buttonPosition.z);
        }
    }

    private void NavigateItems(int direction)
    {
        currentSelection += direction;
        itemScrollIndex += direction;

        List<Item> playerItems = InventoryManager.instance.GetItems();

        if (currentSelection < 0)
        {
            // go to the last skill
            currentSelection = WhichCount() - 1;
            itemScrollIndex = Mathf.Max(0, WhichCount() - 1);
            if (currentSelection > 6)
                WhatMenu();
        }
        else if (currentSelection >= WhichCount())
        {
            //if player is at the bottom go to first skill
            currentSelection = 0;
            itemScrollIndex = 0;
            WhatMenu();
        }

        //check if need to scroll
        if (currentSelection < itemScrollIndex)
        {
            itemScrollIndex = Mathf.Max(0, itemScrollIndex - 1); //scroll up
            WhatMenu();
        }
        else if (currentSelection >= itemScrollIndex + 6)
        {
            itemScrollIndex = Mathf.Min(WhichCount() - 6, itemScrollIndex + 1); //scroll down
            WhatMenu();
        }

        WhichStats();
        UpdateHoverIndicator();
    }

    private void WhichStats()
    {
        if (currentItemList == 0)
        {
            ShowItemScreen();
        }
        else if (currentItemList == 1)
        {
            ShowEquipmentStats();
        }
        else if (currentItemList == 2)
        {
            ShowWeaponStats();
        }
    }

    private int WhichCount()
    {
        if (currentItemList == 0)
        {
            return Items.Count;
        }
        else if (currentItemList == 1)
        {
            return Weapons.Count;
        }
        else if (currentItemList == 2)
        {
            return Armor.Count;
        }
        else
        {
            return 0;
        }        
    }

    private void WhatMenu()
    {
        if (currentItemList == 0)
        {
            PopulateItemMenu(Items);
        }
        else if (currentItemList == 1)
        {
            PopulateItemMenu(Armor);
        }
        else if (currentItemList == 2)
        {
            PopulateItemMenu(Weapons);
        }
    }

    private void PopulateItemMenu(List<Item> items)
    {
        itemMenu = true;

        ItemButtons[0].transform.parent.gameObject.SetActive(true);

        List<Item> playerItems = items;

        //update buttons
        for (int i = 0; i < ItemButtons.Count; i++)
        {
            int itemIndex = itemScrollIndex + i;

            if (itemIndex < playerItems.Count)
            {
                ItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = playerItems[itemIndex].itemName;
                ItemButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                ItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                ItemButtons[i].interactable = false;
            }
        }
        ShowItemScreen();
        UpdateHoverIndicator();
    }

    private void PopulateItemMenu(List<EquipmentItem> items)
    {
        itemMenu = true;

        ItemButtons[0].transform.parent.gameObject.SetActive(true);

        List<EquipmentItem> shopItems = items;

        //update buttons
        for (int i = 0; i < ItemButtons.Count; i++)
        {
            int itemIndex = itemScrollIndex + i;

            if (itemIndex < shopItems.Count)
            {
                ItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = shopItems[itemIndex].itemName;
                ItemButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                ItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                ItemButtons[i].interactable = false;
            }
        }
        ShowEquipmentStats();
        UpdateHoverIndicator();
    }
    private void PopulateItemMenu(List<WeaponItem> items)
    {
        itemMenu = true;

        ItemButtons[0].transform.parent.gameObject.SetActive(true);

        List<WeaponItem> shopItems = items;

        //update buttons
        for (int i = 0; i < ItemButtons.Count; i++)
        {
            int itemIndex = itemScrollIndex + i;

            if (itemIndex < shopItems.Count)
            {
                ItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = shopItems[itemIndex].itemName;
                ItemButtons[i].interactable = true;
                itemIndex++;
            }
            else
            {
                ItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                ItemButtons[i].interactable = false;
            }
        }
        ShowWeaponStats();
        UpdateHoverIndicator();
    }

    private void HandleBackspace()
    {
        if (menuStack.Count > 1)
        {
            menuStack.Pop();
            currentMenu = menuStack.Peek();
            currentSelection = 0;
            itemMenu = false;
            itemScrollIndex = 0;
            equipmentStats.SetActive(false);
            weaponStats.SetActive(false);
            itemShowcase.SetActive(false);
            currentItemList = 0;
            cursor.transform.parent = shop.transform;

            UpdateHoverIndicator();

        }
    }

    private void WillBuy()
    {
        buyChoice.SetActive(true);
        SwitchToMenu(YesNo);
        itemMenu = false;
        choice = true;
        cursor.transform.parent = buyChoice.transform;
        currentSelection = 0;
        UpdateHoverIndicator();
    }

    private IEnumerator Buy()
    {
        string choice = currentMenu[currentSelection].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        if (choice == "Yes")
        {
            if (currentItemList == 0)
            {
                if (InventoryManager.instance.Gold >= Items[currentSelection].itemPrice)
                {
                    InventoryManager.instance.Gold -= Items[currentSelection].itemPrice;
                    InventoryManager.instance.AddItem(Items[currentSelection]);
                }
                else
                {
                    noGold.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    noGold.SetActive(false);
                }
            }
            else if (currentItemList == 1)
            {
                if (InventoryManager.instance.Gold >= Armor[currentSelection].itemPrice)
                {
                    InventoryManager.instance.Gold -= Armor[currentSelection].itemPrice;
                    InventoryManager.instance.AddItem(Armor[currentSelection]);
                }
                else
                {
                    noGold.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    noGold.SetActive(false);
                }
            }
            else if (currentItemList == 2)
            {
                if(InventoryManager.instance.Gold >= Weapons[currentSelection].itemPrice)
                {
                    InventoryManager.instance.Gold -= Weapons[currentSelection].itemPrice;
                    InventoryManager.instance.AddItem(Weapons[currentSelection]);
                }
                else
                {
                    noGold.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    noGold.SetActive(false);
                }
            }
            
        }
        
        buyChoice.SetActive(false);
        menuStack.Pop();
        currentMenu = menuStack.Peek();
        itemMenu = true;
        this.choice = false;
        cursor.transform.parent = shop.transform;
        currentSelection = 0;
        UpdateHoverIndicator();
        
        playerGold.text = InventoryManager.instance.Gold.ToString();
    }

    private void ShowItemScreen()
    {
        itemShowcase.SetActive(true);
        
        Item item = Items[itemScrollIndex];

        //name
        itemShowcase.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
        //sprite
        itemShowcase.transform.GetChild(1).GetComponent<Image>().sprite = item.Sprite;
        //effect
        itemShowcase.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Effect: " + item.potionEffect.ToString();
        //gold
        itemShowcase.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Gold: " + item.itemPrice.ToString();
        //amount
        itemShowcase.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Amount: " + item.effectAmount.ToString();
        //description
        itemShowcase.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = item.description;
    }

    private void ShowWeaponStats()
    {
        weaponStats.SetActive(true);

        WeaponItem item = Weapons[itemScrollIndex];

        if (item != null)
        {
            weaponStats.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.name;
            weaponStats.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = item.Sprite;
            weaponStats.transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Atk: " + item.damage.ToString();
            weaponStats.transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Gold: " + item.itemPrice.ToString();
            weaponStats.transform.GetChild(4).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Skill Damage: " + item.skillDamage.ToString();
            weaponStats.transform.GetChild(6).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.description;
        }
    }

    private void ShowEquipmentStats()
    {
        equipmentStats.SetActive(true);

        EquipmentItem item = Armor[itemScrollIndex];

        if (item != null)
        {
            equipmentStats.transform.GetChild(0).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.name;
            equipmentStats.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = item.Sprite;
            equipmentStats.transform.GetChild(2).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Defense: " + item.armor.ToString();
            equipmentStats.transform.GetChild(3).gameObject.transform.GetComponent<TextMeshProUGUI>().text = "Gold: " + item.itemPrice.ToString();
            equipmentStats.transform.GetChild(5).gameObject.transform.GetComponent<TextMeshProUGUI>().text = item.description;
        }
    }

}
