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

    public OodlerRun(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
        chase = new Chase(boss, this);
        run = new Run(boss, this);
        land = new Land(boss, this);
        vulnerableState = new Vulnerable(boss, this);
        rise = new Rise(boss, this);

    orderedSubStateList = new List<ChildBaseState>
        {
            chase,
            land,
            run,
            vulnerableState,
            rise
        };

    }
    
    
    private Vector3 runPosition;

    
    
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
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
