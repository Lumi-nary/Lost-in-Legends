using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsManager
{
    public float BaseValue;
    public float CurrentValue;
    public float MinValue;
    public float MaxValue;
    public bool HasMaxValue;
    public bool HasMinValue;

    public StatsManager(float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        BaseValue = baseValue;
        CurrentValue = baseValue;
        MinValue = minValue;
        MaxValue = maxValue;
        HasMaxValue = maxValue != float.MaxValue;
        HasMinValue = minValue != float.MinValue;
    }

    public void ModifyValue(float amount)
    {
        CurrentValue += amount;
        if (HasMaxValue) CurrentValue = Mathf.Min(CurrentValue, MaxValue);
        if (HasMinValue) CurrentValue = Mathf.Max(CurrentValue, MinValue);
    }

    public void SetValue(float value)
    {
        CurrentValue = value;
        if (HasMaxValue) CurrentValue = Mathf.Min(CurrentValue, MaxValue);
        if (HasMinValue) CurrentValue = Mathf.Max(CurrentValue, MinValue);
    }

    public void Reset()
    {
        CurrentValue = BaseValue;
    }
}
