using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : DamageDealer
{
    private void OnHit(GameObject target)
    {
        DealDamage(target);
        // Future to add: Destroy projectile or play effects
        // APAKA LAG NA NG PC KO 5 MINUTES KADA CHANGE SA CODE ANO YON
    }
}
