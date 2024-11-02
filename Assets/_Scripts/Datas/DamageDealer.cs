using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour, IDamageDealer
{
    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private DamageType damageType = DamageType.Normal;
    [SerializeField] private bool canDamageMultipleTimes = false;
    [SerializeField] private float damageInterval = 1f;

    [Space(10f)]

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private bool useCustomKnockbackDirection = false;
    [SerializeField] private Vector2 customKnockbackDirection = Vector2.right;

    private float lastDamageTime;
    private HashSet<GameObject> damagedObjects = new HashSet<GameObject>();

    public virtual DamageData GetDamageData(GameObject target)
    {
        // Knockback Direction
        Vector2 knockbackDir = useCustomKnockbackDirection ? customKnockbackDirection.normalized :
            CalculateKnockBack(target);
        return new DamageData(
            damageAmount, 
            gameObject, 
            damageType,
            knockbackDir,
            knockbackForce
        );
    }

    protected virtual bool CanDealDamageTo(GameObject target)
    {
        if (!canDamageMultipleTimes && damagedObjects.Contains(target))
            return false;

        if (Time.time - lastDamageTime < damageInterval)
            return false;

        return true;
    }

    protected virtual void DealDamage(GameObject target)
    {
        if (!CanDealDamageTo(target))
            return;

        // get both interface damageable and knockable 
        var damageable = target.GetComponent<IDamagable>();
        var knockbackable = target.GetComponent<IKnockbackable>();

        if (damageable != null && damageable.CanBeDamagedBy(gameObject))
        {

            // get damage data once to use both damage and knockback
            DamageData damageData = GetDamageData(target);

            // apply damage
            damageable.TakeDamage(damageData);

            // Apply knockback if target implements Knockbackable
            if (knockbackable != null && damageData.knockbackForce > 0)
            {
                knockbackable.ApplyKnockBack(damageData.knockbackDirection, damageData.knockbackForce);
            }
            lastDamageTime = Time.time;
            damagedObjects.Add(target);
        }
    }
    protected virtual Vector2 CalculateKnockBack(GameObject target)
    {
        // calculate direction from this object to the target
        return (target.transform.position - transform.position).normalized;
    }
}
