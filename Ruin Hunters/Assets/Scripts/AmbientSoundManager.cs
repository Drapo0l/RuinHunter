using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientSoundManager : MonoBehaviour
{
    public static AmbientSoundManager instance;

    public AudioSource audioSource;

    [System.Serializable]
    public class AmbientSound
    {
        public string identifier;
        public AudioClip ambientClip;
    }

    public List<AmbientSound> ambientSounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneChanged;
        OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }

    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        UpdateAmbientSound(newScene.name);
    }

    public void UpdateAmbientSound(string identifier)
    {
        foreach (var sound in ambientSounds)
        {
            if (sound.identifier == identifier)
            {
                Debug.Log("Playing ambient sound for: " + identifier); // Debug log
                audioSource.clip = sound.ambientClip;
                audioSource.Play();
                break;
            }
        }
    }

    public void StopCurrentSound()
    {
        if (audioSource.isPlaying)
        {
            Debug.Log("Stopping current sound"); // Debug log
            audioSource.Stop();
        }
    }
}