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
        boss.BossSprite.sortingOrder = 8;

        if (boss.caught == true)
        {
            //boss.ControlAllies(boss.pillar);
            boss.EnableGlichColliders(false);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        
        base.FrameUpdate();
        boss.MoveUp();

        boss.MoveGlichWithOodler();

        if(boss.ReachedAirPosition())
        {
            timer = timer + Time.deltaTime;
            if (timer > 3f)
            {
                if (boss.caught == true) {
                    oodlerStateMachine.ChangeState(boss.oodlerDrop);
                }
                else
                {
                    oodlerStateMachine.ChangeState(boss.oodlerIdle);
                }
            }
        }
    }


}
