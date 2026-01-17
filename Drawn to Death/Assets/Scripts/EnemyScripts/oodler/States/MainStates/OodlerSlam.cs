using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class OodlerSlam : ParentBaseState
{

    bool reachedTarget = false;
    bool delay = false;
    private float timer = 0f;
    private float delayTimer = 0f;
    private bool isSlamFrame = false;


    private bool slamWasActivated = false;
    private AnimationEventNotifier animationEventNotifier;
    List<ChildBaseState> orderedSubStateList;


    public OodlerSlam(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    


    // parent state 
    public override void EnterParentState()
    {
        orderedSubStateList = new List<ChildBaseState>
        {

            new Chase(boss, this, 2f, chaseSpeed: 100f),
            new PrepareAttack(boss, this, 1, 100f),
            new SwingHand(boss, this, chaseSpeed: 100f),
            new Vulnerable(boss, this, vulnerabilityTime: 1f),
            new Rise(boss, this, 1f, 1f),
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


    // child state machine

    // This function is called in the childs update function


}

