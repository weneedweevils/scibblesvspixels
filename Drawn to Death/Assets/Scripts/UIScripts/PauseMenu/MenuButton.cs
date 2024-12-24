using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pause
{
    public class MenuButton : MonoBehaviour
    {
        public GameObject[] activate;
        public GameObject[] deactivate;
        public MenuButton backButton;
        public Sound hoverSound;
        public Sound selectSound;

        public void OnEnable()
        {
            if (backButton == this)
                PauseManager.SetBackButton(this);
        }

        public void Select()
        {
            foreach (GameObject obj in activate) obj.SetActive(true);
            foreach (GameObject obj in deactivate) obj.SetActive(false);
            PauseManager.SetBackButton(backButton, true);
            PlaySoundSelect();

            if (backButton == this)
                PauseManager.Pause(false);
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