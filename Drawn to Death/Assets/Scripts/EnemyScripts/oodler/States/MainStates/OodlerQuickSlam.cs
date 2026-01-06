using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class OodlerQuickSlam : ParentBaseState
{

    // Sub States
    private Chase chase { get; set; }
    private PrepareAttack prepareAttack { get; set; }
    private SwingHand swingHand { get; set; }
    private Rise rise { get; set; }



    bool reachedTarget = false;
    bool delay = false;
    private float timer = 0f;
    private float delayTimer = 0f;
    private bool isSlamFrame = false;


    private bool slamWasActivated = false;
    private AnimationEventNotifier animationEventNotifier;
    //int index = 0;



    public OodlerQuickSlam(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
        chase = new Chase(boss, this);
        prepareAttack = new PrepareAttack(boss,this);
        swingHand = new SwingHand(boss, this);
        rise = new Rise(boss, this);
        

        orderedSubStateList = new List<ChildBaseState>
        {
            chase,
            prepareAttack,
            swingHand,
            rise
        };


    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    // 1. windup - ball up fist over glich shadow gets darker
    // 2. slamfist down
    // 3. 


    // parent state 
    public override void EnterState()
    {
        Debug.Log("<color=red>ENTERING SLAM STATE");
        base.EnterState();
    }

    public override void ExitState()
    {
        

        base.ExitState();
    
        
       
    }

    public override void FrameUpdate()
    {
        Debug.Log("IN QUICK SLAM");
        currentChildState.FrameUpdate();  
    }


    // child state machine

    // This function is called in the childs update function

    public override void NextSubState()
    {
        base.NextSubState();
        index = index + 1;
        if (index < orderedSubStateList.Count)
        {
            ChangeChildState(orderedSubStateList[index]);
        }
        else
        {
            oodlerStateMachine.ChangeState(boss.oodlerSlam);
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

