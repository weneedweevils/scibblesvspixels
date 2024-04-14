using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{

    public GameObject menuButton;
    public GameObject credits;
    private float scrollSpeed = 50;
    
    // Update is called once per frame
    void Update()
    {
        credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            credits.transform.position += scrollSpeed*2 * Vector3.up * Time.deltaTime;
        }
    }
}
