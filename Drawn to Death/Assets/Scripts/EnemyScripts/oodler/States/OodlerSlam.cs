using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OodlerSlam : BaseState
{
    bool reachedTarget = false;
    bool delay = false;
    private float timer = 0f;
    private float delayTimer = 0f;
    private bool isSlamFrame = false;

    private bool slamWasActivated = false;
    private AnimationEventNotifier animationEventNotifier;

    public OodlerSlam(Boss boss, StateMachine oodlerStateMachine, ChildStateMachine childStateMachine) : base(boss, oodlerStateMachine, childStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        boss.grabbing = false;
        slamWasActivated = false;
        reachedTarget = false;
        isSlamFrame = false;

        base.EnterState();

        timer = 0f;
        delayTimer = 0f;    
        delay = false;

        reachedTarget = false;

        boss.SetLastPosition();
        boss.SetAirPosition();
        boss.ShowAttack();

        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.SlamNotifier += AnimationOffset;
        //animationEventNotifier.HitBoxActive += ActivateHitbox;
        
    }


    public override void ExitState()
    {
        delayTimer = 0f;
        boss.SetSlamCooldown(false); // set the cooldown back
        base.ExitState();
        boss.SlamNum++;
        animationEventNotifier.SlamNotifier -= AnimationOffset;
        animationEventNotifier.HitBoxActive -= ActivateHitbox;
        

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        // delay check
        if(!delay){
            delay=OnDelay();
        }

        // check to see if we have reached the players location 
        else if(!reachedTarget && boss.ReachedPlayerReal() && isSlamFrame && slamWasActivated){
                reachedTarget = true;
               
                boss.SetSlamCooldown(true); // set to true so that the oodler does not hurt anyone on the ground
                boss.HideShadow();
                boss.SetBossVulnerability(true);
                boss.animator.SetTrigger("Idle");
                boss.GetShadow().SetTrigger("Idle");
                Debug.Log("DISABLING ATTACK HITBOX");
                boss.EnableAttackHitbox(false);
        }


        // check to see if 
        else if(!reachedTarget && isSlamFrame){
            boss.Slam();

            if(!slamWasActivated && boss.ActivateSlamHitbox()){
                boss.EnableAttackHitbox(true);
                slamWasActivated = true;
            }
        }

        // Logic for once we hit the ground
        else if (reachedTarget)
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
    

    

    // This method is supposed to offset the slam
    public void AnimationOffset(){
        isSlamFrame = true;
        Debug.Log("THE SLAM HAS STARTED");
    }

    public void ActivateHitbox(){
        Debug.Log("Enabled attack Hitbox");
        boss.EnableAttackHitbox(true);
        slamWasActivated = true;
    }




    // Function will return false until the delay is over
    public bool OnDelay(){
        delayTimer+=Time.deltaTime;
        if(delayTimer > boss.slamWarningTime){
            boss.ChangeSpriteSortingOrder(5);
                Debug.Log("about to strike my hand down");
                delay = false;
                boss.EnableAreaHitbox(true);
                boss.animator.SetTrigger("Slam");
                boss.GetShadow().SetTrigger("Slam");
                return true;
        }
        else{
            return false;
        }

    }
  
}

