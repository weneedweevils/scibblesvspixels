using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class Land : ChildBaseState
{
    public Land(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine, BaseState parentState) : base(boss, childStateMachine, parentStateMachine, parentState)
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

    // This method will "Land" the oodler on the ground
    public bool LandForRun(float speed = 15)
    {

        boss.SetAirPosition();
        var step = speed * Time.deltaTime;
        var runGroundPosition = runPosition + new Vector3(0, -12f, 0);
        oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, runGroundPosition, step));
        if (Vector3.Distance(transform.position, runGroundPosition) < 0.3f)
        {
            animator.SetTrigger("Walk");
            shadowAnimator.SetTrigger("Walk");
            oodlerRunDirection = (glich.transform.position - transform.position).normalized;
            HideShadow();
            return true;

        }
        else
        {
            return false;
        }
    }
}
