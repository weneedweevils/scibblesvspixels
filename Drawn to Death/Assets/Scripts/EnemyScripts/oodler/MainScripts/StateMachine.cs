using System;
using UnityEngine;

/// <summary>
/// This class defines a state machine class which has a starting parent state and then switches to parent states
/// </summary>
public class StateMachine
{
    public ParentBaseState currentState { get; set; }

    public void Initialize(ParentBaseState startingState)
    {
        if (currentState != null)
        {
            Debug.LogException(new Exception("Trying to instantiate State Machine more than once"));
            return;
        }
        currentState = startingState;
        currentState.EnterParentState();
    }
   

    public void ChangeState(ParentBaseState newState)
    {
        currentState.ExitParentState();
        currentState = newState;
        currentState.EnterParentState();
    }


    public ParentBaseState GetCurrentState(){
        return currentState;
    }

}
