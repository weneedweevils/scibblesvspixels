using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerChase : OodlerBase
{
    public OodlerChase(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }
   
    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
      
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

}
