using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBarData", menuName = "Stats/Bar Data", order = 1)]
public class barData : ScriptableObject
{
    public string barName; //name of bar (Health, Mana, Stamina, etc)
    public float maxValue = 100f; // default to 100
    public float currentValue;

    // reset to max
    public void ResetValue()
    {
        currentValue = maxValue;
    }
    public void DecreaseValue(float amount)
    {
        currentValue -= amount;
        if (currentValue <= 0) 
            currentValue = 0; // prevent value below zero
    }
    public void IncreaseValue(float amount)
    {
        currentValue += amount;
        if (currentValue >= maxValue)
            currentValue = maxValue;
    }

}
