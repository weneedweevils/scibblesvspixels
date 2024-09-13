using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject volumeControllerObject;
    private VolumeController volumeController;
    [SerializeField] private Slider masterVolSlider, musicVolSlider, sfxVolSlider;
    [SerializeField] private Toggle fancyFontToggle;

    void OnEnable(){
        volumeController = volumeControllerObject.GetComponent<VolumeController>();
        masterVolSlider.value = volumeController.masterVol; 
        musicVolSlider.value = volumeController.musicVol;
        sfxVolSlider.value = volumeController.sfxVol;
        fancyFontToggle.isOn = DialogueManager.fancyFont;
    }

    public void MasterSliderChange(){
        volumeController.SetMasterVolume(masterVolSlider.value);
    }

    public void MusicSliderChange(){
        volumeController.SetMusicVolume(musicVolSlider.value);
    }

    public void SFXSliderChange(){
        volumeController.SetSFXVolume(sfxVolSlider.value);
    }
    
    public void FancyFontChange()
    {
        PlayerPrefs.SetInt("fancyFont", (fancyFontToggle.isOn ? 1 : 0));
        Debug.LogFormat("Set fancyFont to {0}", PlayerPrefs.GetInt("fancyFont"));
    }
}
