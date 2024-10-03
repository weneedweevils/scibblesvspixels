using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OodlerRun : BaseState
{
    public OodlerRun(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }


    
    
    private bool runPosition = false;
    private bool runGroundPosition = false;
    private bool finalRunPosition = false;
    private float speed = 20f;
    float timer = 0f;
    float myRadius = 10f;

    public override void EnterState()
    {
        //boss.SelectRunPosition();
        base.EnterState();
        boss.SelectRunPosition();

    }

    public override void ExitState()
    {
        base.ExitState();
        boss.hitObstacle = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        
       

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
