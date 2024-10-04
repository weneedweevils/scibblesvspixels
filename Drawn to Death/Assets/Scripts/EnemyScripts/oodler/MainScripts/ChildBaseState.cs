using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBaseState : MonoBehaviour
{
    protected Boss boss;
    protected ChildStateMachine childStateMachine;
    protected StateMachine parentStateMachine;
    protected BaseState parentState;


    public ChildBaseState(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine, BaseState parentState) {
        this.boss = boss;
        this.childStateMachine = childStateMachine;
        this.parentStateMachine = parentStateMachine;
        this.parentState = parentState;
    }


    public virtual void EnterState() { }

    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }
}
