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
    private static float timer;
    private static bool pauseTimer = true;
    private static bool finalTime = false;
    private static bool showTimer = false;

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
        if (!pauseTimer && !finalTime)
        {
            timer += Time.unscaledDeltaTime;
            float seconds = timer % 60;
            float minutes = Mathf.Floor(timer / 60);
            float corrected_minutes = minutes % 60;
            float hours = Mathf.Floor(minutes / 60);
            if (showTimer)
            {
                textMeshPro.SetText(string.Format("{0}:{1}:{2}", hours.ToString(), corrected_minutes.ToString("00"), seconds.ToString("00.00")));
            }
            else
            {
                textMeshPro.SetText("");
            }
            
        }
    }

    public static void ResetTimer()
    {
        timer = 0f;
        finalTime = false;
    }
    public static void StartTimer()
    {
        pauseTimer = false;
    }
    public static void PauseTimer()
    {
        pauseTimer = true;
    }
    public static void EndTimer()
    {
        finalTime = true;
    }

    public static void ShowTimer()
    {
        showTimer = true;
    }
    public static void HideTimer()
    {
        showTimer = false;
    }
    
}
