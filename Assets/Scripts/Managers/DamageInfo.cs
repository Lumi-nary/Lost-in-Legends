using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public float Amount { get; private set; }
    public DamageInfo(float amount)
    {
        Amount = amount;
    }
}
