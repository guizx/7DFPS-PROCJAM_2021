using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamepadSliderUI : MonoBehaviour
{
    [SerializeField] private Slider gamepadSlider;
    [SerializeField] private TextMeshProUGUI valueText;

    public static Action<float> OnGamepadSensibilityChanged;
    private void Awake()
    {
        gamepadSlider.onValueChanged.AddListener(OnGamepadSliderValueChanged);
    }

    private void OnDestroy()
    {
        gamepadSlider.onValueChanged.RemoveListener(OnGamepadSliderValueChanged);
    }

    private void OnEnable()
    {
        if (gamepadSlider == null)
            gamepadSlider = GetComponent<Slider>();

        float gamepadSensibility = PlayerPrefs.GetFloat(ConfigData.GAMEPAD_SENSIBILITY_KEY, 0.5f);

        gamepadSlider.minValue = 0f;
        gamepadSlider.maxValue = 1f;
        gamepadSlider.value = gamepadSensibility;
        valueText.SetText(gamepadSensibility.ToString("F2"));
    }

    private void OnGamepadSliderValueChanged(float value)
    {
        OnGamepadSensibilityChanged?.Invoke(value);
        PlayerPrefs.SetFloat(ConfigData.GAMEPAD_SENSIBILITY_KEY, value);
        valueText.SetText(value.ToString("F2"));
    }
}
