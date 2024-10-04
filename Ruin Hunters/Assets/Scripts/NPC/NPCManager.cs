using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPCManager : MonoBehaviour, NPCInteractable
{
    [SerializeField]  SpriteRenderer interactSprite;
    [SerializeField]  float interactDst;

    private Transform playerTransform;

    public GameObject player { get; set ; }
    public bool IsInteractable { get; set; }

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
        if (Vector2.Distance(playerTransform.position, transform.position) < interactDst)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
   
}
