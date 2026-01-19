using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptGrab : ChildBaseState
{

    private bool isGrabFrame = false;
    private bool grabWasActivated = false;
    private bool reachedTarget = false;
    private bool caught = false;
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

        // This script gets two animation event notifiers from the grab animation, and one event from the grab hitbox
        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.GrabNotifier += AnimationOffset;
        animationEventNotifier.HitBoxActive += ActivateHitbox;

        boss.BringSpriteToForeground();
        boss.animator.SetTrigger("Grab"); // This state has a function that calls animation event
        boss.SetLastPosition();
    }

    public override void ExitState()
    {
        base.ExitState();
        animationEventNotifier.GrabNotifier -= AnimationOffset;
        animationEventNotifier.HitBoxActive -= ActivateHitbox;

        grabWasActivated = false;
        reachedTarget = false;
        isGrabFrame = false;
    }


    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(!reachedTarget && isGrabFrame){
            boss.Slam(chaseSpeed);
            if(boss.ReachedPlayerReal()){
                reachedTarget = true;
            }   
        }

        // This statment is for after the oodler gets to the gliches last known position
        else if(isGrabFrame && grabWasActivated){

            if (caught)
            {
                boss.playerScript.DisableInput();
                boss.playerScript.animator.SetTrigger("Grabbed"); 
                boss.animator.SetTrigger("Caught");
                boss.EnableGrabHitbox(false);
                parentBaseState.GoToDropState();
                
            }
            else
            {
                boss.EnableGrabHitbox(false);
                parentBaseState.NextSubState();
            }
        }
    }


    // Helper Functions //

    // Events Fired from Invoke //
    public void AnimationOffset(){
        isGrabFrame = true;
        Debug.Log("THE GRAB HAS STARTED");
    }
    
    public void ActivateHitbox(){
        Debug.Log("Enabled attack Hitbox");
        boss.EnableGrabHitbox(true);
        GrabHitbox.grabbedGlich += SetCaught;  // IT might be better to set the boss caught variable in the boss script for safety
        grabWasActivated = true;
    }

    private void SetCaught() 
    {
        caught = true;
        GrabHitbox.grabbedGlich -= SetCaught;
    }

    public override void ResetState()
    {
        base.ResetState();
       
    }
}