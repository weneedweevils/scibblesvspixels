﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pulse : MonoBehaviour
{
    public AnimationCurve effect;
    private bool waiting = false;
    private bool active = false;
    private float t = 0;
    private Vector3 standard;
    private float duration;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        standard = transform.localScale;
        duration = effect.keys[effect.length - 1].time;
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < 1f && !waiting)
        {
            waiting = true;
            Debug.LogFormat("{0} waiting", name);
        }
        if (waiting && slider.value >= 1f)
        {
            Activate();
            Debug.LogFormat("{0} activated", name);
        }

        if (active)
        {
            t += Time.deltaTime;
            transform.localScale = standard * effect.Evaluate(t);

            if (t > duration)
            {
                t = 0;
                active = false;
            }
        }
    }

    public void Activate()
    {
        waiting = false;
        active = true;
    }
}