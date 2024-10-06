using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : TriggerInteraction
{
    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
    }


    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt doorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;

    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt currentDoorPosition;
    public override void Interact()
    {
        //load new scene
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, doorToSpawnTo);
    }
}
