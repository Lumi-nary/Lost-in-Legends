using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private List<DamageType> immunities = new List<DamageType>();
    private bool isInvulnerable = false;

    public bool CanBeDamagedBy(GameObject source)
    {
        if (isInvulnerable) return false;

        // Check if player should take damage from this source
        var damageDealer = source.GetComponent<IDamageDealer>();
        if (damageDealer == null) return false;

        var damageData = damageDealer.GetDamageData();
        return !immunities.Contains(damageData.type);
    }

    public void TakeDamage(DamageData damageData)
    {
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
    }
}
