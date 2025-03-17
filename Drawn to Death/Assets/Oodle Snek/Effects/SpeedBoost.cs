using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost", menuName = "Status Effects/Speed Boost")]
public class SpeedBoost : StatusEffect
{
    [Header("Effects")]
    public float boostFactor;

    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Apply Speed Boost
            target.speed.multiplier += boostFactor;
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Remove Speed Boost
            target.speed.multiplier -= boostFactor;
        }
    }
}
