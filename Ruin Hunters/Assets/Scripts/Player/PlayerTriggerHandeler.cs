using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Forest") || other.CompareTag("Snow") || other.CompareTag("Desert"))
        {
            Debug.Log("Entered zone: " + other.tag); // Debug log
            AmbientSoundManager.instance.UpdateAmbientSound(other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Forest") || other.CompareTag("Snow") || other.CompareTag("Desert"))
        {
            Debug.Log("Exited zone: " + other.tag); // Debug log
            AmbientSoundManager.instance.StopCurrentSound();
        }
    }
}