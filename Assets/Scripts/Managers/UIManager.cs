using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UISystemConfig systemConfig;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    instance = go.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    private Dictionary<PlayerStatsManager.StatType, List<StatBarAdapter>> statDisplays =
        new Dictionary<PlayerStatsManager.StatType, List<StatBarAdapter>>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        InitializeUIElements();
    }

    private void InitializeUIElements()
    {
        // Find all stat displays in the scene
        var adapters = FindObjectsOfType<StatBarAdapter>();

        foreach (var adapter in adapters)
        {
            RegisterStatDisplay(adapter);
        }
    }

    private void RegisterStatDisplay(StatBarAdapter display)
    {
        if (!statDisplays.ContainsKey(display.StatType))
        {
            statDisplays[display.StatType] = new List<StatBarAdapter>();
        }

        if (!statDisplays[display.StatType].Contains(display))
        {
            statDisplays[display.StatType].Add(display);
        }
    }

    public List<StatBarAdapter> GetDisplaysForStatType(PlayerStatsManager.StatType statType)
    {
        return statDisplays.ContainsKey(statType) ? statDisplays[statType] : new List<StatBarAdapter>();
    }
}
