using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerSlam : OodlerBase
{

    bool reachedTarget = false;
    bool delay = true;
    private float timer = 0f;
    private float delayTimer = 0f;
    BoxCollider2D DamageCollider;
    CircleCollider2D hitboxCollider;
   


    public OodlerSlam(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
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
        delayTimer = 0f;    
        reachedTarget = false;
        boss.SetLastPosition();
        delay = true;
        boss.AttackSprite.color = new Color(243, 255, 104, 1);
        DamageCollider = boss.GetComponent<BoxCollider2D>();
        hitboxCollider = boss.GetComponent <CircleCollider2D>();
        hitboxCollider.enabled = true;



    }

    public override void ExitState()
    {
        delayTimer = 0f;
        boss.PlayerScript.oodlerCooldown = false;
        base.ExitState();
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
                DamageCollider.enabled = false;
                boss.PlayerScript.oodlerCooldown = true;
            }

            // This will continue to move the hand down on glich
            if (!reachedTarget)
            {
                
                DamageCollider.enabled = true;
                boss.Slam();
            }

            // Logic for once we hit the ground
            else
            {
                timer += Time.deltaTime;
                // if the oodler has been on the ground for more than 5 seconds get up
                if (timer > 5f)
                {

                    hitboxCollider.enabled = true;
                    oodlerStateMachine.ChangeState(boss.oodlerIdle);
                }
            }

        }

        // a few seconds of delay and a color shift of shadow to give player time to react
        else
        {

            delayTimer += Time.deltaTime;
            if (delayTimer > 1f)
            {
                Debug.Log("about to strike my hand down");
                boss.AttackSprite.color = new Color(243, 0, 104, 1);
                delay = false;
            }
        }

    }
}
