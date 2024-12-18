using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBaseState 
{
    protected Boss boss;
    protected ChildStateMachine childStateMachine;
    protected StateMachine parentStateMachine;


    public ChildBaseState(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) {
        this.boss = boss;
        this.childStateMachine = childStateMachine;
        this.parentStateMachine = parentStateMachine;
    }


    public virtual void EnterState() { }

    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }
}
