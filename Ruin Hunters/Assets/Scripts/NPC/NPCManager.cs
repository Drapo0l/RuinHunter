using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public abstract class NPCManager : MonoBehaviour, NPCInteractable
{
    [SerializeField] SpriteRenderer interactSprite;
    [SerializeField] TMP_Text interactText; // Reference to the TMP_Text for the button hint
    [SerializeField] float interactDst;
    [SerializeField] List<AudioClip> proximitySounds; // Add this line for multiple proximity sounds
    [SerializeField] AudioSource audioSource; // AudioSource to play the sounds
    private Transform playerTransform;
    private bool hasPlayedProximitySound = false; // Flag to track if the sound has played
    private bool isDialogueActive = false; // Track if dialogue is active
    public GameObject player { get; set; }
    public bool IsInteractable { get; set; }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        interactSprite.gameObject.SetActive(false); // Ensure the sprite is initially disabled
        interactText.gameObject.SetActive(false); // Ensure the text is initially disabled
    }

    void Update()
    {
        bool withinDistance = IsWithinInteractDistance();

        // Enable or disable the interact sprite and text based on distance
        if (withinDistance)
        {
            if (!interactSprite.gameObject.activeSelf)
            {
                interactSprite.gameObject.SetActive(true); // Show sprite
            }
            if (!interactText.gameObject.activeSelf)
            {
                interactText.gameObject.SetActive(true); // Show text
            }
            if (!hasPlayedProximitySound && proximitySounds.Count > 0)
            {
                AudioClip randomClip = proximitySounds[Random.Range(0, proximitySounds.Count)];
                audioSource.clip = randomClip;
                audioSource.Play(); // Play proximity sound once
                hasPlayedProximitySound = true; // Set the flag to true
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                // Interact with NPC
                Interact();
                isDialogueActive = true;
            }
        }
        else
        {
            if (interactSprite.gameObject.activeSelf)
            {
                interactSprite.gameObject.SetActive(false); // Hide sprite
            }
            if (interactText.gameObject.activeSelf)
            {
                interactText.gameObject.SetActive(false); // Hide text
            }
            hasPlayedProximitySound = false; // Reset the flag when player leaves the area

            // Check if dialogue needs to be ended
            if (isDialogueActive)
            {
                DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
                if (dialogueManager != null && dialogueManager.gameObject.activeSelf)
                {
                    dialogueManager.EndChoicesAndDialogue(); // End the dialogue and choices if player moves away
                    isDialogueActive = false;
                }
            }
        }
    }

    public abstract void Interact(); // Can be used for anything

    private bool IsWithinInteractDistance()
    {
        return Vector3.Distance(playerTransform.position, transform.position) < interactDst;
    }

    public void HideInteractElements()
    {
        interactSprite.gameObject.SetActive(false); // Hide sprite
        interactText.gameObject.SetActive(false); // Hide text
    }
}//public void OpenShopUI()
 //    {
 //        // ShopUI shopUI = FindObjectOfType<ShopUI>(); // Find the ShopUI component in the scene
 //        // if (shopUI != null)
 //        // {
 //        //     // Pass the shop items and player gold to the UI
 //        //     shopUI.Initialize(shopSystem, InventoryManager.instance.GetCurrentGold());
 //        //     shopUI.gameObject.SetActive(true); // Show the shop UI
 //        // }
 //    }
 //}