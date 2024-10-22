using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownSounds : MonoBehaviour
{
  public AudioSource TownMusic;
    //[SerializeField] AudioClip AudTownMusic;
    //[SerializeField] float TownMusicVol;
  
    public static TownSounds instance;



    [System.Serializable]
    public class TownSongs
    {
        public string identifier;
        public AudioClip MusicClip;
    }

    public List<TownSongs> townmusic;

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
        TownMusic = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneChanged;
        OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }


    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        UpdateAmbientSound(newScene.name);
    }

    public void UpdateAmbientSound(string identifier)
    {
        foreach (var sound in townmusic)
        {
            if (sound.identifier == identifier)
            {
                Debug.Log("Playing ambient sound for: " + identifier); // Debug log
                TownMusic.clip = sound.MusicClip;
                TownMusic.Play();
                break;
            }
        }
    }

    public void StopCurrentSound()
    {
        if (TownMusic.isPlaying)
        {
            Debug.Log("Stopping current sound"); // Debug log
            TownMusic.Stop();
        }
    }

}
