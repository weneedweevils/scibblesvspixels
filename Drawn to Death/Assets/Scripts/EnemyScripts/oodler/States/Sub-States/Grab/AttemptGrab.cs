using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttemptGrab : ChildBaseState
{

    private bool isGrabFrame = false;
    private bool grabActivated = false;
    private bool grabDeactivated = false;
    private bool reachedTarget = false;
    private bool caught = false;
    private float chaseSpeed;

    private AnimationEventNotifier animationEventNotifier;

    public AttemptGrab(Oodler boss, ParentBaseState parentBaseState, float chaseSpeed) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
    }

  

    public override void EnterState()
    {
        base.EnterState();
        grabActivated = false;
        reachedTarget = false;
        isGrabFrame = false;
        grabDeactivated = false;
        caught = false;


    // This script gets two animation event notifiers from the grab animation, and one event from the grab hitbox
        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.AttackNotifier += ActivateHitbox;
        animationEventNotifier.AttackEndNotifier += DeactivateHitbox;


        boss.SetLastPosition();
        boss.BringSpriteToForeground();
        boss.animator.SetTrigger("Grab"); // This state has a function that calls animation event
    }

    public override void ExitState()
    {
        base.ExitState();

        animationEventNotifier.AttackNotifier -= ActivateHitbox;
        animationEventNotifier.AttackEndNotifier -= DeactivateHitbox;
    
        GrabHitbox.grabbedGlich -= SetCaught;
        grabActivated = false;
        grabDeactivated = false;
    }


    public override void FrameUpdate()
    {

        base.FrameUpdate();
        if (grabActivated)
        {
            if (boss.Slam(chaseSpeed))
            {
                reachedTarget = true;
            }

            if (caught) { 
                boss.playerScript.DisableInput();
                boss.animator.SetTrigger("Caught");
                boss.playerScript.animator.SetTrigger("Grabbed");
                boss.EnableGrabHitbox(false);
                boss.EnableColumnHitbox(false);

                if (reachedTarget)
                {

                    boss.MoveGlichWithOodler();
                    parentBaseState.GoToDropState();

                }
            }
            else if (grabDeactivated)
            {

                parentBaseState.NextSubState();
            }
          }



     }
 


    // Helper Functions //

    // Events Fired from Invoke //    

    // event fired when attack started event called from grabv2 animation
    public void ActivateHitbox(){
        Debug.Log("Enabled attack Hitbox");
        boss.EnableGrabHitbox(true);
        boss.EnableColumnHitbox(true);
        GrabHitbox.grabbedGlich += SetCaught;  // IT might be better to set the boss caught variable in the boss script for safety
        grabActivated = true;
    }


    // event fired when attack ended event called from grabv2 animation
    public void DeactivateHitbox()
    {

        boss.EnableColumnHitbox(false);
        boss.EnableGrabHitbox(false);
        grabDeactivated = true;
    }

    private void SetCaught() 
    {
        caught = true;
        GrabHitbox.grabbedGlich -= SetCaught;
    }
}