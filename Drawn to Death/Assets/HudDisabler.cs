using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HudDisabler : MonoBehaviour
{

    public GameObject hud;
    // Start is called before the first frame update
    void OnEnable()
    {
        //GameObject hud = GameObject.Find("HUD");
        PlayerMovement.OnPlayerDeath += DisableHUD;
        
}

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= DisableHUD;
    }

    // Update is called once per frame
    

    private void DisableHUD()
    {
        hud.SetActive(false);
        PlayerMovement.OnPlayerDeath -= DisableHUD;
    }
}
