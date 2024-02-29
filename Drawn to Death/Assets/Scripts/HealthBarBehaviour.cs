using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider healthBar;
    public Color low;
    public Color high;

    public void SetHealth(float health, float maxHealth)
    {
        healthBar.value = health;
        healthBar.maxValue = maxHealth;

        // Dynamically changes the color of the healthbar based on remaining health
        healthBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, healthBar.normalizedValue);
    }
}
