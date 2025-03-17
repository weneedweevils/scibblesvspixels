using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    // FMOD sound event paths
    [Header("Event References")]
    public FMODUnity.EventReference footstepSFX;
    public FMODUnity.EventReference dashSFX;
    public FMODUnity.EventReference eraserSFX;
    public FMODUnity.EventReference eraserHitSFX;
    public FMODUnity.EventReference lifestealSFX;
    public FMODUnity.EventReference rallySFX;
    public FMODUnity.EventReference reviveSFX;

    private FMOD.Studio.EventInstance lifestealInstance;

    void Awake()
    {
        lifestealInstance = FMODUnity.RuntimeManager.CreateInstance(lifestealSFX);
    }

    public void PlayFootstepSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(footstepSFX);
    }

    public void PlayDashSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(dashSFX);
    }

    public void PlayEraserSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(eraserSFX);
    }

    public void PlayRallySFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(rallySFX);
    }
    
    public void PlayReviveSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot(reviveSFX);
    }

    public void StartLifestealSFX()
    {
        lifestealInstance.start();
    }

    public void StopLifestealSFX()
    {
        lifestealInstance.stop(0);
    }

    private void OnDestroy()
    {
        lifestealInstance.stop(0);
    }
}
