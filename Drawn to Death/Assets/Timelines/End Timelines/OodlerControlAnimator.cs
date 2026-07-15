using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerControlAnimator : MonoBehaviour
{
    public enum AnimationState
    {
        Idle,
        Walk,
        Stunned
    }

    private Animator animator;
    public AnimationState startState;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        SetState(startState.ToString());
    }

    public void SetState(string state)
    {
        state = state.ToLower();
        animator.SetBool("Idle", state == "idle");
        animator.SetBool("Walk", state == "walk");
        animator.SetBool("Stunned", state == "stunned");
    }
}
