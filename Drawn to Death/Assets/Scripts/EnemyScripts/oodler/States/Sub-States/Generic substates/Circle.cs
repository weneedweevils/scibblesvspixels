using UnityEngine;

/// <summary>
/// This sub-state will make the oodler go to glichs position and close in on him
/// </summary>
public class Circle : ChildBaseState
{
    private Vector3 playerOffSet;
    private BossTimer circleTimer;
    private float circleSpeed;
    private float circleTime;
    private float circleRadius;
    private float circleChaseSpeed;
    private bool reachedCircle = false;




    public Circle(Oodler boss, ParentBaseState parentBaseState, float circleTime, float circleSpeed = 1f, float circleRadius = 25f, float circleChaseSpeed = 20f) : base(boss, parentBaseState)
    {
        this.circleSpeed = circleSpeed;
        this.circleTime = circleTime;
        this.circleRadius = circleRadius;
        this.circleChaseSpeed = circleChaseSpeed;
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.ShowShadow();
        playerOffSet = boss.glich.transform.localPosition;
        circleTimer = new BossTimer(circleTime);
        reachedCircle = false;
        boss.floating = true;
        //time we continue following glich


    }

    public override void ExitState()
    {
        base.ExitState();
        ResetState();
        boss.floating = false;

    }

    public override void FrameUpdate()
    {
        boss.CheckSpriteDirection();

        if (!reachedCircle) {
            if (boss.GoToCircle(circleChaseSpeed, circleRadius))
            {
                reachedCircle = true;
            }
        }
        else { 
            if (circleTimer.Update()){
                parentBaseState.NextSubState();
            }
            boss.Circleglich(circleSpeed, circleRadius);

        }
        // If the distance between glich and oodler gets shorter oodler speeds up to glich's position
       
        
    }

    public override void ResetState()
    {
        base.ResetState();
        circleTimer = null;
    }




}