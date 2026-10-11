using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthBarSlider;

    private void Awake()
    {
        healthBarSlider = GetComponentInChildren<Slider>();
    }

    public void UpdateHealthBar(float value)
    {
        healthBarSlider.value = value;
    }
}
