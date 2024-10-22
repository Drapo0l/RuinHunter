using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest")]
    private InventoryManager Items;
    public List<Item> items;
    public List<EquipmentItem> Armors;
    public List<WeaponItem> Weapons;
    private InventoryManager item; 
    public GameObject chest;
    public int MaxRange;
    public int MinRange;

    [Header("Audio")]
    [SerializeField] AudioSource Aud;
    [SerializeField] AudioClip OpenChestAud;
    [SerializeField] float OpenChestVol;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")&& Input.GetKeyDown(KeyCode.F))
        {
            Aud.PlayOneShot(OpenChestAud,OpenChestVol);
            RandomItemsOpen();

        }
    }
    public void RandomItemsOpen()
    {
        int itemCount = Random.Range(MinRange, MaxRange);
        List<Item> ChestItems = new List<Item>();

        for (int i = 0; i < itemCount; i++)
        {
            Item randomItem = items[Random.Range(0, items.Count)];
            EquipmentItem randomArmor = Armors[Random.Range(0, Armors.Count)];
            WeaponItem randomWeapon = Weapons[Random.Range(0, Weapons.Count)];

            if (randomItem != null)
            {

                InventoryManager.instance.AddItem(randomItem);
            }

            if (randomArmor != null)
            {
                InventoryManager.instance.AddItem(randomArmor);
            }

            if (randomWeapon != null)
            {
                InventoryManager.instance.AddItem(randomWeapon);
            }
        }


    }
}



