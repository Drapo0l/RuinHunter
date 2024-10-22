using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;
    public Vector3 teleportDestinationInScene;
    public string sceneToLoad;
    public AudioClip teleportSoundClip; // Use AudioClip instead of AudioSource

    public void TeleportPlayer(Transform player)
    {
        // Create a temporary GameObject to play the sound
        GameObject tempAudio = new GameObject("TempAudio");
        AudioSource tempAudioSource = tempAudio.AddComponent<AudioSource>();
        tempAudioSource.clip = teleportSoundClip;
        tempAudioSource.Play();

        // Ensure the temporary audio object isn't destroyed on load
        DontDestroyOnLoad(tempAudio);

        // Destroy the temporary audio object after the clip duration
        Destroy(tempAudio, teleportSoundClip.length);

        // Perform the teleport
        if (!string.IsNullOrEmpty(sceneToLoad))
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