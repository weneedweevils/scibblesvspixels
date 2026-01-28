using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHand : ChildBaseState
{


    bool reachedTarget = false;
    private bool isSlamFrame = false;
    private bool slamWasActivated = false;
    private AnimationEventNotifier animationEventNotifier;
    private float chaseSpeed;
  
    /// <summary>
    /// This is the state where the oodler swings its hand down on glich
    /// </summary>
    /// <param name="boss"></param>
    /// <param name="parentBaseState"></param>
    /// <param name="chaseSpeed"></param>
    public SwingHand(Oodler boss, ParentBaseState parentBaseState, float chaseSpeed = 100f) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
    }


    public override void EnterState()
    {
        base.EnterState();
        slamWasActivated = false;
        reachedTarget = false;
        isSlamFrame = false;
     

        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.AttackNotifier += AnimationOffset;
        animationEventNotifier.HitBoxActive += ActivateHitbox;
        boss.animator.SetTrigger("Slam");
    }

    public override void ExitState()
    {
        base.ExitState();
       
        ResetState();
    }


    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // This statement makes it so that the oodler will follow glich until its hand commes down
        if(!isSlamFrame){
            boss.MoveToGlich(chaseSpeed);
            boss.SetLastPosition(); // sets glich last position

        }
        // This if statement is for when the fist comes down
        if(!reachedTarget && isSlamFrame){
            if (boss.Slam())
            {
                reachedTarget = true;
            }
        }

        // This statment is for after the fist comes down
        else if(isSlamFrame && slamWasActivated){

            boss.EnableColumnHitbox(false);
            parentBaseState.NextSubState();
            boss.BringSpriteToBackground();
        }
    }


    // Helper Functions //

    public void AnimationOffset(){
        isSlamFrame = true;
    }
    
    public void ActivateHitbox(){
        boss.EnableAttackHitbox(true);
        boss.EnableColumnHitbox(true);
        slamWasActivated = true;
    }

    public override void ResetState()
    {
        slamWasActivated = false;
        reachedTarget = false;
        isSlamFrame = false;
        animationEventNotifier.AttackNotifier -= AnimationOffset;
        animationEventNotifier.HitBoxActive -= ActivateHitbox;
        boss.EnableAttackHitbox(false);
        boss.EnableColumnHitbox(false);

    }
}
