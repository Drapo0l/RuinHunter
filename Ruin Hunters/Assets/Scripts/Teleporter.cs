using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        if(!string.IsNullOrEmpty(sceneToLoad)) 
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            player.position = teleportDestination.position;
        }
    }
}
