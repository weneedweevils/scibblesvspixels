using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This is an attacking state where the oodler will charge towards glich and damage him
/// </summary>
public class OodlerRun : ParentBaseState
{

    public OodlerRun(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }
    
    
    private Vector3 runPosition;

    
    
    public override void EnterState()
    {
        base.EnterState();
        boss.childStateMachine.ChangeState(boss.chase);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //childStateMachine.currentChildState.FrameUpdate();  
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
