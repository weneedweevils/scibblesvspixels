using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

public class ParentBaseState
{

    protected Oodler boss;
    protected StateMachine oodlerStateMachine;

    protected Queue<ChildBaseState> subStateQueue;
    protected ChildBaseState currentChildState { get; set; }
    protected ChildBaseState nextChildState {  get; set; }
    protected int index;


    public ParentBaseState(Oodler boss, StateMachine oodlerStateMachine) {
        this.boss = boss;
        this.oodlerStateMachine = oodlerStateMachine;
        subStateQueue = new Queue<ChildBaseState>();

    }


    protected virtual void ChangeChildState(ChildBaseState newState)
    {
        currentChildState.ExitState();
        currentChildState = newState;
        currentChildState.EnterState();
    }

    protected virtual void ExitChildState()
    {
        currentChildState.ExitState();
    }


    public virtual ChildBaseState GetCurrentChildState()
    {
        return currentChildState;
    }


    public virtual void NextSubState()
    {
        if (subStateQueue.Count > 0)
        {
            nextChildState = subStateQueue.Dequeue();
            ChangeChildState(nextChildState);
        }
        else
        {
            oodlerStateMachine.ChangeState(boss.oodlerIdle);
            // End of Queue reached Handle logic to go to next state (Maybe go to the idle state and idle state determines where to go)
        }



    }

    /// <summary>
    ///  Enters the parent state in function that inherits this make sure to create an ordered substate list, initalize queue, and set substate list to null before calling base
    /// </summary>
    public virtual void EnterParentState() {
        
        

        nextChildState = subStateQueue.Dequeue();
        currentChildState = nextChildState;
        currentChildState.EnterState();
    }

    public virtual void ExitParentState() {
        subStateQueue.Clear();
    } 

    public virtual void ParentFrameUpdate()
    {
        currentChildState.FrameUpdate();
    }



    // make sure this function is called in the class inheriting this
    public virtual void InitializeQueue(List<ChildBaseState> childStates)
    {
        foreach(ChildBaseState child in childStates)
        {
            subStateQueue.Enqueue(child);
        }
    }

    public void SkipStates(int skipCount)
    {
        for(int i = 0; i<skipCount; i++)
        {
            subStateQueue.Dequeue();
        }
    }

    public void GoToDropState()
    {
        oodlerStateMachine.ChangeState(boss.oodlerDrop);
    }






}
