using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Mana Settings")]
    [SerializeField] private float manaRegenDelay = 1f;
    [SerializeField] private float manaRegenRate = 10f;

    private bool isRegenerating = false;
    private Coroutine regenCoroutine;

    private PlayerStatsManager statsManager;

    private void Start()
    {
        statsManager = PlayerStatsManager.Instance;

        if (statsManager != null)
        {
            statsManager.SubscribeToStat(PlayerStatsManager.StatType.Mana, OnManaChanged);
        }
        else
        {
            Debug.LogError("PlayerStatsManager instance is null. Make sure it's initialized.");
        }
    }

    private void OnDestroy()
    {
        if (statsManager != null)
        {
            statsManager.UnsubscribeFromStat(PlayerStatsManager.StatType.Mana, OnManaChanged);
        }

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
    }

    public void ModifyMana(float amount)
    {
        statsManager.ModifyStat(PlayerStatsManager.StatType.Mana, amount);

        // If taking damage, stop regeneration and restart the delay
        if (amount < 0)
        {
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
            regenCoroutine = StartCoroutine(StartManaRegeneration());
        }
    }

    private void OnManaChanged(float currentMana)
    {
        // Start regeneration if not already regenerating
        if (!isRegenerating && currentMana < statsManager.GetMaxValue(PlayerStatsManager.StatType.Mana))
        {
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
            regenCoroutine = StartCoroutine(StartManaRegeneration());
        }
    }

    private IEnumerator StartManaRegeneration()
    {
        isRegenerating = false;
        yield return new WaitForSeconds(manaRegenDelay);
        isRegenerating = true;

        while (isRegenerating &&
               statsManager.GetCurrentValue(PlayerStatsManager.StatType.Mana) < statsManager.GetMaxValue(PlayerStatsManager.StatType.Mana))
        {
            ModifyMana(manaRegenRate * Time.deltaTime);
            yield return null;
        }

        isRegenerating = false;
    }
}