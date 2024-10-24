using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(DamageData damageData);
    bool CanBeDamagedBy(GameObject source);
}
