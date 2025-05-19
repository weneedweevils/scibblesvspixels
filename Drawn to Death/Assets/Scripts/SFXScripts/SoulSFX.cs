using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSFX : MonoBehaviour
{

    [Header("FMOD Events")]
    public FMODUnity.EventReference SoulCollectSFX_1;
    public FMODUnity.EventReference SoulCollectSFX_2;

    public int soulTier = 0;


    public void PlaySound(int count)
    {
        if (count % soulTier == 0)
            FMODUnity.RuntimeManager.PlayOneShot(SoulCollectSFX_2, this.transform.position);
        else 
            FMODUnity.RuntimeManager.PlayOneShot(SoulCollectSFX_1, this.transform.position);
    }

}
