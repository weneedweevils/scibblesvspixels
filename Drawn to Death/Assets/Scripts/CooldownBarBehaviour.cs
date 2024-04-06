using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarBehaviour
{
    private Slider cooldownBar;
    private Color low;
    private Color high;
    private bool filled = false;
    private float frames = 0f;

    public CooldownBarBehaviour(Slider slider, float max, Color lowColor, Color highColor)
    {
        cooldownBar = slider;
        cooldownBar.maxValue = max;
        cooldownBar.value = max;
        low = lowColor;
        high = highColor;
        cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
    }

    // Set Cooldown bar value
    public void SetBar(float current)
    {
        cooldownBar.value = current;

        if (cooldownBar.maxValue - cooldownBar.value < 0.1f && !filled) // Flash white when full
        {
            cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.white;
            if (frames >= 25)
            {
                filled = true;
            }
            else
            {
                frames += 1;
            }
        }
        else if (cooldownBar.maxValue - cooldownBar.value >= 0.1f)
        {
            filled = false;
            frames = 0f;
            // Dynamically changes the color of the healthbar based on remaining health
            cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
        }
        else
        {
            // Dynamically changes the color of the healthbar based on remaining health
            cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
        }
    }
}
