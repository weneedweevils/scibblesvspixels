using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IconManager : MonoBehaviour
{

    public GameObject playstationIcons;
    public GameObject xboxIcons;
    public GameObject kbmIcons;




    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        

    }

    public void OnDeviceChanged(PlayerInput PI)
    {
        if (PI.currentControlScheme.Equals("Playstation")){
 
            playstationIcons.SetActive(true);
            xboxIcons.SetActive(false);
            kbmIcons.SetActive(false);
        }

        if (PI.currentControlScheme.Equals("KBM")){
            Debug.Log("keyboard");
            kbmIcons.SetActive(true);
            playstationIcons.SetActive(false);
            xboxIcons.SetActive(false);
            
        }
    }
}
