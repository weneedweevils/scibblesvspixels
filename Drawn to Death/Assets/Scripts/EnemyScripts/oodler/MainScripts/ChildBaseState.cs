using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBaseState
{
    // Instead if using a child state machine we are going to pass in the parent state

    protected Boss boss;
    protected ParentBaseState parentBaseState;
    private StateMachine parentStateMachine;

    public ChildBaseState(Boss boss, ParentBaseState baseState) {
        this.boss = boss; 
        this.parentBaseState = baseState;
     
    }

    public ChildBaseState(Boss boss, StateMachine parentStateMachine)
    {
        this.boss = boss;
        this.parentStateMachine = parentStateMachine;
    }

    public virtual void EnterState() {
    }

    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }
}
