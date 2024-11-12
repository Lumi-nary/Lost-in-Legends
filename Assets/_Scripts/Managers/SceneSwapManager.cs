using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;

    private static bool _loadFromDoor;

    private GameObject _player;
    private Collider2D _playerColl;
    private Collider2D _doorColl;
    private Vector3 _playerSpawnPosition;

    private DoorInteract.DoorToSpawnAt _doorToSpawnTo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerColl = _player.GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Swap scene using doors
    public static void SwapSceneFromDoorUse(SceneField myScene, DoorInteract.DoorToSpawnAt doorToSpawnTo)
    {
        _loadFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnTo));
    }

    // Swap scene without using dOORORSOROS
    public static void SwapScene(SceneField scene)
    {
        instance.StartCoroutine(instance.FadeOutThenChangeScene(scene));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, DoorInteract.DoorToSpawnAt doorToSpawnAt = DoorInteract.DoorToSpawnAt.None)
    {
        userInput.DeactivatePlayerControls();
        // fade to black
        SceneFadeManager.instance.StartFadeOut();

        // keep fading out
        while (SceneFadeManager.instance.IsFadingOut)
        {
            yield return null;
        }

        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
    }
    private IEnumerator ActivatePlayerControlsAfterFadeIn()
    {
        while (SceneFadeManager.instance.IsFadingIn)
        {
            yield return null;
        }
        userInput.ActivatePlayerControls();
    }
    // called when new scene is loaded (including start of game)
    // HAHAHHASHAHAHA adsjgashuigasyhifguiasfgyuias
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.instance.StartFadeIn();
        if (_loadFromDoor)
        {
            StartCoroutine(ActivatePlayerControlsAfterFadeIn());
            // warps player to door location
            FindDoor(_doorToSpawnTo);
            _player.transform.position = _playerSpawnPosition;
            _loadFromDoor = false;
        }
    }
    // anim na oras na ako nag cocode
    // find door to find the exact door on door scene to spawn at
    private void FindDoor(DoorInteract.DoorToSpawnAt doorSpawnNumber)
    {
        DoorInteract[] doors = FindObjectsOfType<DoorInteract>();
        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i].CurrentDoorPosition == doorSpawnNumber)
            {
                _doorColl = doors[i].gameObject.GetComponent<Collider2D>();
                //calculate spawn pos
                CalculateSpawnPosition();
                return;
            }
        }
    }

    private void CalculateSpawnPosition()
    {
        // calculates player coll to door coll to teleport player just to the ground
        // TO PREVENT FALLING
        float colliderHeight = _playerColl.bounds.extents.y;
        _playerSpawnPosition = _doorColl.transform.position - new Vector3(0f, colliderHeight, 0f);
        Debug.Log(_playerSpawnPosition);
    }
}
