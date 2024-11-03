using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private bool activateOnStart = false;
    [Header("Optional: for saving/loading specific checkpoints")]
    [SerializeField] private string checkpointId = ""; // Optional: for saving/loading specific checkpoints

    [Space(10f)]

    [Header("Visual Effects")]
    [SerializeField] private GameObject activeVisual;
    [SerializeField] private GameObject inactiveVisual;
    [SerializeField] private ParticleSystem activationEffect;

    private bool isActive = false;

    private void OnValidate()
    {
        // Auto-generate checkpoint ID if empty
        if (string.IsNullOrEmpty(checkpointId))
        {
            checkpointId = System.Guid.NewGuid().ToString();
        }
    }

    private void Start()
    {
        CheckpointManager.Instance.RegisterCheckpoint(this);

        if (activateOnStart)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            Activate();
        }
    }

    public void Activate()
    {
        isActive = true;

        // Update visuals
        if (activeVisual != null) activeVisual.SetActive(true);
        if (inactiveVisual != null) inactiveVisual.SetActive(false);

        // Play effects
        if (activationEffect != null)
        {
            activationEffect.Play();
        }

        // Play sound
        AudioManager.Instance.PlaySFX("activationSound");

        CheckpointManager.Instance.ActivateCheckpoint(this);
    }

    public void Deactivate()
    {
        isActive = false;

        // Update visuals
        if (activeVisual != null) activeVisual.SetActive(false);
        if (inactiveVisual != null) inactiveVisual.SetActive(true);

        // Stop effects if they're looping
        if (activationEffect != null)
        {
            activationEffect.Stop();
        }
    }

    private void OnDestroy()
    {
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.UnregisterCheckpoint(this);
        }
    }

    // Public getters
    public bool IsActive => isActive;
    public string CheckpointId => checkpointId;
}
