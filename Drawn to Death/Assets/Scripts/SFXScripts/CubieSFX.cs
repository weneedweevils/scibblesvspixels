using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieSFX : MonoBehaviour
{
    private EnemyAI selfAI;

    [Header("FMOD Events")]
    public FMODUnity.EventReference cubieSwishSFX;
    public FMODUnity.EventReference cubieSwooshSFX;

    void Start()
    {
        selfAI = GetComponent<EnemyAI>();
        if (selfAI == null)
            selfAI = GetComponentInParent<EnemyAI>();
    }

    void PlaySwish()
    {
        if (selfAI != null && selfAI.state != State.idle && !selfAI.isDead() && selfAI.state != State.reviving)
            FMODUnity.RuntimeManager.PlayOneShot(cubieSwishSFX, this.transform.position);
    }

    void PlaySwoosh()
    {
        if (selfAI != null && selfAI.state != State.idle && !selfAI.isDead() && selfAI.state != State.reviving)
            FMODUnity.RuntimeManager.PlayOneShot(cubieSwooshSFX, this.transform.position);
    }
}
