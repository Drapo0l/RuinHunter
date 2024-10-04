using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private static bool _loadFromDoor;

    private GameObject _player;
    private Collider _playerColl;
    private Collider _doorColl;
    private Vector3 _playerSpawnPst;

    private DoorTrigger.DoorToSpawnAt _doorToSpwanTo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerColl = _player.GetComponent<Collider>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoded;
    }

    public static void SwapSceneFromDoorUse(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpwanAt)
    {
        _loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpwanAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorTrigger.DoorToSpawnAt doorToSpawn= DoorTrigger.DoorToSpawnAt.None)//can be use to change scene even if the player has not enter a door
    {
        //start fading to black
        SceneFadeManager.instance.StartFadeOut();

        //keep fading out
     while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        _doorToSpwanTo = doorToSpawn;
        SceneManager.LoadScene(myScene);
    }

    //CALLED WHENEVER A NEW SCENE IS LOADED (INCLUDING THE START OF THE GAME?)
    private void OnSceneLoded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();

        if(_loadFromDoor)
        {
            //warp the player to correct door location
            FindDoor(_doorToSpwanTo);
            _player.transform.position = _playerSpawnPst;

            _loadFromDoor = false;
        }
    }

    private void FindDoor(DoorTrigger.DoorToSpawnAt doorSpawnNum)
    {
        DoorTrigger[] doors = FindObjectsOfType<DoorTrigger>();

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].currentDoorPosition == doorSpawnNum)
            {
                _doorColl = doors[i].gameObject.GetComponent<Collider>();

                CalculateSpwanPosition();
                return;
            }
        }
    }

    private void CalculateSpwanPosition()
    {
        float colliderHeight = _playerColl.bounds.extents.y;
        _playerSpawnPst = _doorColl.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
}
