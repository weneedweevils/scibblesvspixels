using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TextCore;
using Vector3 = UnityEngine.Vector3;

public class Land : ChildBaseState
{
    private float landSpeed;
    public Land(Boss boss, ParentBaseState parentBaseState, float landSpeed) : base(boss, parentBaseState)
    {
    }
    private bool reachedPosition = false;
    private Vector3 runGroundPosition;

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        reachedPosition = false;
        runGroundPosition = boss.transform.position + new Vector3(0, -12f, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        reachedPosition = LandOodler();
        if(reachedPosition){
            boss.BringSpriteToBackground();
            parentBaseState.NextSubState();
            //childStateMachine.ChangeState(boss.run);
        }
        
    }

    // MOVE TO BOSS SCRIPT
    // This method will "Land" the oodler on the ground
    public bool LandOodler(float speed = 15)
    {
        var step = landSpeed * Time.deltaTime;
        boss.oodlerRB.MovePosition(Vector3.MoveTowards(boss.transform.position, runGroundPosition, step));
        if (Vector3.Distance(boss.transform.position, runGroundPosition) < 0.3f)
        {
            boss.oodlerRB.MovePosition(runGroundPosition);
            boss.HideShadow();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ResetState()
    {
        base.ResetState();

    }
}
