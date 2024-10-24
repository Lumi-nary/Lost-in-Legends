using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiManager : MonoBehaviour
{
    [SerializeField] private StatsBar healthBar;
    [SerializeField] private StatsBar manaBar;

    private void Start()
    {
        // Subscribe to stat changes
        PlayerStatsManager.Instance.SubscribeToStat(
            PlayerStatsManager.StatType.Health,
            UpdateHealthBar);

        PlayerStatsManager.Instance.SubscribeToStat(
            PlayerStatsManager.StatType.Mana,
            UpdateManaBar);

        // Initial setup
        UpdateHealthBar(PlayerStatsManager.Instance.GetCurrentValue(PlayerStatsManager.StatType.Health));
        UpdateManaBar(PlayerStatsManager.Instance.GetCurrentValue(PlayerStatsManager.StatType.Mana));
    }

    private void UpdateHealthBar(float value)
    {
        healthBar.SetValue(value);
    }

    private void UpdateManaBar(float value)
    {
        manaBar.SetValue(value);
    }

    private void OnDestroy()
    {
        // Clean up subscriptions
        if (PlayerStatsManager.Instance != null)
        {
            PlayerStatsManager.Instance.UnsubscribeFromStat(
                PlayerStatsManager.StatType.Health,
                UpdateHealthBar);
            PlayerStatsManager.Instance.UnsubscribeFromStat(
                PlayerStatsManager.StatType.Mana,
                UpdateManaBar);
        }
    }
}
