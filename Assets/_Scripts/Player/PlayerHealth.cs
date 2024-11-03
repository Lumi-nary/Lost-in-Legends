using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [Header("Health Settings")]
    [SerializeField] private List<DamageType> immunities = new List<DamageType>();
    [SerializeField] private float deathDelay = 1f;

    private bool isInvulnerable = false;
    private bool isDead = false;

    private Rigidbody2D rb;

    private PlayerStatsManager statsManager;
    private PlayerAnimator playerAnimator;


    private void Start()
    {
        statsManager = PlayerStatsManager.Instance; // Initialize statsManager

        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<PlayerAnimator>();

        if (statsManager != null)
        {
            statsManager.SubscribeToStat(PlayerStatsManager.StatType.Health, OnHealthChanged);
        }
        else
        {
            Debug.LogError("PlayerStatsManager instance is null. Make sure it's initialized.");
        }

        CheckpointManager.Instance.onPlayerRespawn.AddListener(OnPlayerRespawn);
    }

    private void OnDestroy()
    {
        if (statsManager != null)
        {
            statsManager.UnsubscribeFromStat(PlayerStatsManager.StatType.Health, OnHealthChanged);
        }
        // Unsubscribe from checkpoint respawn events
        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.onPlayerRespawn.RemoveListener(OnPlayerRespawn);
        }
    }
    public bool CanBeDamagedBy(GameObject source)
    {
        if (isInvulnerable || isDead) return false;

        // Check if player should take damage from this source
        var damageDealer = source.GetComponent<IDamageDealer>();
        if (damageDealer == null) return false;

        var damageData = damageDealer.GetDamageData(source);
        return !immunities.Contains(damageData.type);
    }

    public void TakeDamage(DamageData damageData)
    {
        if (isDead) return;

        // Apply damage modifiers, armor, resistances etc.
        float finalDamage = CalculateFinalDamage(damageData);

        // Apply damage to player stats
        PlayerStatsManager.Instance.ModifyStat(PlayerStatsManager.StatType.Health, -finalDamage);

        // Trigger effects, animations, etc.
        OnDamaged(damageData);
    }

    private float CalculateFinalDamage(DamageData damageData)
    {
        // Add damage calculation logic here
        return damageData.amount;
    }

    protected virtual void OnDamaged(DamageData damageData)
    {
        // Handle damage effects, animations, sounds etc.
        playerAnimator.PlayHurtAnimation();
        AudioManager.Instance.PlaySFXOneShot("playerHurt");

    }
    private void OnHealthChanged(float currentHealth)
    {
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    private void Die()
    {
        if (isDead) return;

        isDead = true;

        // Disable player control
        //DisablePlayerControl();

        // Play death effects
        //if (deathEffectPrefab != null)
        //{
        //    Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        //}


        // Play death sound
        AudioManager.Instance.PlaySFXOneShot("playerDie");
        AudioManager.Instance.PlaySFXOneShot("playerDieBG");


        // Start death sequence
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Wait for death animation/effects
        playerAnimator.PlayDeathAnimation();
        yield return new WaitForSeconds(deathDelay);

        // Trigger respawn
        Respawn();
    }

    private void Respawn()
    {
        // Reset death state
        isDead = false;
        // Trigger checkpoint respawn
        CheckpointManager.Instance.RespawnPlayer();

        // Reset health to max
        float maxHealth = statsManager.GetMaxValue(PlayerStatsManager.StatType.Health);
        statsManager.ModifyStat(PlayerStatsManager.StatType.Health, maxHealth);

        playerAnimator.ResetDeathState();
        // Grant temporary invulnerability
        //StartCoroutine(GrantTemporaryInvulnerability());

        // Re-enable player control
        //EnablePlayerControl();
    }

    private void OnPlayerRespawn(Vector2 respawnPosition)
    {
        // Move the player to the respawn position
        transform.position = respawnPosition;
    }
}
