using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;

public class GoToRunPosition : ChildBaseState
{
    public GoToRunPosition(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine, BaseState parentState) : base(boss, childStateMachine, parentStateMachine, parentState)
    {
    }

    private Vector3 runPosition;
    private bool reachedPosition;

    public override void EnterState()
    {
        base.EnterState();

        SelectRunPosition();
       

    }
    

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        reachedPosition = MoveToRunPosition();

        if(reachedPosition) {
           childStateMachine.ChangeState(parentState)
        }

    }
    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


    // This Function will create a circle around glich and Will choose a single random location out of 360 that is not outside a wall
    public void SelectRunPosition()
    {
       
        float starting_angle = 0;
        float radius = 30f;
        var Positions = new List<Vector3>();
        int layerMask = 1 << 8;


        for (float i = starting_angle; i < 360f; i = i + 1f)
        {
            float x = boss.glich.transform.position.x + (Mathf.Cos(i) * radius);
            float y = boss.glich.transform.position.y + (Mathf.Sin(i) * radius);
            Vector3 landingSpot = new Vector3(x, y, 0);
            Vector3 direction = (boss.glich.transform.position - landingSpot).normalized;

            RaycastHit2D hit = Physics2D.Raycast(landingSpot, direction, Mathf.Infinity, layerMask);


            Debug.Log("I hit a wall at a distance of " + hit.distance + " from the point");
            Debug.DrawLine(landingSpot, boss.glich.transform.position, Color.magenta, 5f);
            if (hit.distance > radius)
            {
                Positions.Add(landingSpot);
                Debug.DrawLine(landingSpot, boss.glich.transform.position, Color.magenta, 5f);
                //     Debug.Log(i + " is a valid angle and there are not obstacles in the way");
            }
        }

        var rnd = new Random();
        int index = Random.Range(0, Positions.Count);
        runPosition = Positions[index];
           
    }

    // This method will move the ooodler to the position where it will try to run glich over
    public bool MoveToRunPosition(float speed = 50)
    {
        var step = speed * Time.deltaTime;
        boss.oodlerRB.MovePosition(Vector3.MoveTowards(transform.position, runPosition, step));
        boss.MoveShadowSprite();
        if (Vector3.Distance(transform.position, runPosition) < 0.3f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
