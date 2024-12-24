using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryGlich : ChildBaseState
{

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
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exiting Empty state");
    }

    public override void FrameUpdate()
    {
        Debug.Log("Updating...");
        base.FrameUpdate();
        
        if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught()){
             boss.MoveGlichWithOodler();
        }
    }
}
