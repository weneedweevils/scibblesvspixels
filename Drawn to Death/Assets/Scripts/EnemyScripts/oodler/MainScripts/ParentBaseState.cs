using System.Collections.Generic;



/// <summary>
/// This class defines a parent base state where the behaviours of a group of states is defined
/// </summary>
public class ParentBaseState
{

    // Initialization // 
    protected Oodler boss;
    protected StateMachine oodlerStateMachine;

    protected Queue<ChildBaseState> subStateQueue;
    protected ChildBaseState currentChildState { get; set; }


    public ParentBaseState(Oodler boss, StateMachine oodlerStateMachine) {
        this.boss = boss;
        this.oodlerStateMachine = oodlerStateMachine;
        subStateQueue = new Queue<ChildBaseState>();

    }

    
    /// <summary>
    /// This function exits the currentChildState and enters the new one
    /// </summary>
    /// <param name="newState"></param>
    protected virtual void ChangeChildState(ChildBaseState newState)
    {
        currentChildState.ExitState();
        currentChildState = newState;
        currentChildState.EnterState();
    }

 


    /// <summary>
    /// This function will select the next substate to go to in the queue, if there are none it will switch to the idle state
    /// </summary>
    public virtual void NextSubState()
    {
        if (subStateQueue.Count > 0)
        {
            var nextChildState = subStateQueue.Dequeue();
            ChangeChildState(nextChildState);
        }
        else
        {
            oodlerStateMachine.ChangeState(boss.oodlerIdle);
        }

    }


    /// <summary>
    ///  Enters the parent state in function that inherits this make sure to create an ordered substate list, initalize queue, and set substate list to null before calling base
    /// </summary>
    public virtual void EnterParentState() {
        var nextChildState = subStateQueue.Dequeue();
        currentChildState = nextChildState;
        currentChildState.EnterState();
    }

    // Exits the current parent state
    public virtual void ExitParentState() {
        subStateQueue.Clear();
    } 

    // Updates the current child state every frame
    public virtual void ParentFrameUpdate()
    {
        currentChildState.FrameUpdate();
    }


    // Initializes the Queue
    public virtual void InitializeQueue(List<ChildBaseState> childStates)
    {
        foreach(ChildBaseState child in childStates)
        {
            subStateQueue.Enqueue(child);
        }
    }

    // This is a special function used to go to another parent state
    public void GoToDropState()
    {
        oodlerStateMachine.ChangeState(boss.oodlerDrop);
    }

    // Gets Current Child State
    public virtual ChildBaseState GetCurrentChildState()
    {
        return currentChildState;
    }
}
