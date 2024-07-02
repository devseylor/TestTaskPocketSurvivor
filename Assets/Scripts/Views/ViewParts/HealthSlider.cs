using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private TMP_Text valueText;

    private float _maxValue;
    
    public void InitSlider(string name, float currentValue, float maxValue)
    {
        _maxValue = maxValue;
        labelText.text = name;
        valueText.text = currentValue.ToString();
        slider.value = currentValue / maxValue;
    }

    public void SetValue(float currentValue)
    {
        slider.value = currentValue / _maxValue;
    }
}