using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSounds : MonoBehaviour
{
    public AudioSource TownMusic;
     void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !TownMusic.isPlaying)
        {
            TownMusic.Play();
        }
    }
}
