using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made using this tutorial for help: https://www.youtube.com/watch?v=_D-OZaCH8os&ab_channel=ScottGameSounds
public class VolumeController : MonoBehaviour {
    private FMOD.Studio.VCA masterVca, musicVca, sfxVca;

    void Start()
    {
        masterVca = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        musicVca = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        sfxVca = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
    } 

    public void SetMasterVolume(float volume){
        masterVca.setVolume(volume);
    }
    
    public void SetMusicVolume(float volume){
        musicVca.setVolume(volume);
    }

    public void SetSFXVolume(float volume){
        sfxVca.setVolume(volume);

    }
    public void PlaySFXSample(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIAccept");
    }
}