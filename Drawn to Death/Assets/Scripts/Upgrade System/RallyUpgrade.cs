using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyUpgrade : Upgrade
{
    [Header("Level 1")]
    public float cooldownReduction;

    [Header("Level 2")]
    public float allyHealIncrease;

    [Header("Level 3")]
    public float allyStrIncrease;
    public float allySpdIncrease;
    public float allyAtkSpdModAdjustment;

    [Header("Level 4")]
    public float buffDurationIncrease;

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

        //Level 2 Buff
        if (level >= 2)
        {
            player.allyHealPercentage += allyHealIncrease;
        }

        //Level 3 Buff
        if (level >= 3)
        {
            player.allyStrModifier += allyStrIncrease;
            player.allySpdModifier += allySpdIncrease;
            player.allyAtkSpdModifier = Mathf.Max(0, player.allyAtkSpdModifier + allyAtkSpdModAdjustment);
        }

        //Level 4 Buff
        if (level >= 4)
        {
            player.allyBuffDuration += buffDurationIncrease;
            foreach (EnemyAI ai in FindObjectsOfType<EnemyAI>())
            {
                if (ai.buffTimer != null)
                {
                    ai.buffTimer.SetDuration(player.allyBuffDuration);
                }
            }
        }
    }
}
