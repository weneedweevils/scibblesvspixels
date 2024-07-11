using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public GameObject volumeControllerObject;
    private VolumeController volumeController;

    void Start(){
        volumeController = volumeControllerObject.GetComponent<VolumeController>();
    }

    public void MasterSliderChange(float value){
        volumeController.SetMasterVolume(value);
    }

    public void MusicSliderChange(float value){
        volumeController.SetMusicVolume(value);
    }

    public void SFXSliderChange(float value){
        volumeController.SetSFXVolume(value);
    }
    
}
