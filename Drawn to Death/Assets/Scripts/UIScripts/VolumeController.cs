using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made using this tutorial for help: https://www.youtube.com/watch?v=_D-OZaCH8os&ab_channel=ScottGameSounds
public class VolumeController : MonoBehaviour {
    private FMOD.Studio.VCA masterVca, musicVca, sfxVca, dialogueVca;
    public float masterVol, musicVol, sfxVol, dialogueVol;
    public float cutsceneReduction;
    private float volumeReduction;
    public bool inCutscene;

    void Start()
    {
        masterVca = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        musicVca = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        sfxVca = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        dialogueVca = FMODUnity.RuntimeManager.GetVCA("vca:/Dialogue");
        
        masterVol = PlayerPrefs.GetFloat("MasterVolume", 1);
        musicVol = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1);
        dialogueVol = PlayerPrefs.GetFloat("SFXVolume", 1);

        inCutscene = false;
        volumeReduction = 1;
        Debug.Log(volumeReduction);
    } 

    void Update()
    {
        masterVca.setVolume(masterVol);
        dialogueVca.setVolume(dialogueVol);

        musicVca.setVolume(musicVol * volumeReduction);
        sfxVca.setVolume(sfxVol * volumeReduction);

        if (inCutscene){
            if (volumeReduction > cutsceneReduction){
                volumeReduction -= (float)0.1;
            }
            if (volumeReduction < cutsceneReduction){
                volumeReduction = cutsceneReduction;
            }
        } else{
            if (volumeReduction < 1){
                volumeReduction += (float) 0.1;
            }
            if (volumeReduction > 1){
                volumeReduction = 1;
            }
        }
        
    }

    public void SetMasterVolume(float volume){
        masterVol = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    
    public void SetMusicVolume(float volume){
        musicVol = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume){
        sfxVol = volume;
        dialogueVol = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlaySFXSample();
    }

    public void PlaySFXSample(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIAccept");
    }
}