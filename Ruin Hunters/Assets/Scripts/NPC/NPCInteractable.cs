using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NPCInteractable
{
    GameObject player { get; set; }

    bool IsInteractable { get; set; }
     void Interact();
}

