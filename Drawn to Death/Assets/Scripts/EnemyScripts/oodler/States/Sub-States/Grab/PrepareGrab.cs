using UnityEngine;

public class PrepareGrab : ChildBaseState
{
   
    private bool reachedTarget = false;

    private float chaseSpeed;
    private float grabHoverTime;

    private BossTimer grabHoverTimer;


    public PrepareGrab(Boss boss, ParentBaseState parentBaseState, float grabHoverTime, float chaseSpeed = 100f) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
        this.grabHoverTime = grabHoverTime;
    }

    public override void EnterState()
    {
        base.EnterState();

        boss.SetBossCaught(false);
        reachedTarget = false;

        grabHoverTimer = new BossTimer(grabHoverTime);

        boss.animator.SetTrigger("GrabWindUp");
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // This code will follow glich in hand open position
        reachedTarget = boss.MoveToGlich(chaseSpeed);
        if(reachedTarget){
                if(grabHoverTimer.Update()){
                    parentBaseState.NextSubState();
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