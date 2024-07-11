using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject volumeControllerObject;
    private VolumeController volumeController;
    private Slider masterVolSlider, musicVolSlider, sfxVolSlider;

    void Awake(){
        volumeController = volumeControllerObject.GetComponent<VolumeController>();
        masterVolSlider = transform.GetChild(0).GetChild(1).GetComponent<Slider>();
        musicVolSlider = transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        sfxVolSlider = transform.GetChild(2).GetChild(1).GetComponent<Slider>();
    }
    void OnEnable(){
        masterVolSlider.value = volumeController.masterVol; 
        musicVolSlider.value = volumeController.musicVol;
        sfxVolSlider.value = volumeController.sfxVol;
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
