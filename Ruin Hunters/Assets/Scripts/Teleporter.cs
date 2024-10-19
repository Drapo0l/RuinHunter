using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;
    public Vector3 teleportDestinationInScene;
    public string sceneToLoad;

    public void TeleportPlayer(Transform player)
    {
        if(!string.IsNullOrEmpty(sceneToLoad)) 
        {
            SceneManager.LoadScene(sceneToLoad);
            player.transform.position = teleportDestinationInScene;
        }
        else
        {
            player.position = teleportDestination.position;
        }
    }
}
