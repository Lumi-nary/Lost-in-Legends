using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    private WeaponData weaponData;
    private float lastAttackTime;
    private bool isCharging;
    private Coroutine loopingSoundCoroutine;

    public override void Initialize(ItemData itemData)
    {
        if (itemData is WeaponData weaponSpecificData)
        {
            data = itemData;
            weaponData = weaponSpecificData;
        }
        else
        {
            Debug.LogError("Tried to initialize Weapon with non-weapon data!");
        }
    }

    public void Attack(Vector2 direction)
    {
        if (Time.time < lastAttackTime + weaponData.attackRate) return;

        if (weaponData.weaponType == WeaponType.Melee)
        {
            PerformMeleeAttack(direction);
        }
        else
        {
            PerformRangedAttack(direction);
        }

        lastAttackTime = Time.time;
    }

    private void PerformMeleeAttack(Vector2 direction)
    {
        AudioManager.Instance.PlaySFX(weaponData.useSound);
        // Implementation...
    }

    private void PerformRangedAttack(Vector2 direction)
    {
        AudioManager.Instance.PlaySFX(weaponData.useSound);

        if (weaponData.projectilePrefab != null)
        {
            GameObject projectile = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);

            var projectileDamage = projectile.GetComponent<ProjectileDamage>();
            if (projectileDamage != null)
            {
                // Set damage based on weaponData
            }

            var rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * weaponData.projectileSpeed;
            }
        }
    }

    public override void OnEquip(PlayerController player)
    {
        // Implementation...
    }

    public override void OnUnequip(PlayerController player)
    {
        if (loopingSoundCoroutine != null)
        {
            StopCoroutine(loopingSoundCoroutine);
        }
    }
}
