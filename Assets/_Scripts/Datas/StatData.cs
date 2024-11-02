using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatData
{
    public float currentValue;
    public float maxValue;
    public UnityEvent<float> onValueChanged;

    public StatData(float max)
    {
        maxValue = max;
        currentValue = max;
        onValueChanged = new UnityEvent<float>();
    }
}
