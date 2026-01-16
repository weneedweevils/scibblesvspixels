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
    private Vulnerable vulnerable { get; set; }
    private EmptyChildState emptyChild { get; set; }



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
      


    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    // 1. windup - ball up fist over glich shadow gets darker
    // 2. slamfist down
    // 3. 

    private void SetSlamNum()
    {

    }

    // parent state 
    public override void EnterState()
    {
        Debug.Log("<color=red>ENTERING SLAM STATE");
        
        chase = new Chase(boss, this, chaseTime: 3f, chaseSpeed: 50f);
        prepareAttack = new PrepareAttack(boss, this, 1, 100f);
        swingHand = new SwingHand(boss, this, chaseSpeed: 100f);
        rise = new Rise(boss, this, 1f, 1f);
        vulnerable = new Vulnerable(boss, this, vulnerabilityTime: 1f);
        emptyChild = new EmptyChildState(boss, this);


        orderedSubStateList = new List<ChildBaseState>
        {
            chase,
            prepareAttack,
            swingHand,
            vulnerable,
            rise,
            emptyChild,
        };

        base.EnterState(); // always go at the end
    }

    public override void ExitState()
    {
        chase = null;
        prepareAttack = null;
        swingHand = null;
        rise = null;
        vulnerable = null;
        emptyChild = null;

        base.ExitState();
    
        
       
    }

    public override void FrameUpdate()
    {
   
        base.FrameUpdate();  
    }


    // child state machine

    // This function is called in the childs update function

    public override void NextSubState()
    {
        base.NextSubState();
        index = index + 1;
        if (index < orderedSubStateList.Count)
        {
            Debug.Log("OUR NEXT SUBSTATE WE WILL GO TO IS " + orderedSubStateList[index]);
            ChangeChildState(orderedSubStateList[index]);
        }
        else
        {
            ExitChildState();
            oodlerStateMachine.ChangeState(boss.oodlerQuickSlam);
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

