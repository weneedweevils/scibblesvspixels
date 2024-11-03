using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pause
{
    public class SettingsUI : MonoBehaviour
    {
        private VolumeController volumeController;
        [SerializeField] private Slider masterVolSlider, musicVolSlider, sfxVolSlider;
        [SerializeField] private Toggle fancyFontToggle;
        private bool setup = false;

        void OnEnable()
        {
            StartCoroutine("Setup");
        }

        IEnumerator Setup()
        {
            setup = false;

            yield return null;  //Give volumeController some time to start, otherwise all values will be set to 1

            volumeController = VolumeController.instance;

            SetVolume(Volume.Master, volumeController.masterVol);
            SetVolume(Volume.Music, volumeController.musicVol);
            SetVolume(Volume.SFX, volumeController.sfxVol);

            fancyFontToggle.isOn = DialogueManager.fancyFont;

            setup = true;
        }

        private void OnDisable()
        {
            PlayerPrefs.Save();
            setup = false;
        }

        public void SetVolume(Volume volume, float value)
        {
            switch (volume)
            {
                case Volume.Master:
                    masterVolSlider.value = value;
                    volumeController.SetMasterVolume(value);
                    break;
                case Volume.Music:
                    musicVolSlider.value = value;
                    volumeController.SetMusicVolume(value);
                    break;
                case Volume.SFX:
                    sfxVolSlider.value = value;
                    volumeController.SetSFXVolume(value);
                    break;
                default:
                    Debug.LogErrorFormat("Trying to change volume of unsupported volume type {0}", volume);
                    break;
            }
        }

        public void MasterSliderChange()
        {
            if (setup)
                volumeController?.SetMasterVolume(masterVolSlider.value);
        }

        public void MusicSliderChange()
        {
            if (setup)
                volumeController?.SetMusicVolume(musicVolSlider.value);
        }

        public void SFXSliderChange()
        {
            if (setup)
                volumeController?.SetSFXVolume(sfxVolSlider.value);
        }

        public void FancyFontChange()
        {
            PlayerPrefs.SetInt("fancyFont", (fancyFontToggle.isOn ? 1 : 0));
            DialogueManager.fancyFont = fancyFontToggle.isOn;
        }
    }
}