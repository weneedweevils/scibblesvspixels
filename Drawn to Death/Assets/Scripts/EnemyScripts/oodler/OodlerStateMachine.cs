using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerStateMachine
{
    public OodlerBase currentOodlerState { get; set; }

    public void Initialize(OodlerBase startingState)
    {
        currentOodlerState = startingState;
        currentOodlerState.EnterState();
    }
   

    public void ChangeState(OodlerBase newState)
    {
        currentOodlerState.ExitState();
        currentOodlerState = newState;
        currentOodlerState.EnterState();
    }
}
