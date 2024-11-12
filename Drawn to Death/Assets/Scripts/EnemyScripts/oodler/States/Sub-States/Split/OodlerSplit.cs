using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerSplit : ChildBaseState
{
    public OodlerSplit(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
       boss.Split();
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exiting Empty state");
    }

    public override void FrameUpdate()
    {
        Debug.Log("Updating...");
        base.FrameUpdate();
    }
}