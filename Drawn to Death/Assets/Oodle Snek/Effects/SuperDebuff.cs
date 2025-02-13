using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Super Debuff", menuName = "Status Effects/Super Debuff")]
public class SuperDebuff : StatusEffect
{
    public bool firstTime = true;

    [Header("Effects")]
    public float playerSlowdownFactor;
    public float slowdownFactor;
    public float atkSlowdownFactor;
    [Tooltip("Flat decrease in the amount of damage dealt")]
    public float damageReduction;
    [Tooltip("Flat increase in the amount of damage taken")]
    public float damageTaken;

    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Apply Confusion Effect
            if (Random.Range(0f, 1f) > 0.3f)
                target.speed.setSign = VariableStat.Sign.Positive;
            else
                target.speed.setSign = VariableStat.Sign.Negative;

            if (firstTime)
            {
                firstTime = false;

                //Apply Weakness Effects
                target.damage.baseIncrease -= damageReduction;
                target.incomingDamage.baseIncrease += damageTaken;
                target.canHeal = false;

                //Apply Slowness Effects
                target.speed.multiplier -= slowdownFactor;
                target.attackCooldown.multiplier += atkSlowdownFactor;
                target.attackTimer.SetCooldown(target.attackCooldown.value);
            }
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.enemyAI != null)
        {
            //Get target EnemyAI referfence
            EnemyAI target = controller.enemyAI;

            //Remove Confusion Effect
            target.speed.setSign = VariableStat.Sign.Neutral;

            //Remove Weakness Effects
            target.damage.baseIncrease += damageReduction;
            target.incomingDamage.baseIncrease -= damageTaken;
            target.canHeal = true;

            //Remove Slowness Effects
            target.speed.multiplier += slowdownFactor;
            target.attackCooldown.multiplier -= atkSlowdownFactor;
            target.attackTimer.SetCooldown(target.attackCooldown.value);

            //Reset image color
            target.selfImage.color = (target.team == Team.player ? target.allyCol : Color.white);
        }
    }
}