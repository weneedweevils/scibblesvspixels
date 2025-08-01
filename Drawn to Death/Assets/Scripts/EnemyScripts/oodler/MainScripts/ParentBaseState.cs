using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

public class ParentBaseState
{

    protected Boss boss;
    protected StateMachine oodlerStateMachine;
    protected List<ChildBaseState> orderedSubStateList;
    protected ChildBaseState currentChildState { get; set; }
    protected int index;


    public ParentBaseState(Boss boss, StateMachine oodlerStateMachine) {
        this.boss = boss;
        this.oodlerStateMachine = oodlerStateMachine;
        orderedSubStateList = new List<ChildBaseState>();

    }
  

    public virtual void EnterState() {
        // don't know why I initialized here
        index = 0;
        Initialize(orderedSubStateList[index]);
    }

    public virtual void ExitState() {
        index = 0;
    } 

    public virtual void FrameUpdate() { }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }





    public virtual void Initialize(ChildBaseState startingState)
    {
        currentChildState = startingState;
        currentChildState.EnterState();
    }


    protected virtual void ChangeChildState(ChildBaseState newState)
    {
        currentChildState.ExitState();
        currentChildState = newState;
        currentChildState.EnterState();
    }


    public virtual ChildBaseState GetCurrentChildState()
    {
        return currentChildState;
    }

    public virtual void NextSubState()
    {

        
    }

  

}
