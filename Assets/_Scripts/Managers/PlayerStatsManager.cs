using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{
    private static PlayerStatsManager instance;
    public static PlayerStatsManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("PlayerStatsManager is not initialized!");
            return instance;
        }
    }

    [SerializeField] private StatsBarConfig healthConfig;
    [SerializeField] private StatsBarConfig manaConfig;
    [SerializeField] private RectTransform healthBar;

    [SerializeField] private float baseMaxValue = 100f; // Reference value for base width
    [SerializeField] private float baseWidth = 200f;    // Base width in pixels

    private Dictionary<StatType, StatData> stats;
    public enum StatType
    {
        Health,
        Mana,
        // Easy to add more stats here
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Clear the instance when the object is destroyed
        // but only if this is the current instance
        if (instance == this)
        {
            instance = null;
        }
    }

    private void InitializeStats()
    {
        stats = new Dictionary<StatType, StatData>
        {
            { StatType.Health, new StatData(healthConfig.maxValue) },
            { StatType.Mana, new StatData(manaConfig.maxValue) }
        };
    }

    public void ModifyStat(StatType statType, float amount)
    {
        if (!stats.ContainsKey(statType)) return;

        var stat = stats[statType];
        stat.currentValue = Mathf.Clamp(stat.currentValue + amount, 0, stat.maxValue);
        stat.onValueChanged?.Invoke(stat.currentValue);
    }

    public float GetCurrentValue(StatType statType)
    {
        return stats.ContainsKey(statType) ? stats[statType].currentValue : 0f;
    }

    public float GetMaxValue(StatType statType)
    {
        return stats.ContainsKey(statType) ? stats[statType].maxValue : 0f;
    }

    public void SetMaxValue(StatType statType, float newMaxValue)
    {
        if (!stats.ContainsKey(statType)) return;

        // Get the appropriate config
        StatsBarConfig config = GetConfig(statType);
        if (config == null) return;

        // Update both the config and the runtime stat
        var stat = stats[statType];

        float maxValue = stat.currentValue + newMaxValue;

        // Store the previous percentage of current to max value
        float previousPercentage = stat.maxValue > 0 ? stat.currentValue / stat.maxValue : 1f;

        // Update max values in both config and runtime stat
        config.maxValue = Mathf.Max(0f, maxValue);
        stat.maxValue = config.maxValue;

        // Maintain the same percentage of the stat
        stat.currentValue = maxValue * previousPercentage;

        // Update the UI bar width
        UpdateBarWidth(statType, maxValue);

        // Notify listeners of the change
        stat.onValueChanged?.Invoke(stat.currentValue);
#if UNITY_EDITOR
        // Ensure the change is saved in the editor
        UnityEditor.EditorUtility.SetDirty(config);
#endif
    }

    public void SubscribeToStat(StatType statType, UnityAction<float> callback)
    {
        if (stats.ContainsKey(statType))
            stats[statType].onValueChanged.AddListener(callback);
    }

    public void UnsubscribeFromStat(StatType statType, UnityAction<float> callback)
    {
        if (stats.ContainsKey(statType))
            stats[statType].onValueChanged.RemoveListener(callback);
    }

    private StatsBarConfig GetConfig(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health:
                return healthConfig;
            case StatType.Mana:
                return manaConfig;
            default:
                return null;
        }
    }
    private void UpdateBarWidth(StatType statType, float newMaxValue)
    {
        RectTransform targetTransform = GetBarTransform(statType);
        if (targetTransform == null) return;

        // Calculate the new width based on the ratio of new max value to base max value
        float widthRatio = (newMaxValue - 50) / baseMaxValue;
        float newWidth = baseWidth * widthRatio;

        // Update the RectTransform's width
        Vector2 sizeDelta = targetTransform.sizeDelta;
        sizeDelta.x = newWidth;
        targetTransform.sizeDelta = sizeDelta;
    }
    private RectTransform GetBarTransform(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health:
                return healthBar;
            //case StatType.Mana:
            //    return manaBarTransform;
            default:
                return null;
        }
    }
}
