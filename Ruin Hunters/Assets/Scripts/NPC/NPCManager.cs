using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class NPCManager : MonoBehaviour, NPCInteractable
{
    [SerializeField] SpriteRenderer interactSprite;
    [SerializeField] float interactDst;

    private Transform playerTransform;

    public GameObject player { get; set; }
    public bool IsInteractable { get; set; }

    public static UnityAction<ShopSystem, InventoryItemLists> OnShoWindowRequest;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && IsWithingInteractDistance())
        {
            //interact with NPC
            Interact();
        }
        if (interactSprite.gameObject.activeSelf && !IsWithingInteractDistance())
        {
            //turn off sprite
            interactSprite.gameObject.SetActive(false);
        }
        else if (!interactSprite.gameObject.activeSelf && IsWithingInteractDistance())
        {
            //turn on sprite
            interactSprite.gameObject.SetActive(true);

        }
    }
    public abstract void Interact();//can be use for anithing 

    private bool IsWithingInteractDistance()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) < interactDst)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void OpenShopUI()
    {
        //ShopUI shopUI = FindObjectOfType<ShopUI>(); // Find the ShopUI component in the scene
        //if (shopUI != null)
        //{
        //    // Pass the shop items and player gold to the UI
        //    shopUI.Initialize(shopSystem, InventoryManager.instance.GetCurrentGold());
        //    shopUI.gameObject.SetActive(true); // Show the shop UI
        //}
    }

}
