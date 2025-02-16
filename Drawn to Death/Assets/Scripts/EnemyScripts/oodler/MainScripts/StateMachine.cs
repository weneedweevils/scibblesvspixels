﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class StateMachine
{
    public BaseState currentOodlerState { get; set; }

    public void Initialize(BaseState startingState)
    {
        currentOodlerState = startingState;
        currentOodlerState.EnterState();
    }
   

    public void ChangeState(BaseState newState)
    {
        currentOodlerState.ExitState();
        currentOodlerState = newState;
        currentOodlerState.EnterState();
    }


    public BaseState GetCurrentState(){
        return currentOodlerState;
    }

}
