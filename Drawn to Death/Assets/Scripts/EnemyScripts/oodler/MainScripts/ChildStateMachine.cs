using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildStateMachine 
{
    public ChildBaseState currentChildState { get; set; }

    public void Initialize(ChildBaseState startingState)
    {
        currentChildState = startingState;
        currentChildState.EnterState();
    }
   
    public void ChangeState(ChildBaseState newState)
    {
        currentChildState.ExitState();
        currentChildState = newState;
        currentChildState.EnterState();
    }
}
