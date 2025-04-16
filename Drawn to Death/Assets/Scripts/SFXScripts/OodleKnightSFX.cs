using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleKnightSFX : MonoBehaviour
{
    private EnemyAI selfAI;

    [Header("FMOD Events")]
    public FMODUnity.EventReference oodleKnightWalkSFX;
    public FMODUnity.EventReference oodleKnightAttackSFX;

    void Start()
    {
        selfAI = GetComponent<EnemyAI>();
        if (selfAI == null)
            selfAI = GetComponentInParent<EnemyAI>();
    }

    void PlaySound()
    {
        if (selfAI != null && selfAI.state != State.idle && !selfAI.isDead() && selfAI.state != State.reviving)
            FMODUnity.RuntimeManager.PlayOneShot(oodleKnightWalkSFX, this.transform.position);
    }

    void PlayAttack()
    {
        if (selfAI != null && selfAI.state != State.idle && !selfAI.isDead() && selfAI.state != State.reviving)
            FMODUnity.RuntimeManager.PlayOneShot(oodleKnightAttackSFX, this.transform.position);
    }
}
