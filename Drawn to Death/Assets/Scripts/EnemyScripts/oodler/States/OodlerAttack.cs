using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerAttack : OodlerBase
{

    bool attackCharged;
    public OodlerAttack(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Attack State");
        // change colour to red
        //boss.ShowAttack();

    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Attack State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        boss.Stalk();

        attackCharged = boss.RevealAttack();
        if(attackCharged)
        {
            oodlerStateMachine.ChangeState(boss.oodlerGrab);
        }
    }

 
}
