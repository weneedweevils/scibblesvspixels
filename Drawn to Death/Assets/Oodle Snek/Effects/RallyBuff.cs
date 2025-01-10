using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rally Buff", menuName = "Status Effects/Rally Buff")]
public class RallyBuff : StatusEffect
{
    [Header("General Effects")]
    [Range(0, 1)] public float healPercentage = 0.3f;
    public float strModifier = 0.5f;
    
    [Header("Unit Specific Effects")]
    public float hopperStrModifier = 1f;
    
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
        }
    }
}
