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

    // 1. windup - ball up fist over glich shadow gets darker
    // 2. slamfist down
    // 3. 

    public override void EnterState()
    {
      
        
        childStateMachine.ChangeState(boss.chase);
        base.EnterState();

        
    }


    public override void ExitState()
    {
        
        base.ExitState();
    
        
        

    }

    public override void FrameUpdate()
    {
       
        childStateMachine.currentChildState.FrameUpdate();  
        if(childStateMachine.currentChildState == boss.emptyChildState){
            childStateMachine.ChangeState(boss.chase);
        }
    }

  
}

