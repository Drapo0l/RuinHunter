using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  

public class ButtonsFuncsForInventory : MonoBehaviour
{
    public Button wButton; 

    public Button sButton;


    public void UsingWandSKey() // makes it so the buttons can be selected and you can move between them with W and S keys
                                //if it doesn't happen at first, click it with mouse and use the keys and now it would work

    {

        float verticalInput = Input.GetAxis("Vertical");



        if (verticalInput > 0) // W key pressed

        {

            wButton.onClick.Invoke();

        }

        else if (verticalInput < 0) // S key pressed

        {

            sButton.onClick.Invoke();

        }

    }
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

    public void OpenItemmenu() //Opens the Accessory menu
    {
        InventoryMenu.Instance.ItemMenu();
    }

    public void OpenSkillmenu() //Opens the Accessory menu
    {
        InventoryMenu.Instance.Skillmenu();
    }

    public void GoBack() //Go back to the party info menu
    {
        InventoryMenu.Instance.returnToPartyInfo();
    }

}
