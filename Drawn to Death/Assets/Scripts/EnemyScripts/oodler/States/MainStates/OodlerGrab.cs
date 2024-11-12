using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OodlerGrab : BaseState
{   


    bool reachedTarget = false;
    bool delay = true;
    private float timer = 0f;
    private float delayTimer = 0f;

    public OodlerGrab(Boss boss, StateMachine oodlerStateMachine, ChildStateMachine childStateMachine) : base(boss, oodlerStateMachine, childStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.SetBossCaught(false);
        childStateMachine.ChangeState(boss.chase);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(childStateMachine.currentChildState==boss.emptyChildState){
            oodlerStateMachine.ChangeState(boss.oodlerGrab);
        }


        // // if the delay is over
        // if (!delay)
        // {
        //     // disable collider once we reach the ground and set reach target to true
        //     if (!reachedTarget && boss.ReachedPlayerReal())
        //     {
        //         reachedTarget = true;
        //         boss.EnableAttackHitbox(false);
        //         boss.SetSlamCooldown(true); // set to true so that the oodler does not hurt anyone on the ground
        //         boss.HideShadow();
        //         boss.SetBossVulnerability(true);
        //     }

        //     // This will continue to move the hand down on glich
        //     if (!reachedTarget)
        //     {
        //         boss.Slam();
        //         // activates the attack hitbox a few units above gliches position
        //         if (boss.transform.position.y < boss.GetLastPosition().y + 0.01f)
        //         {
        //             boss.EnableAttackHitbox(true);
        //         }
        //     }

        //     // Logic for once we hit the ground and if we caught them 
        //     else
        //     {
        //         if (boss.IsCaught())
        //         {
        //             boss.ControlAllies(boss.dropZoneObject, true); // change this to only occur when caught
        //             oodlerStateMachine.ChangeState(boss.oodlerRecover);
                    
        //         }

          
        //         // if the oodler has been on the ground for more than 5 seconds get up
        //         else if (timer > boss.bossVulnerabilityTime)
        //         {
        //             oodlerStateMachine.ChangeState(boss.oodlerRecover);
        //         }

        //         timer += Time.deltaTime;
        //     }

        // }

        // // a few seconds of delay and a color shift of shadow to give player time to react
        // else
        // {
        //     delayTimer += Time.deltaTime;
        //     if (delayTimer > boss.grabWarningTime)
        //     {
        //         delay = false;
        //         boss.EnableAreaHitbox(true);
        //         boss.ChangeSpriteSortingOrder(5);

        //     }
        // }
    }


   
}

