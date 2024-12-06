using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHand : ChildBaseState
{


    bool reachedTarget = false;
    private bool isSlamFrame = false;
    private bool slamWasActivated = false;
    private AnimationEventNotifier animationEventNotifier;
  

    public SwingHand(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        slamWasActivated = false;
        reachedTarget = false;
        isSlamFrame = false;
        //boss.SetAirPosition(); // sets the last position the oodler was in the air since we have downward motion instead of doing this lets try using +12
        boss.ShowAttack();

        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.SlamNotifier += AnimationOffset;
        animationEventNotifier.HitBoxActive += ActivateHitbox;
        boss.ChangeSpriteSortingOrder(5);
        boss.animator.SetTrigger("Slam");
        boss.GetShadow().SetTrigger("Slam"); // the shadow shrinks in its animator when you 
    }

    public override void ExitState()
    {
        base.ExitState();
        animationEventNotifier.SlamNotifier -= AnimationOffset;
        animationEventNotifier.HitBoxActive -= ActivateHitbox;
    }


    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // This statement makes it so that the oodler will follow glich until its hand commes down
        if(!isSlamFrame){
            boss.Stalk(false,100f);
            boss.SetLastPosition(); // sets glich last position

        }
        // This if statement is for when the fist comes down
        if(!reachedTarget && isSlamFrame){
            boss.Slam();
            if(!slamWasActivated && boss.CloseToTarget()){
                boss.EnableAttackHitbox(true);
                slamWasActivated = true;
            }

            if(boss.ReachedPlayerReal()){
                reachedTarget = true;
            }
        }

        // This statment is for after the fist comes down
        else if(isSlamFrame && slamWasActivated){
                childStateMachine.ChangeState(boss.vulnerableState);
        }
    }


    // Helper Functions //

    public void AnimationOffset(){
        isSlamFrame = true;
        Debug.Log("THE SLAM HAS STARTED");
    }
    
    public void ActivateHitbox(){
        Debug.Log("Enabled attack Hitbox");
        boss.EnableAttackHitbox(true);
        slamWasActivated = true;
    }
}
