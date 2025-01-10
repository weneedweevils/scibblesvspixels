using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rally Buff", menuName = "Status Effects/Rally Buff")]
public class RallyBuff : StatusEffect
{
    [Header("General Effects")]
    public float strModifier;
    
    [Header("Unit Specific Effects")]
    public float hopperStrModifier;
    
    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            EnemyAI target = controller.enemyAI;
            
            if (target.type == Type.hopper)
            {
                target.damage.multiplier += hopperStrModifier;
            }
            else
            {
                target.damage.multiplier += strModifier;
            }
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            EnemyAI target = controller.enemyAI;

            if (target.type == Type.hopper)
            {
                target.damage.multiplier -= hopperStrModifier;
            }
            else
            {
                target.damage.multiplier -= strModifier;
            }
        }
    }
}
