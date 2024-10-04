using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OodlerRun : BaseState
{
    public OodlerRun(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    // Sub States Needed
    // 1. Go to valid Run Position using radius
    // 2. Land at Position
    // 3. Charge towards glich
    public GoToRunPosition goToRunPosition { get; set; }
    public Land land;
    
    private bool runPosition = false;
    private bool runGroundPosition = false;
    private bool finalRunPosition = false;
    private float speed = 20f;
    float timer = 0f;
    float myRadius = 10f;

    
    public override void EnterState()
    {
        goToRunPosition = new GoToRunPosition(boss, childStateMachine, oodlerStateMachine, this);
        land = new Land(boss, childStateMachine, oodlerStateMachine, this);


        //boss.SelectRunPosition();
        base.EnterState();
        // In the first Child State We find the run position
        childStateMachine.Initialize(goToRunPosition);
       

    }

    public override void ExitState()
    {
        base.ExitState();

        goToRunPosition = null;



        boss.hitObstacle = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        childStateMachine.currentChildState.FrameUpdate();
        
       

        // if(timer>5f){
        //     boss.CircleGlich(5f, myRadius);
        //     myRadius = myRadius - 1*Time.deltaTime;
        // }
        // else{
        //     boss.Stalk();
        // }
        // timer = timer+Time.deltaTime;


        //ORIGINAL CODE//
        if (!runPosition){
            runPosition = boss.MoveToRunPosition();
        }   
        else if(!runGroundPosition){
             runGroundPosition = boss.LandForRun();
        }
        else if(!finalRunPosition){
            boss.EnableRunHitbox(true);
            finalRunPosition = boss.Run(speed);
            speed = speed + 1f;
        }
        else{
            boss.EnableRunHitbox(false);
            Debug.Log("Here");
        }
        
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
