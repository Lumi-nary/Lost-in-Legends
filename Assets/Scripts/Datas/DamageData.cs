using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageData
{
    public float amount;
    public GameObject source;
    public DamageType type;

    public DamageData(float amount, GameObject source, DamageType type = DamageType.Normal)
    {
        this.amount = amount;
        this.source = source;
        this.type = type;
    }
}
