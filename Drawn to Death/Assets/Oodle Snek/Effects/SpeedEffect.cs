using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed", menuName = "Status Effects/Speed")]
public class SpeedEffect : StatusEffect
{
    public override void ApplyEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            controller.player.maxVelocity += 20;
        }
    }

    public override void EndEffect(StatusEffectController controller)
    {
        if (controller.isPlayer)
        {
            controller.player.maxVelocity -= 20;
        }
    }
}