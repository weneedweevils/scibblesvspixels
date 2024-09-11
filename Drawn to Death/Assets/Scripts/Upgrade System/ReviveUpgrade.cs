using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveUpgrade : Upgrade
{
    [Header("Per Level")]
    public float radiusIncrease;
    public float cooldownDecrease;

    public override void ApplyUpgrade(int level)
    {
        //Collect the eraser attack reference
        Attack eraser = FindObjectOfType<Attack>();

        //Increase revive radius
        eraser.reviveRadius += radiusIncrease * level;

        //Decrease the revive cooldown
        eraser.reviveCooldown = Mathf.Max(0, eraser.reviveCooldown - cooldownDecrease * level);
        eraser.reviveTimer.SetCooldown(eraser.reviveCooldown);
    }
}
