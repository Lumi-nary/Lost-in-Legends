using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsBar))]
public class StatBarAdapter : MonoBehaviour
{
    [SerializeField] private PlayerStatsManager.StatType statType;
    private StatsBar statsBar;

    public PlayerStatsManager.StatType StatType => statType;

    private void Awake()
    {
        statsBar = GetComponent<StatsBar>();
    }

    private void Start()
    {
        // Subscribe to stat changes
        PlayerStatsManager.Instance.SubscribeToStat(statType, UpdateValue);

        // Initial setup
        UpdateValue(PlayerStatsManager.Instance.GetCurrentValue(statType));
    }

    private void OnDestroy()
    {
        if (PlayerStatsManager.Instance != null)
        {
            PlayerStatsManager.Instance.UnsubscribeFromStat(statType, UpdateValue);
        }
    }

    private void UpdateValue(float value)
    {
        statsBar.SetValue(value);
    }
}
