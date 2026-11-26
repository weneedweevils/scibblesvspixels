using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

/// <summary>
/// This sub-state will make the oodler go to glichs position and close in on him
/// </summary>
public class Taunt : ChildBaseState
{
     
   
    private BossTimer waitTimer;
    // Taunt state for when the player dies
    public Taunt(Oodler boss,  ParentBaseState parentBaseState) : base(boss, parentBaseState)
    {
    
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.ShowShadow();
        boss.floating = true;
        waitTimer = new BossTimer(2f);


    }

    public override void ExitState()
    {
        base.ExitState();
        boss.floating = false;
        ResetState();

    }

    public override void FrameUpdate()
    {
        if (boss.isPlayerSelfReviving())
        {   
            //boss.animator.SetTrigger("Taunt");
            boss.BringSpriteOnTop();
            boss.MoveOutOfFrame();
            //parentBaseState.NextSubState();
            
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        waitTimer = null;
    }




}
