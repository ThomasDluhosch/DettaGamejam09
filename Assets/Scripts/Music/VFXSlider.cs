using UnityEngine;
using UnityEngine.UI;

public class VFXSlider : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Initialize slider with current music volume
        slider.value = VFXManager.Instance.GetVFXVolume();

        // Listen for value changes
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        VFXManager.Instance.SetVFXVolume(value);
    }
}
