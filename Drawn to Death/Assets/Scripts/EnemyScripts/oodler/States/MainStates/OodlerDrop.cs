using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerDrop : ParentBaseState
{
    List<ChildBaseState> orderedSubStateList;
    public OodlerDrop(Oodler boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }



    public override void EnterParentState()
    {

        orderedSubStateList = new List<ChildBaseState>
        {
            new RiseWithGlich(boss, this, riseSpeed: 20f, riseDelay: 0.5f),
            new CarryGlich(boss, this, dropZoneHoverTime: 2f, dropZoneSpeed: 50f),
            new EmptyChildState(boss, this),
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

   



