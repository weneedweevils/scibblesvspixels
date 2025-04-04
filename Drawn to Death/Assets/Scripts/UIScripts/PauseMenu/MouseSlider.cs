using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pause
{
    public class MouseSlider : MonoBehaviour
    {
        public UnityEngine.UI.Slider slider;
        public Sound hoverSound;
        public Sound selectSound;

        public void SetSensitivity()
        {
            PauseManager.instance.settings.SetSensitivity(slider.value);
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