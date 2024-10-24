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

    private float lastDamageTime;
    private HashSet<GameObject> damagedObjects = new HashSet<GameObject>();

    public virtual DamageData GetDamageData()
    {
        return new DamageData(damageAmount, gameObject, damageType);
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

        var damageable = target.GetComponent<IDamagable>();
        if (damageable != null && damageable.CanBeDamagedBy(gameObject))
        {
            damageable.TakeDamage(GetDamageData());
            lastDamageTime = Time.time;
            damagedObjects.Add(target);
        }
    }
}
