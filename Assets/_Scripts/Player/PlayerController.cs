using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    private PlayerHealth playerHealth;
    private PlayerMana playerMana;
    private PlayerMovement playerMovement;
    private PlayerAnimator playerAnimator;

    [Header("Wall Jump Settings")]
    private bool canWallJump = false;

    private void Awake()
    {
        // Get references to required components
        playerHealth = GetComponent<PlayerHealth>();
        playerMana = GetComponent<PlayerMana>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();

        if (playerHealth == null || playerMovement == null || playerAnimator == null)
        {
            Debug.LogError("Missing required player components!");
        }
    }

    // Methods for Armor system
    public void ModifySpeed(float multiplier)
    {
        if (playerMovement != null && playerMovement.Data != null)
        {
            playerMovement.Data.runMaxSpeed *= multiplier;
        }
    }

    public void EnableWallJump()
    {
        //playerMovement.
    }

    public void DisableWallJump()
    {
        canWallJump = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collectibles, triggers, etc.
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                item.OnEquip(this);
            }
        }
    }
}