using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerSlam : OodlerBase
{
    bool reachedTarget = false;
    bool delay = true;
    private float timer = 0f;
    private float delayTimer = 0f;

    public OodlerSlam(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        boss.grabbing = false;
        base.EnterState();
        timer = 0f;
        delayTimer = 0f;    
        reachedTarget = false;
        boss.SetLastPosition();
        boss.SetAirPosition();
        delay = true;
        boss.ShowAttack();
        //boss.animator.SetTrigger("Slam");
    }


    public override void ExitState()
    {
        delayTimer = 0f;
        boss.oodlerSlamCooldown = false; // set the cooldown back
        base.ExitState();
        boss.SlamNum++;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // if the delay is over
        if (!delay)
        {
            // disable collider once we reach the ground and set reach target to true
            if (!reachedTarget && boss.ReachedPlayerReal())
            {
                reachedTarget = true;
                boss.EnableAttackHitbox(false);
                boss.oodlerSlamCooldown = true; // set to true so that the oodler does not hurt anyone on the ground
                boss.HideShadow();
                boss.vulnerable = true;
            }

            // This will continue to move the hand down on glich
            if (!reachedTarget)
            {
                boss.Slam();

                // activates the attack hitbox a few units above gliches position
                if (boss.transform.position.y < boss.GetLastPosition().y + 0.01f)
                {
                    boss.EnableAttackHitbox(true);
                }
            }

            // Logic for once we hit the ground
            else
            {
                timer += Time.deltaTime;
                // if the oodler has been on the ground for more than 5 seconds get up
                if (timer > boss.bossVulnerabilityTime)
                {
                    
                    oodlerStateMachine.ChangeState(boss.oodlerRecover);
                    boss.ChangeSpriteSortingOrder(8);
                    
                }
            }

        }

        // a few seconds of delay to give player time to react
        else
        {

            delayTimer += Time.deltaTime;
            if (delayTimer > boss.slamWarningTime)
            {
               
                boss.ChangeSpriteSortingOrder(5);
                Debug.Log("about to strike my hand down");
                delay = false;
                
                boss.EnableAreaHitbox(true);

            }
        }

    }
}
