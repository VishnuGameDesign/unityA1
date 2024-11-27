using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderValue : MonoBehaviour
{
    [SerializeField] protected Vector2 _outMinMax = new Vector2(-0.5f, 0.5f);
    [SerializeField] protected Slider _slider;

    private void OnValidate()
    {
        _slider = GetComponent<Slider>();
    }

    protected virtual void OnEnable()
    {
        _slider.onValueChanged.AddListener(SetValue);
    }

    protected virtual void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(SetValue);
    }

    public virtual void SetValue(float value)
    {
        // we'll override this in our child classes
    }

    protected float SliderValueToOutputValue(float value)
    {
        // remap from slider to outMinMax range
        float percentage = value / _slider.maxValue;
        float remappedValue = Mathf.Lerp(_outMinMax.x, _outMinMax.y, percentage);
        return remappedValue;
    }

    protected float OutputValueToSliderValue(float value)
    {
        // remap from outMinMax range to slider range
        float percentage = Mathf.InverseLerp(_outMinMax.x, _outMinMax.y, value);
        float sliderValue = percentage * _slider.maxValue;
        return sliderValue;
    }
}