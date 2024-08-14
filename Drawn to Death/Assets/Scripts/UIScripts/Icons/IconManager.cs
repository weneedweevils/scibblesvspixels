using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IconManager : MonoBehaviour
{

    public GameObject playstationIcons;
    public GameObject xboxIcons;
    public GameObject kbmIcons;
    public GameObject gamepadIcons;


    void Start()
    {
        PlayerInput playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();

    }

    public void OnDeviceChanged(PlayerInput PI)
    {
        if (PI.currentControlScheme.Equals("Playstation")){
            ChangeToPlaystation();
        }

        if (PI.currentControlScheme.Equals("KBM")){
            ChangeToKBM();
        }

        if (PI.currentControlScheme.Equals("Xbox"))
        {
            ChangeToXbox();
        }

        if (PI.currentControlScheme.Equals("Gamepad"))
        {
            ChangeToGamepad();
        }
    }


    public void ChangeToPlaystation()
    {
        playstationIcons.SetActive(true);
        xboxIcons.SetActive(false);
        kbmIcons.SetActive(false);
        gamepadIcons.SetActive(false);
    }

    public void ChangeToKBM()
    {
        kbmIcons.SetActive(true);
        playstationIcons.SetActive(false);
        xboxIcons.SetActive(false);
        gamepadIcons.SetActive(false);
    }

    public void ChangeToXbox()
    {
        xboxIcons.SetActive(true);
        playstationIcons.SetActive(false);
        kbmIcons.SetActive(false);
        gamepadIcons.SetActive(false);
    }

    public void ChangeToGamepad()
    {
        gamepadIcons.SetActive(true);
        xboxIcons.SetActive(false);
        playstationIcons.SetActive(false);
        kbmIcons.SetActive(false);
    }
}
