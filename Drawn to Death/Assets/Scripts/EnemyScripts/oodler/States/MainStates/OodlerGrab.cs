using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OodlerGrab : ParentBaseState
{   


    bool reachedTarget = false;
    bool delay = true;
    private float timer = 0f;
    private float delayTimer = 0f;
    private List<ChildBaseState> orderedSubStateList;

  
    public OodlerGrab(Oodler boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void EnterParentState()
    {
        Debug.Log("<color=red>ENTERING SLAM STATE");
        orderedSubStateList = new List<ChildBaseState>
        {
            new Chase(boss, this, chaseTime: 2f, chaseSpeed: 50f),
            new PrepareGrab(boss, this, grabHoverTime: 1f, chaseSpeed: 100),
            new AttemptGrab(boss, this, chaseSpeed: 100),
            new Vulnerable(boss, this, vulnerabilityTime: 7f),
            new Rise(boss, this, 1f, 1f),
            new EmptyChildState(boss, this),
        };


        base.InitializeQueue(orderedSubStateList);
        orderedSubStateList = null;
        base.EnterParentState(); // always go at the end


    }

    // special function to go to drop state 
    

    public override void ExitParentState()
    {
        
        base.ExitParentState();
    }

    public override void ParentFrameUpdate()
    {
         base.ParentFrameUpdate();
      
    }

    public override void NextSubState()
    {
        base.NextSubState();
       

    }



}

