using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryGlich : ChildBaseState
{
    private bool reachedDropZone = false;
    private BossTimer bossTimer;
    private float dropZoneSpeed;

    public CarryGlich(Boss boss, ParentBaseState parentBaseState, float dropZoneSpeed = 20f) : base(boss, parentBaseState)
    {
        this.dropZoneSpeed = dropZoneSpeed;
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
        
        if(parentBaseState == boss.oodlerGrab && boss.IsCaught()){
            if (reachedDropZone || boss.MoveToDropZone(dropZoneSpeed))
            {
                reachedDropZone = true;
                if (bossTimer.Update())
                {
                    boss.animator.SetTrigger("Idle");
                    boss.GetShadow().SetTrigger("idle");
                    if (boss.DropGlich())
                    {
                        
                        boss.playerScript.animator.SetTrigger("Dropped");
                        boss.playerScript.EnableInput();
                        //childStateMachine.ChangeState(boss.chase);
                    }
                }
            }
        }
    }

    public override void ResetState()
    {
        base.ResetState();
       
    }
}
