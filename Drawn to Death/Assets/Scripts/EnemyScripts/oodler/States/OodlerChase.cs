using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class OodlerChase : OodlerBase
{

    // Random Values needed
    Vector3 offScreen;
    
    bool reachedTarget = false;
    bool calculateTime = false;
    private float timer = 0f;

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
        timer = 0f;
        reachedTarget = false;
        Debug.Log("Entering Chase State");
        boss.ShowShadow();
    }

    public override void ExitState()
    {
        reachedTarget = false;
        base.ExitState();
        Debug.Log("Exiting Chase State");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // if we reached target say that we have reached it
        if (!reachedTarget && boss.ReachedPlayer())
        {
            reachedTarget = true;
            Debug.Log("WE have reached the target!@");
        }

        // start counting once we reached our target
        if (reachedTarget)
        {
            timer += Time.deltaTime;
        }

        //switch states once we have been following for more than 5 seconds
        if (timer > 1.5f)
        {
            oodlerStateMachine.ChangeState(boss.oodlerChargeAttack);
        }


        // gradually follow glich's position
        if (Vector3.Distance(boss.Glich.transform.position, boss.transform.position) < 20f)
        {
            Debug.Log("something happened Here");
            var speed = 50f;
            boss.Stalk(speed);
        }
        else
        {
            boss.Stalk();// Set this back to empty
        }
        



    }
}
