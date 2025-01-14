using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slow", menuName = "Status Effects/Slow")]
public class Slowness : StatusEffect
{
    [Header("Effects")]
    public float slowdownFactor;
    public float atkSlowdownFactor;

    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            //TODO: Apply slow effect to Player
        }

        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Apply Effects
            target.speed.multiplier -= slowdownFactor;
            target.attackCooldown.multiplier += atkSlowdownFactor;
            target.attackTimer.SetCooldown(target.attackCooldown.value);
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            //TODO: Remove slow effect from Player
        }

        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Remove Effects
            target.speed.multiplier += slowdownFactor;
            target.attackCooldown.multiplier -= atkSlowdownFactor;
            target.attackTimer.SetCooldown(target.attackCooldown.value);

            //Reset image color
            target.selfImage.color = (target.team == Team.player ? target.allyCol : Color.white);
        }
    }
}
