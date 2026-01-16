using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

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
        currentState.EnterState();
    }
   

    public void ChangeState(ParentBaseState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }


    public ParentBaseState GetCurrentState(){
        return currentState;
    }

}
