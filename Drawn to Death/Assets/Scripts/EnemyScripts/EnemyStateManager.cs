using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;



// was following this tutorial but did not complete it https://www.youtube.com/watch?v=Vt8aZDPzRjI
public class EnemyStateManager : MonoBehaviour
{

    EnemyBaseState currentState;
    EnemyAI movingState = new EnemyAI();
    IdleState idle = new IdleState();

    // Start is called before the first frame update
    void Start()
    {
        currentState = idle;
        currentState.EnterState(this);
    }


    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }


    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
