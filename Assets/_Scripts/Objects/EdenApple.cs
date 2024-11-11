using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EdenApple : TriggerInteractionBase
{
    [SerializeField] private float setMaxHealth;

    public override void Interact()
    {
        PlayerStatsManager.Instance.SetMaxValue(PlayerStatsManager.StatType.Health, setMaxHealth);
        Destroy(gameObject);
    }
}
