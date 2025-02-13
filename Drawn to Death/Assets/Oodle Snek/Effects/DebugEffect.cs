using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DEBUG", menuName = "Status Effects/Debug")]
public class DebugEffect : StatusEffect
{
    public override void ApplyEffect(StatusEffectController controller)
    {
        Debug.LogFormat(
            "DEBUG - Applying Effect...\nIs Player: {0}\nPlayer Ref: {1}\nEnemyAI Ref: {2}",
            controller.isPlayer,
            controller.player == null ? "Null" : "Not Null",
            controller.enemyAI == null ? "Null" : "Not Null"
        );
    }

    public override void EndEffect(StatusEffectController controller)
    {
        Debug.LogFormat(
            "DEBUG - End Effect...\nIs Player: {0}\nPlayer Ref: {1}\nEnemyAI Ref: {2}",
            controller.isPlayer,
            controller.player == null ? "Null" : "Not Null",
            controller.enemyAI == null ? "Null" : "Not Null"
        );
    }
}
