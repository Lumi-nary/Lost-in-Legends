using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Bar Config", menuName = "Game/UI/Stats Bar Configuration")]
public class StatsBarConfig : ScriptableObject
{
    [Header("Bar Settings")]
    public float maxValue = 100f;
    public bool smoothFill = true;
    public float smoothSpeed = 10f;
    public bool showValue;

    [Header("Color Settings")]
    public bool useColorChange = false;
    public Color _maxColor = Color.green;
    public Color _minColor = Color.red;
    public float lowThreshold = 0.3f;

    [Header("Display Settings")]
    public string barName = "Stats";
    public bool showMaxValue = true;
    public string valueFormat = "F0"; // "F0" for no decimals, "F1" for 1 decimal place

    // Properties to ensure color updates trigger dirty
    public Color maxColor
    {
        get => _maxColor;
        set
        {
            if (_maxColor != value)
            {
                _maxColor = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }
    }

    public Color minColor
    {
        get => _minColor;
        set
        {
            if (_minColor != value)
            {
                _minColor = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }
    }
}