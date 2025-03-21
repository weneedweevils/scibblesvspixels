﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weak", menuName = "Status Effects/Weak")]
public class Weakness : StatusEffect
{
    [Header("Effects")]
    [Tooltip("Flat decrease in the amount of damage dealt")]
    public float damageReduction;
    [Tooltip("Flat increase in the amount of damage taken")]
    public float damageTaken;

    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            //Apply Effects
            controller.playerAttack.damage.baseIncrease -= damageReduction;
            controller.player.incomingDamage.baseIncrease += damageTaken;
            controller.player.canHeal = false;
        }

        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Apply Effects
            target.damage.baseIncrease -= damageReduction;
            target.incomingDamage.baseIncrease += damageTaken;
            target.canHeal = false;
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            //Remove Effects
            controller.playerAttack.damage.baseIncrease += damageReduction;
            controller.player.incomingDamage.baseIncrease -= damageTaken;
            controller.player.canHeal = true;
        }

        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Remove Effects
            target.damage.baseIncrease += damageReduction;
            target.incomingDamage.baseIncrease -= damageTaken;
            target.canHeal = true;
        }
    }
}