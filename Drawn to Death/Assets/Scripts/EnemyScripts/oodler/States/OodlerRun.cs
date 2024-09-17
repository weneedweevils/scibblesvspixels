using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OodlerRun : OodlerBase
{
    public OodlerRun(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }
    
    private bool runPosition = false;
    private bool runGroundPosition = false;
    private bool finalRunPosition = false;
    private float speed = 20f;

    public override void EnterState()
    {
        boss.SelectRunPosition();
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!runPosition){
            runPosition = boss.MoveToRunPosition();
        }   
        else if(!runGroundPosition){
             runGroundPosition = boss.LandForRun();
        }
        else if(!finalRunPosition){
            Debug.Log("skrrrskrrr");
            finalRunPosition = boss.Run(speed);
            speed = speed + 1f;
        }
        else{
            Debug.Log("Here");
        }
        
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
