using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class StatsBar : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private StatsBarConfig config;

    [Header("References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI valueText;

    [SerializeField, HideInInspector] private float editorFillAmount = 1f;

    private float currentValue;
    private float targetFillAmount = 1f;
    private StatsBarConfig previousConfig;

    public event Action<float> OnValueChanged;
    public event Action OnEmpty;
    public event Action OnFull;

    public void SetEditorFillAmount(float amount)
    {
        editorFillAmount = amount;
        if (!Application.isPlaying && fillImage != null)
        {
            fillImage.fillAmount = amount;
            UpdateColor(amount);
        }
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            // Initialize in editor
            InitializeBar();
        }
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            InitializeBar();
        }
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            HandlePlaymodeUpdate();
        }
        else
        {
            HandleEditorUpdate();
        }
    }
    #region Handlers
    private void HandlePlaymodeUpdate()
    {
        if (config != null && config.smoothFill && !Mathf.Approximately(fillImage.fillAmount, targetFillAmount))
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * config.smoothSpeed);
            UpdateColor(fillImage.fillAmount);
        }
    }

    private void HandleEditorUpdate()
    {
        if (config != null && (previousConfig != config || !Application.isPlaying))
        {
            previousConfig = config;
            InitializeBar();
        }
    }

    private void InitializeBar()
    {
        if (config == null)
        {
            Debug.LogError("Stats Bar Config is not assigned!", this);
            return;
        }

        if (fillImage == null)
        {
            fillImage = transform.Find("Fill")?.GetComponent<Image>();
            if (fillImage == null)
            {
                Debug.LogError("Fill Image not found!", this);
                return;
            }
        }

        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;

        if (!Application.isPlaying)
        {
            // In editor, use the stored editor fill amount
            fillImage.fillAmount = editorFillAmount;
            UpdateColor(editorFillAmount);
        }
        else
        {
            currentValue = config.maxValue;
            targetFillAmount = 1f;
            UpdateVisuals(currentValue);
        }
    }
    #endregion
    #region Updater
    public void SetValue(float value)
    {
        if (!Application.isPlaying) return;

        float previousValue = currentValue;
        currentValue = Mathf.Clamp(value, 0f, config.maxValue);
        targetFillAmount = currentValue / config.maxValue;

        if (!config.smoothFill)
        {
            fillImage.fillAmount = targetFillAmount;
            UpdateVisuals(currentValue);
        }

        OnValueChanged?.Invoke(currentValue);

        if (currentValue <= 0f && previousValue > 0f)
            OnEmpty?.Invoke();
        else if (currentValue >= config.maxValue && previousValue < config.maxValue)
            OnFull?.Invoke();
    }

    private void UpdateVisuals(float value)
    {
        float fillAmount = value / config.maxValue;
        fillImage.fillAmount = fillAmount;

        if (config.showValue && valueText != null)
        {
            string valueString = value.ToString(config.valueFormat);
            string maxValueString = config.maxValue.ToString(config.valueFormat);

            valueText.text = config.showMaxValue ?
                $"{valueString}/{maxValueString}" :
                valueString;
        }

        UpdateColor(fillAmount);
    }

    private void UpdateColor(float fillAmount)
    {
        if (!config.useColorChange)
        {
            fillImage.color = Color.white;
            return;
        }

        if (fillAmount <= config.lowThreshold)
        {
            float t = fillAmount / config.lowThreshold;
            fillImage.color = Color.Lerp(config.minColor, config.maxColor, t);
        }
        else
        {
            fillImage.color = config.maxColor;
        }
    }
    #endregion
    #region Utility Methods
    // Public utility methods
    public float GetCurrentValue() => currentValue;
    public float GetMaxValue() => config.maxValue;
    public float GetPercentage() => currentValue / config.maxValue;
    public void IncrementValue(float amount) => SetValue(currentValue + amount);
    public void DecrementValue(float amount) => SetValue(currentValue - amount);
    public void SetPercentage(float percentage) => SetValue(config.maxValue * Mathf.Clamp01(percentage));
    public void ResetToMax() => SetValue(config.maxValue);
    public void ResetToZero() => SetValue(0f);

    public void SetConfig(StatsBarConfig newConfig)
    {
        config = newConfig;
        InitializeBar();
    }
    #endregion
}
