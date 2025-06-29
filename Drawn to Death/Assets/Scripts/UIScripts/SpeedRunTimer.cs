using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;
using System;
using System.Text;

public class SpeedRunTimer : Singleton<SpeedRunTimer>
{
    private TMPro.TextMeshProUGUI textMeshPro;
    private float timer;



    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        timer = 0f;
    }

    
    // Update is called once per frame
    void Update()
    {
        //if (SceneManager.GetActiveScene().name == "menu" )
        //{
            timer += Time.deltaTime;
            float seconds = timer % 60;
            float minutes = Mathf.Floor(timer / 60);
            float corrected_minutes = minutes % 60;
            float hours = Mathf.Floor(minutes / 60);
            textMeshPro.SetText(string.Format("{0}:{1}:{2}", hours.ToString(), corrected_minutes.ToString("00"), seconds.ToString("00.00")));
        //}
        
    }
}
