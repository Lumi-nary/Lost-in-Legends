using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI System Config", menuName = "Game/UI/UI System Configuration")]
public class UISystemConfig : ScriptableObject
{
    [System.Serializable]
    public class StatBarMapping
    {
        public PlayerStatsManager.StatType statType;
        public StatsBarConfig config;
    }

    public List<StatBarMapping> statConfigurations = new List<StatBarMapping>();

    public StatsBarConfig GetConfigForStatType(PlayerStatsManager.StatType statType)
    {
        return statConfigurations.Find(x => x.statType == statType)?.config;
    }
}
