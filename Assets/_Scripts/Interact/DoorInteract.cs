using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorInteract : TriggerInteractionBase
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
    [SerializeField] private SceneField _sceneToLoad;
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [Space(10f)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;
    public override void Interact()
    {
        //load scene
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
    }
}
