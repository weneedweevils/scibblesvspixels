using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pause
{
    public enum Volume {Master, Music, SFX}
    public enum Sound {Hover, Accept, Back, Load}

    public class PauseManager : Singleton<PauseManager>
    {
        public SettingsUI settings;
        public GameObject[] activate;
        public GameObject[] deactivate;
        private static MenuButton backButton = null;
        public static bool paused = false;
        private PlayerInput playerInput;
        public GameObject shopUI;

        private static Dictionary<Sound, string> SFXMap = new Dictionary<Sound, string>()
        {
            { Sound.Hover, "event:/UI/UIHover"},
            { Sound.Accept, "event:/UI/UIAccept"},
            { Sound.Back, "event:/UI/UIBack"},
            { Sound.Load, "event:/UI/UILoad"},
        };

        public void Start()
        {
            playerInput = CustomInput.instance.playerInput;
            Pause(false);
           
            
        }

       

        public void Update()
        {
            if (playerInput.actions["Escape"].triggered && shopUI.activeSelf == false)
            {
                if (paused)
                {
                    backButton.Select();
                }
                else
                {
                    foreach (GameObject obj in activate) obj.SetActive(true);
                    foreach (GameObject obj in deactivate) obj.SetActive(false);
                    Pause(true);
                }
            }
        }

        

        public static void PlaySound(Sound sound)
        {
            // Verify the sound exists in the SFXMap
            if (SFXMap.TryGetValue(sound, out string soundPath))
            {
                // Play the sound using FMOD
                FMODUnity.RuntimeManager.PlayOneShot(soundPath);
            }
            else
            {
                Debug.LogErrorFormat("Sound {0} not found in the SFXMap.", sound);
            }
        }

        public static void Pause(bool pause)
        {
            paused = pause;
            if (paused)
            {
                instance.playerInput.SwitchCurrentActionMap("UI");
            }
            else
            {
                instance.playerInput.SwitchCurrentActionMap("Player");
            }
            foreach (GameObject obj in instance.activate) obj.SetActive(paused);
            foreach (GameObject obj in instance.deactivate) obj.SetActive(!paused);
            Time.timeScale = (paused ? 0f : 1f);
        }

        public void PlaySoundHover()
        {
            PlaySound(Sound.Hover);
        }
        public void PlaySoundAccept()
        {
            PlaySound(Sound.Accept);
        }
        public void PlaySoundBack()
        {
            PlaySound(Sound.Back);
        }
        public void PlaySoundLoad()
        {
            PlaySound(Sound.Load);
        }

        public static void SetBackButton(MenuButton button, bool forceSet = false)
        {
            if ((forceSet || backButton == null) && button != null)
            {
                backButton = button;
            }
        }
        public static void ClearBackButton(MenuButton button, bool forceClear = false)
        {
            if (forceClear || backButton == button)
            {
                backButton = null;
            }
        }
    }
}