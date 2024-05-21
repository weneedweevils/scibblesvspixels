using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerIdle : OodlerBase
{

    // Random Values needed
    Vector3 offScreen;
    
    bool reachedTarget = false;
    bool calculateTime = false;
    private float timer = 0f;

    public OodlerIdle(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
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
        reachedTarget = false;
        Debug.Log("Entering Idle State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting Idle State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // if we reached target say that we have reached it
        if (!reachedTarget && boss.ReachedPlayer())
        {
                reachedTarget = true;
        }

        // start counting once we reached our target
        if (reachedTarget)
        {
            timer += Time.deltaTime;
        }

        //switch states once we have been following for more than 5 seconds
        if (timer > 2f)
        {
            oodlerStateMachine.ChangeState(boss.oodlerAttack);
        }


        // gradually follow glich's position
        boss.Stalk();



    }
}
