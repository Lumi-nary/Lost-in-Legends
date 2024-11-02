using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager instance;
    public static CheckpointManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("CheckpointManager is not initialized!");
            }
            return instance;
        }
    }

    [Header("Configuration")]
    [SerializeField] private Transform defaultSpawnPoint;

    [Header("Events")]
    public UnityEvent<Checkpoint> onCheckpointActivated;
    public UnityEvent<Vector2> onPlayerRespawn;

    private Checkpoint currentCheckpoint;
    private List<Checkpoint> checkpoints = new List<Checkpoint>();

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
    private void OnDestroy()
    {
        // Clear the instance when the object is destroyed
        // but only if this is the current instance
        if (instance == this)
        {
            instance = null;
        }
    }
    public void RegisterCheckpoint(Checkpoint checkpoint)
    {
        if (!checkpoints.Contains(checkpoint))
        {
            checkpoints.Add(checkpoint);
        }
    }

    public void UnregisterCheckpoint(Checkpoint checkpoint)
    {
        checkpoints.Remove(checkpoint);
        if (currentCheckpoint == checkpoint)
        {
            currentCheckpoint = null;
        }
    }

    public void ActivateCheckpoint(Checkpoint checkpoint)
    {
        // Deactivate current checkpoint if exists
        if (currentCheckpoint != null && currentCheckpoint != checkpoint)
        {
            currentCheckpoint.Deactivate();
        }

        currentCheckpoint = checkpoint;
        onCheckpointActivated?.Invoke(checkpoint);
    }

    public void RespawnPlayer()
    {
        Vector2 respawnPosition = currentCheckpoint != null
            ? currentCheckpoint.transform.position
            : defaultSpawnPoint.position;
        Debug.Log("Respawning player at: " + respawnPosition);
        onPlayerRespawn?.Invoke(respawnPosition);
    }

    public void ResetCheckpoints()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.Deactivate();
        }
        currentCheckpoint = null;
    }

    // Editor utility method
    private void OnDrawGizmos()
    {
        if (defaultSpawnPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(defaultSpawnPoint.position, 1f);
        }
    }
}
