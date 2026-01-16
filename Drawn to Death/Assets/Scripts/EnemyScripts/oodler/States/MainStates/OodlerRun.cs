using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This is an attacking state where the oodler will charge towards glich and damage him
/// </summary>
public class OodlerRun : ParentBaseState
{

    private Chase chase { get; set; }
    private Land land { get; set; }
    private Run run { get; set; }
    private Vulnerable vulnerableState { get; set; }
    private Rise rise { get; set; }

    private Vector3 runPosition;

    public OodlerRun(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
      

   

    }
    
    
   

    
    
    public override void EnterState()
    {
       
        chase = new Chase(boss, this, 0f, 100f);
        land = new Land(boss, this, 10f);
        run = new Run(boss, this, 15f,25f);
        vulnerableState = new Vulnerable(boss, this, vulnerabilityTime: 2);
        rise = new Rise(boss, this, 10f, 1f);

        orderedSubStateList = new List<ChildBaseState>
        {
            chase,
            land,
            run,
            vulnerableState,
            rise
        };

        base.EnterState();
    }

    public override void ExitState()
    {
        chase = null;
        land = null;
        run = null;
        vulnerableState = null;
        rise = null;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        currentChildState.FrameUpdate();
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }



    public override void NextSubState()
    {
        base.NextSubState();
        index = index + 1;
        if (index < orderedSubStateList.Count)
        {
            ChangeChildState(orderedSubStateList[index]);
        }
        else // Change to next state in state machine // THIS DOES NOT RUN THE EXIT FUNCTION OF THE LAST STATE
        {
            ExitChildState();
            oodlerStateMachine.ChangeState(boss.oodlerRun);
        }

    }

    public override void Initialize(ChildBaseState startingState)
    {
        base.Initialize(startingState);
    }


    // simple getter
    public override ChildBaseState GetCurrentChildState()
    {
        return base.GetCurrentChildState();
    }
}
