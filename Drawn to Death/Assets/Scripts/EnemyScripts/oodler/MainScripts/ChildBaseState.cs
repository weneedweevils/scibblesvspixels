using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBaseState
{
    // Instead if using a child state machine we are going to pass in the parent state
    // PUT A LIST OF THINGS HERE THAT YOU SHOULD ALWAYS DO WHEN EXITING ANY CHILD STATE

    protected Oodler boss;
    protected ParentBaseState parentBaseState;
    private StateMachine parentStateMachine;
    private bool success = true;

    public ChildBaseState(Oodler boss, ParentBaseState baseState) {
        this.boss = boss; 
        this.parentBaseState = baseState;
     
    }

    public virtual void EnterState()
    {
        success = true;
    }

    public virtual void ResetState() { }
    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }


    public bool GetSuccess()
    {
        return success;
    }
}
