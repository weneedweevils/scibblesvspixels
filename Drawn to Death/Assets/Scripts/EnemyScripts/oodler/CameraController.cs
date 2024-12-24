using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public GameObject playerReference;
    public GameObject oodlerReference;
    private Camera m_Camera;
    public float maxDistance = 1000f;
    public float minDistance = 50f;
    private float defaultOrthographicSize = 20f;
    private float targetMaxOrthographicSize = 25f;
    private float cameraAcc = 0.0005f;

    
    

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponentInChildren<Camera>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (playerReference.transform.position-oodlerReference.transform.position).magnitude;

        if (distance < minDistance)
        {
            AdjustCamera(defaultOrthographicSize);
        }
        
        else
        {
            AdjustCamera(targetMaxOrthographicSize);
        }
       

    }
    
    public void AdjustCamera(float targetSize)
    {
        
        if (Mathf.Abs(targetSize - m_Camera.orthographicSize)>0.5f){
            float size = Mathf.Lerp(m_Camera.orthographicSize, targetSize, Time.deltaTime * cameraAcc);
            m_Camera.orthographicSize = targetSize;
            cameraAcc =+ 0.005f;
        }
        else
        {
            m_Camera.orthographicSize = targetSize;
            cameraAcc = 0.005f;
        }

        
    }
}
