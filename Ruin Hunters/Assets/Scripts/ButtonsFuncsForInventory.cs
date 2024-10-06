using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFuncsForInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenWeaponmenu()  //Opens the Weapon menu
    {
        InventoryMenu.Instance.Weaponmenu();    
      
    }

    public void OpenAmourmenu() //Opens the Amour menu
    {
        InventoryMenu.Instance.Amourmenu();
    }

    public void OpenAccessoryymenu() //Opens the Accessory menu
    {
        InventoryMenu.Instance.Accessoryymenu();
    }

    public void GoBack() //Go back to the party info menu
    {
        InventoryMenu.Instance.returnToPartyInfo();
    }

}
