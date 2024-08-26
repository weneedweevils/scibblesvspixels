using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerChargeAttack : OodlerBase
{

    
    public OodlerChargeAttack(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
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
        bool attackCharged = boss.RevealAttack();


        if(attackCharged)
        {
            boss.attackType = Boss.AttackType.Slam;//(Boss.AttackType)Random.Range(0, 2);
            Debug.Log("ATTACK TYPE IS " + boss.attackType);


            if (boss.attackType == Boss.AttackType.Slam)
            {
                oodlerStateMachine.ChangeState(boss.oodlerSlam);
            }

            else if (boss.attackType == Boss.AttackType.Grab)
            {
                oodlerStateMachine.ChangeState(boss.oodlerGrab);
            }
            
        }
    }

 
}
