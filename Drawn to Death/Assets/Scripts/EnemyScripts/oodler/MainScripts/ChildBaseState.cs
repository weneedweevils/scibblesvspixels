using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines a child base state where the behaviours of individual states are implemented
/// </summary>
public class ChildBaseState
{
   

    protected Oodler boss;
    protected ParentBaseState parentBaseState;

    public ChildBaseState(Oodler boss, ParentBaseState baseState) {
        this.boss = boss; 
        this.parentBaseState = baseState;
     
    }

    public virtual void EnterState(){}

    public virtual void ResetState() { }
    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }


}
