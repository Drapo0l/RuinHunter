using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteraction : MonoBehaviour, NPCInteractable
{


    public GameObject player { get; set ; }
    public bool IsInteractable { get; set; }

    public virtual void Interact(){ }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInteractable)
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == player)
        {
            IsInteractable = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == player)
        {
            IsInteractable = false;
        }
    }
}
