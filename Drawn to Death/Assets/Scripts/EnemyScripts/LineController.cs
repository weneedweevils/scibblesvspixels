using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    private LineRenderer lineRenderer;

    [SerializeField]
    private Texture[] textures;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();    
    }

    public void DrawLine(Vector3 crystalPos, Vector3 oodlerPos)
    {
        lineRenderer.SetPosition(0,crystalPos);
        lineRenderer.SetPosition(1, oodlerPos);
    }


    
}
