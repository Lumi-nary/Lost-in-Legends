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
}
