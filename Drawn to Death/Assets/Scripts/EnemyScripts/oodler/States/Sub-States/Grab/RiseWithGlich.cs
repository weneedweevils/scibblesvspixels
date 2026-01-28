using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RiseWithGlich : ChildBaseState
{

    private Vector3 airPosition;
    BossTimer riseTimer;
    private float riseSpeed;
    private float riseDelay;
    private bool riseStarted;

    public RiseWithGlich(Oodler boss, ParentBaseState parentBaseState, float riseSpeed, float riseDelay) : base(boss, parentBaseState)
    {
        this.riseSpeed = riseSpeed;
        this.riseDelay = riseDelay;

    }


    public override void EnterState()
    {
        base.EnterState();
        boss.EnableAreaHitbox(false);
        boss.EnableSpriteHitbox(true);
        SpriteHitbox.HitBorder += ChangeGlichSpriteOrder; // the event subscribes to the event in oodlers sprite hitbox script so that glich will not clip 
        boss.SetAirPosition();
        riseTimer = new BossTimer(riseDelay);
        riseStarted = false;
        boss.EnableGlichColliders(false);

   

        Debug.Log("Entered Rise with glich state");

    }

    public override void ExitState()
    {
        SpriteHitbox.HitBorder -= ChangeGlichSpriteOrder;
        boss.EnableSpriteHitbox(false);
        Debug.Log("Changed sprite order back to 8");
        boss.BringSpriteToForeground();
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
      
        boss.ControlAllies(boss.dropZoneObject, true);

        if (riseTimer.Update())
        {

            if (!riseStarted)
            {
                boss.ShowShadow();
                riseStarted = true;
            }

            boss.MoveGlichWithOodler();
            if (boss.RiseOodler())
            {
              
              
                parentBaseState.NextSubState();
            }

        }
    }


    // MOVE TO BOSS FUNCTION
   

    public void IfCaught()
    {
        //if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught()){
        //boss.MoveGlichWithOodler();
        //}

    }

    public override void ResetState()
    {
        base.ResetState();

    }

    private void ChangeGlichSpriteOrder()
    {
        boss.playerScript.ChangeSpriteSortingOrder(6);
    }
}