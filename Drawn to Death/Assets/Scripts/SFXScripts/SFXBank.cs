using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFX Bank", menuName = "SFX Bank")]
public class SFXBank : ScriptableObject
{
    public FMODUnity.EventReference[] SoundEffects;

    public void PlaySFX(int id)
    {
        if(id >= 0 && id < SoundEffects.Length)
        {
            FMODUnity.RuntimeManager.PlayOneShot(SoundEffects[id]);
        }
    }
}
