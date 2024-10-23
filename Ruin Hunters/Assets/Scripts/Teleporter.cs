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
    [SerializeField] GameObject worldEnemies;
    [SerializeField] GameObject desertEnemies;
    [SerializeField] GameObject snowEnemies;

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
            CheckScene();
            SceneManager.LoadScene(sceneToLoad);
            player.transform.position = teleportDestinationInScene;
        }
        else
        {
            player.position = teleportDestination.position;
        }
    }

    private void CheckScene()
    {
        if (sceneToLoad == "Desert")
        {
            worldEnemies.SetActive(false);
            snowEnemies.SetActive(false);
            desertEnemies.SetActive(true);
        }
        else if (sceneToLoad == "Snow")
        {
            worldEnemies.SetActive(false);
            snowEnemies.SetActive(true);
            desertEnemies.SetActive(false);
        }
        else if (sceneToLoad == "Map")
        {
            worldEnemies.SetActive(true);
            snowEnemies.SetActive(false);
            desertEnemies.SetActive(false);
        }
    }

}