using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lifesteal", menuName = "Status Effects/Lifesteal")]
public class LifestealEffect : StatusEffect
{
    [Header("Effects")]
    public float slowdownFactor;
    public float atkSlowdownFactor;

    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;
            
            target.slowed = true;
            target.speed.multiplier -= slowdownFactor;
            target.attackCooldown.multiplier += atkSlowdownFactor;
            target.attackTimer.SetCooldown(target.attackCooldown.value);
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            target.slowed = false;
            target.speed.multiplier += slowdownFactor;
            target.attackCooldown.multiplier -= atkSlowdownFactor;
            target.attackTimer.SetCooldown(target.attackCooldown.value);

            target.selfImage.color = (target.team == Team.player ? target.allyCol : Color.white);
        }
    }
}
