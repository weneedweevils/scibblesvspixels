using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pause
{
    public class VolumeSlider : MonoBehaviour
    {
        public Volume volume;
        public UnityEngine.UI.Slider slider;
        public Sound hoverSound;
        public Sound selectSound;

        public void SetVolume()
        {
            PauseManager.instance.settings.SetVolume(volume, slider.value);
        }

        public void PlaySoundHover()
        {
            PauseManager.PlaySound(hoverSound);
        }
        public void PlaySoundSelect()
        {
            PauseManager.PlaySound(selectSound);
        }
    }
}