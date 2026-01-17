using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This is an attacking state where the oodler will charge towards glich and damage him
/// </summary>
public class OodlerRun : ParentBaseState
{
    private Vector3 runPosition;
    List<ChildBaseState> orderedSubStateList;

    public OodlerRun(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }
    
    public override void EnterParentState()
    {
        orderedSubStateList = new List<ChildBaseState>
        {
            new Chase(boss, this, 0f, 100f),
             new Land(boss, this, 10f),
             new Run(boss, this, 15f,25f),
             new Vulnerable(boss, this, vulnerabilityTime: 2),
             new Rise(boss, this, 10f, 1f),
        };

        base.InitializeQueue(orderedSubStateList);
        orderedSubStateList = null;
        base.EnterParentState();
    }

    public override void ExitParentState()
    {
       base.ExitParentState();
    }

    public override void ParentFrameUpdate()
    {
        base.ParentFrameUpdate();
    }

}
