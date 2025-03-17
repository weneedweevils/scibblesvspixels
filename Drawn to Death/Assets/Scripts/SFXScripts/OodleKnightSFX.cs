using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleKnightSFX : MonoBehaviour
{
    [Header("FMOD Events")]
    public FMODUnity.EventReference oodleKnightWalkSFX;
    public FMODUnity.EventReference oodleKnightAttackSFX;

    void PlaySound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oodleKnightWalkSFX, this.transform.position);
    }

    void PlayAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oodleKnightAttackSFX, this.transform.position);
    }
}
