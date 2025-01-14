using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Confusion", menuName = "Status Effects/Confusion")]
public class Confusion : StatusEffect
{
    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            //TODO: Apply Confusion effect to Player
        }

        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Apply Effects
            if (Random.Range(0f, 1f) > 0.3f)
                target.speed.setSign = VariableStat.Sign.Positive;
            else
                target.speed.setSign = VariableStat.Sign.Negative;
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            //TODO: Remove Confusion effect from Player
        }

        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            target.speed.setSign = VariableStat.Sign.Neutral;
        }
    }
}
