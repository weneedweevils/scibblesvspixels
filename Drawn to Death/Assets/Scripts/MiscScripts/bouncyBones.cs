using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncyBones : MonoBehaviour
{
    Rigidbody2D rb;
    int i;
    float randx;
    float randy;
    float randrotation;

    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       i = 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.name == "Floor" && i>0)
        {
            randrotation = Random.Range(0f, 360f);
            randx = Random.Range(-1000f,1000f);
            randy = Random.Range(700f, 1000f);
            rb.AddForce(new Vector3(randx,randy,0));
            rb.rotation += randrotation;
            i--;
        }
        if (collision.gameObject.name == "rwall" && i > 0)
        {
            randrotation = Random.Range(0f, 360f);
            rb.AddForce(new Vector3(-1000,0,0));
            rb.rotation += randrotation;
            i--;
        }

        if (collision.gameObject.name == "lwall" && i > 0)
        {
            randrotation = Random.Range(0f, 360f);
            rb.AddForce(new Vector3(1000, 0, 0));
            rb.rotation += randrotation;
            i--;
        }

    }
}
