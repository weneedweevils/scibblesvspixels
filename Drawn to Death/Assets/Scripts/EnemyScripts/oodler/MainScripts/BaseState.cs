using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

public class BaseState
{

    protected Boss boss;
    protected StateMachine oodlerStateMachine;
    protected ChildStateMachine childStateMachine;
    protected List<ChildBaseState> orderedSubStateList;
    

    public BaseState(Boss boss, StateMachine oodlerStateMachine, ChildStateMachine childStateMachine) {
        this.boss = boss;
        this.oodlerStateMachine = oodlerStateMachine;
        this.childStateMachine = childStateMachine;
        orderedSubStateList = new List<ChildBaseState>();
    }
  
    public virtual void EnterState() { }

    public virtual void ExitState() { } 

    public virtual void FrameUpdate() {
        childStateMachine.currentChildState.FrameUpdate();
     }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }

    public virtual void getChildStateMachine() { }

    public virtual List<ChildBaseState> getNextSubState(){
        return orderedSubStateList;
    }

}
