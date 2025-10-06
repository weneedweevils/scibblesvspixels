using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DeathScreen : MonoBehaviour
{

    public GameObject panel;
    public GameObject lights;
    public GameObject buttons;
    Color blackzero = new Color(0, 0, 0, 0);
    Color black = new Color(0, 0, 0, 1);
    private bool dead = false;
    UnityEngine.UI.Image image;
    private float timer = 0f;
    public static event Action OnDeathUiActive;
    //private UnityEngine.UI.Image image = panel.GetComponent<UnityEngine.UI.Image>();


    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += StartDeathScreen;
        image = panel.GetComponent<UnityEngine.UI.Image>();

    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= StartDeathScreen;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead && timer > 2f)
        {
            if (image.color.a < 1f)
            {
                var temp = image.color;
                temp.a += 0.5f * Time.deltaTime;
                image.color = temp;
            }
            else if (dead && image.color.a > 0.9f)
            {
                buttons.SetActive(true);
                OnDeathUiActive?.Invoke();
            }
        }

        timer += Time.deltaTime;
    }

    private void StartDeathScreen()
    {

        Debug.Log("MADE IT to the Death Screen");
        //StartCoroutine(MenuManager.ReloadScene());
        

        //panel.SetActive(true);
        lights.SetActive(false);
        
        dead = true;



        PlayerMovement.OnPlayerDeath -= StartDeathScreen;
    }
}
