using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealer
{
    DamageData GetDamageData(GameObject target);
}
