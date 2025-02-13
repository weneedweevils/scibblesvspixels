using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RallyUpgrade : Upgrade
{
    [Header("Level 1")]
    public float cooldownReduction;
    public RallyBuff lvl1Buff;

    [Header("Level 2")]
    public RallyBuff lvl2Buff;

    [Header("Level 3")]
    public RallyBuff lvl3Buff;

    [Header("Level 4")]
    public RallyBuff lvl4Buff;

    public override void ApplyUpgrade(int level)
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        //Level 1 Buff
        if (level >= 1)
        {
            player.recallCooldown = Mathf.Max(0, player.recallCooldown - cooldownReduction);
            if (player.recallTimer != null)
            {
                player.recallTimer.SetCooldown(player.recallCooldown);
            }
        }

        switch (level)
        {
            case 0:
                return;
            case 1:
                player.rallyEffect = lvl1Buff;
                return;
            case 2:
                player.rallyEffect = lvl2Buff;
                return;
            case 3:
                player.rallyEffect = lvl3Buff;
                return;
            case 4:
                player.rallyEffect = lvl4Buff;
                return;
            default:
                throw new ArgumentOutOfRangeException(string.Format(
                    "Error applying Rally Upgrade - Level {0} is out of the expected range of expected values 0-4",
                    level
                    ));
        }
    }
}
