using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealUpgrade : Upgrade
{
    [Header("Per Level")]
    public float radiusIncrease;
    public float extraDamage;
    public float cooldownReduction;

    public override void ApplyUpgrade(int level)
    {
        //Collect Player reference
        Attack eraser = FindObjectOfType<Attack>();

        //Increase radius of lifesteal
        eraser.lifestealImage.transform.localScale /= eraser.lifestealRadius * 10.45f;
        eraser.lifestealRadius += radiusIncrease * level;
        eraser.lifestealImage.transform.localScale *= eraser.lifestealRadius * 10.45f;

        //Increase damage/sec of Lifesteal
        eraser.lifestealDamage += extraDamage * level * eraser.lifestealDuration;

        //Reduce cooldown of Lifesteal
        eraser.lifestealCooldown -= cooldownReduction * level;
        eraser.lifestealTimer.SetCooldown(eraser.lifestealCooldown);
        eraser.lifestealRatio = eraser.lifestealCooldown / eraser.lifestealDuration;
    }
}
