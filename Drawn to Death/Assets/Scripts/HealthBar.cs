using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Color low;
    public Color high;

    public abstract void SetHealth(float health, float maxHealth);

    public void Enable()
    {
        healthBar.gameObject.SetActive(true);
    }
    public void Disable()
    {
        healthBar.gameObject.SetActive(false);
    }
}
