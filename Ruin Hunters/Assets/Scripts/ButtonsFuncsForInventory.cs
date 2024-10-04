using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFuncsForInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenWeaponmenu() 
    {
        InventoryMenu.Instance.Weaponmenu();    
      
    }

    public void OpenAmourmenu()
    {
        InventoryMenu.Instance.Amourmenu();
    }

    public void OpenAccessoryymenu()
    {
        InventoryMenu.Instance.Accessoryymenu();
    }

    public void GoBack()
    {
        InventoryMenu.Instance.returnToPartyInfo();
    }

}
