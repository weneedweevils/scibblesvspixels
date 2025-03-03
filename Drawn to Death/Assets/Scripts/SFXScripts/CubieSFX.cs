using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieSFX : MonoBehaviour
{
    [Header("FMOD Events")]
    public FMODUnity.EventReference cubieSwishSFX;
    public FMODUnity.EventReference cubieSwooshSFX;

    void PlaySwish()
    {
        FMODUnity.RuntimeManager.PlayOneShot(cubieSwishSFX, this.transform.position);
    }

    void PlaySwoosh()
    {
        FMODUnity.RuntimeManager.PlayOneShot(cubieSwooshSFX, this.transform.position);
    }
}
