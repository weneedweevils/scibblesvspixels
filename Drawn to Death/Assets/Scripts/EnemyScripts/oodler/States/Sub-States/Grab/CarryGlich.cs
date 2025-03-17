using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryGlich : ChildBaseState
{
    private bool reachedDropZone = false;
    private BossTimer bossTimer;

    public CarryGlich(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {

    }
    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Carrying Glich");
        reachedDropZone = false;
        bossTimer = new BossTimer(2f);
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.EnableGrabHitbox(false);
        boss.EnableGlichColliders(true);
        boss.SetBossCaught(false);
        boss.ControlAllies(boss.glich, false);
        Debug.Log("exiting Empty state");
    }

    public override void FrameUpdate()
    {
        Debug.Log("Updating...");
        base.FrameUpdate();
        
        if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught()){
            if (reachedDropZone || boss.MoveToDropZone())
            {
                reachedDropZone = true;

                if (bossTimer.Update())
                {
                    if (boss.DropGlich())
                    {
                        
                        boss.playerScript.animator.SetTrigger("Dropped");
                        boss.animator.SetTrigger("Idle");
                        boss.shadowAnimator.SetTrigger("idle");
                        boss.playerScript.EnableInput();
                        childStateMachine.ChangeState(boss.chase);
                    }
                }
            }
        }
    }
}
