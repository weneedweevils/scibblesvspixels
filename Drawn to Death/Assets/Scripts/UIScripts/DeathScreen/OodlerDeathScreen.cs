using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerDeathScreen : MonoBehaviour
{

    // Ooodler Objects
    private GameObject oodler;
    private RectTransform oodlerPos;
    private Animator oodlerAnimator;


    // Black bar
    private GameObject blackBar;
    private RectTransform blackBarPos;

    //other
    private bool died = false;
    private bool drawing = true;
    int num = 0;

    void OnEnable()
    {
        //GameObject hud = GameObject.Find("HUD");
        DeathScreen.OnDeathUiActive += OodlerDraw;
        oodler = this.gameObject.transform.GetChild(1).gameObject;
        blackBar = this.gameObject.transform.GetChild(0).gameObject;

        
        oodlerPos = oodler.GetComponent<RectTransform>();
        oodlerAnimator = oodler.GetComponent<Animator>();
        blackBarPos = blackBar.GetComponent<RectTransform>();
        

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
        // x = 18, y = -581

        switch (num)
        {
            case 0:
                break;
            case 1:
                
                oodlerPos.localPosition = oodlerPos.localPosition + (new Vector3(0, -1,0) * (Time.deltaTime * 1000f));
                num = oodlerPos.localPosition.y > 0 ? num + 0 : num + 1;
                break;
            case 2:
                oodlerAnimator.SetBool("Drawing", true);
                blackBarPos.localPosition = oodlerPos.localPosition;
                num = oodlerPos.localPosition.x >= 500 ? num + 1 : num + 0;
                break;
            case 3:
                oodlerAnimator.SetBool("Drawing", false);
                Debug.Log("Case 3");
                break;


        }


        //if (died)
        //{
        //    if (oodlerPos.localPosition.y > 0) {

        //        oodlerPos.localPosition = oodlerPos.localPosition + (new Vector3(0, -1, 0) * (Time.deltaTime*1000f));
                
        //    }
        //    else if(oodlerPos.localPosition.y <= 0)
        //    {

        //        oodlerAnimator.SetBool("Drawing", true);
        //        blackBarPos.localPosition = oodlerPos.localPosition;
                

        //        if (!drawing)
        //        {
        //            oodlerAnimator.SetBool("Drawing", false);
        //        }
        //    }

        //}

    }

    private void OodlerDraw()
    {

        // Move from off screen to on screen

        
        died = true;
        num += 1;
        DeathScreen.OnDeathUiActive -= OodlerDraw;
        Debug.Log("my num is : "+ num);


    }

   
}
