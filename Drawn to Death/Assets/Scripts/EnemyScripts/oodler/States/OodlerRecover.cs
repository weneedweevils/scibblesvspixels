using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerRecover : OodlerBase
{
    private float timer = 0f;
    public OodlerRecover(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
        
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        boss.ShowShadow();
        boss.vulnerable = false;
        timer = 0f;
        boss.EnableAreaHitbox(false);

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        
        base.FrameUpdate();
        boss.MoveUp();
        if(boss.ReachedAirPosition())
        {
            timer = timer + Time.deltaTime;
            if (timer > 3f)
            {
                boss.BossSprite.sortingOrder = 8;
                oodlerStateMachine.ChangeState(boss.oodlerIdle);
            }
        }
    }


}
