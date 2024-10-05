using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OodlerRun : BaseState
{

    public OodlerRun(Boss boss, StateMachine oodlerStateMachine, ChildStateMachine childStateMachine) : base(boss, oodlerStateMachine, childStateMachine)
    {
    }
    
    // Sub States Needed
    // 1. Go to valid Run Position using radius
    // 2. Land at Position
    // 3. Charge towards glich
    private Vector3 runPosition;

    
    public override void EnterState()
    {



        //boss.SelectRunPosition();
        base.EnterState();
        // In the first Child State We find the run position
        boss.childStateMachine.ChangeState(boss.goToRunPosition);
       

    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        childStateMachine.currentChildState.FrameUpdate();
        
       


        //ORIGINAL CODE//
        // if (!runPosition){
        //     runPosition = boss.MoveToRunPosition();
        // }   
        // else if(!runGroundPosition){
        //      runGroundPosition = boss.LandForRun();
        // }
        // else if(!finalRunPosition){
        //     boss.EnableRunHitbox(true);
        //     finalRunPosition = boss.Run(speed);
        //     speed = speed + 1f;
        // }
        // else{
        //     boss.EnableRunHitbox(false);
        //     Debug.Log("Here");
        // }
        
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
