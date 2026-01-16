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

    private Chase chase { get; set; }
    private PrepareGrab prepareGrab { get; set; }
    private AttemptGrab attemptGrab { get; set; }
    private Rise rise { get; set; }
    private Vulnerable vulnerable { get; set; }
    private CarryGlich carryGlich { get; set; }
    private EmptyChildState emptyChild { get; set; }

    public OodlerGrab(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        Debug.Log("<color=red>ENTERING SLAM STATE");

        chase = new Chase(boss, this, chaseTime: 3f, chaseSpeed: 50f);
        prepareGrab = new PrepareGrab(boss, this, grabHoverTime: 5f, chaseSpeed: 100);
        attemptGrab = new AttemptGrab(boss, this, chaseSpeed: 100);
        vulnerable = new Vulnerable(boss, this, vulnerabilityTime: 1f);
        rise = new Rise(boss, this, 1f, 1f);
        carryGlich = new CarryGlich(boss, this, dropZoneHoverTime: 5f, dropZoneSpeed: 20f);
        emptyChild = new EmptyChildState(boss, this);


        orderedSubStateList = new List<ChildBaseState>
        {
            chase,
            prepareGrab,
            attemptGrab,
            vulnerable,
            rise,
            //carryGlich,
            emptyChild,
        };
        Debug.Log("We have entered the grabbing sub-state");
        base.EnterState(); // always go at the end


    }

    public override void ExitState()
    {
        chase = null;
        prepareGrab = null;
        attemptGrab = null;
        vulnerable = null;
        rise = null;
        carryGlich = null;
        emptyChild = null;

        base.ExitState();
    }

    public override void FrameUpdate()
    {
         base.FrameUpdate();
      
    }

    public override void NextSubState()
    {
        base.NextSubState();
        if (currentChildState.GetSuccess())
        {
            index = index + 1;
        }
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



}

