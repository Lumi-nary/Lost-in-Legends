using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageData
{
    public float amount;
    public GameObject source;
    public DamageType type;
    public Vector2 knockbackDirection;
    public float knockbackForce;

    public DamageData(float amount, GameObject source, DamageType type = DamageType.Normal
        , Vector2 knockbackDirection = default, float knockbackForce = 0f)
    {
        this.amount = amount;
        this.source = source;
        this.type = type;
        this.knockbackDirection = knockbackDirection;
        this.knockbackForce = knockbackForce;
    }
}
