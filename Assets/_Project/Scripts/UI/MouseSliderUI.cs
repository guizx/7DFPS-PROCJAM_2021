using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSliderUI : MonoBehaviour
{
    [SerializeField] private Slider mouseSlider;
    [SerializeField] private TextMeshProUGUI valueText;
    public static Action<float> OnMouseSensibilityChanged;
    private void Awake()
    {
        mouseSlider.onValueChanged.AddListener(OnMouseSliderValueChanged);
    }

    private void OnDestroy()
    {
        mouseSlider.onValueChanged.RemoveListener(OnMouseSliderValueChanged);
    }

    private void OnEnable()
    {
        if (mouseSlider == null)
            mouseSlider = GetComponent<Slider>();

        float mouseSensibility = PlayerPrefs.GetFloat(ConfigData.MOUSE_SENSIBILITY_KEY, 0.5f);

        mouseSlider.minValue = 0f;
        mouseSlider.maxValue = 1f;
        mouseSlider.value = mouseSensibility;
        valueText.SetText(mouseSensibility.ToString("F2"));
    }



    private void OnMouseSliderValueChanged(float value)
    {
        OnMouseSensibilityChanged?.Invoke(value);
        PlayerPrefs.SetFloat(ConfigData.MOUSE_SENSIBILITY_KEY, value);
        valueText.SetText(value.ToString("F2"));
    }
}
