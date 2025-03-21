﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rally Buff", menuName = "Status Effects/Rally Buff")]
public class RallyBuff : StatusEffect
{
    [Header("General Effects")]
    [Range(0, 1)] public float healPercentage = 0.3f;
    public float strModifier = 0.5f;
    public float spdModifier = 0.5f;
    public float atkSpdModifier = 0.5f;

    [Header("Unit Specific Effects")]
    public float hopperStrModifier = 1f;
    public float crabSpdModifier = 1f;
    public float crabAtkSpdModifier = 0.8f;

    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Mark the target as buffed
            target.buffed = true;

            /* ----- Apply the buff effects ----- */

            //Heal the target
            target.Heal(target.maxHealth * healPercentage);
            
            //Strength buff
            if (target.type == Type.hopper)
            {
                target.damage.multiplier += hopperStrModifier;
            }
            else
            {
                target.damage.multiplier += strModifier;
            }

            //Speed buff
            if (target.type == Type.crab)
            {
                target.speed.multiplier += crabSpdModifier;
            }
            else
            {
                target.speed.multiplier += spdModifier;
            }

            //Attack Cooldown Buff
            if (target.type == Type.crab)
            {
                target.attackCooldown.multiplier -= crabAtkSpdModifier;
            }
            else
            {
                target.attackCooldown.multiplier -= atkSpdModifier;
            }

            target.attackTimer.SetCooldown(target.attackCooldown.value);

            //Special - Snake
            if (target.type == Type.snek)
            {
                OodleSnek snek = (OodleSnek)target;
                snek.SetEffect(snek.rallyEffect);
            }
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //De-Mark the target as buffed
            target.buffed = false;

            /* ----- Remove the buff effects ----- */

            //Strength buff
            if (target.type == Type.hopper)
            {
                target.damage.multiplier -= hopperStrModifier;
            }
            else
            {
                target.damage.multiplier -= strModifier;
            }

            //Speed buff
            if (target.type == Type.crab)
            {
                target.speed.multiplier -= crabSpdModifier;
            }
            else
            {
                target.speed.multiplier -= spdModifier;
            }

            //Attack Cooldown Buff
            if (target.type == Type.crab)
            {
                target.attackCooldown.multiplier += crabAtkSpdModifier;
            }
            else
            {
                target.attackCooldown.multiplier += atkSpdModifier;
            }
            target.attackTimer.SetCooldown(target.attackCooldown.value);

            //Reset color
            target.selfImage.color = Color.white;

            //Special - Snake
            if (target.type == Type.snek)
            {
                OodleSnek snek = (OodleSnek)target;
                snek.SetEffect(snek.defaultEffect);
            }
        }
    }
}