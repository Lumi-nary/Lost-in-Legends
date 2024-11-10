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
    public bool isFromPlayer;

    public DamageData(float amount, GameObject source, ElementType type = ElementType.Normal
        , Vector2 knockbackDirection = default, float knockbackForce = 0f, bool isFromPlayer = false)
    {
        this.amount = amount;
        this.source = source;
        this.type = type;
        this.knockbackDirection = knockbackDirection;
        this.knockbackForce = knockbackForce;
        this.isFromPlayer = isFromPlayer;
    }
}
