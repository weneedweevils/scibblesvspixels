using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IconManager : MonoBehaviour
{

    public GameObject playstationIcons;
    public GameObject xboxIcons;
    public GameObject kbmIcons;

    public PlayerInput playerInput;




    void Awake(){
        //kbmIcons.SetActive(true);
    }
    public void OnDeviceChanged(PlayerInput PI)
    {
        if (PI.currentControlScheme.Equals("Playstation")){
 
            playstationIcons.SetActive(true);
            xboxIcons.SetActive(false);
            kbmIcons.SetActive(false);
        }

        else if (PI.currentControlScheme.Equals("KBM")){
            kbmIcons.SetActive(true);
            playstationIcons.SetActive(false);
            xboxIcons.SetActive(false);
        }

         else if (PI.currentControlScheme.Equals("Gamepad")){
            kbmIcons.SetActive(false);
            playstationIcons.SetActive(false);
            xboxIcons.SetActive(true);
        }
    }
}
