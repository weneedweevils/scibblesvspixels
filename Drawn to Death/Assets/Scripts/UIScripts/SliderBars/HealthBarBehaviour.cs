using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : HealthBar
{
    // FMOD Glich Hurt event
    public string sfx;

    public override void SetHealth(float health, float maxHealth)
    {
        if (sfx != null && !sfx.Equals("") && health < healthBar.value){
            // Play the FMOD event 
            FMODUnity.RuntimeManager.PlayOneShot(sfx);
        }

        healthBar.value = health;
        healthBar.maxValue = maxHealth;

        // Dynamically changes the color of the healthbar based on remaining health
        healthBar.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, healthBar.normalizedValue);
    }
}
