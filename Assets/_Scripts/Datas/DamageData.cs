using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageData
{
    public float amount;
    public GameObject source;
    public ElementType type;
    public Vector2 knockbackDirection;
    public float knockbackForce;

    public DamageData(float amount, GameObject source, ElementType type = ElementType.Normal
        , Vector2 knockbackDirection = default, float knockbackForce = 0f)
    {
        this.amount = amount;
        this.source = source;
        this.type = type;
        this.knockbackDirection = knockbackDirection;
        this.knockbackForce = knockbackForce;
    }
}
