using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryGlich : ChildBaseState
{
    private bool reachedDropZone = false;
    private float dropZoneSpeed;
    private float dropZoneHoverTime;
    private BossTimer dropZoneHoverTimer;

    public CarryGlich(Boss boss, ParentBaseState parentBaseState, float dropZoneHoverTime, float dropZoneSpeed = 20f) : base(boss, parentBaseState)
    {
        this.dropZoneSpeed = dropZoneSpeed;
        this.dropZoneHoverTime = dropZoneHoverTime;
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
        dropZoneHoverTimer = new BossTimer(dropZoneHoverTime);
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.EnableGrabHitbox(false);
        boss.EnableGlichColliders(true);
        dropZoneHoverTimer = null;
    }

    public override void FrameUpdate()
    {
        Debug.Log("Updating... in Carry GLich");
        base.FrameUpdate();
        
        if (reachedDropZone || boss.MoveToDropZone(dropZoneSpeed))
        {
            reachedDropZone = true;
            if (dropZoneHoverTimer.Update())
            {
                
                if (boss.DropGlich())
                {
                    boss.animator.SetTrigger("Idle");
                    boss.playerScript.animator.SetTrigger("Dropped");
                    boss.EnableGlichColliders(true);
                    boss.playerScript.EnableInput();
                    boss.playerScript.ChangeSpriteSortingOrder(5);
                    parentBaseState.NextSubState();
                }
            }
        }
        
    }

    public override void ResetState()
    {
        base.ResetState();
        dropZoneHoverTimer = null;
       
    }
}
