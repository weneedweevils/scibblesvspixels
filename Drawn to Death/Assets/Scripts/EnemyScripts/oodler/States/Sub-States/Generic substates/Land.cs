using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TextCore;
using Vector3 = UnityEngine.Vector3;

public class Land : ChildBaseState
{
    private float landSpeed;
    private bool reachedPosition;
    public Land(Oodler boss, ParentBaseState parentBaseState, float landSpeed) : base(boss, parentBaseState)
    {
        this.landSpeed = landSpeed;
    }
    private Vector3 runGroundPosition;

    public override void EnterState()
    {
        base.EnterState();
        reachedPosition = false;
        boss.SetLandPosition();
        Debug.Log("entered landing state");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        reachedPosition = boss.LandOodler(landSpeed);
        if(reachedPosition){

            Debug.Log("Going to next child state frpm land");
            boss.BringSpriteToBackground();
            parentBaseState.NextSubState();
            //childStateMachine.ChangeState(boss.run);
        }
        
    }

    // MOVE TO BOSS SCRIPT
   

    public override void ResetState()
    {
        base.ResetState();

    }
}
