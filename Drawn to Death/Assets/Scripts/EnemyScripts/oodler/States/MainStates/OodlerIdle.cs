using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerIdle : BaseState
{

    private float timer = 0f;

    public OodlerIdle(Boss boss, StateMachine oodlerStateMachine, ChildStateMachine childStateMachine) : base(boss, oodlerStateMachine, childStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        timer = 0f;
        boss.ShowShadow();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (boss.ReachedOffScreen())
        {
            timer = timer + Time.deltaTime;
            if (timer > 1f) {
               // oodlerStateMachine.ChangeState(boss.oodlerChase);
            }
        }
        else
        {
            boss.MoveOffScreen(150f);
        }
    }

}
