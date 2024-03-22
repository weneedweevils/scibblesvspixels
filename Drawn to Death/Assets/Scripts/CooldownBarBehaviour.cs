using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarBehaviour
{
    private Slider cooldownBar;
    private Color low;
    private Color high;

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

        // Dynamically changes the color of the healthbar based on remaining health
        cooldownBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, cooldownBar.normalizedValue);
    }
}
