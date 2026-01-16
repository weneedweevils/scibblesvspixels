using UnityEngine;

public class PrepareGrab : ChildBaseState
{
    private BossTimer grabHoverTimer;
    private bool reachedTarget = false;
    private bool attackCharged = false;
    private bool stopOodler = false;
    private float chaseSpeed;
    private float grabHoverTime;


    public PrepareGrab(Boss boss, ParentBaseState parentBaseState, float grabHoverTime, float chaseSpeed = 100f) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
        this.grabHoverTime = grabHoverTime;
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.SetBossCaught(false);
        boss.animator.SetTrigger("GrabWindUp");
        //boss.GetShadow().SetTrigger("GrabWindUp");
        //boss.GetShadow().SetTrigger();
        reachedTarget = false;
        attackCharged = false;
        grabHoverTimer = new BossTimer(grabHoverTime);
        stopOodler = false;
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Following if statement will stalk glich, once the redoutline is fully revealed we will stop the oodler for sometime to give the player time to react
        if(!stopOodler){
            reachedTarget = boss.MoveToGlich(chaseSpeed);
            if(reachedTarget){
                    if(grabHoverTimer.Update()){
                        stopOodler = true;
                        parentBaseState.NextSubState();
                        // change our state to the actual attack state
                    }
                
            }
        }
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void ResetState()
    {
        base.ResetState();

    }
}