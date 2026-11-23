using System.Collections.Generic;


public class OodlerTaunt : ParentBaseState
{

    List<ChildBaseState> orderedSubStateList;


    public OodlerTaunt(Oodler boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    // parent state 
    public override void EnterParentState()
    {
        orderedSubStateList = new List<ChildBaseState>
        {
            new Chase(boss, this, chaseTime: 2f),
            new Taunt(boss,this),

        };

        base.InitializeQueue(orderedSubStateList);
        orderedSubStateList = null;
        base.EnterParentState(); // always go at the end
    }

    public override void ExitParentState()
    {
        base.ExitParentState();
    }

    public override void ParentFrameUpdate()
    {
        base.ParentFrameUpdate();
    }


    // child state machine

    // This function is called in the childs update function

    public override void NextSubState()
    {
        base.NextSubState();

    }



}