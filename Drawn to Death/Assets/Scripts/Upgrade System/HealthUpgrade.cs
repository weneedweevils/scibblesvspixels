using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : Upgrade
{
    [Header("Per Level")]
    public int extraHP;

    public override void ApplyUpgrade(int level)
    {
        //Collect Player reference
        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        //Increment HP
        player.maxHealth += extraHP * level;
        player.health = player.maxHealth;
    }
}