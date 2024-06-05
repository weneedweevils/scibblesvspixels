using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLayerRender : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer thisObject;

    // Start is called before the first frame update
    void Start()
    {
        thisObject = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            enabled = false;
        }
        if (player.transform.position.y > transform.position.y)
        {
            thisObject.sortingOrder = 4;
        }
        else
        {
            thisObject.sortingOrder = 5;
        }
    }
}
