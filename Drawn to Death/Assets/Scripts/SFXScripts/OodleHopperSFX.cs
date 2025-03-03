using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleHopperSFX : MonoBehaviour {
    
    private FMOD.Studio.EventInstance idleSFXInstance;

    public FMODUnity.EventReference idleSFX;
    private FMOD.Studio.PLAYBACK_STATE playbackState;

    private OodleHopper oodleHopper;

    void Start()
    {
        idleSFXInstance = FMODUnity.RuntimeManager.CreateInstance(idleSFX);  
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(idleSFXInstance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        oodleHopper = GetComponent<OodleHopper>();
        idleSFXInstance.start();
    }

    void Update()
    {
        idleSFXInstance.getPlaybackState(out playbackState);

        // Check if the enemy is dead (or jumping) or not, then play/stop the sound effect as required
        if (!oodleHopper.isDead() && !oodleHopper.isHopping &&
            playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED){
            // start the sfx playback again if it has stopped
            // We have to attach the instance again here too
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(idleSFXInstance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
            idleSFXInstance.start();
        }
        if ((oodleHopper.isDead() || oodleHopper.isHopping) &&
            playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING){
            // stop the sfx if the enemy is dead (or jumping)
            idleSFXInstance.stop(0);
        }
    }

    void OnDestroy()
    {
        idleSFXInstance.stop(0);
    }


}