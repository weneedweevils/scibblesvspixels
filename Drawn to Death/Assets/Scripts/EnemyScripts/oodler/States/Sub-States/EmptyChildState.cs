using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyChildState : ChildBaseState
{
    public EmptyChildState(Boss boss, ParentBaseState parentBaseState) : base(boss, parentBaseState)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Empty  State");
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
        parentBaseState.NextSubState();
    }

    public override void ResetState()
    {
        base.ResetState();

    }
}
