using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptGrab : ChildBaseState
{


    bool reachedTarget = false;
    private bool isGrabFrame = false;
    private bool grabWasActivated = false;
    private float chaseSpeed;
    private AnimationEventNotifier animationEventNotifier;

    public AttemptGrab(Boss boss, ParentBaseState parentBaseState, float chaseSpeed) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        grabWasActivated = false;
        reachedTarget = false;
        isGrabFrame = false;
        boss.ShowAttack();
        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.GrabNotifier += AnimationOffset;
        animationEventNotifier.HitBoxActive += ActivateHitbox;
        boss.BringSpriteToForeground();
        boss.animator.SetTrigger("Grab");
        //boss.GetShadow().SetTrigger("Grab");
        //boss.GetShadow().SetTrigger("Slam"); // the shadow shrinks in its animator when you 
    }

    public override void ExitState()
    {
        base.ExitState();
        animationEventNotifier.GrabNotifier -= AnimationOffset;
        animationEventNotifier.HitBoxActive -= ActivateHitbox;
        ResetState();
    }


    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // This statement makes it so that the oodler will follow glich until its hand commes down
        if(!isGrabFrame){
            boss.MoveToGlich(chaseSpeed);
            boss.SetLastPosition(); // sets glich last position

        }
        // This if statement is for when the fist comes down
        if(!reachedTarget && isGrabFrame){
            boss.Slam(chaseSpeed);
            if(!grabWasActivated && boss.CloseToTarget()){
                boss.EnableAttackHitbox(true);
                grabWasActivated = true;
            }

            if(boss.ReachedPlayerReal()){
                reachedTarget = true;
            }
        }

        // This statment is for after the fist comes down
        else if(isGrabFrame && grabWasActivated){
                if(boss.IsCaught()){
                    Debug.Log("GO to the state where we are holding glich");
                    boss.playerScript.DisableInput();
                    boss.animator.SetTrigger("Caught");
                    boss.GetShadow().SetTrigger("Caught");
                }
                else{
                Debug.Log("something went wrong with the grab!");
                     boss.SetBossCaught(false);
                     boss.animator.SetTrigger("Idle");
                     boss.GetShadow().SetTrigger("idle");
                     boss.EnableGrabHitbox(false);
            }
                //childStateMachine.ChangeState(boss.rise);
                //childStateMachine.ChangeState(boss.vulnerableState);
        }
    }


    // Helper Functions //
    public void AnimationOffset(){
        isGrabFrame = true;
        Debug.Log("THE GRAB HAS STARTED");
    }
    
    public void ActivateHitbox(){
        Debug.Log("Enabled attack Hitbox");
        boss.EnableGrabHitbox(true);
        grabWasActivated = true;
    }

    public override void ResetState()
    {
        base.ResetState();
        grabWasActivated = false;
        reachedTarget = false;
        isGrabFrame = false;
    }
}