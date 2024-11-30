using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarBehaviour
{
    private Slider cooldownBar;
    public float maxValue = 1f;
    private Color low;
    private Color high;
    private bool filled = false;
    private float frames = 0f;

    public CooldownBarBehaviour(Slider slider, float max)
    {
        SliderColors sliderColors = slider.GetComponent<SliderColors>();
        cooldownBar = slider;
        maxValue = max;
        cooldownBar.value = max;
        low = sliderColors.low;
        high = sliderColors.high;
        cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
    }

    // Set Cooldown bar value
    public void SetBar(float current)
    {
        cooldownBar.value = current / maxValue;

        // Dynamically changes the color of the healthbar based on remaining health
        cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);

        //if (cooldownBar.maxValue - cooldownBar.value < 0.1f && !filled) // Flash white when full
        //{
        //    cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.white;
        //    if (frames >= 25)
        //    {
        //        filled = true;
        //    }
        //    else
        //    {
        //        frames += 1;
        //    }
        //}
        //else if (cooldownBar.maxValue - cooldownBar.value >= 0.1f)
        //{
        //    filled = false;
        //    frames = 0f;
        //    // Dynamically changes the color of the healthbar based on remaining health
        //    cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
        //}
        //else
        //{
        //    // Dynamically changes the color of the healthbar based on remaining health
        //    cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
        //}
    }

    public void UpdateCooldown(float value)
    {
        maxValue = value;
        cooldownBar.value = maxValue;
    }
}
