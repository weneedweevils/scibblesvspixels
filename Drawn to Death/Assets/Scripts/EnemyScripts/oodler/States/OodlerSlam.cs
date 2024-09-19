using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OodlerSlam : OodlerBase
{
    bool reachedTarget = false;
    bool delay = false;
    private float timer = 0f;
    private float delayTimer = 0f;
    private bool slamFrame = false;

    private AnimationEventNotifier animationEventNotifier;

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
        delay = false;

        reachedTarget = false;

        boss.SetLastPosition();
        boss.SetAirPosition();
        boss.ShowAttack();

        animationEventNotifier = boss.GetComponentInChildren<AnimationEventNotifier>(); //get animation event notifier
        animationEventNotifier.SlamNotifier += AnimationOffset;
        
    }


    public override void ExitState()
    {
        delayTimer = 0f;
        boss.SetSlamCooldown(false); // set the cooldown back
        base.ExitState();
        boss.SlamNum++;
        animationEventNotifier.SlamNotifier -= AnimationOffset;
        

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        // delay check
        if(!delay){
            delay=OnDelay();
        }

        // check to see if we have reached the players location 
        else if(!reachedTarget && boss.ReachedPlayerReal() && slamFrame){
                reachedTarget = true;
                boss.EnableAttackHitbox(false);
                boss.SetSlamCooldown(true); // set to true so that the oodler does not hurt anyone on the ground
                boss.HideShadow();
                boss.SetBossVulnerability(true);
                boss.animator.SetTrigger("Idle");
                boss.GetShadow().SetTrigger("Idle");
        }


        // check to see if 
        else if(!reachedTarget && slamFrame){
            boss.Slam();

            if (boss.transform.position.y < boss.GetLastPosition().y + 0.01f)
                {
                    boss.EnableAttackHitbox(true);
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
        slamFrame = true;
        Debug.Log("THE SLAM HAS STARTED");
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

