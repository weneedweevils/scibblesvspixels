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
        bossTimer = new BossTimer(15f);
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.EnableGrabHitbox(false);
        boss.EnableGlichColliders(true);
        boss.SetBossCaught(false);
        boss.playerScript.animator.SetTrigger("Dropped");
        Debug.Log("exiting Empty state");
        boss.ControlAllies(boss.glich, false);
        boss.playerScript.PausePlayerInput(false);


    }

    public override void FrameUpdate()
    {
      
        base.FrameUpdate();
        
        if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught()){
            if(reachedDropZone || boss.MoveToDropZone()){
              
                reachedDropZone = true;
                
                if(bossTimer.Update()){
                    if(boss.DropGlich()){
                        childStateMachine.ChangeState(boss.chase);
                    }
                }
            }
        }
    }

     // This function moves the oodler to the drop zone where they drop glich
   
}
