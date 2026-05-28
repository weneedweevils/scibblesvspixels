using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class OodlerDeathScreen : MonoBehaviour
{

    // Ooodler Objects
    private Animator oodlerAnimator;

    // Text
    public RectMask2D mask;
    public RectTransform text;

    //other
    private float starting = -500f;
    private float target;
    private float maxMaskVal;
    private bool reachedPos = false;
    private float diff;

  

   

    void OnEnable()
    {
        //GameObject hud = GameObject.Find("HUD");
        DeathScreen.OnDeathUiActive += OodlerDraw;
        target = text.sizeDelta.x/2f;
        starting = -target;
        diff = target - starting;
        oodlerAnimator = GetComponent<Animator>();
        maxMaskVal = mask.padding.z;
}

    private void OnDisable()
    {
        DeathScreen.OnDeathUiActive -= OodlerDraw;
    }

    void Start()
    {
    }

    void Update()
    {
        if (transform.localPosition.x> -500f && transform.position.x<500f)
        {
            
            var val = maxMaskVal - (transform.localPosition.x + 500f);
            mask.padding = new UnityEngine.Vector4(0, 0, val, 0);
          
        }
    }


    private void OodlerDraw()
    {
        // Move from off screen to on screen
        DeathScreen.OnDeathUiActive -= OodlerDraw;
    }
   
}
